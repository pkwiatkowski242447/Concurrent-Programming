﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Model
{
    internal class ModelBall : BallInterface
    {
        private int topValue;
        public int TopValue
        {
            get { return topValue; }
            set
            {
                if (topValue == value) return;
                topValue = value;
                NotifyPropertyChanged();
            }
        }
        private int leftValue;
        public int LeftValue
        {
            get { return leftValue; }
            set
            {
                if (leftValue == value) return;
                leftValue = value;
                NotifyPropertyChanged();
            }
        }
        public int BallRadius { get; }

        public event PropertyChangedEventHandler ?PropertyChanged; // Tu być może coś do zmiany

        public ModelBall(int topValue, int leftValue, int ballRadius)
        {
            this.topValue = topValue;
            this.leftValue = leftValue;
            this.BallRadius = ballRadius;
        }

        public void Move(int topValue, int leftValue)
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