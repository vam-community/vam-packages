using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MSideways : State
        {
            public override void OnEnter()
            {
                //interestArousal += 0.1f;
                //interestValence += 0.2f;
                morphMouthAction = true;
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
                mGlareTarget = 0.0f;
				mHappyTarget = 0.0f;
                mTongueInOutTarget = 1.0f;
                mTongueSideSideTarget = 0.0f;
                mTongueBendTipTarget = 0.0f;
                mVisFTarget = 0.0f;
                mHappyTarget = 0.0f;
                mFlirtingTarget = 0.0f;
                mMouthOpenTarget = -0.05f;
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                mSmileSimpleLeftTarget = 0.0f;
                mSmileSimpleRightTarget = 0.0f;
                if (Random.Range(0.0f, 100.0f) > pExtraversion)
                {
                    mMouthSideLeftTarget = Random.Range(0.1f, 0.6f);
                    mMouthSideRightTarget = 0.0f;
                }
                else
                {
                    mMouthSideLeftTarget = 0.0f;
                    mMouthSideRightTarget = Random.Range(0.1f, 0.6f);
                }
                mLipsPuckerTarget = 0.0f;
                mLipsPuckerWideTarget = 0.0f;
                Duration = Random.Range(1.25f, 2.25f) * (1.0f + lookVariation);
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                mFlirtingTarget = 0.0f;
            }
        }
    }

}