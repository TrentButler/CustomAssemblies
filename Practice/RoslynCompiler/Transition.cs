using System;
using System.Collections.Generic;
using System.Text;

namespace Practice
{
    public class Transition
    {
        public State state_a, state_b;

        public Transition(State a, State b) { state_a = a; state_b = b; }

        public override string ToString()
        {
            return string.Format("{0} => {1}", state_a.ToString(), state_b.ToString());
        }
    }
}
