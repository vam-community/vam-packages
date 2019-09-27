using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class EClosed : State
        {
            public override void OnEnter()
            {
                if (morphBlinking == false)
                {
                    interestArousal -= 0.05f;
                    interestValence += 0.2f;
                    morphEyeAction = true;
                    if (lookVariation < 0.5f)
                    {
                        lookVariation += 1.0f;
                    }
                    saccadeAmount = Random.Range(0.0f, 0.0f);
                    mEyesClosedLeftTarget = 0.95f;
                    mEyesClosedRightTarget = 0.95f;
                    mEyesSquintTarget = 0.0f;
                    morphBlinking = false;
                    Duration = 0.1f;
                }
            }
            public override void OnInterrupt(string parameter)
            {
                morphEyeAction = false;
            }
            public override void OnTimeout()
            {
                morphEyeAction = false;
                eyeClock = 0.0f;
            }
        }
    }

}