﻿using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MSuck : State
        {
            public override void OnEnter()
            {
				currentMouth = "Sucking";
                //interestArousal += 2.0f;
                //interestValence -= 0.1f;
                morphMouthAction = true;
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
				if (playerTipToHead < interactionDistance / 2.0f)
				{
					sexActionNeckX = Random.Range(-10.0f, 10.0f);
				}
                mGlareTarget = 0.0f;
                mHappyTarget = 0.0f;
                mFlirtingTarget = 0.0f;
                mTongueBendTipTarget = Random.Range(-0.2f, 0.3f);
                mTongueInOutTarget = 1.0f;//Random.Range(-1.8f, 0.4f);
				if (lipsTouchCount > 0.0f && Random.Range(0.0f,100.0f) > 90.0f)
				{
					mTongueInOutTarget = Random.Range(-1.4f, 0.4f);
				}
                mTongueSideSideTarget = Random.Range(-0.7f, 0.7f);
                mMouthOpenTarget = Random.Range(0.3f, 0.7f);
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                mSmileSimpleLeftTarget = 0.0f;
                mSmileSimpleRightTarget = 0.0f;
                mLipsPuckerTarget = Random.Range(0.0f, 0.4f);
                mLipsPuckerWideTarget = Random.Range(0.0f, 0.3f);
                Duration = Random.Range(0.4f, 0.7f);// * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphMouthAction = false;
				mTongueInOutTarget = 1.0f;
                //mLipsPuckerTarget = 0.0f;
            }
        }
    }

}