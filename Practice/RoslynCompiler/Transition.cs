using System;
using System.Collections.Generic;
using System.Text;

namespace OLD_RoslynCompiler
{
    public class Globals
    {
        public State A;
        public State B;
        public Transition T;
     
        //T = new Transition(A,B);
        
        public class State
        {
            public string Data { get; set; }
            public State(string data) { Data = data; }
            public override string ToString()
            {
                return Data;
            }
        }

        public class Transition
        {

            public State state_a, state_b;
            public Transition() { }
            public Transition(State a, State b) { state_a = a; state_b = b; }

            public override string ToString()
            {
                return string.Format("{0} => {1}", state_a.ToString(), state_b.ToString());
            }
        }
    }
}
