using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class MKiss : State
        {
            public override void OnEnter()
            {
				currentMouth = "Kissing";
                //interestArousal += 5.0f;
                //interestValence += 1.0f;
                morphMouthAction = true;
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
                mGlareTarget = 0.0f;
				//sexActionNeckX = -5.0f;
				if (lipsOnly == false)
				{
					if (mTongueInOutTarget < 0.0f)
					{
						mTongueInOutTarget = Random.Range(0.0f, 0.3f) * kissingAmount;
						mTongueSideSideTarget = Random.Range(-0.2f, 0.2f) * kissingAmount;
						if (mTongueInOutTarget < 0.3f)
						{
							mTongueBendTipTarget = Random.Range(-0.2f, 0.2f) * kissingAmount;
						}
						else
						{
							mTongueBendTipTarget = 0.0f;
						}
					}
					else
					{
						mTongueInOutTarget = Random.Range(-0.8f, -0.5f) * kissingAmount;
						mTongueSideSideTarget = Random.Range(-0.6f, 0.6f) * kissingAmount;
						mTongueBendTipTarget = 0.0f;
					}
				}
				else
				{
						mTongueInOutTarget = 1.0f;
						mTongueSideSideTarget = 0.0f;
						mTongueBendTipTarget = 0.0f;
				}
				mLipsCloseTarget = Random.Range(-0.3f, 0.0f) * kissingAmount;
                mHappyTarget = 0.0f;
                mFlirtingTarget = 0.0f;
				if (mMouthOpenTarget >= 0.2f && Random.Range(0.0f,100.0f) > 60.0f)
				{
					mMouthOpenTarget = Random.Range(0.0f, 0.0f);
					mMouthOpenWideTarget = Random.Range(0.0f, 0.0f);
					mLipsPuckerTarget = Random.Range(0.0f, 0.1f) * kissingAmount;
					mLipsPuckerWideTarget = Random.Range(0.0f, 0.1f) * kissingAmount;
				}
				else
				{
					mMouthOpenTarget = Random.Range(0.0f, 0.6f) * kissingAmount;
					mMouthOpenWideTarget = Random.Range(0.0f, 0.3f) * kissingAmount;
					mLipsPuckerTarget = Random.Range(0.4f, 1.1f) * kissingAmount;
					mLipsPuckerWideTarget = Random.Range(0.1f, 0.4f) * kissingAmount;
				}
                mMouthSideLeftTarget = 0.0f;
                mMouthSideRightTarget = 0.0f;
                mSmileSimpleLeftTarget = 0.0f;
                mSmileSimpleRightTarget = 0.0f;
                Duration = Random.Range(0.5f, 1.0f) * lookVariation;
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