using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LKissing : State
        {
            public override void OnEnter()
            {
				currentLook = "Kissing";
                //LogError("Kissing");
				shoulderUp = Random.Range(0.2f,0.2f);
				sexActionNeckX = 0.0f;
                //gHeadSpeed = 1.0f;
                interestKissing = true;
                saccadeAmount = Random.Range(0.0f, 0.0f);
                saccadeClock = 0.0f;
				tempFloat2 = playerHeadController.transform.eulerAngles.z - headController.transform.eulerAngles.z;
				if (tempFloat2 > 180.0f){tempFloat2 -= 360.0f;}
				if (tempFloat2 < -180.0f){tempFloat2 += 360.0f;}
				//gHeadRollTarget = Mathf.Clamp(tempFloat + Random.Range(-15.0f,15.0f),-40.0f,40.0f);
				tempFloat = playerHeadTransform.eulerAngles.z;
				if (tempFloat > 180.0f)
				{
					tempFloat = (360.0f - tempFloat) * -1.0f;
				}
				if (tempFloat > 0.0f && tempFloat < 15.0f)
				{
					gHeadRollTarget = tempFloat + 20.0f;
				}
				if (tempFloat < 0.0f && tempFloat > -15.0f)
				{
					gHeadRollTarget = tempFloat - 20.0f;
				}
				if (Mathf.Abs(tempFloat2) > 20.0f && Mathf.Abs(tempFloat2) < 50.0f)
				{
					//gHeadRollTarget -= tempFloat2 / 2.0f;
				}
				if (gHeadRollTarget < 0.0f)
				{
					gHeadRollTarget = Mathf.Clamp(gHeadRollTarget,-40.0f,-15.0f);
				}
				else
				{
					gHeadRollTarget = Mathf.Clamp(gHeadRollTarget,15.0f,40.0f);
				}
				
				
                peronalityAdjustH = 0.0f * Mathf.Deg2Rad;//Random.Range(-2.0f,2.0f) * Mathf.Deg2Rad;
                peronalityAdjustV = 0.0f * Mathf.Deg2Rad;//Random.Range(-1.0f,1.0f) * Mathf.Deg2Rad;

                lookAction = true;
                lookVariation = Random.Range(1.0f, 1.0f);
                browVariation = Random.Range(0.25f, 0.5f);
                eyeVariation = Random.Range(0.25f, 0.5f);
                mouthVariation = Random.Range(0.15f, 0.35f);
                browSM.SwitchRandom(new State[] {
                            bApprehensive,
                            bRaised,
                            bConcentrate
                        });
				if (lipsTouchCount > 0.0f)
				{
					mouthSM.Switch(mKiss);
				}
				else
				{
					mouthSM.Switch(mBiteLip);
				}
						eyesSM.SwitchRandom(new State[] {
									eClosed,
									eClosed,
									eClosed,
									eClosed,
									eClosed,
									eClosed,
									eClosed,
									eClosed,
									eClosed,
									eClosed,
									eOpen
								});
                float rand = Random.Range(0.0f, 100.0f);
                if (rand > 33.0f)
                {
                    mLHandFistTarget = Random.Range(0.3f, 0.6f);
                }
                if (rand < 66.0f)
                {
                    mRHandFistTarget = Random.Range(0.3f, 0.6f);
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
                interestKissing = false;
            }
        }



    }

}