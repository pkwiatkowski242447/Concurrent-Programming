﻿using Logika;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ViewModel
{
    public class RelayCommand : ICommand
    {
        public RelayCommand(Action execute) : this(execute, null) { }
        public RelayCommand(Action execute, Func<bool> canExcute)
        {
            this.m_Execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.m_CanExecute = canExcute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.m_CanExecute == null)
            {
                return true;
            }
            if (parameter == null)
            {
                return this.m_CanExecute();
            }
            return this.m_CanExecute();
        }

        public virtual void Execute(object parameter)
        {
            this.m_Execute();
        }

        public event EventHandler? CanExecuteChanged;

        internal void RaiseCanExcuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        private readonly Action m_Execute;
        private readonly Func<bool> m_CanExecute;
    }
}
