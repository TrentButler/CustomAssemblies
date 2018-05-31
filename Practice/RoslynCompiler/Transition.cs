using System;
using System.Collections.Generic;
using System.Text;

namespace RoslynCompiler
{
    public class Globals
    {
        public State A;
        public State B;
        public Transition T;
     
        //T = new Transition(A,B);
        
        public class State
        {
            public State() { }
        }

        public class Transition
        {

            public State state_a, state_b;
            public Transition() { }
            public Transition(State a, State b) { state_a = a; state_b = b; }

            public override string ToString()
            {
                return string.Format("{0} => {1}", "trent", "is a beast.jpg");
            }
        }
        public override string ToString()
        {
            return string.Format("{0} => {1}", "trent", "beast.jpg");
        }
    }
}
