using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class BApprehensive : State
        {
            public override void OnEnter()
            {
                interestArousal += 0.5f;
                interestValence -= 0.2f;
                morphBrowAction = true;
                mExcitementTarget = 0.0f;
                mBrowDownTarget = 0.7f;
                mBrowUpTarget = 0.0f;
                mBrowCenterUpTarget = Mathf.Clamp(0.05f + (interestArousal / 10.0f), 0.05f, 0.65f);
                mSmileFullFaceTarget = 0.0f;
                mSmileOpenFullFaceTarget = 0.0f;
                mBrowOuterUpLeftTarget = 0.0f;
                mBrowOuterUpRightTarget = 0.0f;
                Duration = Random.Range(0.65f, 1.35f);
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphBrowAction = false;
                //mBrowDownTarget = 0.0f;
                //mBrowCenterUpTarget = 0.0f;
            }
        }
    }

}