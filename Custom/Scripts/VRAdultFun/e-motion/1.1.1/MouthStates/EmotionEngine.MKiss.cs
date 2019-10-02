using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MKiss : State
        {
            public override void OnEnter()
            {
                //interestArousal += 5.0f;
                //interestValence += 1.0f;
                morphMouthAction = true;
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
                mGlareTarget = 0.0f;
				sexActionNeckX = -5.0f;
                mTongueInOutTarget = Random.Range(-0.6f, 0.3f);
                mTongueSideSideTarget = Random.Range(-0.2f, 0.2f);
                mTongueBendTipTarget = 0.0f;
                mHappyTarget = 0.0f;
                mFlirtingTarget = 0.0f;
                mMouthOpenTarget = Random.Range(0.4f, 0.8f);
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                mSmileSimpleLeftTarget = 0.0f;
                mSmileSimpleRightTarget = 0.0f;
                mLipsPuckerTarget = Random.Range(0.4f, 1.1f);
                mLipsPuckerWideTarget = 0.0f;
                Duration = Random.Range(1.5f, 3.0f) * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
                mTongueInOutTarget = 0.0f;
                mTongueSideSideTarget = 0.0f;
                mMouthOpenTarget = 0.0f;
                mLipsPuckerTarget = 0.0f;
				sexActionNeckX = 0.0f;
            }
        }
    }

}