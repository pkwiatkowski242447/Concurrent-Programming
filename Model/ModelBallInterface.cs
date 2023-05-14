using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Model
{
    public abstract class ModelBallInterface : INotifyPropertyChanged
    {
        public abstract double TopValue { get; set; }
        public abstract double LeftValue { get; set; }
        public abstract double BallRadius { get; }

        public static ModelBallInterface CreatModelBall(double topValue, double leftValue, double ballRadius)
        {
            return new ModelBall(topValue, leftValue, ballRadius);
        }

        public abstract void Move(double topValue, double leftValue);

        public abstract event PropertyChangedEventHandler? PropertyChanged;

        private class ModelBall : ModelBallInterface
        {
            private double _TopValue;
            private double _LeftValue;
            public override double TopValue
            {
                get { return _TopValue; }
                set
                {
                    _TopValue = value;
                    NotifyPropertyChanged();
                }
            }

            public override double LeftValue
            {
                get { return _LeftValue; }
                set
                {
                    _LeftValue = value;
                    NotifyPropertyChanged();
                }
            }
            public override double BallRadius { get; }

            public ModelBall(double topValue, double leftValue, double ballRadius)
            {
                this._TopValue = topValue;
                this._LeftValue = leftValue;
                this.BallRadius = ballRadius;
            }

            public override void Move(double topValue, double leftValue)
            {
                this.TopValue = topValue;
                this.LeftValue = leftValue;
            }

            public override event PropertyChangedEventHandler? PropertyChanged;

            private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
