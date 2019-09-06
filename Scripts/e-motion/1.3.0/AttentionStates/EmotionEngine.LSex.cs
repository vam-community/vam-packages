using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LSex : State
        {
            public override void OnEnter()
            {
				currentLook = "Sex";
                //LogError("Sex");
                //gHeadSpeed = 2.0f;
				shoulderUp = Random.Range(-0.2f,0.5f);
                peronalityAdjustH = Random.Range(-15.0f, 15.0f) * Mathf.Deg2Rad;
                peronalityAdjustV = Random.Range(-45.0f, 45.0f) * Mathf.Deg2Rad;
                if (playerTipToPelvis < interactionDistance * 1.5f || interestArousal > 8.0f)
                {
                    peronalityAdjustH = Random.Range(-35.0f, 35.0f) * Mathf.Deg2Rad;
                    peronalityAdjustV = Random.Range(45.0f, interestArousal * 10.0f) * Mathf.Deg2Rad;
                }
                if (Random.Range(0.0f, 100.0f) < 15.0f)
                {
                    gHeadRollTarget = Random.Range(-5.0f, 5.0f);
                }
                else
                {
                    gHeadRollTarget = 0.0f;
                }
                saccadeAmount = Random.Range(0.0f, 5.0f);
                saccadeClock = 0.0f;
                lookAction = true;
                lookVariation = Random.Range(0.15f, 0.35f);
                browVariation = Random.Range(0.20f, 0.55f);
                eyeVariation = Random.Range(0.05f, 0.35f);
                mouthVariation = Random.Range(0.25f, 0.35f);
				sexActionNeckX = Random.Range(-15.0f, 10.0f);
				mDeserveItTarget = Random.Range(0.0f, 0.5f);
				mTakingItTarget = Random.Range(0.0f, 0.5f);
                browSM.SwitchRandom(new State[] {
                            bApprehensive,
                            bApprehensive,
                            bApprehensive,
                            bConcentrate,
                            bConcentrate,
                            bConcentrate
                        });
                mouthSM.SwitchRandom(new State[] {
                            mJoy,
                            mJoy,
                            mJoy,
                            mJoy,
                            mJoy,
                            mOpen,
                            mBiteLip,
                            mBiteLip,
                            mBiteLip,
                            mSmile,
                            mOpen,
                            mBiteLip,
                            mSmirk,
                            mSmirk,
                        });
                eyesSM.SwitchRandom(new State[] {
                            eClosed,
                            eClosed,
                            eClosed,
                            eClosed,
                            eFocus,
                            eOpen,
                            eOpen,
                            eFocus,
                            eSquint
                        });
                float rand = Random.Range(0.0f, 100.0f);
                if (rand > pExtraversion)
                {
                    if (rand > 33.0f)
                    {
                        mLHandFistTarget = Random.Range(0.0f, 1.2f);
                    }
                    if (rand < 66.0f)
                    {
                        mRHandFistTarget = Random.Range(0.0f, 1.2f);
                    }
                }
                else
                {
                    if (rand > 33.0f)
                    {
                        mLHandStraightenTarget = Random.Range(0.0f, 0.5f);
                        mLHandFistTarget = Random.Range(0.0f, 0.5f);
                    }
                    if (rand < 66.0f)
                    {
                        mRHandStraightenTarget = Random.Range(0.0f, 0.5f);
                        mRHandFistTarget = Random.Range(0.0f, 0.5f);
                    }
                }
                Duration = Random.Range(0.5f, 2.0f);
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                lookAction = false;
				sexActionNeckX = 0.0f;
				mDeserveItTarget = 0.0f;
				mTakingItTarget = 0.0f;
            }
        }
    }

}