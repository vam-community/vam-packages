using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MSmirk : State
        {
            public override void OnEnter()
            {
				currentMouth = "Pout";
                //interestArousal += 0.5f;
                //interestValence += 0.05f;
                morphMouthAction = true;
                mSmileFullFaceTarget = Mathf.Lerp(Random.Range(0.2f,0.6f),0.0f,interestArousal/10.0f);
                mSmileOpenFullFaceTarget = 0.0f;
				mHappyTarget = 0.0f;
                mGlareTarget = 0.0f;
                mTongueInOutTarget = 1.0f;
                mTongueSideSideTarget = 0.0f;
                mTongueBendTipTarget = 0.0f;
                mVisFTarget = 0.0f;
                //mFlirtingTarget = Random.Range(0.1f,0.4f);
                mMouthOpenTarget = -0.1f + (0.4f * ((10.0f-interestArousal)/10.0f));
				//mFlirtingTarget = Mathf.Lerp(0.7f,0.0f,interestArousal/10.0f);
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                /*if (pStableness > 50.0f)
                {
                    mSmileSimpleLeftTarget = Random.Range(0.5f, 1.0f);
                    mSmileSimpleRightTarget = 0.2f;
                }
                else
                {
                    mSmileSimpleLeftTarget = 0.2f;
                    mSmileSimpleRightTarget = Random.Range(0.5f, 1.0f);
                }*/
                mLipsPuckerTarget = 0.3f * (1.0f+(interestArousal/10.0f));
                mLipsPuckerWideTarget = 0.0f;
                Duration = Random.Range(0.25f, 0.75f) * (1.0f + lookVariation);
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
                mSmileSimpleLeftTarget = 0.0f;
                mSmileSimpleRightTarget = 0.0f;
                mFlirtingTarget = 0.0f;
				mLipsPuckerTarget = 0.0f;
				mMouthOpenTarget = 0.0f;
				mMouthOpenWideTarget = Random.Range(0.0f,0.2f);
            }
        }
    }

}