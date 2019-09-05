using Random = UnityEngine.Random;
using UnityEngine;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MBigSmile : State
        {
            public override void OnEnter()
            {
                //interestValence += 0.3f;
                morphMouthAction = true;
                mSmileFullFaceTarget = 0.3f + Random.Range(0.0f,0.25f);
                mSmileOpenFullFaceTarget = (0.2f * (interestValence / 10.0f)) + Random.Range(0.0f,0.15f);
                mGlareTarget = 0.0f;
                mHappyTarget = Random.Range(0.0f,0.15f);
                mFlirtingTarget = 0.2f;
                mMouthOpenTarget = -0.2f;
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                mVisFTarget = 0.0f;
                mTongueInOutTarget = 1.0f;
                mTongueSideSideTarget = 0.0f;
                mTongueBendTipTarget = 0.0f;
                mSmileSimpleLeftTarget = 0.2f + Random.Range(0.0f,0.25f);
                mSmileSimpleRightTarget = 0.2f + Random.Range(0.0f,0.25f);
                mLipsPuckerTarget = 0.0f;
                mLipsPuckerWideTarget = -0.05f;
                Duration = Random.Range(2.5f, 4.5f) * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
                //mSmileOpenFullFaceTarget = 0.0f;
                mMouthOpenTarget = 0.0f;
				mSmileOpenFullFaceTarget = mSmileOpenFullFaceTarget / 3.0f;
            }
        }
    }

}