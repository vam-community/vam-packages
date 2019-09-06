using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MJoy : State
        {
            public override void OnEnter()
            {
				currentMouth = "Joy";
                morphMouthAction = true;
                mVisFTarget = 0.0f;
                mTongueInOutTarget = Random.Range(0.5f, 1.0f);
                mTongueSideSideTarget = 0.0f;
                mTongueBendTipTarget = 0.0f;
                if (interestArousal > 8.5f && Random.Range(0.0f, 100.0f) < 10.0f)
                {
                    mMouthOpenWideTarget = Random.Range(0.5f, 1.0f);
                    mMouthOpenTarget = 0.0f;
                }
                else
                {
                    mMouthOpenTarget = Random.Range(0.1f, 0.3f);
                    mMouthOpenWideTarget = 0.0f;
                }
                mHappyTarget = Mathf.Clamp(Random.Range(0.1f, 0.4f) - mMouthOpenTarget, 0.0f, 1.0f);
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                mSmileSimpleLeftTarget = 0.0f;
                mSmileSimpleRightTarget = 0.0f;
                mLipsPuckerTarget = Random.Range(0.2f, 0.4f);
                mLipsPuckerWideTarget = 0.0f;
                interestArousal += 5.0f;
                interestValence += 0.25f;
                Duration = Random.Range(1.0f, 3.0f) * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
				mDeserveItTarget = 0.0f;
				mTakingItTarget = 0.0f;
                mLipsPuckerTarget = 0.0f;
				mMouthOpenWideTarget = 0.0f;
				mMouthOpenTarget = 0.0f;
            }
        }
    }

}