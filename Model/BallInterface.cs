using System.ComponentModel;

namespace Model
{
    public interface BallInterface : INotifyPropertyChanged
    {
        public double TopValue { get; }
        public double LeftValue { get; }
        public double BallRadius { get; }
    }
}
