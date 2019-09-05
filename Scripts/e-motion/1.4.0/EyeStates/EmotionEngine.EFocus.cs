using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class EFocus : State
        {
            public override void OnEnter()
            {
                if (morphBlinking == false)
                {
					currentEye = "Focus";
                    //interestArousal += 0.1f;
                    //interestValence += 0.6f;
                    morphEyeAction = true;
                    mEyesClosedLeftTarget = 0.0f;
                    mEyesClosedRightTarget = 0.0f;
                    mEyesSquintTarget = Random.Range(0.2f, 0.6f) * (interestArousal / 10.0f);
					//mMouthOpenTarget = mEyesSquintTarget / 2.0f;
                    //morphBlinking = false;
                    Duration = Random.Range(1.5f, 3.0f) * lookVariation;
                }
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphEyeAction = false;
                //mEyesSquintTarget = 0.0f;
            }
        }
    }

}