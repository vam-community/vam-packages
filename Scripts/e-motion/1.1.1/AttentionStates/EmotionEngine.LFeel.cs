using Random = UnityEngine.Random;
using UnityEngine;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LFeel : State
        {
            public override void OnEnter()
            {
                peronalityAdjustH = Random.Range(-25.0f, 25.0f) * Mathf.Deg2Rad;
                peronalityAdjustV = Random.Range(-1.0f, 5.0f) * Mathf.Deg2Rad;
				currentLook = "Feel";
                //LogError("Feel");
                //gHeadSpeed = 2.0f;
                if (Random.Range(0.0f, 100.0f) < 15.0f)
                {
                    gHeadRollTarget = Random.Range(-15.0f, 15.0f);
                }
                else
                {
                    gHeadRollTarget = 0.0f;
                }
                saccadeAmount = Random.Range(0.0f, 5.0f);
                mBrowUpTarget = Random.Range(0.2f, 0.5f);
                saccadeClock = 0.0f;
                lookAction = true;
                lookVariation = Random.Range(0.15f, 0.35f);
                browVariation = Random.Range(0.20f, 0.55f);
                eyeVariation = Random.Range(0.05f, 0.35f);
                mouthVariation = Random.Range(0.25f, 0.35f);
                browSM.SwitchRandom(new State[] {
							bRaised,
                            bApprehensive,
                            bConcentrate,
                            bConcentrate,
                            bConcentrate
                        });
                mouthSM.SwitchRandom(new State[] {
                            mOpen,
                            mOpen,
                            mOpen,
                            mOpen,
                            mOpen,
                            mSmile,
                            mBigSmile,
                            mSmirk,
                        });
                eyesSM.SwitchRandom(new State[] {
                            eClosed,
                            eClosed,
                            eClosed,
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
                Duration = Random.Range(2.0f, 7.0f);
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