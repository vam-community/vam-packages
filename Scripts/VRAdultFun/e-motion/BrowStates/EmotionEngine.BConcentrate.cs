using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class BConcentrate : State
        {
            public override void OnEnter()
            {
                interestArousal += 0.1f;
                interestValence += 0.02f;
                morphBrowAction = true;
                mExcitementTarget = Mathf.Min(0.1f + ((10.0f - interestValence) / 10.0f), 0.3f);
                mBrowDownTarget = 0.0f;
                mFlirtingTarget = 0.0f;
                mBrowUpTarget = 0.0f;
                mBrowCenterUpTarget = 0.0f;
                mBrowOuterUpLeftTarget = 0.0f;
                mBrowOuterUpRightTarget = 0.0f;
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
                mMouthOpenTarget = 0.0f;
                mLipsPuckerTarget = 0.0f;
                mHappyTarget = 0.0f;
                Duration = Random.Range(0.15f, 1.35f) * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphBrowAction = false;
                //mExcitementTarget = 0.0f;
            }
        }
    }

}