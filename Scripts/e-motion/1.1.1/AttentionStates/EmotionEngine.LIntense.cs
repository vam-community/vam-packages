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
                //LogError("Intense");
                //gHeadSpeed = 1.0f;
                peronalityAdjustH = Random.Range(-45.0f, 45.0f) * Mathf.Deg2Rad;
                peronalityAdjustV = Random.Range(-20.0f, -5.0f) * Mathf.Deg2Rad;
                saccadeAmount = Random.Range(0.0f, 5.0f);
                if (Random.Range(0.0f, 100.0f) < 27.0f)
                {
                    gHeadRollTarget = Random.Range(-10.0f, 10.0f);
                }
                else
                {
                    //gHeadRollTarget = 0.0f;
                }
                if (gHeadRollTarget > 10.0f || gHeadRollTarget < -10.0f)
                {
                    gHeadRollTarget = Random.Range(-10.0f, 10.0f);
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
                                    bOneRaise,
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
                            browSM.Switch(bRaised);
                        }
                        else
                        {
                            if (lastBrowState == bRaised)
                            {
                                browSM.SwitchRandom(new State[] {
                                            bLowered,
                                            bApprehensive
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
                if (Random.Range(0.0f, 100.0f) > 5.0f && morphMouthAction == false)
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
                                    mSideways,
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
                                        mSmile,
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
                                mouthSM.Switch(mClosed);
                            }
                            else
                            {
                                mouthSM.SwitchRandom(new State[] {
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
                Duration = Random.Range(2.0f, 6.0f);
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