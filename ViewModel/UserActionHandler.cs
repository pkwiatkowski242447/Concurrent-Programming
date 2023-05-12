using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class UserActionHandler : INotifyPropertyChanged
    {
        private readonly ModelAbstractAPI ModelAPI;

        public ObservableCollection<ModelBallInterface> ListOfObservableBalls { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand? StartSimulation { get; set; }
        public ICommand? EndSimulation { get; set; }
        public string? NumberOfBalls { get; set; }
        private bool StartButtonDisabled { get; set; }
        private bool EndButtonDisable { get; set; }
        public bool Start
        {
            get { return this.StartButtonDisabled; }
            set
            {
                this.StartButtonDisabled = value;
                NotifyPropertyChanged();
            }
        }
        public bool End
        {
            get { return this.EndButtonDisable; }
            set
            {
                this.EndButtonDisable = value;
                NotifyPropertyChanged();
            }
        }

        public UserActionHandler()
        {
            ModelAPI = ModelAbstractAPI.CreateModelAPIInstance();
            ListOfObservableBalls = new ObservableCollection<ModelBallInterface>();
            StartButtonDisabled = true;
            EndButtonDisable = false;

            StartSimulation = new RelayCommand(() => StartSimulationHandler());
            EndSimulation = new RelayCommand(() => EndSimulationHandler());
        }

        public void StartSimulationHandler()
        {
            Start = false;
            End = true;
            int selectedNumberOfBalls = ParseEnteredStringToInt();
            ModelAPI.CreateBalls(selectedNumberOfBalls);
            for (int i = 0; i < selectedNumberOfBalls; i++)
            {
                ListOfObservableBalls.Add(ModelAPI.GetModelBall(i));
            }
            ModelAPI.MoveBalls();
        }

        public void EndSimulationHandler()
        {
            Start = true;
            End = false;
            ModelAPI.ClearPoolTable();
            ListOfObservableBalls.Clear();
        }

        public int ParseEnteredStringToInt()
        {
            if (int.TryParse(NumberOfBalls, out int SomeOutput) && NumberOfBalls != "0")
            {
                return int.Parse(NumberOfBalls);
            }
            return 0;
        }

        private void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}