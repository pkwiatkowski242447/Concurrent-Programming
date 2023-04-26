using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public class ModelBall : BallInterface
    {
        private double topValue;
        public double TopValue
        {
            get { return topValue; }
            set
            {
                if (topValue == value) return;
                topValue = value;
                NotifyPropertyChanged();
            }
        }
        private double leftValue;
        public double LeftValue
        {
            get { return leftValue; }
            set
            {
                if (leftValue == value) return;
                leftValue = value;
                NotifyPropertyChanged();
            }
        }
        public double BallRadius { get; }

        public event PropertyChangedEventHandler ?PropertyChanged;

        public ModelBall(double topValue, double leftValue, double ballRadius)
        {
            this.topValue = topValue;
            this.leftValue = leftValue;
            this.BallRadius = ballRadius;
        }

        public void Move(double topValue, double leftValue)
        {
            this.TopValue = topValue;
            this.LeftValue = leftValue;
        } 

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
