using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MClosed : State
        {
            public override void OnEnter()
            {
                morphMouthAction = true;
                mTongueInOutTarget = 0.5f;
                mTongueSideSideTarget = 0.0f;
				mHappyTarget = 0.0f;
                mMouthOpenTarget = -0.20f;
                mVisFTarget = 0.0f;
                mLipsPuckerWideTarget = 0.0f;
                mLipsPuckerTarget = 0.0f;
                mSmileSimpleLeftTarget = Random.Range(0.0f, 0.1f);
                mSmileSimpleRightTarget = mSmileSimpleLeftTarget;
				mSmileFullFaceTarget = Random.Range(0.0f, 0.3f);
                Duration = Random.Range(0.5f, 2.0f) * lookVariation;
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