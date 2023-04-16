using Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class UserActionHandler : INotifyPropertyChanged
    {
        private readonly int widthOfTheTable = 754;
        private readonly int heightOfTheTable = 748;

        private ModelAbstractAPI ModelAPI;
        private int numberOfSelectedBalls = 0;

        public ObservableCollection<ModelBall> ListOfObservableBalls { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand? StartSimulation { get; set; }
        public ICommand? EndSimulation { get; set; }
        public string? NumberOfBalls { get; set; }
        private bool StartButtonDisabled { get; set; }
        private bool EndButtonDisable { get; set; }
        public bool _Start 
        {
            get { return this.StartButtonDisabled; }
            set
            {
                this.StartButtonDisabled = value;
            }
        }
        public bool _End
        {
            get { return this.EndButtonDisable; }
            set
            {
                this.EndButtonDisable = value;
            }
        }

        public UserActionHandler()
        {
            ModelAPI = ModelAbstractAPI.CreateModelAPIInstance(widthOfTheTable, heightOfTheTable);
            ListOfObservableBalls = new ObservableCollection<ModelBall>();
            StartButtonDisabled = true;
            EndButtonDisable = false;

            StartSimulation = new RelayCommand(() => StartSimulationHandler());
            EndSimulation = new RelayCommand(() => EndSimulationHandler());
        }

        public void StartSimulationHandler()
        {
            StartButtonDisabled = false;
            EndButtonDisable = true;
            int selectedNumberOfBalls = ParseEnteredStringToInt();
            ModelAPI.MoveGeneratedBalls(selectedNumberOfBalls);
            for (int i = 0; i < selectedNumberOfBalls; i++)
            {
                ListOfObservableBalls.Add(ModelAPI.GetModelBall(i));
            }
        }

        public void EndSimulationHandler()
        {
            ModelAPI.ClearPoolTable();
            ListOfObservableBalls.Clear();
            StartButtonDisabled = true;
            EndButtonDisable = false;
        }

        public int ParseEnteredStringToInt()
        {
            if (int.TryParse(NumberOfBalls, out int SomeOutput) && NumberOfBalls != "0")
            {
                return int.Parse(NumberOfBalls);
            }
            return 0;
        }

        private void NotifyPropertyChanged([CallerFilePath] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}