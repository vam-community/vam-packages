namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class EBlink : State
        {
            public override void OnEnter()
            {
                //lookVariation = 0.0f;
                //lookVariation = Random.Range(9.0f,11.0f);
                morphEyeAction = true;
                morphBlinking = true;
                mEyesClosedLeftTarget = 0.95f;
                mEyesClosedRightTarget = 0.95f;
                Duration = 0.1f;
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                morphEyeAction = false;
                mEyesClosedLeftTarget = 0.0f;
                mEyesClosedRightTarget = 0.0f;
            }
        }
    }

}