﻿using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class EOpen : State
        {
            public override void OnEnter()
            {
                if (morphBlinking == false)
                {
					currentEye = "Open";
                    morphEyeAction = true;
                    mEyesClosedLeftTarget = 0.0f - (interestArousal / 75.0f);
                    mEyesClosedRightTarget = 0.0f - (interestArousal / 75.0f);
                    mEyesClosedLeftBlinkTarget = 0.0f;
                    mEyesClosedRightBlinkTarget = 0.0f;
					mEyesSquintTarget = 0.0f;
                    Duration = Random.Range(1.0f,10.0f);
                }
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphEyeAction = false;
            }
        }
    }

}