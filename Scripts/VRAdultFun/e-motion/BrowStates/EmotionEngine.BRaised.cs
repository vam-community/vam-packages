using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class BRaised : State
        {
            public override void OnEnter()
            {
                interestArousal += 0.1f;
                interestValence -= 0.15f;
                morphBrowAction = true;
                mExcitementTarget = 0.0f;
                if (mBrowDownTarget > 0.0f)
                {
                    mBrowDownTarget = 0.0f;
                    mBrowUpTarget = 0.0f;
                }
                else
                {
                    mBrowDownTarget = 0.0f;
                    mBrowUpTarget = Mathf.Min(0.5f + (interestValence / 10.0f), 1.0f);
                }
                mBrowCenterUpTarget = 0.0f;
                mBrowOuterUpLeftTarget = 0.0f;
                mBrowOuterUpRightTarget = 0.0f;
                Duration = Random.Range(0.15f, 0.35f) * lookVariation;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphBrowAction = false;
                mBrowUpTarget = 0.0f;
            }
        }
    }

}