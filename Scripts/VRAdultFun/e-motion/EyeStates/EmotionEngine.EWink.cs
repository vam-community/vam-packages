using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class EWink : State
        {
            public override void OnEnter()
            {
				currentEye = "Wink";
                //interestArousal += 0.5f;
                //interestValence += 0.1f;
                morphEyeAction = true;
                if (Random.Range(0.0f, 100.0f) > 50.0f)
                {
                    mEyesClosedLeftTarget = 0.95f;
                    mEyesClosedRightTarget = 0.0f;
                    mSmileSimpleLeftTarget = 0.4f;
                    mSmileSimpleRightTarget = 0.0f;
                }
                else
                {
                    mEyesClosedLeftTarget = 0.0f;
                    mEyesClosedRightTarget = 0.95f;
                    mSmileSimpleLeftTarget = 0.0f;
                    mSmileSimpleRightTarget = 0.4f;
                }
                mEyesSquintTarget = 0.0f;
                Duration = Random.Range(0.25f, 0.3f);
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphEyeAction = false;
                //eyeClock = 0.0f;
                mEyesClosedLeftTarget = 0.0f;
                mEyesClosedRightTarget = 0.0f;
            }
        }
    }

}