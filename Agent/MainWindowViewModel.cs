using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Agent.Algorythms;
using Agent.Annotations;
using Agent.Graphs;
using Agent.Graphs.GraphCreators;
using Agent.Models;
using Agent.Others;
using Agent.Solutions;

namespace Agent
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            ActionFieldHeight = 4;
            ActionFieldWidth = 4;
            CookiesCount = 3;
        }

        private ActionField _actionField;

        public ActionField ActionField
        {
            get { return _actionField; }
            set
            {
                _actionField = value;
                OnPropertyChanged();
            }
        }

        private int _actionFieldHeight;
        private int _actionFieldWidth;
        private int _cookiesCount;
        public int ActionFieldHeight
        {
            set
            {
                _actionFieldHeight = value;
                OnPropertyChanged();
            }
            get { return _actionFieldHeight; }
        }

        public int ActionFieldWidth
        {
            set
            {
                _actionFieldWidth = value;
                OnPropertyChanged();
            }
            get { return _actionFieldWidth; }
        }

        public int CookiesCount
        {
            set
            {
                _cookiesCount = value;
                OnPropertyChanged();
            }
            get { return _cookiesCount; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private RelayCommand generateCommand;

        public RelayCommand GenerateCommand
        {
            get
            {
                return generateCommand ?? (generateCommand = new RelayCommand(obj =>
                {
                    ActionField = new ActionField(ActionFieldHeight, ActionFieldWidth, CookiesCount);
                }));
            }
        }

        private RelayCommand _startCommand;

        public RelayCommand StartCommand
        {
            get { return _startCommand ?? (_startCommand = new RelayCommand(obj => { StartSolvationProcess(); })); }
        }

        private void StartSolvationProcess()
        {
            Task.Run(() =>
            {
                var solver = new BfsSolver();
                var solution = solver.Solve(ActionField);
                var stepsQueue = new Queue<GraphNode>();
                for (int i = solution.Count - 1; i >= 0; i--)
                {
                    stepsQueue.Enqueue(solution[i]);
                }

                GraphNode previousStepNode = null;
                while (stepsQueue.Count > 0)
                {
                    if (previousStepNode != null)
                    {
                        ActionField.UpdateFieldNodeType(previousStepNode.Point, previousStepNode.NodeType);
                    }
                    var nextNode = stepsQueue.Dequeue();
                    ActionField.UpdateFieldNodeType(nextNode.Point, NodeType.Agent);
                    previousStepNode = nextNode;
                    Thread.Sleep(1000);
                }
            });
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
