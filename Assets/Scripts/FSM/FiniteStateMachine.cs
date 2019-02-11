using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fsm
{
    class FSMParameters
    {
        public Dictionary<string, bool> boolParameters = new Dictionary<string, bool>();
        public Dictionary<string, bool> triggerParameters = new Dictionary<string, bool>();
        public Dictionary<string, int> intParameters = new Dictionary<string, int>();
        public Dictionary<string, float> floatParameters = new Dictionary<string, float>();
    }
    public class FiniteStateMachine<T>
    {
        private State<T> currentState = null;
        private FSMParameters parameters = new FSMParameters();

        public void AddBool(string name)
        {
            parameters.boolParameters.Add(name, false);
        }

        public void AddTrigger(string name)
        {
            parameters.triggerParameters.Add(name, false);
        }

        public void AddInt(string name)
        {
            parameters.intParameters.Add(name, 0);
        }

        public void AddFloat(string name)
        {
            parameters.floatParameters.Add(name, 0f);
        }

        public bool GetBool(string name)
        {
            foreach (KeyValuePair<string, bool> parameter in parameters.boolParameters)
                if (parameter.Key == name)
                    return parameter.Value;

            return false;
        }

        public bool GetTrigger(string name)
        {
            bool value = parameters.triggerParameters[name];
            parameters.triggerParameters[name] = false;
            return value;
        }

        public int GetInt(string name)
        {
            foreach (KeyValuePair<string, int> parameter in parameters.intParameters)
                if (parameter.Key == name)
                    return parameter.Value;

            return 0;
        }

        public float GetFloat(string name)
        {
            foreach (KeyValuePair<string, float> parameter in parameters.floatParameters)
                if (parameter.Key == name)
                    return parameter.Value;

            return 0f;
        }

        public void SetBool(string name, bool value)
        {
            parameters.boolParameters[name] = value;
        }

        public void SetInt(string name, int value)
        {
            parameters.intParameters[name] = value;
        }

        public void SetFloat(string name, float value)
        {
            parameters.floatParameters[name] = value;
        }

        public void SetTrigger(string name)
        {
            parameters.triggerParameters[name] = true;
        }

        public void Execute()
        {
            if (currentState == null)
                return;

            State<T> state = currentState.CheckTransitions();
            if (state != null)
                currentState = state;

            currentState.Execute();
        }

        public void AddInitialState(State<T> state)
        {
            currentState = state;
        }

        public State<T> GetCurrentState()
        {
            return currentState;
        }
    }
}