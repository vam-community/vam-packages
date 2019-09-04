// Gaze v1.1 by MacGruber. Fixes when calculating the target direction. Various improvements.
// 
// History:
// - Gaze v1.0 by MacGruber. Just minor changes.
// - Code ported to new style VaM plugin by VeeRifter 25/10/2018
// - Original VaM ScriptEngine plugin created by MacGruber 07/09/2018

using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

namespace MacGruber
{
    public class Gaze : MVRScript
    {	
        public override void Init()
		{
			try 
			{
				if (containingAtom.type != "Person")
				{
					SuperController.LogError($"Requires atom of type person, instead selected: {containingAtom.type}");
					return;
				}

				person = containingAtom;
				head = GetTransform(person, "headControl");
				eyeTarget = GetTransform(person, "eyeTargetControl");
				reference = GetTransform(person, "chestControl");
				
				maxAngleH = SetupSlider("Max Angle Horizontal", 50.0f, 10.0f, 130.0f, false);
				maxAngleV = SetupSlider("Max Angle Vertical", 30.0f, 10.0f, 70.0f, true);
								
				focusChangeDurationMin = SetupSlider("Focus Duration Min", 1f, 1f, 10f, true);
				focusChangeDurationMax = SetupSlider("Focus Duration Max", 4f, 1f, 10f, true);
				focusChangeDurationMin.setCallbackFunction = dmin => focusChangeDurationMax.SetVal(Mathf.Max(focusChangeDurationMax.val, dmin));				
				focusChangeDurationMax.setCallbackFunction = dmax => focusChangeDurationMin.SetVal(Mathf.Min(focusChangeDurationMin.val, dmax));

				focusAngleH = SetupSlider("Focus Angle Horizontal", 4f, 1f, 15f, false);				
				focusAngleV = SetupSlider("Focus Angle Vertical", 6f, 1f, 15f, false);		
				gazeDuration = SetupSlider("Gaze Duration", 0.7f, 0f, 3f, false);
				
				rollChangeDurationMin = SetupSlider("Roll Duration Min", 2f, 1f, 10f, true);
				rollChangeDurationMax = SetupSlider("Roll Duration Max", 6f, 1f, 10f, true);
				rollChangeDurationMin.setCallbackFunction = rmin => rollChangeDurationMax.SetVal(Mathf.Max(rollChangeDurationMax.val, rmin));
				rollChangeDurationMax.setCallbackFunction = rmax => rollChangeDurationMin.SetVal(Mathf.Min(rollChangeDurationMin.val, rmax));

				rollAngleMax = SetupSlider("Roll Angle Max", 6f, 1f, 15f, false);
				eyePositionOffset = SetupSlider("Eye Position Offset", 0.1f, -0.2f, 0.2f, false);
				eyeAngleOffset = SetupSlider("Eye Angle Offset", -5.0f, -20.0f, 20.0f, false);
				lookAtOffset = SetupSlider("LookAt Position Offset", 0.0f, -0.2f, 0.2f, true);
				
				lookAtEyeTarget = SetupToggle("LookAt EyeTarget", false, true);
			}
			catch (System.Exception e) {
				SuperController.LogError("Exception caught: " + e);
			}
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
            if (head == null || reference == null)
                return;

            // compute horizontal and vertical angles
			Transform lookAtTarget = CameraTarget.centerTarget.transform;
			if (lookAtEyeTarget.val)
				lookAtTarget = eyeTarget;
			Vector3 lookAtPosition = lookAtTarget.TransformPoint(lookAtOffset.val * Vector3.up);
            Vector3 actualDir = reference.InverseTransformDirection(head.forward);
			Vector3 eyeCenterPosition = head.TransformPoint(eyePositionOffset.val * Vector3.up);
            Vector3 targetDir = lookAtPosition - eyeCenterPosition;
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
			
			targetV += eyeAngleOffset.val * Mathf.Deg2Rad;;

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
			float mah = maxAngleH.val * Mathf.Deg2Rad;
			float mav = maxAngleV.val * Mathf.Deg2Rad;
            targetH = Mathf.Clamp(targetH, -mah, mah);
            targetV = Mathf.Clamp(targetV, -mav, mav);

            actualH = Mathf.SmoothDamp(actualH, targetH, ref velocityH, gazeDuration.val, Mathf.Infinity, Time.fixedDeltaTime);
            actualV = Mathf.SmoothDamp(actualV, targetV, ref velocityV, gazeDuration.val, Mathf.Infinity, Time.fixedDeltaTime);

            // recombine
            actualDir = RecombineDirection(actualH, actualV);
            targetDir = RecombineDirection(targetH, targetV);
            actualDir = reference.TransformDirection(actualDir);
            head.transform.LookAt(head.transform.position + actualDir);
			
            // apply roll
            rollChangeClock += rollChangeSpeed * Time.fixedDeltaTime;
            if (rollChangeClock >= 1.0f)
            {
                rollChangeSpeed = 1.0f / Random.Range(rollChangeDurationMin.val, rollChangeDurationMax.val);
                rollChangeClock = 0.0f;
                rollPrev = rollNext;
                rollNext = Random.Range(-rollAngleMax.val, rollAngleMax.val) * Mathf.Deg2Rad;
            }
            t = Mathf.SmoothStep(0.0f, 1.0f, rollChangeClock);
            float roll = Mathf.Lerp(rollPrev, rollNext, t);
            Vector3 eulerAngles = head.transform.localEulerAngles;
            eulerAngles.z = roll * Mathf.Rad2Deg;
            head.transform.localEulerAngles = eulerAngles;

            // compute angle
            currentAngle = Vector3.Angle(actualDir, targetDir);
        }

        public Transform GetTransform(Atom atom, string controlID)
        {
            return atom.GetStorableByID(controlID).transform;
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

        private Atom person;  
        private Transform head;		
        private Transform reference;
		private Transform eyeTarget;

        // tweak parameters
		private JSONStorableFloat maxAngleH;
		private JSONStorableFloat maxAngleV;
		private JSONStorableFloat gazeDuration;
		private JSONStorableFloat focusChangeDurationMin;
		private JSONStorableFloat focusChangeDurationMax;
		private JSONStorableFloat focusAngleV;
		private JSONStorableFloat focusAngleH;
		private JSONStorableFloat rollChangeDurationMin;
		private JSONStorableFloat rollChangeDurationMax;
		private JSONStorableFloat rollAngleMax;
		private JSONStorableFloat eyePositionOffset;
		private JSONStorableFloat eyeAngleOffset;
		private JSONStorableFloat lookAtOffset;
		private JSONStorableBool lookAtEyeTarget;

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
    }
}
