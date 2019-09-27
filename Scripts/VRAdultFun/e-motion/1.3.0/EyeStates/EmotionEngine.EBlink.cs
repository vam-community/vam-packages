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
                Duration = 0.07f;
                morphEyeAction = true;
				if (blinkTimer > 0.3f)
				{
					morphBlinking = true;
					mEyesClosedLeftTarget = eyeCloseMaxMorph - 0.1f;
					mEyesClosedRightTarget = eyeCloseMaxMorph - 0.1f;
					blinkTimer = 0.0f;
				}
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
				//eyeClock = 0.0f;
            }
        }
    }

}