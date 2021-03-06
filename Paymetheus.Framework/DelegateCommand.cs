﻿// Copyright (c) 2016 The btcsuite developers
// Copyright (c) 2016 The Decred developers
// Licensed under the ISC license.  See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Paymetheus.Framework
{
    public class DelegateCommandBase
    {
        public bool Executable { get; set; } = true;

        protected bool ActionExecuting = false;

        public bool CanExecute(object parameter) => Executable && !ActionExecuting;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }

    public class DelegateCommand : DelegateCommandBase, ICommand
    {
        public DelegateCommand(Action execute)
        {
            _execute = execute;
        }

        private readonly Action _execute;

        public void Execute(object parameter) => _execute();
    }

    public class DelegateCommandAsync : DelegateCommandBase, ICommand
    {
        public DelegateCommandAsync(Func<Task> t)
        {
            _task = t;
        }

        private readonly Func<Task> _task;

        public async void Execute(object parameter)
        {
            try
            {
                ActionExecuting = true;
                await _task();
            }
            finally
            {
                ActionExecuting = false;
            }
        }
    }


    public class DelegateCommand<T> : DelegateCommandBase, ICommand
    {
        public DelegateCommand(Action<T> execute)
        {
            _execute = execute;
        }

        private readonly Action<T> _execute;

        public void Execute(object parameter) => _execute((T)parameter);
    }
}
