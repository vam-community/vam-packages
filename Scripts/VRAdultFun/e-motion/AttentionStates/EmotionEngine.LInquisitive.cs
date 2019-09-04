using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LInquisitive : State
        {
            public override void OnEnter()
            {
				currentLook = "Inquisitive";
				sexActionNeckX = 0.0f;
                //LogError("Inquisitive");
                peronalityAdjustH = Random.Range(-45.0f, 45.0f) * Mathf.Deg2Rad;
                peronalityAdjustV = Random.Range(0.0f, -25.0f * (pExtraversion / 100.0f)) * Mathf.Deg2Rad;                //gHeadSpeed = 4.0f;
                if (Random.Range(0.0f, 100.0f) < 65.0f)
                {
                    gHeadRollTarget = Random.Range(-25.0f, 25.0f);
                }
                else
                {
                    //gHeadRollTarget = 0.0f;
                }
                if (gHeadRollTarget > 5.0f || gHeadRollTarget < -5.0f)
                {
                    gHeadRollTarget = Random.Range(-5.0f, 5.0f);
                }
                saccadeAmount = Random.Range(5.0f, 20.0f);
                saccadeClock = 0.0f;
                lookAction = true;
                lookVariation = Random.Range(0.45f, 0.8f);
                browVariation = Random.Range(0.45f, 0.6f);
                eyeVariation = Random.Range(0.25f, 0.4f);
                mouthVariation = Random.Range(0.15f, 0.3f);

                if (Random.Range(0.0f, 100.0f) > 10.0f && morphBrowAction == false)
                {
                    if (amGlancing)
                    {
                        browSM.SwitchRandom(new State[] {
                                    bApprehensive,
                                    bConcentrate
                                });
                    }
                    else
                    {
                        if (lastBrowState == bLowered || lastBrowState == bApprehensive)
                        {
                            browSM.SwitchRandom(new State[] {
                                        bRaised,
                                        bRaised,
                                        bRaised,
                                        bOneRaise,
                                        bOneRaise
                                    });
                        }
                        else
                        {
                            if (lastBrowState == bRaised)
                            {
                                browSM.SwitchRandom(new State[] {
                                            bRaised,
                                            bRaised,
                                            bLowered,
                                            bApprehensive
                                        });
                            }
                            else
                            {
                                browSM.SwitchRandom(new State[] {
                                            bRaised,
                                            bRaised,
                                            bRaised,
                                            bRaised,
                                            bLowered,
                                            bOneRaise,
                                            bOneRaise,
                                            bApprehensive
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
                                    mOpen,
                                    mClosed,
                                    mClosed,
                                    mClosed,
									mBiteLip,
									mBigSmile,
									mBigSmile,
                                    mSmirk,
                                    mSmirk,
                                    mSideways,
                                    mSideways
                                });
                    }
                    else
                    {
                        if (lastMouthState == mOpen)
                        {
                            mouthSM.SwitchRandom(new State[] {
                                        mOpen,
                                        mClosed,
                                        mClosed,
                                        mBiteLip,
                                        mBigSmile,
                                        mBigSmile,
                                        mBigSmile,
                                        mSmile,
                                        mSmile,
                                        mSmile,
                                        mSmile,
                                        mSmile,
                                        mSmile,
                                        mSideways,
                                        mSideways,
                                        mSideways,
                                        mSideways,
                                        mSmirk
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
                                            mOpen,
                                            mClosed,
                                            mClosed,
                                            mClosed,
                                            mClosed,
                                            mBiteLip,
                                            mSmile,
                                            mSmile,
                                            mBigSmile,
                                            mBigSmile,
                                            mSideways,
											mSideways,
                                            mSmirk,
                                            mSmirk,
                                            mSmirk
                                        });
                            }
                        }
                    }
                }

                eyesSM.SwitchRandom(new State[] {
                            eOpen,
                            eOpen,
                            eOpen,
                            eFocus,
                            eFocus,
                            eSquint
                        });
                float rand = Random.Range(0.0f, 100.0f);
                if (rand > 33.0f)
                {
                    mLHandStraightenTarget = Random.Range(0.0f, 0.2f);
                    mLHandFistTarget = Random.Range(0.0f, 0.2f);
                }
                if (rand < 66.0f)
                {
                    mRHandStraightenTarget = Random.Range(0.0f, 0.2f);
                    mRHandFistTarget = Random.Range(0.0f, 0.2f);
                }
                Duration = Random.Range(3.0f, 7.0f);
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