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
				shoulderUp = Random.Range(0.2f,0.5f);
				if (playerTipToHead < interactionDistance)
				{
					if (Random.Range(0.0f,100.0f) > 50.0f)
					{
						mDeserveItTarget = Random.Range(0.0f, 0.5f);
						mTakingItTarget = 0.0f;
					}
					else
					{
						if (Random.Range(0.0f,100.0f) > 10.0f)
						{
							mDeserveItTarget = 0.0f;
							mTakingItTarget = Random.Range(0.0f, 0.5f);
						}
						else
						{
							mDeserveItTarget = Random.Range(0.0f, 0.25f);
							mTakingItTarget = Random.Range(0.0f, 0.25f);
						}						
					}
                peronalityAdjustH = Random.Range(-5.0f, 5.0f) * Mathf.Deg2Rad;
                peronalityAdjustV = Random.Range(-10.0f, 18.0f) * Mathf.Deg2Rad;
				if (Mathf.Abs(gHeadRollTarget) > 10.0f)
				{
					if (Random.Range(0.0f,100.0f) < 50.0f)
					{
						gHeadRollTarget = Random.Range(-10.0f,10.0f);
					}
					else
					{
						if (gHeadRollTarget > 0.0f)
						{
							gHeadRollTarget = Random.Range(-30.0f,-10.0f);
						}
						else
						{
							gHeadRollTarget = Random.Range(10.0f,30.0f);
						}
					}
				}
				gHeadRollTarget = Random.Range(-30.0f, 30.0f);
				sexActionNeckX += Random.Range(-2.0f,2.0f);
				sexActionNeckX = Mathf.Clamp(sexActionNeckX,-20.0f,20.0f);
				}
				else
				{
                peronalityAdjustH = 0.0f;
                peronalityAdjustV = 0.0f;
				gHeadRollTarget = 0.0f;
				sexActionNeckX = 0.0f;
				}	
                //gHeadSpeed = 3.0f;
                saccadeAmount = Random.Range(0.1f, 0.2f);
                saccadeClock = 0.0f;
                
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
				mDeserveItTarget = 0.0f;
				mTakingItTarget = 0.0f;
				sexActionNeckX = 0.0f;
            }
        }
    }

}