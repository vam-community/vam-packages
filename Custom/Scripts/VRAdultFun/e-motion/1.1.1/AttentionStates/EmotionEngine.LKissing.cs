using UnityEngine;
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LKissing : State
        {
            public override void OnEnter()
            {
				currentLook = "Kissing";
                //LogError("Kissing");
                peronalityAdjustH = 0.0f;
                peronalityAdjustV = 0.0f;
                //gHeadSpeed = 1.0f;
                interestKissing = true;
                saccadeAmount = Random.Range(0.0f, 0.0f);
                saccadeClock = 0.0f;
                Vector3 eulerAngles = playerHeadTransform.localEulerAngles;
                if (eulerAngles.z < 180.0f)
                {
                    gHeadRollTarget = Random.Range(5.0f, 40.0f);
                }
                else
                {
                    gHeadRollTarget = Random.Range(-5.0f, -40.0f);
                }

                lookAction = true;
                lookVariation = Random.Range(1.0f, 1.0f);
                browVariation = Random.Range(0.25f, 0.5f);
                eyeVariation = Random.Range(0.25f, 0.5f);
                mouthVariation = Random.Range(0.15f, 0.35f);
                browSM.SwitchRandom(new State[] {
                            bApprehensive,
                            bRaised,
                            bOneRaise
                        });
                mouthSM.Switch(mKiss);
                eyesSM.Switch(eClosed);
                float rand = Random.Range(0.0f, 100.0f);
                if (rand > 33.0f)
                {
                    mLHandFistTarget = Random.Range(0.3f, 0.6f);
                }
                if (rand < 66.0f)
                {
                    mRHandFistTarget = Random.Range(0.3f, 0.6f);
                }
                Duration = Random.Range(0.5f, 0.7f);
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                lookAction = false;
                interestKissing = false;
            }
        }



    }

}