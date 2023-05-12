using System.ComponentModel;

namespace Model
{
    public interface BallInterface : INotifyPropertyChanged
    {
        public int TopValue { get; }
        public int LeftValue { get; }
        public int BallRadius { get; }
    }
}
