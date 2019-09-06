using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LIntense : State
        {
            public override void OnEnter()
            {
				currentLook = "Intense";
				float randomRoll = Random.Range(0.0f, 100.0f);
                //LogError("Intense");
                //gHeadSpeed = 1.0f;
				sexActionNeckX = 0.0f;
				if (interestClock <= 0.0f)
				{
					shoulderUp = Random.Range(0.0f,0.3f);
					peronalityAdjustH = Random.Range(-45.0f, 45.0f) * (1.0f-((interestValence-2.0f)/10.0f)) * Mathf.Deg2Rad;
					peronalityAdjustV = Random.Range(-20.0f, -5.0f) * (interestArousal/10.0f) * Mathf.Deg2Rad;
				}
                saccadeAmount = Random.Range(6.0f, 10.0f);
                if (randomRoll < 27.0f)
                {
                    gHeadRollTarget = Random.Range(-10.0f, 10.0f);
                }
                else
                {
                    //gHeadRollTarget = 0.0f;
                }
				
				if (Mathf.Abs(gHeadRollTarget) < 5.0f)
				{
					tempFloat = Random.Range(25.0f,40.0f);
					if (randomRoll > 50.0f)
					{
						gHeadRollTarget = tempFloat;
					}
					else
					{
						gHeadRollTarget = -tempFloat;
					}
				}
				else
				{
					tempFloat = Random.Range(0.0f,15.0f);
					if (randomRoll > 50.0f)
					{
						gHeadRollTarget = tempFloat;
					}
					else
					{
						gHeadRollTarget = -tempFloat;
					}
				}
                saccadeClock = 0.0f;
                lookAction = true;
                lookVariation = Random.Range(0.25f, 0.5f);
                browVariation = Random.Range(0.10f, 0.25f);
                eyeVariation = Random.Range(0.25f, 0.5f);
                mouthVariation = Random.Range(0.5f, 0.45f);
                if (Random.Range(0.0f, 100.0f) > 10.0f && morphBrowAction == false)
                {
                    if (amGlancing)
                    {
                        browSM.SwitchRandom(new State[] {
                                    bLowered,
                                    bApprehensive,
                                    bConcentrate,
                                    bRaised,
                                    bRaised,
                                    bRaised
                                });
                    }
                    else
                    {
                        if (lastBrowState == bLowered)
                        {
                                browSM.SwitchRandom(new State[] {
                                            bLowered,
                                            bApprehensive,
                                            bApprehensive,
                                            bConcentrate,
                                            bLowered,
                                            bRaised
                                        });
                        }
                        else
                        {
                            if (lastBrowState == bRaised)
                            {
                                browSM.SwitchRandom(new State[] {
                                            bLowered,
                                            bApprehensive,
                                            bConcentrate
                                        });
                            }
                            else
                            {
                                browSM.SwitchRandom(new State[] {
                                            bLowered,
                                            bApprehensive,
                                            bApprehensive,
                                            bRaised,
                                            bRaised,
                                            bRaised
                                        });
                            }
                        }
                    }
                }
                if (randomRoll > 5.0f && morphMouthAction == false)
                {
                    if (lastMouthState == mClosed)
                    {
                        mouthSM.SwitchRandom(new State[] {
                                    mClosed,
                                    mClosed,
                                    mClosed,
                                    mOpen,
                                    mOpen,
                                    mSmirk,
                                    mBiteLip,
                                    mBiteLip
                                });
                    }
                    else
                    {
                        if (lastMouthState == mOpen)
                        {
                            mouthSM.SwitchRandom(new State[] {
                                        mClosed,
                                        mClosed,
                                        mClosed,
                                        mOpen,
                                        mOpen,
                                        mSmirk,
                                        mSmirk,
                                        mSmile,
                                        mSmile,
                                        mSmile,
                                        mSmile,
                                        mSmile,
                                        mBiteLip
                                    });
                        }
                        else
                        {
                            if (lastMouthState == mBiteLip || lastMouthState == mSmirk || lastMouthState == mSideways)
                            {
                                mouthSM.SwitchRandom(new State[] {
                                            mClosed,
                                            mOpen
                                        });
                            }
                            else
                            {
                                mouthSM.SwitchRandom(new State[] {
                                            mClosed,
                                            mClosed,
                                            mClosed,
                                            mOpen,
                                            mSmirk,
                                            mSmile,
                                            mSmile,
                                            mBiteLip
                                        });
                            }
                        }
                    }
                }
                eyesSM.SwitchRandom(new State[] {
                            eOpen,
                            eSquint,
                            eSquint,
                            eSquint,
                            eFocus,
                            eFocus,
                            eFocus,
                            eFocus,
                            eFocus
                        });
                float rand = Random.Range(0.0f, 100.0f);
                if (rand > 33.0f)
                {
                    mLHandFistTarget = Random.Range(0.0f, 0.4f);
                }
                if (rand < 66.0f)
                {
                    mRHandFistTarget = Random.Range(0.0f, 0.4f);
                }
                Duration = Random.Range(2.0f, 4.0f);
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