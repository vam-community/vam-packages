﻿using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LCasual : State
        {

            public override void OnEnter()
            {
				currentLook = "Casual";
				sexActionNeckX = 0.0f;
                //LogError("Casual");
				if (interestClock <= 0.0f)
				{
					shoulderUp = Random.Range(0.0f,0.2f);
					peronalityAdjustH = Random.Range(-25.0f, 25.0f) * Mathf.Deg2Rad;
					peronalityAdjustV = Random.Range(-5.0f, 2.0f) * Mathf.Deg2Rad;              //gHeadSpeed = 0.5f;
				}
                if (Random.Range(0.0f, 100.0f) < 55.0f)
                {
                    gHeadRollTarget = Random.Range(-7.0f, 7.0f);
                }
                saccadeAmount = Random.Range(5.0f, 20.0f);
                saccadeClock = 0.0f;
                lookAction = true;
                lookVariation = Random.Range(0.2f, 0.35f);
                browVariation = Random.Range(0.1f, 0.25f);
                eyeVariation = Random.Range(0.4f, 0.65f);
                mouthVariation = Random.Range(0.2f, 0.45f);
                if (Random.Range(0.0f, 100.0f) > 50.0f && morphBrowAction == false)
                {
                    browSM.SwitchRandom(new State[] {
                                bRaised,
                                bLowered,
                                bRaised,
                                bLowered,
                                bRaised,
                                bLowered,
                                bApprehensive,
                                bConcentrate,
                                bApprehensive,
                                bApprehensive
                            });
                }
                if (Random.Range(0.0f, 100.0f) > 50.0f && morphMouthAction == false)
                {
					if (interestValence > 9.0f)
					{
						mouthSM.SwitchRandom(new State[] {
									mOpen,
									mClosed,
									mOpen,
									mClosed,
									mOpen,
									mClosed,
									mSmile,
									mSmile,
									mSmile,
									mSmirk,
									mBigSmile,
									mBigSmile
								});
					}
					else
					{
						mouthSM.SwitchRandom(new State[] {
									mOpen,
									mOpen,
									mClosed,
									mClosed,
									mClosed,
									mSmile,
									mSmile,
									mSmile,
									mSmirk,
									mBigSmile,
									mBigSmile,
									mBiteLip
								});
					}
                }
                eyesSM.SwitchRandom(new State[] {
                            eOpen,
                            eOpen,
                            eWide,
                            eFocus,
                            eFocus
                        });
                float rand = Random.Range(0.0f, 100.0f);
                if (rand > 33.0f)
                {
                    mLHandStraightenTarget = Random.Range(0.0f, 0.2f);
                    mLHandFistTarget = Random.Range(0.0f, 0.5f);
                }
                if (rand < 66.0f)
                {
                    mRHandStraightenTarget = Random.Range(0.0f, 0.2f);
                    mRHandFistTarget = Random.Range(0.0f, 0.5f);
                }
                Duration = Random.Range(2.0f, 5.0f);
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                lookAction = false;
            }
        }
    }

}