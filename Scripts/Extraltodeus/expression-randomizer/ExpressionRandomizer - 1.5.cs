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
            "Finger",
            "Hip",
            "Legs",
            "Neck",
            "Feet",
            "Waist"
        };

        string[] defaultOn =
        {
            "Brow Outer Up Left",
            "Brow Outer Up Right",
						"Brow Inner Up Left",
            "Brow Inner Up Right",
            "Concentrate",
            "Flirting",
						"Pupils Dialate",
						"Eyes Squint Left",
            "Eyes Squint Right",
						"Lips Pucker"
        };

				string[] preset1 =
				{
						"Brow Inner Up Left",
						"Brow Inner Up Right",
						"01-Extreme Pleasure",
						"Concentrate",
						"Confused",
						"Pain",
						"Eyes Squint Left",
						"Eyes Squint Right",
						"Surprise",
						"Mouth Smile Simple",
						"Pupils Dialate"
				};

				string[] preset2 =
				{
						"Brow Inner Up Left",
						"Brow Inner Up Right",
						"Brow Outer Down Left",
						"Brow Outer Down Right",
						"Brow Squeeze",
						"Confused",
						"Sad",
						"Afraid",
						"Mouth Frown",
						"Mouth Narrow",
						"Pain",
						"Eyes Closed",
						"Eyes Squint Left",
						"Eyes Squint Right",
						"Fear",
						"Pupils Dialate",
				};

				// particular morph names to add
				string[] tailorList =
				{
					"Pupils Dialate",
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

				protected JSONStorableFloat masterSpeedSlider;
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


					Dictionary<string, UIDynamicToggle> togglesOn = toggles.Where(t => t.Value.toggle.isOn).ToDictionary(p => p.Key, p => p.Value);

					// define the random values to switch to
					foreach(KeyValuePair<string, UIDynamicToggle> entry in togglesOn)
					{
						string name = entry.Key;
						if (name != "Play")
						{
							DAZMorph morph = morphControl.GetMorphByDisplayName(name);
							if (morph.animatable == false)
							{
								float valeur = UnityEngine.Random.Range(minSlider.val, maxSlider.val) * multiSlider.val;
								newMorphValues[name] = valeur;
							}
						}
					}
        }

				void Update() {
					timer -= Time.deltaTime;
					if (timer <= 0f)
					{
							timer = animWaitSlider.val/masterSpeedSlider.val;
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

							Dictionary<string, UIDynamicToggle> togglesOn = toggles.Where(t => t.Value.toggle.isOn).ToDictionary(p => p.Key, p => p.Value);

              // morph progressively every morphs to their new values
							foreach(KeyValuePair<string, UIDynamicToggle> entry in togglesOn)
							{
								string name = entry.Key;
								if (name != "Play")
								{
									DAZMorph morph = morphControl.GetMorphByDisplayName(name);
									if (morph.animatable == false)
									{
										float valeur = Mathf.Lerp(CurrentMorphsValues[name], newMorphValues[name], Time.deltaTime * animLengthSlider.val * masterSpeedSlider.val);
										CurrentMorphsValues[name] = morph.morphValue = valeur;
									}
								}
							}
            }
        }

        private void UpdateInitialMorphs()
        {
            JSONStorable geometry = containingAtom.GetStorableByID("geometry");
            DAZCharacterSelector character = geometry as DAZCharacterSelector;
            GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;
            morphControl.GetMorphDisplayNames().ForEach((name) =>
            {
                if (toggles.ContainsKey(name))
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
                    if (toggles.ContainsKey(name))
                    {
                        DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                        morph.morphValue = initialMorphValues[name];
                    }

                }
            });
        }

        private void ZeroMorphs()
        {
            JSONStorable geometry = containingAtom.GetStorableByID("geometry");
            DAZCharacterSelector character = geometry as DAZCharacterSelector;
            GenerateDAZMorphsControlUI morphControl = character.morphsControlUI;

            morphControl.GetMorphDisplayNames().ForEach((name) =>
            {
                if (toggleRelations.ContainsKey(name))
                {
                    if (toggles.ContainsKey(name) && toggles[name].toggle.isOn)
                    {
                        DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                        morph.morphValue = 0;
                    }
                }
            });
        }

        public override void Init() {
			try {
                UpdateInitialMorphs();
                UpdateNewMorphs();
                UpdateCurrentMorphs();

                #region Sliders
                minSlider = new JSONStorableFloat("Minimum value", -0.2f, -1f, 1.0f, true);
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

                JSONStorableBool playingBool = new JSONStorableBool("Play", true);
                playingBool.storeType = JSONStorableParam.StoreType.Full;
                RegisterBool(playingBool);
                toggles["Play"] = CreateToggle((playingBool), false);

                morphControl.GetMorphDisplayNames().ForEach((name) =>
                {
                    DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                    regions.Add(morph.region);

                    if (
                         (poseRegion.Any((str) => morph.region.Contains(str)) &&
                        !bodyRegion.Any((str) => morph.region.Contains(str))) ||
												tailorList.Any((str) => name.Contains(str))
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

								UIDynamicButton selectDefault = CreateButton("Select Default", true);
								selectDefault.button.onClick.AddListener(delegate () {
										foreach (KeyValuePair<string, UIDynamicToggle> entry in toggles)
										{
											if (entry.Key != "Play")
												toggles[entry.Key].toggle.isOn = defaultOn.Any((str) => entry.Key.Equals(str));
										};
								});

								UIDynamicButton selectPres1 = CreateButton("Select preset 1", true);
								selectPres1.button.onClick.AddListener(delegate () {
										foreach (KeyValuePair<string, UIDynamicToggle> entry in toggles)
										{
											if (entry.Key != "Play")
												toggles[entry.Key].toggle.isOn = preset1.Any((str) => entry.Key.Equals(str));
										};
								});

								UIDynamicButton selectPres2 = CreateButton("Select preset 2", true);
								selectPres2.button.onClick.AddListener(delegate () {
										foreach (KeyValuePair<string, UIDynamicToggle> entry in toggles)
										{
											if (entry.Key != "Play")
												toggles[entry.Key].toggle.isOn = preset2.Any((str) => entry.Key.Equals(str));
										};
								});
								CreateSpacer(true).height = 10f;;
                // foreach (string LastRegion in LastRegions)
                // {
                //     buttons[LastRegion] = CreateButton(LastRegion, true);
                //     buttons[LastRegion].button.onClick.AddListener(delegate () {
                //         ResetMorphs();
                //         foreach (KeyValuePair<string, UIDynamicToggle> entry in toggles)
                //         {
                //             if (toggleRelations.ContainsKey(entry.Key))
                //             {
                //                 entry.Value.toggle.isOn = (toggleRelations[entry.Key] == LastRegion);
                //             }
                //         }
                //     });
                // }
                #endregion

                #region Region checkbox generation
                foreach (KeyValuePair<string, string> entry in temporaryToggles)
                {
                    JSONStorableBool checkBoxTick = new JSONStorableBool(entry.Value, defaultOn.Any((str) => entry.Key.Equals(str)), (bool on)=>{
											if (!on && entry.Key != "Play") {
												DAZMorph morph = morphControl.GetMorphByDisplayName(entry.Key);
												morph.morphValue = 0;
											}
										});
                    checkBoxTick.storeType = JSONStorableParam.StoreType.Full;
                    RegisterBool(checkBoxTick);

                    toggles[entry.Key] = CreateToggle(checkBoxTick, true);
                }
                #endregion

                #endregion

                //CreateSpacer();
                UIDynamicButton animatableButton = CreateButton("Clear Animatable (from selected)", false);
                animatableButton.button.onClick.AddListener(delegate ()
                {
                    morphControl.GetMorphDisplayNames().ForEach((name) =>
                    {
                        DAZMorph morph = morphControl.GetMorphByDisplayName(name);
                        if (toggles.ContainsKey(name) && toggles[name].toggle.isOn)
                            morph.animatable = false;
                    });
                });

                #region SetAsDef button
                UIDynamicButton setasdefButton = CreateButton("Set current state as default", false);
                setasdefButton.button.onClick.AddListener(delegate ()
                {
                  UpdateInitialMorphs();
                });
                #endregion

                #region Reset button
                UIDynamicButton resetButton = CreateButton("Reset", false);
                resetButton.button.onClick.AddListener(delegate ()
                {
									toggles["Play"].toggle.isOn = false;
                  ResetMorphs();
                });
                #endregion

                #region ZeroMorph button
                UIDynamicButton ZeroMorphButton = CreateButton("Zero Selected", false);
                ZeroMorphButton.button.onClick.AddListener(delegate ()
                {
									toggles["Play"].toggle.isOn = false;
                  ZeroMorphs();
                });
                #endregion

								masterSpeedSlider = new JSONStorableFloat("Master speed", 1f, 0f, 10f, true);
								masterSpeedSlider.storeType = JSONStorableParam.StoreType.Full;
								RegisterFloat(masterSpeedSlider);
								CreateSlider(masterSpeedSlider, false);

                animWaitSlider = new JSONStorableFloat("Loop length", 2f, 0.1f, 20f, true);
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
