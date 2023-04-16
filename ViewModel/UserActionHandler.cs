using Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class UserActionHandler : INotifyPropertyChanged
    {
        private readonly int widthOfTheTable = 758;
        private readonly int heightOfTheTable = 745;

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
                NotifyPropertyChanged();
            }
        }
        public bool _End
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
            ModelAPI = ModelAbstractAPI.CreateModelAPIInstance(widthOfTheTable, heightOfTheTable);
            ListOfObservableBalls = new ObservableCollection<ModelBall>();
            StartButtonDisabled = true;
            EndButtonDisable = false;

            StartSimulation = new RelayCommand(() => StartSimulationHandler());
            EndSimulation = new RelayCommand(() => EndSimulationHandler());
        }

        public void StartSimulationHandler()
        {
            NumberOfBalls = "1";
            _Start = false;
            _End = true;
            int selectedNumberOfBalls = ParseEnteredStringToInt();
            ModelAPI.MoveGeneratedBalls(selectedNumberOfBalls);
            for (int i = 0; i < selectedNumberOfBalls; i++)
            {
                ListOfObservableBalls.Add(ModelAPI.GetModelBall(i));
            }
        }

        public void EndSimulationHandler()
        {
            _Start = true;
            _End = false;
            ModelAPI.ClearPoolTable();
            ListOfObservableBalls.Clear();
        }

        public int ParseEnteredStringToInt()
        {
            if (int.TryParse(NumberOfBalls, out int SomeOutput) && NumberOfBalls != "0")
            {
                Console.WriteLine(NumberOfBalls);
                return int.Parse(NumberOfBalls);
            }
            return 0;
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}