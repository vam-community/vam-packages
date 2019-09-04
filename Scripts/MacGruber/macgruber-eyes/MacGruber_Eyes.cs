// Eyes v0.1 by MacGruber.
// Simple plugin that slowly closes eyes for a moment
// every now and then, overriding AutoBlink.

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

namespace MacGruber
{
    public class Eyes : MVRScript
    {	
		private DAZMorph eyesClosed;
		private JSONStorableBool autoBlink;
		
		private float clock = 0.0f;
		private float duration = 0.0f;
		private float eyesClosedPrev = 0.0f;
		private float eyesClosedTarget = 0.0f;
		private int state = 0;
		
        public override void Init()
		{
			try 
			{
				if (containingAtom.type != "Person")
				{
					SuperController.LogError($"Requires atom of type person, instead selected: {containingAtom.type}");
					return;
				}

				JSONStorable geometry = containingAtom.GetStorableByID("geometry");
				DAZCharacterSelector dcs = geometry as DAZCharacterSelector;
				GenerateDAZMorphsControlUI morphUI = dcs.morphsControlUI;
				if (morphUI != null)
					eyesClosed = morphUI.GetMorphByDisplayName("Eyes Closed");
				
				JSONStorable eyelids = containingAtom.GetStorableByID("EyelidControl");
				autoBlink = eyelids.GetBoolJSONParam("blinkEnabled");
			}
			catch (System.Exception e) {
				SuperController.LogError("Exception caught: " + e);
			}
		}	

        protected void FixedUpdate()
		{
			clock += Time.fixedDeltaTime;
			if (clock >= duration)
			{				
				eyesClosed.morphValue = eyesClosedPrev = eyesClosedTarget;			
				clock = 0.0f;
				switch (state)
				{
					case 0: // opened
						eyesClosedTarget = 0.0f;
						autoBlink.val = true;
						duration = UnityEngine.Random.Range(20.0f, 30.0f);
						state = 1;
						break;
					case 1: // closing
						eyesClosedTarget = 0.8f;
						autoBlink.val = false;
						duration = UnityEngine.Random.Range(0.4f, 0.7f);
						state = 2;
						break;
					case 2: // closed
						eyesClosedTarget = 0.8f;
						autoBlink.val = false;
						duration = UnityEngine.Random.Range(1.0f, 5.0f);
						state = 3;
						break;
					case 3: // opening
					default:
						eyesClosedTarget = 0.0f;
						autoBlink.val = false;
						duration = UnityEngine.Random.Range(0.7f, 1.0f);
						state = 0;
						break;
				}
			}
			else
			{
				float t = Mathf.Clamp01(clock / duration);
				eyesClosed.morphValue = Mathf.SmoothStep(eyesClosedPrev, eyesClosedTarget, t);
			}
        }
		
		
    }
}
