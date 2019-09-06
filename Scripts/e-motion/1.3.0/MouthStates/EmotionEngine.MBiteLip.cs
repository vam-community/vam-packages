using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MBiteLip : State
        {
            public override void OnEnter()
            {
				currentMouth = "Demure";
                //interestArousal += 0.25f;
                //interestValence += 0.1f;
                morphMouthAction = true;
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
                mGlareTarget = 0.0f;
                mHappyTarget = Random.Range(0.1f, 0.3f);
                //mTongueInOutTarget = Random.Range(-0.15f,0.5f);
                //mTongueSideSideTarget = 0.5f;
                mTongueBendTipTarget = 0.0f;
                ///mVisFTarget = Random.Range(0.5f,1.0f);
                mMouthOpenTarget = -1.0f;
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                //mSmileSimpleLeftTarget = -1.0f;
                //mSmileSimpleRightTarget = -1.0f;
                mLipsPuckerTarget = Random.Range(0.1f, 0.3f);
                mLipsPuckerWideTarget = 0.0f;
                mExcitementTarget = 0.0f;
                Duration = Random.Range(1.0f, 2.5f) * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
                mHappyTarget = 0.0f;
                mMouthOpenTarget = 0.0f;
                mLipsPuckerTarget = 0.0f;
				mMouthOpenWideTarget = Random.Range(0.0f,0.2f);
            }
        }
    }

}