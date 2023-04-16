using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public interface BallInterface
    {
        public int TopValue { get; }
        public int LeftValue { get; }
        public int BallRadius { get; }
    }
}
