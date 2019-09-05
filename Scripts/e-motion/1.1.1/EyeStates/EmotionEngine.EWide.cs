using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class EWide : State
        {
            public override void OnEnter()
            {
                if (morphBlinking == false)
                {
                    interestArousal += 0.5f;
                    interestValence -= 0.1f;
                    morphEyeAction = true;
                    mEyesClosedLeftTarget = -0.10f;
                    mEyesClosedRightTarget = -0.10f;
                    morphBlinking = false;
                    mEyesSquintTarget = -0.52f;
                    Duration = Random.Range(0.5f, 1.0f) * lookVariation;
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