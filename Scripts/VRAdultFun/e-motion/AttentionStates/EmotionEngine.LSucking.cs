using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LSucking : State
        {
            public override void OnEnter()
            {
				currentLook = "Sucking";
                //LogError("Sucking");
                peronalityAdjustH = 0.0f;
                peronalityAdjustV = Random.Range(-5.0f, 5.0f) * Mathf.Deg2Rad;
                //gHeadSpeed = 3.0f;
                saccadeAmount = Random.Range(0.1f, 0.2f);
                saccadeClock = 0.0f;
                gHeadRollTarget = Random.Range(-60.0f, 60.0f);
                lookAction = true;
                lookVariation = Random.Range(0.1f, 0.1f);
                browVariation = Random.Range(0.1f, 0.1f);
                eyeVariation = Random.Range(0.1f, 0.3f);
                mouthVariation = Random.Range(0.14f, 0.3f);
                browSM.SwitchRandom(new State[] {
                            bApprehensive,
                            bApprehensive,
                            bApprehensive,
                            bConcentrate,
                            bLowered,
                            bRaised
                        });
                mouthSM.Switch(mSuck);
                eyesSM.SwitchRandom(new State[] {
                            eOpen,
                            eFocus,
                            eClosed,
                            eClosed,
                            eClosed,
                            eClosed,
                            eClosed,
                            eWide,
                            eWide,
                            eWide
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
                Duration = Random.Range(0.5f, 0.7f);
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