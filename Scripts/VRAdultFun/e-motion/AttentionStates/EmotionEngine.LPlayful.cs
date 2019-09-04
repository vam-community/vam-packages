using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LPlayful : State
        {
            public override void OnEnter()
            {
                peronalityAdjustH = Random.Range(-35.0f, 35.0f) * Mathf.Deg2Rad;
                peronalityAdjustV = Random.Range(-4.0f, Mathf.Lerp(-2.0f,-10.0f,interestArousal / 10.0f)) * Mathf.Deg2Rad;
				currentLook = "Playful";
                //LogError("Playful");
                //gHeadSpeed = 1.0f;
                if (Random.Range(0.0f, 100.0f) < 25.0f)
                {
                    gHeadRollTarget = Random.Range(-15.0f, 15.0f);
                }
                else
                {
                    gHeadRollTarget = 0.0f;
                }
                saccadeAmount = Random.Range(10.0f, 25.0f);
                saccadeClock = 0.0f;
                lookAction = true;
                mBrowUpTarget = Random.Range(0.0f, 0.2f);
                lookVariation = Random.Range(0.25f, 0.45f);
                browVariation = Random.Range(0.15f, 0.55f);
                eyeVariation = Random.Range(0.05f, 0.15f);
                mouthVariation = Random.Range(0.15f, 0.35f);
                browSM.SwitchRandom(new State[] {
                            bRaised,
                            bApprehensive,
                            bApprehensive,
                            bApprehensive,
                            bApprehensive
                        });
                mouthSM.SwitchRandom(new State[] {
                            mOpen,
                            mOpen,
                            mBiteLip,
                            mBiteLip,
                            mBiteLip,
                            mSmirk,
							mBigSmile,
							mSmile,
                            mSideways
        					//mKiss
        				});
                eyesSM.SwitchRandom(new State[] {
                            eOpen,
                            eFocus,
                            eFocus,
                            eFocus,
                            eFocus,
                            eSquint
                        });
                Duration = Random.Range(2.5f, 6.0f);
            }
            public override void OnTimeout()
            {
                lookAction = false;
            }
        }
    }

}