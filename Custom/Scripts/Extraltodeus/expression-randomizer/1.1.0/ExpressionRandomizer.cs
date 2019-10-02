using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJSON;
using System.Threading;
using System.Text.RegularExpressions;

namespace extraltodeuslExpRandPlugin {
	public class ExpressionRandomizer : MVRScript {
        string[] bodyRegion =
        {
            "Arms",
            "Body",
            "Chest",
            "Hip",
            "Legs",
            "Neck",
            "Feet",
            "Waist"
        };

        string[] poseRegion =
        {
            "Pose",
            "Expressions"
        };

        Dictionary<string, float>  initialMorphValues   = new Dictionary<string, float>();
        Dictionary<string, float>  newMorphValues       = new Dictionary<string, float>();
        Dictionary<string, float>  CurrentMorphsValues  = new Dictionary<string, float>();
        Dictionary<string, UIDynamicToggle> toggles = new Dictionary<string, UIDynamicToggle>();
        Dictionary<string, UIDynamicButton> buttons = new Dictionary<string, UIDynamicButton>();
        Dictionary<string, string> toggleRelations = new Dictionary<string, string>();

        protected JSONStorableFloat minSlider;
        protected JSONStorableFloat maxSlider;
        protected JSONStorableFloat multiSlider;
        protected JSONStorableFloat animLengthSlider;
        protected JSONStorableFloat animWaitSlider;
        protected JSONStorableBool  play;
        protected float timer;
        protected float forceTimer;

        protected void UpdateRandomParams()
        {
            JSONStorable geometry = containingAtom.GetStorableByID("geometry");
            DAZCharacterSelector character = geometry as DAZCharacterSelector;
            GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;

            // define the random values to switch to
            morphControl.GetMorphDisplayNames().ForEach((name) =>
            {
                DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                if (toggles.ContainsKey(name) == false)
                {
                    return;
                }

                if (toggles[name].toggle.isOn)
                {
                    if (morph.animatable == false)
                    {
                        float valeur = UnityEngine.Random.Range(minSlider.val, maxSlider.val) * multiSlider.val;
                        newMorphValues[name] = valeur;
                    }
                }
            });
        }

        protected void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = animWaitSlider.val;
                UpdateRandomParams();
            }
        }

        protected void FixedUpdate()
        {
            if (toggles["Play"].toggle.isOn)
            {
                JSONStorable geometry = containingAtom.GetStorableByID("geometry");
                DAZCharacterSelector character = geometry as DAZCharacterSelector;
                GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;

                // morph progressively every morphs to their new values
                morphControl.GetMorphDisplayNames().ForEach((name) =>
                {
                    DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                    if (toggles.ContainsKey(name) && toggles[name].toggle.isOn)
                    {
                        if (morph.animatable == false)
                        {
                            float valeur = Mathf.Lerp(CurrentMorphsValues[name], newMorphValues[name], Time.deltaTime * animLengthSlider.val);
                            morph.morphValue = valeur;
                        }
                    }
                });
                UpdateCurrentMorphs();
            }    
        }

        private void UpdateInitialMorphs()
        {
            JSONStorable geometry = containingAtom.GetStorableByID("geometry");
            DAZCharacterSelector character = geometry as DAZCharacterSelector;
            GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;
            morphControl.GetMorphDisplayNames().ForEach((name) =>
            {
                initialMorphValues[name] = morphControl.GetMorphByDisplayName(name).morphValue;
            });
        }

        private void UpdateNewMorphs()
        {
            JSONStorable geometry = containingAtom.GetStorableByID("geometry");
            DAZCharacterSelector character = geometry as DAZCharacterSelector;
            GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;
            morphControl.GetMorphDisplayNames().ForEach((name) =>
            {
                if (toggles.ContainsKey(name))
                    newMorphValues[name] = morphControl.GetMorphByDisplayName(name).morphValue;
            });
        }

        private void UpdateCurrentMorphs()
        {
            JSONStorable geometry = containingAtom.GetStorableByID("geometry");
            DAZCharacterSelector character = geometry as DAZCharacterSelector;
            GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;
            morphControl.GetMorphDisplayNames().ForEach((name) =>
            {
                    CurrentMorphsValues[name] = morphControl.GetMorphByDisplayName(name).morphValue;
            });
        }

        private void ResetMorphs()
        {
            JSONStorable geometry = containingAtom.GetStorableByID("geometry");
            DAZCharacterSelector character = geometry as DAZCharacterSelector;
            GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;

            morphControl.GetMorphDisplayNames().ForEach((name) =>
            {
                if (toggleRelations.ContainsKey(name))
                {
                    DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                    morph.morphValue = initialMorphValues[name];
                }
            });
        }

        public override void Init() {
			try {
                UpdateInitialMorphs();
                UpdateNewMorphs();
                UpdateCurrentMorphs();
                
                #region Sliders
                minSlider = new JSONStorableFloat("Minimum value", -0.3f, -1f, 1.0f, true);
                minSlider.storeType = JSONStorableParam.StoreType.Full;
                RegisterFloat(minSlider);
                CreateSlider(minSlider, false);

                maxSlider = new JSONStorableFloat("Maximum value", 0.3f, -1f, 1.0f, true);
                maxSlider.storeType = JSONStorableParam.StoreType.Full;
                RegisterFloat(maxSlider);
                CreateSlider(maxSlider, false);

                multiSlider = new JSONStorableFloat("Multiplier", 1f, 0f, 2f, true);
                multiSlider.storeType = JSONStorableParam.StoreType.Full;
                RegisterFloat(multiSlider);
                CreateSlider(multiSlider, false);
                #endregion

                #region Region Buttons Preparation
                JSONStorable geometry = containingAtom.GetStorableByID("geometry");
                DAZCharacterSelector character = geometry as DAZCharacterSelector;
                GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;

                HashSet<string> regions = new HashSet<string>();
                HashSet<string> LastRegions = new HashSet<string>();
                Dictionary<string, string> temporaryToggles = new Dictionary<string, string>();

                JSONStorableBool playingBool = new JSONStorableBool("Play", false);
                playingBool.storeType = JSONStorableParam.StoreType.Full;
                RegisterBool(playingBool);
                toggles["Play"] = CreateToggle((playingBool), false);

                morphControl.GetMorphDisplayNames().ForEach((name) =>
                {
                    DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                    regions.Add(morph.region);
                    
                    if (
                         poseRegion.Any((str) => morph.region.Contains(str)) &&
                        !bodyRegion.Any((str) => morph.region.Contains(str)) &&
                        !morph.region.Contains("Reloaded")
                    )
                    {
                        string[] posePaths = Regex.Split(morph.region, "/");
                        string morphUpperRegion = "";

                        foreach (string posePath in posePaths)
                        {
                            morphUpperRegion = posePath;
                        }

                        LastRegions.Add(morphUpperRegion);
                        toggleRelations[name] = morphUpperRegion;
                        temporaryToggles[name] = morphUpperRegion + "/" + name;
                    }
                });

                #region Region Helper Buttons
                UIDynamicButton selectAll = CreateButton("Select All", true);
                selectAll.button.onClick.AddListener(delegate () {
                    toggles.Values.ToList().ForEach((toggle) =>
                    {
                        toggle.toggle.isOn = true;
                    });
                });
                UIDynamicButton selectNone = CreateButton("Select None", true);
                selectNone.button.onClick.AddListener(delegate () {
                    toggles.Values.ToList().ForEach((toggle) =>
                    {
                        toggle.toggle.isOn = false;
                    });
                });

                foreach (string LastRegion in LastRegions)
                {
                    buttons[LastRegion] = CreateButton(LastRegion, true);
                    buttons[LastRegion].button.onClick.AddListener(delegate () {
                        ResetMorphs();
                        foreach (KeyValuePair<string, UIDynamicToggle> entry in toggles)
                        {
                            if (toggleRelations.ContainsKey(entry.Key))
                            {
                                entry.Value.toggle.isOn = (toggleRelations[entry.Key] == LastRegion);
                            }
                        }
                    });
                }
                #endregion

                #region Region checkbox generation
                foreach (KeyValuePair<string, string> entry in temporaryToggles)
                {
                    JSONStorableBool checkBoxTick = new JSONStorableBool(entry.Value, false);
                    checkBoxTick.storeType = JSONStorableParam.StoreType.Full;
                    RegisterBool(checkBoxTick);

                    toggles[entry.Key] = CreateToggle(checkBoxTick, true);
                }
                #endregion

                #endregion

                //CreateSpacer();

                #region SetAsDef button
                UIDynamicButton setasdefButton = CreateButton("Set current as default", false);
                setasdefButton.button.onClick.AddListener(delegate ()
                {
                    UpdateInitialMorphs();
                });
                #endregion

                UIDynamicButton animatableButton = CreateButton("Clear Animatable", false);
                animatableButton.button.onClick.AddListener(delegate ()
                {
                    morphControl.GetMorphDisplayNames().ForEach((name) =>
                    {
                        DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                        morph.animatable = false;
                    });
                });

                #region Reset button
                UIDynamicButton resetButton = CreateButton("Reset", false);
                resetButton.button.onClick.AddListener(delegate ()
                {
                    ResetMorphs();
                });
                #endregion

                animWaitSlider = new JSONStorableFloat("Loop length", 1f, 0.1f, 20f, true);
                animWaitSlider.storeType = JSONStorableParam.StoreType.Full;
                RegisterFloat(animWaitSlider);
                CreateSlider(animWaitSlider, false);

                animLengthSlider = new JSONStorableFloat("Morphing speed", 1.0f, 0.1f, 20f, true);
                animLengthSlider.storeType = JSONStorableParam.StoreType.Full;
                RegisterFloat(animLengthSlider);
                CreateSlider(animLengthSlider, false);
            }
            catch (Exception e) {
				SuperController.LogError("Exception caught: " + e);
			}
		}

        void Start()
        {
            timer = 0f;
            UpdateInitialMorphs();
            UpdateNewMorphs();
        }
	}
}