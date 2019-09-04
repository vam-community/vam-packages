using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class ESquint : State
        {
            public override void OnEnter()
            {
                if (morphBlinking == false)
                {
                    interestArousal += 0.2f;
                    interestValence += 0.05f;
                    morphEyeAction = true;
                    mEyesClosedLeftTarget = 0.0f;
                    mEyesClosedRightTarget = 0.0f;
                    morphBlinking = false;
                    mEyesSquintTarget = Random.Range(0.3f, 0.8f);
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
                mEyesSquintTarget = 0.0f;
            }
        }
    }

}