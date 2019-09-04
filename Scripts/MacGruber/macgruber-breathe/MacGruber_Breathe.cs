using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;

// Breathing v1.1 by MacGruber. Controlling VariableTriggers and various improvements.
// 
// History:
// - Breathing v1.0 by MacGruber. Introduces audio driven breathing.
// - Breathe_Deluxe_Standalone_V1_3_1 by u/JustLookingForNothin
// - Original "Breathe.cs" Script from Dollmaster by u/VAM Deluxe

namespace MacGruber
{
    public class Breathe : MVRScript
    {
		private static readonly string SCENE_DIR = "file:///Saves/Assets/MacGruber/";
		private static readonly string AUDIO_DIR = "Audio/";
		private static readonly int RECEIVER_COUNT = 2;
		
		private BreathEntry[] breathEntries = new BreathEntry[] {
			new BreathEntry("00.wav", 0.372f, 0.509f, 0.850f, 1.227f, false, false),
			new BreathEntry("01.wav", 0.391f, 0.524f, 0.800f, 0.977f, false, false),
			new BreathEntry("02.wav", 0.624f, 1.199f, 2.183f, 2.350f, true , false),
			new BreathEntry("03.wav", 0.266f, 0.400f, 0.650f, 0.878f, false, false),
			new BreathEntry("04.wav", 0.449f, 0.604f, 0.850f, 1.159f, false, true ),
			new BreathEntry("05.wav", 0.435f, 0.597f, 1.100f, 1.389f, false, true ),
			new BreathEntry("06.wav", 0.343f, 0.697f, 1.106f, 1.358f, false, false),
			new BreathEntry("07.wav", 0.497f, 0.905f, 1.202f, 1.439f, false, true ),
			new BreathEntry("08.wav", 0.325f, 0.545f, 0.833f, 0.979f, false, false),
			new BreathEntry("09.wav", 0.304f, 0.516f, 0.848f, 1.152f, false, false),
			new BreathEntry("10.wav", 0.307f, 0.447f, 0.756f, 0.900f, true , false),
			new BreathEntry("11.wav", 0.612f, 0.835f, 1.122f, 1.250f, false, true ),
			new BreathEntry("12.wav", 0.800f, 1.070f, 1.539f, 1.793f, false, true ),
			new BreathEntry("13.wav", 0.614f, 0.812f, 1.191f, 1.558f, false, true ),
			new BreathEntry("14.wav", 0.355f, 0.455f, 0.909f, 1.348f, false, false),
			new BreathEntry("15.wav", 0.421f, 0.557f, 0.817f, 1.045f, false, false),
			new BreathEntry("16.wav", 0.391f, 0.435f, 0.668f, 1.387f, false, false),
			new BreathEntry("17.wav", 0.371f, 0.443f, 0.809f, 1.561f, false, false),
			new BreathEntry("18.wav", 0.379f, 0.472f, 0.780f, 1.012f, false, false),
			new BreathEntry("19.wav", 0.386f, 0.505f, 0.811f, 1.065f, false, true ),
			new BreathEntry("20.wav", 0.454f, 0.590f, 0.817f, 1.244f, true , true ),
			new BreathEntry("21.wav", 0.331f, 0.447f, 0.744f, 1.003f, true , false),
			new BreathEntry("22.wav", 0.457f, 0.596f, 0.927f, 1.161f, true , true ),
			new BreathEntry("23.wav", 0.294f, 0.454f, 0.773f, 0.927f, false, false),
			new BreathEntry("24.wav", 0.600f, 0.679f, 1.160f, 1.640f, true , true ),
			new BreathEntry("25.wav", 0.444f, 0.515f, 0.816f, 1.039f, false, true ),
			new BreathEntry("26.wav", 0.427f, 0.478f, 0.689f, 0.987f, false, true ),
			new BreathEntry("27.wav", 0.299f, 0.325f, 0.635f, 0.877f, false, false),
			new BreathEntry("28.wav", 0.266f, 0.334f, 0.685f, 1.142f, false, false),
			new BreathEntry("29.wav", 0.414f, 0.487f, 0.799f, 1.040f, false, true ),
			new BreathEntry("30.wav", 0.303f, 0.340f, 0.495f, 0.892f, false, false),
			new BreathEntry("31.wav", 0.652f, 0.738f, 0.985f, 1.462f, false, true ),
			new BreathEntry("32.wav", 0.436f, 0.495f, 0.747f, 1.208f, false, false),
			new BreathEntry("33.wav", 0.384f, 0.525f, 0.990f, 1.538f, false, true ),
			new BreathEntry("34.wav", 0.482f, 0.546f, 0.927f, 1.100f, true , false),
			new BreathEntry("35.wav", 0.381f, 0.432f, 0.909f, 0.991f, true , true ),
			new BreathEntry("36.wav", 0.441f, 0.467f, 0.743f, 0.852f, false, true ),
			new BreathEntry("37.wav", 0.320f, 0.376f, 0.712f, 0.900f, true , false),
			new BreathEntry("38.wav", 0.429f, 0.558f, 0.907f, 1.250f, false, false),
			new BreathEntry("39.wav", 0.326f, 0.371f, 0.595f, 0.827f, false, false)
		};
		
		private BreathEntry[] breathEntriesNoMoan;
		

		
		private float breatheCycle = 0.0f;
		private float breatheDuration = 1.0f;
		private float breatheDurationAverage = 1.0f;
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
	
		private JSONStorableFloat intensity;
		private JSONStorableFloat variance;
		private JSONStorableFloat speedAdjust;
		private JSONStorableFloat rhythmAdapt;
		private JSONStorableFloat stomachPower;
		private JSONStorableFloat chestSpring;
		private JSONStorableFloat chestDriveMin;
		private JSONStorableFloat chestDriveMax;
		private JSONStorableFloat mouthOpenMin;
		private JSONStorableFloat mouthOpenMax;
		private JSONStorableBool  allowMoan;
		private JSONStorableBool  rhythmAdaptForceOnce;
		
		private Receiver[] receivers = new Receiver[RECEIVER_COUNT];
		
		// MVRScript doesn't support enums...workaround
		private const int BlendMode_OutIn       = 0;
		private const int BlendMode_OutHold     = 1;
		private const int BlendMode_InHold      = 2;
		private const int BlendMode_OutInSmooth = 3;
		private const int BlendMode_OutSmooth   = 4;
		private const int BlendMode_InSmooth    = 5;
		
		private class Receiver 
		{
			public JSONStorableStringChooser vartrigChooser;
			public JSONStorableStringChooser blendModeChooser;
			public JSONStorableFloat offsetStart;
			public JSONStorableFloat offsetEnd;
			public JSONStorableFloat power;
			
			public JSONStorableFloat vartrig = null;
			public int blendMode = BlendMode_OutIn;
			public int index;
			
			public Receiver(Breathe script, List<string> atoms, int index, bool rightSide)
			{
				this.index = index;
				
				script.CreateSpacer(rightSide);
				
				vartrigChooser = new JSONStorableStringChooser("receiver"+index, atoms, "", "Variable Trigger "+index);
				vartrigChooser.setCallbackFunction += (string atomId) => { 
					Atom atom = script.GetAtomById(atomId);
					JSONStorable storable = (atom != null) ? storable = atom.GetStorableByID("Trigger") : null;
					vartrig = (storable != null) ? storable.GetFloatJSONParam("value") : null;
				};
				script.CreateScrollablePopup(vartrigChooser, rightSide);
				script.RegisterStringChooser(vartrigChooser);
				
				List<string> modes = new List<string>() { "Out+In", "OutHold", "InHold", "OutInSmooth", "OutSmooth", "InSmooth" };
				blendModeChooser = new JSONStorableStringChooser("mode"+index, modes, modes[blendMode], "Mode "+index);
				blendModeChooser.setCallbackFunction += (string modeName) => {
					blendMode = modes.FindIndex((string entry) => { return entry == modeName; });
				};
				script.CreateScrollablePopup(blendModeChooser, rightSide);
				script.RegisterStringChooser(blendModeChooser);
				
				offsetStart = script.SetupSlider("Time Offset Start "+index, 0.0f, -0.5f, 0.5f, rightSide);
				offsetEnd = script.SetupSlider("Time Offset End "+index, 0.0f, -0.5f, 2.0f, rightSide);
				power = script.SetupSlider("Power "+index, 1.0f, 0.0f, 1.0f, rightSide);
			}
						
			public void Update(Breathe script)
			{
				if (vartrig == null)
					return;
				
				float os = offsetStart.val;
				float oe = offsetEnd.val;
				float t = 0.0f;
				switch (blendMode)
				{
					case BlendMode_OutIn:
						t = script.BlendOutIn(os, os, oe, oe);
						break;
					case BlendMode_OutHold:
						t = script.BlendOut(os, oe, 0.25f);
						break;
					case BlendMode_InHold:
						t = script.BlendIn(os, oe, 0.25f);
						break;
					case BlendMode_OutInSmooth:
						t = script.BlendOutInSmooth(os, oe);
						break;
					case BlendMode_OutSmooth:
						t = script.BlendOutSmooth(os, oe);
						break;
					case BlendMode_InSmooth:
						t = script.BlendInSmooth(os, oe);
						break;
				}
				
				float p = (script.breatheIntensity*0.5f+0.5f) * script.breathePower * power.val;
				p = Mathf.Clamp01(p);
				t = Mathf.SmoothStep(0.0f, p, t);
				vartrig.val = t;
			}
		}
		
		private struct BreathEntry
		{
			public string name;
			public float breatheOut;
			public float holdOut;
			public float breatheIn;
			public float holdInReference;
			public bool noseIn;
			public bool moan;
			public NamedAudioClip audioClip;			
			
			public BreathEntry(string name, float breatheOut, float holdOut, float breatheIn, float holdIn, bool noseIn, bool moan)
			{
				this.name = name;
				this.breatheOut = breatheOut;
				this.holdOut = holdOut;
				this.breatheIn = breatheIn;
				this.holdInReference = holdIn - breatheIn;
				this.noseIn = noseIn;
				this.moan = moan;
				
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
                
				SetupUI();
				
				if (chestController != null)
                {
                    chestController.jointRotationDriveSpring = chestSpring.val;
                    chestController.jointRotationDriveDamper = 1.0f;
                }
				
				List<BreathEntry> tmp = new List<BreathEntry>();
				for (int i=0; i<breathEntries.Length; ++i)
				{
					if (!breathEntries[i].moan)
						tmp.Add(breathEntries[i]);
				}
				breathEntriesNoMoan = tmp.ToArray();				
				
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
		
		private void SetupUI()
		{
			intensity = SetupSlider("Intensity", 0.0f, 0.0f, 1.0f, false);
			variance = SetupSlider("Variance", 0.2f, 0.0f, 0.5f, true);			
			speedAdjust = SetupSlider("SpeedAdjust", 0.0f, 0.0f, 1.0f, false);		
			rhythmAdapt = SetupSlider("RhythmAdapt", 0.9f, 0.0f, 0.9f, true);		
			stomachPower = SetupSlider("StomachPower", 0.3f, 0.0f, 1.0f, false);
			chestSpring = SetupSlider("ChestSpring", 90.0f, 0.0f, 250.0f, true);
			chestDriveMin = SetupSlider("ChestDriveMin", -3.0f, -20.0f, 20.0f, false);
			chestDriveMax = SetupSlider("ChestDriveMax", 8.0f, -20.0f, 20.0f, true);
			mouthOpenMin = SetupSlider("MouthOpenMin", 0.0f, -0.5f, 2.0f, false);
			mouthOpenMax = SetupSlider("MouthOpenMax", 0.8f, -0.5f, 2.0f, true);
			allowMoan = SetupToggle("AllowMoan", true, false);
			rhythmAdaptForceOnce = SetupToggle("RhythmAdaptForceOnce", false, true);
			
			List<string> filteredAtoms = FilterVarTriggers(GetAtomUIDs());
				
			bool rightSide = false;
			for (int i=0; i<receivers.Length; ++i)
			{
				receivers[i] = new Receiver(this, filteredAtoms, i+1, rightSide);
				rightSide = !rightSide;
			}
			
			SuperController.singleton.onAtomUIDsChangedHandlers += (List<string> atoms) => {
				atoms = FilterVarTriggers(atoms);
				for (int i=0; i<receivers.Length; ++i)
				{
					JSONStorableStringChooser vartrigChooser = receivers[i].vartrigChooser;
					if (!atoms.Contains(vartrigChooser.val))
						vartrigChooser.val = "";
					vartrigChooser.choices = atoms;
				}
			};
			
			CreateSpacer(false);
		}
		
		private List<string> FilterVarTriggers(List<string> atoms)
		{
			List<string> filtered = new List<string>();
			for (int i=0; i<atoms.Count; ++i)
			{
				Atom atom = GetAtomById(atoms[i]);
				if (atom != null && atom.type == "VariableTrigger")
					filtered.Add(atoms[i]);
			}
			return filtered;
		}
		
		private JSONStorableFloat SetupSlider(string name, float defaultValue, float minValue, float maxValue, bool rightSide)
		{
			JSONStorableFloat storable = new JSONStorableFloat(name, defaultValue, minValue, maxValue, true, true);
			storable.storeType = JSONStorableParam.StoreType.Full;
			CreateSlider(storable, rightSide);
			RegisterFloat(storable);
			return storable;
		}
		
		private JSONStorableBool SetupToggle(string name, bool defaultValue, bool rightSide)
		{
			JSONStorableBool storable = new JSONStorableBool(name, defaultValue);
			storable.storeType = JSONStorableParam.StoreType.Full;
			CreateToggle(storable, rightSide);
			RegisterBool(storable);
			return storable;
		}
		
        protected void FixedUpdate()
        {		
			float breatheMorph = 0.0f;
			breatheCycle += Time.fixedDeltaTime;
			if (breatheCycle >= breatheDuration)
			{
				BreathEntry[] entries = allowMoan.val ? breathEntries : breathEntriesNoMoan;
				
				breatheIntensity = intensity.val;
				breathePower = UnityEngine.Random.Range(0.8f, 1.2f);
				float v = variance.val * (entries.Length-1);	
				float bi = breatheIntensity * (entries.Length-1);
				float min = Mathf.Max(bi-v-1.0f, -0.5f);
				float max = Mathf.Min(bi+v+1.0f, entries.Length-0.5f);
				if (min < breatheIndex && max > breatheIndex)
					max -= 1.0f;			
				int index = Mathf.RoundToInt(UnityEngine.Random.Range(min, max));
				index = Mathf.Clamp(index, 0, entries.Length-2);
				breatheIndex = index < breatheIndex ? index : index+1;				
				breatheEntry = entries[breatheIndex];
				
				float holdTime = breatheEntry.holdInReference;
				holdTime *= 1.0f-speedAdjust.val;
				breatheDuration = breatheEntry.breatheIn + Mathf.Max(holdTime, 0.1f);
				
				if (breatheNeedInit || rhythmAdaptForceOnce.val)
				{
					breatheDurationAverage = breatheDuration;
					rhythmAdaptForceOnce.val = false;
				}
				else
				{
					breatheDurationAverage = Mathf.Lerp(breatheDuration, breatheDurationAverage, rhythmAdapt.val);
				}
					
				breatheDuration = Mathf.Max(breatheDurationAverage, breatheEntry.breatheIn+0.1f);								
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
			
			
			for (int i=0; i<receivers.Length; ++i)
			{
				receivers[i].Update(this);
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
		
		private float BlendOutInSmooth(float st, float bi)
		{
			if (breatheNeedInit)
				return 0.0f;
			
			float e = 0.001f;
			bi += breatheEntry.breatheIn;
			bi = Mathf.Clamp(bi, 3*e, breatheDuration-e);
			st = Mathf.Clamp(st, 0.0f, bi-3*e);			

			float a,b,c,d;
			a = st;
			b = (st+bi)*0.5f;
			c = b+e;
			d = bi;
			return BlendInternal(a,b,c,d);
		}
		
		private float BlendOutSmooth(float st, float bo)
		{
			if (breatheNeedInit)
				return 0.0f;
			
			float e = 0.001f;
			bo += breatheEntry.breatheOut;
			bo = Mathf.Clamp(bo, 3*e, breatheDuration-e);
			st = Mathf.Clamp(st, 0.0f, bo-3*e);			

			float a,b,c,d;
			a = st;
			b = (st+bo)*0.5f;
			c = b+e;
			d = bo;	
			return BlendInternal(a,b,c,d);
		}
		
		private float BlendInSmooth(float ho, float bi)
		{
			if (breatheNeedInit)
				return 0.0f;
			
			float e = 0.001f;
			ho += breatheEntry.holdOut;
			bi += breatheEntry.breatheIn;
			bi = Mathf.Clamp(bi, 3*e, breatheDuration-e);
			ho = Mathf.Clamp(ho, 0.0f, bi-3*e);			

			float a,b,c,d;
			a = ho;
			b = (ho+bi)*0.5f;
			c = b+e;
			d = bi;
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
