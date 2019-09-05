using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class BLowered : State
        {
            public override void OnEnter()
            {
                interestArousal -= 0.05f;
                interestValence -= 0.1f;
                morphBrowAction = true;
                mExcitementTarget = 0.0f;
                if (mBrowUpTarget > 0.0f)
                {
                    mBrowDownTarget = 0.0f;
                    mBrowUpTarget = 0.0f;
                }
                else
                {
                    mBrowDownTarget = Mathf.Min(0.5f + ((10.0f - interestValence) / 10.0f), 1.0f);
                    mBrowUpTarget = 0.0f;
                }
                mBrowCenterUpTarget = 0.0f;
                mBrowOuterUpLeftTarget = 0.0f;
                mBrowOuterUpRightTarget = 0.0f;
                Duration = Random.Range(0.65f, 1.35f) * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphBrowAction = false;
                mBrowDownTarget = 0.0f;
            }
        }
    }

}