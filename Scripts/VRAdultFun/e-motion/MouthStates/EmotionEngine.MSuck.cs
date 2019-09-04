using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MSuck : State
        {
            public override void OnEnter()
            {
                //interestArousal += 2.0f;
                //interestValence -= 0.1f;
                morphMouthAction = true;
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
                sexActionNeckX = Random.Range(-5.0f, 5.0f);
                mGlareTarget = 0.0f;
                mHappyTarget = 0.0f;
                mFlirtingTarget = 0.0f;
                mTongueBendTipTarget = Random.Range(-0.4f, 0.1f);
                mTongueInOutTarget = Random.Range(-0.8f, 0.4f);
                mTongueSideSideTarget = Random.Range(-0.5f, 0.5f);
                mMouthOpenTarget = Random.Range(0.3f, 0.7f);
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                mSmileSimpleLeftTarget = 0.0f;
                mSmileSimpleRightTarget = 0.0f;
                mLipsPuckerTarget = Random.Range(0.3f, 0.8f);
                mLipsPuckerWideTarget = Random.Range(0.3f, 0.8f);
                Duration = Random.Range(0.4f, 0.7f);// * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
                sexActionNeckX = 0.0f;
                //mLipsPuckerTarget = 0.0f;
            }
        }
    }

}