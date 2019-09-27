using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

namespace VariousScientistsPlugin {
    public class LookAtMePlugin : MVRScript {

 
        private Transform LookTargetL;
        private Atom person;
        private JSONStorable eyes;
        private FreeControllerV3 eyestarget;
        private Vector3 CamVector;
        private Vector3 ActualOffset;
        protected JSONStorableFloat TimeIntervalMax;
        protected JSONStorableFloat TimeIntervalMin;
        protected JSONStorableFloat XRange;
        protected JSONStorableFloat YRange;
        protected JSONStorableFloat ZRange;
        protected JSONStorableFloat LookAtMeOffset;
        protected JSONStorableFloat LookAtMeChance;
        private float lookchance;
        protected JSONStorableFloat negXRange;
        protected JSONStorableFloat negYRange;
        protected JSONStorableFloat negZRange;
        private float ActualRangeX;
        private float ActualRangeY;
        private float ActualRangeZ;
        private float time = 1;
        private bool lookatme;


        public override void Init() {
			try {

                if (containingAtom.type != "Person")
                {
                    SuperController.LogError($"Please add this plugin to your target Person Atom, not '{containingAtom.type}'");
                    return;
                }

                //V1.0 initial release.
                //V1.1 fixed bug where atom would look opposite direction when player head turns.
                //V1.2 fixed error in "look At Me Time Length".
                //V1.3 added independent axis look ranges and "Look At Me Chance"

                pluginLabelJSON.val = "Look At Me - VariousScientists V1.3";
                person = containingAtom;
                eyes = person.GetStorableByID("Eyes");
                eyestarget = (FreeControllerV3)person.GetStorableByID("eyeTargetControl");

                TimeIntervalMax = new JSONStorableFloat("Time Length Max", 3f, 0f, 6f, true, true);
                RegisterFloat(TimeIntervalMax);
                CreateSlider(TimeIntervalMax, true);

                TimeIntervalMin = new JSONStorableFloat("Time Length Min", 1f, 0f, 6f, true, true);
                RegisterFloat(TimeIntervalMin);
                CreateSlider(TimeIntervalMin, true);

                XRange = new JSONStorableFloat("X Look Range Positive", 0.1f, 0f, 0.5f, true, true);
                RegisterFloat(XRange);
                CreateSlider(XRange, false);

                negXRange = new JSONStorableFloat("X Look Range Negative", -0.1f, -0.5f, 0f, true, true);
                RegisterFloat(negXRange);
                CreateSlider(negXRange, false);

                YRange = new JSONStorableFloat("Y Look Range Positive", 0.1f, 0f, 0.5f, true, true);
                RegisterFloat(YRange);
                CreateSlider(YRange, false);

                negYRange = new JSONStorableFloat("Y Look Range Negative", -0.1f, -0.5f, 0f, true, true);
                RegisterFloat(negYRange);
                CreateSlider(negYRange, false);

                ZRange = new JSONStorableFloat("Z Look Range Positive", 0.1f, 0f, 0.5f, true, true);
                RegisterFloat(ZRange);
                CreateSlider(ZRange, false);

                negZRange = new JSONStorableFloat("Z Look Range Negative", -0.1f, -0.5f, 0f, true, true);
                RegisterFloat(negZRange);
                CreateSlider(negZRange, false);

                LookAtMeOffset = new JSONStorableFloat("Look At Me Time Length", 0f, 0f, 5f, true, true);
                RegisterFloat(LookAtMeOffset);
                CreateSlider(LookAtMeOffset, true);

                LookAtMeChance = new JSONStorableFloat("Look At Me Chance", 0.5f, 0f, 1f, true, true);
                RegisterFloat(LookAtMeChance);
                CreateSlider(LookAtMeChance, true);
                }
			catch (Exception e) {
				SuperController.LogError("Exception caught: " + e);
			}
		}


		void Start() {
			try {
                               
            }
			catch (Exception e) {
				SuperController.LogError("Exception caught: " + e);
			}
		}
        protected void UpdateParamsLookAway()
        {
            ActualRangeX = UnityEngine.Random.Range(XRange.val,negXRange.val);
            ActualRangeY = UnityEngine.Random.Range(YRange.val,negYRange.val);
            ActualRangeZ = UnityEngine.Random.Range(ZRange.val,negZRange.val);
            ActualOffset = new Vector3(ActualRangeX, ActualRangeY, ActualRangeZ);
            lookatme = UnityEngine.Random.value < lookchance;
        }

        protected void UpdateParamsLookAtMe()
        {
            ActualRangeX = 0;
            ActualRangeY = 0;
            ActualRangeZ = 0;
            ActualOffset = new Vector3(ActualRangeX, ActualRangeY, ActualRangeZ);
            lookatme = UnityEngine.Random.value < lookchance;
        }
      
        void Update() {
			try {
                
                lookchance = LookAtMeChance.val;
                CamVector = CameraTarget.centerTarget.transform.forward / 10;
                LookTargetL = CameraTarget.centerTarget.transform;

                if (lookatme == true)
                { eyestarget.transform.position = LookTargetL.position - CamVector; }
                else
                { eyestarget.transform.position = LookTargetL.position + ActualOffset - (CamVector); }

                    time -= Time.deltaTime;
                    if (time <= 0f)
                    {
                        if (lookatme == true)
                        {
                            time = UnityEngine.Random.Range(TimeIntervalMin.val, TimeIntervalMax.val) + LookAtMeOffset.val;
                            UpdateParamsLookAtMe();
                        }
                        else
                        {
                            time = UnityEngine.Random.Range(TimeIntervalMin.val, TimeIntervalMax.val);
                            UpdateParamsLookAway();
                        }
                    }
                

            }
			catch (Exception e) {
				SuperController.LogError("Exception caught: " + e);
			}
		}

		void FixedUpdate() {
			try {

                }
			catch (Exception e) {
				SuperController.LogError("Exception caught: " + e);
			}
		}

		void OnDestroy() {
		}

	}
}