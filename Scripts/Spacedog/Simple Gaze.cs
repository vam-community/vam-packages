// 07/09/2018 MacGruber Original: VaM ScriptEngine plugin created.
// 25/10/2018 VeeRifter Mod V1.0: McGruber code ported to new style standalone VaM plugin with eight user adjustable settings.
// 29/11/2018 VeeRifter Mod V2.0: Controls for horizontal and vertical gaze offsets, plus selectabe gaze target option added.
// Namespace change & public function calls added for compatability with Dollmaster (all credit to VAMDeluxe)
// 30/11/2018 VeeRifter Mod V2.1: Minor bugfix release.
// 21/12/2018 VeeRifter Mod V3.0: Fixed a bug whereby the gaze target selection was not being restored when a saved .json was reloaded.
// 05/02/2019 Physis Mod V1.0 Added activation range so the gaze can ignore eye saccades caused by other plugins
// Incorporated the two range tweaks to focus horizontal and vertical sliders, as per MaxRupert Modelers variant of my Mod V1.0 script.
// Added motion to the chest and pelvis joints to swivel naturally about the waist when the head turns.
// This action is enabled by default but can be turned off in the plugin settings.
// When enabled, the head horizontal rotation limit is increased from 90 to 140 degrees.
// 28/07/19 Spacedog Mod V1.0: Adjusted defaults, increased vertical look range, and set referance node to abdomen2Control to avoid unexpected head movement with hip rotated.
// Disabled head roll to fix incorrect head movement when root node is rotated.

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

namespace Spacedog
{
    public class SimpleGaze : MVRScript
    {
	
        public override void Init() {
        try {
				if (containingAtom.type != "Person")
				{
					SuperController.LogError($"This plugin is for use with 'Person' atom only, not '{containingAtom.type}'");
					return;
				}
				pluginLabelJSON.val = "Simple Gaze Mod V1.0";
				person = containingAtom;

				eyes = person.GetStorableByID("Eyes");
				if (eyes != null) {
					lookMode = eyes.GetStringChooserJSONParam("lookMode");
					if (lookMode == null) {
						SuperController.LogError("Could not find lookMode param on eyeTargetControl");
					}
				}
                personEyeTargetControl = person.GetStorableByID("eyeTargetControl") as FreeControllerV3;
                personEyeTarget = personEyeTargetControl.transform;
                playerTarget = CameraTarget.centerTarget.transform;
				//Initialise gazeTarget from current lookMode value
				if(lookMode.val == "Player") {
				gazeTarget = playerTarget;					
				} else {
					gazeTarget = personEyeTarget;
				}

				
				lookAtPosition = gazeTarget.TransformPoint(gazeOffset);
				
				chestControl = person.GetStorableByID( "chestControl") as FreeControllerV3;					
				pelvisControl = person.GetStorableByID( "pelvisControl") as FreeControllerV3;
				head = person.GetStorableByID( "headControl").transform;
				reference = person.GetStorableByID( "abdomen2Control").transform;	

                activationDistance = new JSONStorableFloat("Activation Range (ignore eye saccades)", 0.1f, 0f, 10f, true, true);
				RegisterFloat(activationDistance);
				CreateSlider(activationDistance, false);			
				
                gazeDuration = new JSONStorableFloat("Gaze Duration", 0.7f, 0f, 5f, true, true);
				RegisterFloat(gazeDuration);
				CreateSlider(gazeDuration, false);			
				
				focusChangeDurationMin = new JSONStorableFloat("Focus Change Duration Minimum", 1f, dmin => focusChangeDurationMax.SetVal(Mathf.Max(focusChangeDurationMax.val, dmin)), 1f, 10f, true,true);
				RegisterFloat(focusChangeDurationMin);
				CreateSlider(focusChangeDurationMin, true);

				focusChangeDurationMax = new JSONStorableFloat("Focus Change Duration Maximum", 4f, dmax => focusChangeDurationMin.SetVal(Mathf.Min(focusChangeDurationMin.val, dmax)), 1f, 10f, true, true);
				RegisterFloat(focusChangeDurationMax);
				CreateSlider(focusChangeDurationMax, true);	

				focusAngleH = new JSONStorableFloat("Focus Angle Horizontal", 3f, 0f, 40f, true, true);
				RegisterFloat(focusAngleH);
				CreateSlider(focusAngleH, false);	
				
				focusAngleV = new JSONStorableFloat("Focus Angle Vertical", 3f, 0f, 40f, true, true);
				RegisterFloat(focusAngleV);
				CreateSlider(focusAngleV, false);				
				
				/*rollChangeDurationMin = new JSONStorableFloat("Roll Change Duration Minimum", 2f, rmin => rollChangeDurationMax.SetVal(Mathf.Max(rollChangeDurationMax.val, rmin)), 1f, 10f, true, true);
				RegisterFloat(rollChangeDurationMin);
				CreateSlider(rollChangeDurationMin, true);

				rollChangeDurationMax = new JSONStorableFloat("Roll Change Duration Maximum", 6f, rmax => rollChangeDurationMin.SetVal(Mathf.Min(rollChangeDurationMin.val, rmax)), 1f, 10f, true, true);
				RegisterFloat(rollChangeDurationMax);
				CreateSlider(rollChangeDurationMax, true);		

				rollAngleMax = new JSONStorableFloat("Roll Angle Maximum", 1f, 0f, 40f, true, true);
				RegisterFloat(rollAngleMax);
				CreateSlider(rollAngleMax, false);*/
				
				offsetV = new JSONStorableFloat("Vertical Offset Angle", -5.0f, -30.0f, 30.0f, true, true);
				RegisterFloat(offsetV);
				CreateSlider(offsetV, false);				

				offsetH = new JSONStorableFloat("Horizontal Offset Angle", 0.0f, -30.0f, 30.0f, true, true);
				RegisterFloat(offsetH);
				CreateSlider(offsetH, true);
				
				List<string> choices = new List<string>();
				choices.Add("Player");
				choices.Add("Target");
				JSONStorableStringChooser chooser = new JSONStorableStringChooser("Gaze Target Choice", choices, lookMode.val, "Gaze Follows", setGazeTarget);
				UIDynamicPopup udp = CreatePopup(chooser, true);

                swivelEnabled = new JSONStorableBool("Enable Body Swivel Action", true);
                swivelEnabled.storeType = JSONStorableParam.StoreType.Full;
                RegisterBool(swivelEnabled);
                CreateToggle(swivelEnabled, false);	
				
			}
			catch (System.Exception e) {
				SuperController.LogError("Exception caught: " + e);
			}
		}
        protected Vector3 lookAtPosition;

        protected void FixedUpdate()
        {
            if (gazeTarget == null || head == null || reference == null)
                return;

            // compute horizontal and vertical angles
			gazeOffset = offsetV.val * Mathf.Deg2Rad * Vector3.up + offsetH.val * Mathf.Deg2Rad * Vector3.left;

            // physis mod: only update lookAtPosition if it has changed significantly (so gaze doesn't follow eye sacades)
            if (Vector3.Distance(lookAtPosition, gazeTarget.TransformPoint(gazeOffset)) > activationDistance.val)
            {
                lookAtPosition = gazeTarget.TransformPoint(gazeOffset);
            }

            //  lookAtPosition = gazeTarget.TransformPoint(gazeOffset);

            Vector3 actualDir = reference.InverseTransformDirection(head.forward);			
            Vector3 targetDir = lookAtPosition - head.position;
            targetDir.Normalize();
            targetDir = reference.InverseTransformDirection(targetDir);
            Vector2 actualDirH = new Vector2(actualDir.x, actualDir.z);
            Vector2 targetDirH = new Vector2(targetDir.x, targetDir.z);
            Vector2 actualDirV = new Vector2(actualDirH.magnitude, actualDir.y);
            Vector2 targetDirV = new Vector2(targetDirH.magnitude, targetDir.y);
            actualDirH.Normalize();
            targetDirH.Normalize();
            actualDirV.Normalize();
            targetDirV.Normalize();
            float actualH = Mathf.Atan2(actualDirH.x, actualDirH.y);
            float targetH = Mathf.Atan2(targetDirH.x, targetDirH.y);
            float actualV = Mathf.Atan2(actualDirV.y, actualDirV.x);
            float targetV = Mathf.Atan2(targetDirV.y, targetDirV.x);

            // apply focus
            focusChangeClock += focusChangeSpeed * Time.fixedDeltaTime;
            if (focusChangeClock >= 1.0f)
            {
                focusChangeSpeed = 1.0f / Random.Range(focusChangeDurationMin.val, focusChangeDurationMax.val);
                focusChangeClock = 0.0f;
                focusPrev = focusNext;
                focusNext = Random.insideUnitCircle;
            }
            float t = Mathf.SmoothStep(0.0f, 1.0f, focusChangeClock);
            targetH += Mathf.Lerp(focusPrev.x, focusNext.x, t) * focusAngleH.val * Mathf.Deg2Rad;
            targetV += Mathf.Lerp(focusPrev.y, focusNext.y, t) * focusAngleV.val * Mathf.Deg2Rad;

            // adjust angles
            targetH = Mathf.Clamp(targetH, -maxAngleH, maxAngleH);
            targetV = Mathf.Clamp(targetV, -maxAngleV, maxAngleV);
            actualH = Mathf.SmoothDamp(actualH, targetH, ref velocityH, gazeDuration.val, Mathf.Infinity, Time.fixedDeltaTime);
            actualV = Mathf.SmoothDamp(actualV, targetV, ref velocityV, gazeDuration.val, Mathf.Infinity, Time.fixedDeltaTime);

            // recombine
            actualDir = RecombineDirection(actualH, actualV);
            targetDir = RecombineDirection(targetH, targetV);
            actualDir = reference.TransformDirection(actualDir);		
			
            head.transform.LookAt(head.transform.position + actualDir);

            // apply roll
            /*rollChangeClock += rollChangeSpeed * Time.fixedDeltaTime;
            if (rollChangeClock >= 1.0f)
            {
                rollChangeSpeed = 1.0f / Random.Range(rollChangeDurationMin.val, rollChangeDurationMax.val);
                rollChangeClock = 0.0f;
                rollPrev = rollNext;
                rollNext = Random.Range(-(rollAngleMax.val * Mathf.Deg2Rad), rollAngleMax.val * Mathf.Deg2Rad);
            }
            t = Mathf.SmoothStep(0.0f, 1.0f, rollChangeClock);
            float roll = Mathf.Lerp(rollPrev, rollNext, t);
            Vector3 eulerAngles = head.transform.localEulerAngles;
            eulerAngles.z = roll * Mathf.Rad2Deg;
            head.transform.localEulerAngles = eulerAngles;*/

            // compute angle
            currentAngle = Vector3.Angle(actualDir, targetDir);

			// Set chest and pelvis joint swivel.
            if (swivelEnabled.val && chestControl != null && pelvisControl != null)
			{
				maxAngleH = 140.0f * Mathf.Deg2Rad;
				float driveYTarget = Mathf.Clamp(actualH * -10.0f, -20.0f, 20.0f);
				chestControl.jointRotationDriveYTarget = driveYTarget;
				driveYTarget = Mathf.Clamp(actualH * -7.5f, -15.0f, 15.0f);		
				pelvisControl.jointRotationDriveYTarget = driveYTarget;
	        }
			else
			{
				maxAngleH = 90.0f * Mathf.Deg2Rad;
			}
        }
	
        // Set a reference object to determine where "forward" is.
        public void SetReference(Transform transform)
        {
            reference = transform;
        }

        // Set a reference object to determine where "forward" is.
        public void SetReference(string atomID, string controlID)
        {
            reference = null;
            Atom atom = GetAtomById(atomID);
            if (atom == null)
            {
                SuperController.LogError("[GazeController] Atom '{0}' not found. " + atomID);
                return;
            }

            JSONStorable storable = atom.GetStorableByID(controlID);
            if (storable == null)
            {
                SuperController.LogError("[GazeController] Control '{0}/{1}' not found. " + atomID + " " + controlID);
                return;
            }

            reference = storable.transform;
        }
	
        // Set player as target to look at.
        public void SetLookAtPlayer()
        {
            gazeTarget = CameraTarget.centerTarget.transform;
            gazeOffset = Vector3.zero;
        }

        // Set player as target to look at.
        public void SetLookAtPlayer(Vector3 offset)
        {
            gazeTarget = CameraTarget.centerTarget.transform;
            gazeOffset = offset;
        }

        // Set transform as target to look at.
        public void SetLookAt(Transform transform)
        {
            gazeTarget = transform;
            gazeOffset = Vector3.zero;
        }

        // Set transform as target to look at.
        public void SetLookAt(Transform transform, Vector3 offset)
        {
            gazeTarget = transform;
            gazeOffset = offset;
        }

        // Set maximum offset angle (in degrees) from directly looking at the target during idle animations.
		//Values closer to 0 mean the character will be more focused on the target.
        public void SetFocusAngles(JSONStorableFloat angleH, JSONStorableFloat angleV)
        {
            focusAngleH.val = angleH.val * Mathf.Deg2Rad;
            focusAngleV.val = angleV.val * Mathf.Deg2Rad;
        }

        // Set min/max duration between random focus point changes.
        public void SetFocusChangeDuration(float min, float max)
        {
            focusChangeDurationMin.val = Mathf.Clamp(min, 0.01f, max);
            focusChangeDurationMax.val = Mathf.Max(max, focusChangeDurationMin.val);
        }

        // Set how fast the gaze adapts to a new target position.
        public void SetGazeDuration(float duration)
        {
            gazeDuration.val = Mathf.Max(duration, 0.001f);
        }

        // Current angle (in degrees) between look direction and target direction.
        public float GetCurrentAngle()
        {
            return currentAngle;
        }

        public void ClearLookAt()
        {
            gazeTarget = null;
        }

        private Vector3 RecombineDirection(float angleH, float angleV)
        {
            float cosV = Mathf.Cos(angleV);
            return new Vector3(
                Mathf.Sin(angleH) * cosV,
                Mathf.Sin(angleV),
                Mathf.Cos(angleH) * cosV
            );
        }

	    // Set gaze vertical offset.
        public void SetGazeOffsetV(float offset)
        {
			offsetV.val = offset;
		}	
		
	    // Set gaze horizontalal offset.
        public void SetGazeOffsetH(float offset)
        {
			offsetH.val = offset;
		}		
		
		// Chooser callback to set gaze target
		public void setGazeTarget(string choice) {
			if (lookMode != null && gazeTarget != null) {
				lookMode.val = choice;
				if (choice == "Player") {
					gazeTarget = playerTarget;
				} else {
					gazeTarget = personEyeTarget;
				}
			}
		}	
		
		// Can be called from FixedUpdate() of another plugin using this script.
        public void OnFixedUpdate()	{
			FixedUpdate();
		}	

        private Atom person;
        private FreeControllerV3 personEyeTargetControl;
        private FreeControllerV3 chestControl;			
	    private FreeControllerV3 pelvisControl;		
        private Transform playerTarget;
        private Transform personEyeTarget;
        private Transform gazeTarget;
        private Vector3 gazeOffset;
        private Transform head;
        private Transform reference;

        // tweak parameters
        protected JSONStorableFloat activationDistance;
		protected JSONStorableFloat gazeDuration;
		protected JSONStorableFloat offsetV;
		protected JSONStorableFloat offsetH;		
		protected JSONStorableFloat focusChangeDurationMin;
		protected JSONStorableFloat focusChangeDurationMax;
		protected JSONStorableFloat focusAngleV;
		protected JSONStorableFloat focusAngleH;
		protected JSONStorableFloat rollChangeDurationMin;
		protected JSONStorableFloat rollChangeDurationMax;
		protected JSONStorableFloat rollAngleMax;
		protected JSONStorableStringChooser lookMode;
		protected JSONStorableBool swivelEnabled;		
		protected JSONStorable eyes;		
		
        // runtime data
        private float velocityH = 0.0f;
        private float velocityV = 0.0f;
        private float focusChangeClock = 1.0f;
        private float focusChangeSpeed = 1.0f;
        private Vector2 focusNext = Vector2.zero;
        private Vector2 focusPrev = Vector2.zero;
        private float rollNext = 0.0f;
        private float rollPrev = 0.0f;
        private float rollChangeClock = 1.0f;
        private float rollChangeSpeed = 1.0f;
        private float currentAngle = 0.0f;
        private float maxAngleH = 90.0f * Mathf.Deg2Rad;
        private float maxAngleV = 140.0f * Mathf.Deg2Rad;
    }
}
