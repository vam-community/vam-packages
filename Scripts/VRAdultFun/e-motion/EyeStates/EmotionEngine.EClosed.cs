using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class EClosed : State
        {
            public override void OnEnter()
            {
                //if (morphBlinking == false)
                //{
					currentEye = "Closed";
                    morphEyeAction = true;
                    if (lookVariation < 0.5f)
                    {
                        lookVariation += 1.0f;
                    }
					
                    saccadeAmount = Random.Range(0.0f, 0.0f);
					
					if (Mathf.Abs(Vector3.Angle(eyeController.transform.position - headController.transform.position, headController.followWhenOff.forward)) > 30.0f)
					{
						mEyesClosedLeftTarget = eyeCloseMaxMorph + 0.1f;
						mEyesClosedRightTarget = eyeCloseMaxMorph + 0.1f;
					}
					else
					{
						mEyesClosedLeftTarget = eyeCloseMaxMorph;
						mEyesClosedRightTarget = eyeCloseMaxMorph;
					}
                    mEyesSquintTarget = 0.0f;
                    Duration = Random.Range(1.5f,2.5f);
                //}
            }
            public override void OnInterrupt(string parameter)
            {
                morphEyeAction = false;
            }
            public override void OnTimeout()
            {
                morphEyeAction = false;
                //eyeClock = 0.0f;
            }
        }
    }

}