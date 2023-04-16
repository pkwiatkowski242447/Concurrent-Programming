using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class BallChangeEvent : EventArgs
    {
        public BallInterface Ball {  get; internal set; }
    }
}
