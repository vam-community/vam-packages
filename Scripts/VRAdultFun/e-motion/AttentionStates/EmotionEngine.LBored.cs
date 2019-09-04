using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LBored : State
        {
            public override void OnEnter()
            {
				currentLook = "Bored";
                //LogError("Bored");
				shoulderUp = Random.Range(0.0f,0.1f);
                peronalityAdjustH = Random.Range(-15.0f, 15.0f) * Mathf.Deg2Rad;
                peronalityAdjustV = Random.Range(-2.0f, 10.0f) * Mathf.Deg2Rad;
				sexActionNeckX = 0.0f;
				gHeadSpeed = 20.0f;
                if (Random.Range(0.0f, 100.0f) < 20.0f)
                {
                    gHeadRollTarget = Random.Range(-30.0f, 30.0f);
                }
                saccadeAmount = Random.Range(10.0f, 20.0f);
                mBrowDownTarget = Random.Range(0.0f, 0.2f);
                saccadeClock = 0.0f;
                lookAction = true;
                lookVariation = Random.Range(0.5f, 1.25f);
                browVariation = Random.Range(0.5f, 1.25f);
                eyeVariation = Random.Range(0.5f, 1.25f);
                mouthVariation = Random.Range(0.35f, 0.45f);
                if (Random.Range(0.0f, 100.0f) > 33.0f && morphBrowAction == false)
                {
                    browSM.SwitchRandom(new State[] {
                                bApprehensive,
                                bApprehensive,
                                bApprehensive,
                                bApprehensive,
                                bApprehensive,
                                bConcentrate,
                                bConcentrate,
                                bConcentrate,
                                bConcentrate,
								bLowered,
								bLowered,
								bLowered
                            });
                }
                mouthSM.SwitchRandom(new State[] {
                            mOpen,
                            mClosed,
                        });
                eyesSM.SwitchRandom(new State[] {
                            eOpen,
                            eFocus,
                            eOpen,
                            eFocus,
                            eOpen,
                            eFocus,
                            eOpen,
                            eFocus,
                            eSquint
                        });
                float rand = Random.Range(0.0f, 100.0f);
                if (rand > 33.0f)
                {
                    mLHandStraightenTarget = Random.Range(0.0f, 0.4f);
                    mLHandFistTarget = Random.Range(0.0f, 0.4f);
                }
                if (rand < 66.0f)
                {
                    mRHandStraightenTarget = Random.Range(0.0f, 0.4f);
                    mRHandFistTarget = Random.Range(0.0f, 0.4f);
                }
                Duration = Random.Range(1.0f, 3.0f);
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