
using Random = UnityEngine.Random;

namespace VRAdultFun
{
    class StateMachine
    {
        public State CurrentState { get; private set; }
        public State NextState { get; private set; }

        public void Switch(State state)
        {
            //SuperController.LogMessage("Switch: " + state.GetType().ToString());
            NextState = state;
        }

        public void SwitchRandom(State[] states)
        {
            if (states.Length == 0)
                return;
            int i = Random.Range(0, states.Length);
            Switch(states[i]);
        }

        public void OnUpdate()
        {
            if (NextState != null)
            {
                if (CurrentState != null)
                    CurrentState.OnExit();
                CurrentState = NextState;
                NextState = null;
                if (CurrentState != null)
                {
                    CurrentState.Timestamp = System.DateTime.UtcNow.Ticks;
                    CurrentState.OnEnter();
                }
            }

            if (CurrentState != null)
            {
                CurrentState.OnUpdate();
                if (CurrentState.IsTimeout())
                    CurrentState.OnTimeout();
            }
        }

        public void OnInterrupt(string parameter)
        {
            if (CurrentState != null)
                CurrentState.OnInterrupt(parameter);
        }
    }

    class State
    {
        public virtual void OnEnter() { }
        public virtual void OnUpdate() { }
        public virtual void OnExit() { }
        public virtual void OnTimeout() { }
        public virtual void OnInterrupt(string parameter) { }

        // Time since this state got activated (in seconds)
        public float Clock()
        {
            return TimeSince(Timestamp);
        }

        // Should OnTimeout() be triggered?
        public bool IsTimeout()
        {
            return Duration > 0.0f && TimeSince(Timestamp) > Duration;
        }
        //from Mcgrubers ScriptEngine source    
        private float TimeSince(long timestamp)
        {
            long duration = System.DateTime.UtcNow.Ticks - timestamp;
            return (float)new System.TimeSpan(duration).TotalSeconds;
        }

        public float Duration { get; set; }
        public long Timestamp { get; set; }
    }
}
