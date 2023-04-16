using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model
{
    public interface BallInterface : INotifyPropertyChanged
    {
        public int TopValue { get; }
        public int LeftValue { get; }
        public int BallRadius { get; }
    }
}
