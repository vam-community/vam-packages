using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MSmirk : State
        {
            public override void OnEnter()
            {
                //interestArousal += 0.5f;
                //interestValence += 0.05f;
                morphMouthAction = true;
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
				mHappyTarget = 0.0f;
                mGlareTarget = 0.0f;
                mTongueInOutTarget = 1.0f;
                mTongueSideSideTarget = 0.0f;
                mTongueBendTipTarget = 0.0f;
                mVisFTarget = 0.0f;
                mFlirtingTarget = 0.2f;
                mMouthOpenTarget = -0.1f;
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
                mLipsPuckerTarget = 0.3f;
                mLipsPuckerWideTarget = 0.0f;
                Duration = Random.Range(1.5f, 2.0f) * (1.0f + lookVariation);
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
            }
        }
    }

}