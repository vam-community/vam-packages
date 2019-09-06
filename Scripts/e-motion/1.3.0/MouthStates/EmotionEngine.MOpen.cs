using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MOpen : State
        {
            public override void OnEnter()
            {
				currentMouth = "Open";
                //interestArousal += 0.1f;
                //interestValence += 0.05f;
                morphMouthAction = true;
                mSmileFullFaceTarget = -1.0f;
                mSmileOpenFullFaceTarget = 0.0f;
				mHappyTarget = 0.0f;
                mFlirtingTarget = 0.0f;
                mVisFTarget = 0.0f;
                mTongueInOutTarget = Random.Range(0.75f, 1.0f);
                mTongueSideSideTarget = 0.0f;
                mTongueBendTipTarget = 0.0f;
                mMouthOpenTarget = Random.Range(0.2f, 0.4f); ;
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
				mDeserveItTarget = 0.0f;
				mTakingItTarget = 0.0f;
                mSmileSimpleLeftTarget = Random.Range(0.0f, 0.3f);;
                mSmileSimpleRightTarget = mSmileSimpleLeftTarget;
                mLipsPuckerTarget = Random.Range(0.0f, 0.3f); ;
                mLipsPuckerWideTarget = 0.0f;
                Duration = Random.Range(1.0f, 3.0f) * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
            }
        }
    }

}