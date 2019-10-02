using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;

// Breathing MacGruber v1.0
// 
// History:
// - Breathe_Deluxe_Standalone_V1_3_1 by u/JustLookingForNothin
// - Original "Breathe.cs" Script from Dollmaster by u/VAM Deluxe

namespace MacGruber
{
    public class Breathe : MVRScript
    {
		private static readonly string SCENE_DIR = "file:///Saves/scene/MacGruber/Breathe/";
		private static readonly string AUDIO_DIR = "Audio/";
		
		private BreathEntry[] breathEntries = new BreathEntry[] {
			new BreathEntry("00.wav", 0.372f, 0.509f, 0.850f, 1.227f, false),
			new BreathEntry("01.wav", 0.391f, 0.524f, 0.800f, 0.977f, false),
			new BreathEntry("02.wav", 0.624f, 1.199f, 2.183f, 2.350f, true ),
			new BreathEntry("03.wav", 0.266f, 0.400f, 0.650f, 0.878f, false),
			new BreathEntry("04.wav", 0.449f, 0.604f, 0.850f, 1.159f, false),
			new BreathEntry("05.wav", 0.435f, 0.597f, 1.100f, 1.389f, false),
			new BreathEntry("06.wav", 0.343f, 0.697f, 1.106f, 1.358f, false),
			new BreathEntry("07.wav", 0.497f, 0.905f, 1.202f, 1.439f, false),
			new BreathEntry("08.wav", 0.325f, 0.545f, 0.833f, 0.979f, false),
			new BreathEntry("09.wav", 0.304f, 0.516f, 0.848f, 1.152f, false),
			new BreathEntry("10.wav", 0.307f, 0.447f, 0.756f, 0.900f, true ),
			new BreathEntry("11.wav", 0.612f, 0.835f, 1.122f, 1.250f, false),
			new BreathEntry("12.wav", 0.800f, 1.070f, 1.539f, 1.793f, false),
			new BreathEntry("13.wav", 0.614f, 0.812f, 1.191f, 1.558f, false),
			new BreathEntry("14.wav", 0.355f, 0.455f, 0.909f, 1.348f, false),
			new BreathEntry("15.wav", 0.421f, 0.557f, 0.817f, 1.045f, false),
			new BreathEntry("16.wav", 0.391f, 0.435f, 0.668f, 1.387f, false),
			new BreathEntry("17.wav", 0.371f, 0.443f, 0.809f, 1.561f, false),
			new BreathEntry("18.wav", 0.379f, 0.472f, 0.780f, 1.012f, false),
			new BreathEntry("19.wav", 0.386f, 0.505f, 0.811f, 1.065f, false),
			new BreathEntry("20.wav", 0.454f, 0.590f, 0.817f, 1.244f, true ),
			new BreathEntry("21.wav", 0.331f, 0.447f, 0.744f, 1.003f, true ),
			new BreathEntry("22.wav", 0.457f, 0.596f, 0.927f, 1.161f, true ),
			new BreathEntry("23.wav", 0.294f, 0.454f, 0.773f, 0.927f, false),
			new BreathEntry("24.wav", 0.600f, 0.679f, 1.160f, 1.640f, true ),
			new BreathEntry("25.wav", 0.444f, 0.515f, 0.816f, 1.039f, false),
			new BreathEntry("26.wav", 0.427f, 0.478f, 0.689f, 0.987f, false),
			new BreathEntry("27.wav", 0.299f, 0.325f, 0.635f, 0.877f, false),
			new BreathEntry("28.wav", 0.266f, 0.334f, 0.685f, 1.142f, false),
			new BreathEntry("29.wav", 0.414f, 0.487f, 0.799f, 1.040f, false),
			new BreathEntry("30.wav", 0.303f, 0.340f, 0.495f, 0.892f, false),
			new BreathEntry("31.wav", 0.652f, 0.738f, 0.985f, 1.462f, false),
			new BreathEntry("32.wav", 0.436f, 0.495f, 0.747f, 1.208f, false),
			new BreathEntry("33.wav", 0.384f, 0.525f, 0.990f, 1.538f, false),
			new BreathEntry("34.wav", 0.482f, 0.546f, 0.927f, 1.100f, true ),
			new BreathEntry("35.wav", 0.381f, 0.432f, 0.909f, 0.991f, true ),
			new BreathEntry("36.wav", 0.441f, 0.467f, 0.743f, 0.852f, false),
			new BreathEntry("37.wav", 0.320f, 0.376f, 0.712f, 0.900f, true ),
			new BreathEntry("38.wav", 0.429f, 0.558f, 0.907f, 1.250f, false),
			new BreathEntry("39.wav", 0.326f, 0.371f, 0.595f, 0.827f, false)
		};
		
		
		
		
		private float breatheCycle = 0.0f;
		private float breatheDuration = 1.0f;
		private int breatheIndex = 0;
		private float breatheIntensity = 0.0f;
		private float breathePower = 1.0f;
		private bool breatheNeedInit = true;
		private BreathEntry breatheEntry;
		
		private DAZMorph stomachMorph;
		private DAZMorph mouthMorph;
		private DAZMorph nosePinchMorph;
		private DAZMorph noseFlareMorph;
		private FreeControllerV3 chestController;
		private JSONStorable headAudio;
	
		public JSONStorableFloat intensity;
		public JSONStorableFloat variance;
		public JSONStorableFloat stomachPower;
		public JSONStorableFloat chestSpring;
		public JSONStorableFloat chestDriveMin;
		public JSONStorableFloat chestDriveMax;
		public JSONStorableFloat mouthOpenMin;
		public JSONStorableFloat mouthOpenMax;
		
		
		private struct BreathEntry
		{
			public string name;
			public float breatheOut;
			public float holdOut;
			public float breatheIn;
			public float holdInReference;
			public bool noseIn;
			public NamedAudioClip audioClip;			
			
			public BreathEntry(string name, float breatheOut, float holdOut, float breatheIn, float holdIn, bool noseIn)
			{
				this.name = name;
				this.breatheOut = breatheOut;
				this.holdOut = holdOut;
				this.breatheIn = breatheIn;
				this.holdInReference = holdIn - breatheIn;
				this.noseIn = noseIn;
				
				string filename = "./"+AUDIO_DIR+name;
				audioClip = URLAudioClipManager.singleton.GetClip(filename);
				if (audioClip == null)
				{
					URLAudioClipManager.singleton.QueueClip(SCENE_DIR+AUDIO_DIR+name);
					audioClip = URLAudioClipManager.singleton.GetClip(filename);
				}
			}
		}
		
        public override void Init()
        {
            try
            {
                JSONStorable js = containingAtom.GetStorableByID("geometry");
                if (js != null)
                {
                    DAZCharacterSelector dcs = js as DAZCharacterSelector;
                    GenerateDAZMorphsControlUI morphUI = dcs.morphsControlUI;
                    if (morphUI != null)
                    {
                        stomachMorph = morphUI.GetMorphByDisplayName("Breath1");
                        mouthMorph = morphUI.GetMorphByDisplayName("Mouth Open");
						nosePinchMorph  = morphUI.GetMorphByDisplayName("Nose Pinch");
						noseFlareMorph = morphUI.GetMorphByDisplayName("Nostrils Flare");
                    }
                }
				
				headAudio = containingAtom.GetStorableByID("HeadAudioSource");				
                chestController = containingAtom.GetStorableByID("chestControl") as FreeControllerV3;
                
				
				intensity = CreateFloatSlider("Intensity", 0.0f, 0.0f, 1.0f);
				variance = CreateFloatSlider("Variance", 0.2f, 0.0f, 0.5f);
				stomachPower = CreateFloatSlider("StomachPower", 0.3f, 0.0f, 1.0f);
				chestSpring = CreateFloatSlider("ChestSpring", 90.0f, 0.0f, 250.0f);
				chestDriveMin = CreateFloatSlider("ChestDriveMin", -3.0f, -20.0f, 20.0f);
				chestDriveMax = CreateFloatSlider("ChestDriveMax", 8.0f, -20.0f, 20.0f);
				mouthOpenMin = CreateFloatSlider("MouthOpenMin", 0.0f, -0.5f, 2.0f);
				mouthOpenMax = CreateFloatSlider("MouthOpenMax", 0.8f, -0.5f, 2.0f);
				
				if (chestController != null)
                {
                    chestController.jointRotationDriveSpring = chestSpring.val;
                    chestController.jointRotationDriveDamper = 1.0f;
                }
				
				
				breatheNeedInit = true;
				
				float v = variance.val * (breathEntries.Length-1);	
				float bi = breatheIntensity * (breathEntries.Length-1);
				float min = Mathf.Max(bi-v-1.0f, -0.5f);
				float max = Mathf.Min(bi+v+1.0f, breathEntries.Length-0.5f);
				int index = Mathf.RoundToInt(UnityEngine.Random.Range(min, max));
				breatheIndex = Mathf.Clamp(index, 0, breathEntries.Length-1);

				breatheCycle = 0.0f;
				breatheIntensity = intensity.val;
				breathePower = UnityEngine.Random.Range(0.8f, 1.2f);
				breatheDuration = UnityEngine.Random.Range(0.0f, 1.5f);
            }
			catch (Exception e)
			{
				SuperController.LogError("Exception caught: " + e);
			}
        }
		
		private JSONStorableFloat CreateFloatSlider(string name, float defaultValue, float minValue, float maxValue)
		{
			JSONStorableFloat storable = new JSONStorableFloat(name, defaultValue, minValue, maxValue, true, true);
			storable.storeType = JSONStorableParam.StoreType.Full;
			CreateSlider(storable);
			RegisterFloat(storable);
			return storable;
		}
		
        public void Update()
        {
			float breatheMorph = 0.0f;
			breatheCycle += Time.deltaTime;
			if (breatheCycle >= breatheDuration)
			{				
				breatheIntensity = intensity.val;
				breathePower = UnityEngine.Random.Range(0.8f, 1.2f);
				float v = variance.val * (breathEntries.Length-1);	
				float bi = breatheIntensity * (breathEntries.Length-1);
				float min = Mathf.Max(bi-v-1.0f, -0.5f);
				float max = Mathf.Min(bi+v+1.0f, breathEntries.Length-0.5f);
				if (min < breatheIndex && max > breatheIndex)
					max -= 1.0f;			
				int index = Mathf.RoundToInt(UnityEngine.Random.Range(min, max));
				index = Mathf.Clamp(index, 0, breathEntries.Length-2);
				breatheIndex = index < breatheIndex ? index : index+1;				
				breatheEntry = breathEntries[breatheIndex];
				breatheDuration = breatheEntry.breatheIn + UnityEngine.Random.Range(breatheEntry.holdInReference*0.9f, breatheEntry.holdInReference*1.1f);
				breatheCycle = 0.0f;
				breatheNeedInit = false;				
				headAudio.CallAction("PlayNow", breatheEntry.audioClip);
			}	
			
            if (stomachMorph != null)
            {
				float power = breatheIntensity*0.7f+0.3f;
				power *= breathePower;
				float t = 1.0f - BlendOutIn(0.0f, 0.0f, 0.0f, 0.0f);
				float max = stomachPower.val * power;
                stomachMorph.morphValue = Mathf.SmoothStep(0.3f, -max, t);
            }
			
            if (chestController != null)
            {	
				float power = breatheIntensity*0.5f+0.5f;
				power *= breathePower;
				float t = 1.0f - BlendOutIn(0.0f, 0.0f, 0.0f, 0.0f);
				float max = chestDriveMin.val + power * (chestDriveMax.val - chestDriveMin.val);
				float target = Mathf.SmoothStep(chestDriveMin.val, max, t);
                chestController.jointRotationDriveXTarget = target;
				chestController.jointRotationDriveSpring = chestSpring.val;
            }

            if (mouthMorph != null && nosePinchMorph != null && noseFlareMorph != null)
            {				
				float mouth = 0.0f;
				float nose = 0.0f;
				if (breatheEntry.noseIn) // mouth out, nose in
				{
					mouth = BlendOut(0.0f, 0.05f, 0.25f);
					nose = BlendIn(0.0f, 0.0f, 0.25f);
				}
				else // mouth only breath
				{					
					mouth = BlendOutIn(0.0f, -0.1f, 0.0f, 0.3f);
					nose = 0.0f;
				}
				
				float power = (breatheIntensity*0.5f+0.5f) * breathePower;
				mouth *= power;
				nose *= power;
				
				mouthMorph.morphValue = Mathf.SmoothStep(mouthOpenMin.val, mouthOpenMax.val, mouth);
				nosePinchMorph.morphValue = Mathf.SmoothStep(0.0f, 0.2f, nose);
				noseFlareMorph.morphValue = Mathf.SmoothStep(0.0f, 0.8f, nose);
            }
        }
		
		private float BlendOutIn(float st, float bo, float ho, float bi)
		{
			if (breatheNeedInit)
				return 0.0f;
			
			bo += breatheEntry.breatheOut;
			ho += breatheEntry.holdOut;
			bi += breatheEntry.breatheIn;
			
			float e = 0.001f;
			bo = Mathf.Clamp(bo, e, breatheDuration-e);
			ho = Mathf.Clamp(ho, e, breatheDuration-e);
			bi = Mathf.Clamp(bi, e, breatheDuration-e);
			if (bo+3*e>bi)
			{
				bo = (bo+bi) * 0.5f;
				bi = bo + 3*e;
			}
			st = Mathf.Clamp(st, 0.0f, bo-e);
			ho = Mathf.Clamp(ho, bo+e, bi-e);
			
			return BlendInternal(st, bo, ho, bi);
		}
		
		private float BlendOut(float st, float bo, float duration)
		{
			if (breatheNeedInit)
				return 0.0f;
			
			float e = 0.001f;
			bo += breatheEntry.breatheOut;
			st = Mathf.Clamp(st, 0.0f, bo-e);

			float a,b,c,d;
			
			float hd = duration * 0.5f;
			a = Mathf.Clamp(st-hd, 0.0f, breatheDuration-e);
			d = Mathf.Clamp(bo+hd, a+3*e, breatheDuration-e);			
			if (d - a < duration + 2*e)
			{
				b = (a+d)*0.5f - e;
				c = (a+d)*0.5f + e;
			}
			else
			{
				b = Mathf.Clamp(st+hd, a+e, d-2*e);
				c = Mathf.Clamp(bo-hd, b+e, d-e);
			}			
			return BlendInternal(a,b,c,d);
		}
		
		private float BlendIn(float ho, float bi, float duration)
		{
			if (breatheNeedInit)
				return 0.0f;
			
			ho += breatheEntry.holdOut;
			bi += breatheEntry.breatheIn;
			
			float a,b,c,d;
			float e = 0.001f;
			float hd = duration * 0.5f;
			a = Mathf.Clamp(ho-hd, 0.0f, breatheDuration-e);
			d = Mathf.Clamp(bi+hd, a+3*e, breatheDuration-e);			
			if (d - a < duration + 2*e)
			{
				b = (a+d)*0.5f - e;
				c = (a+d)*0.5f + e;
			}
			else
			{
				b = Mathf.Clamp(ho+hd, a+e, d-2*e);
				c = Mathf.Clamp(bi-hd, b+e, d-e);
			}			
			return BlendInternal(a,b,c,d);
		}
		
		private float BlendInternal(float a, float b, float c, float d)
		{	
			if (breatheCycle < a)
				return 0.0f;
			else if (breatheCycle < b)
				return Mathf.Clamp01((breatheCycle - a) / (b - a));
			else if (breatheCycle < c)
				return 1.0f;
			else if (breatheCycle < d)
				return 1.0f - Mathf.Clamp01((breatheCycle - c) / (d - c));
			else
				return 0.0f;
		}
    }
}
