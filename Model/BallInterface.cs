using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Model
{
    public interface BallInterface : INotifyPropertyChanged
    {
        int ball_x { get; }
        int ball_y { get; }
        int radius { get; }
    }
}
