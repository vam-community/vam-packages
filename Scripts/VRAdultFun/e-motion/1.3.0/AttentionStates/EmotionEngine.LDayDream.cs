using Random = UnityEngine.Random;


namespace VRAdultFun
{
    partial class EmotionEngine
    {
        private class LDayDream : State
        {
            public override void OnEnter()
            {
                peronalityAdjustH = 0.0f;
                peronalityAdjustV = 0.0f;
				sexActionNeckX = 0.0f;
				shoulderUp = Random.Range(0.0f,0.3f);
				currentLook = "Daydream";
                //LogError("Daydream");
                //gHeadSpeed = 7.0f;
                if (Random.Range(0.0f, 100.0f) < 50.0f)
                {
                    gHeadRollTarget = Random.Range(-1.0f, 1.0f);
                }
                else
                {
                    gHeadRollTarget = 0.0f;
                }
                saccadeAmount = Random.Range(0.0f, 5.0f);
                mBrowUpTarget = Random.Range(0.2f, 0.5f);
                saccadeClock = 0.0f;
                lookAction = true;
                lookVariation = Random.Range(0.15f, 0.35f);
                browVariation = Random.Range(0.05f, 0.15f);
                eyeVariation = Random.Range(0.15f, 0.35f);
                mouthVariation = Random.Range(0.15f, 0.25f);
                browSM.SwitchRandom(new State[] {
                            bRaised,
                            bApprehensive,
                            bConcentrate
                        });
                mouthSM.SwitchRandom(new State[] {
                            mOpen,
                            mClosed,
                            mSmile,
                            mBigSmile,
                            mSmirk,
                        });
                eyesSM.SwitchRandom(new State[] {
                            //eClosed,
                            //eClosed,
                            //eClosed,
                            eOpen,
                            eOpen,
                            eFocus,
                            eSquint
                        });
                Duration = Random.Range(2.0f, 3.0f);
                float rand = Random.Range(0.0f, 100.0f);
                if (rand > 33.0f)
                {
                    mLHandStraightenTarget = Random.Range(0.0f, 0.4f);
                    mLHandFistTarget = Random.Range(0.0f, 0.4f);
                }
                if (rand < 66.0f)
                {
                    mRHandStraightenTarget = Random.Range(0.0f, 0.4f);
                    mRHandFistTarget = Random.Range(0.0f, 0.4f);
                }
            }
            public override void OnInterrupt(string parameter)
            {
                OnTimeout();
            }
            public override void OnTimeout()
            {
                lookAction = false;
            }
        }
    }

}