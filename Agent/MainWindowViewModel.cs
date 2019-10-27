using System;
using System.Collections;
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

        private bool _generationEnabled = true;

        public bool GenerationEnabled
        {
            set
            {
                _generationEnabled = value;
                OnPropertyChanged();
            }
            get { return _generationEnabled; }
        }

        private bool _startEnabled = false;

        public bool StartEnabled
        {
            set
            {
                _startEnabled = value;
                OnPropertyChanged();
            }
            get { return _startEnabled; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private RelayCommand generateRandomCommand;

        public RelayCommand GenerateRandomCommand
        {
            get
            {
                return generateRandomCommand ?? (generateRandomCommand = new RelayCommand(obj =>
                {
                    StartEnabled = true;
                    ActionField = new ActionField(ActionFieldHeight, ActionFieldWidth, CookiesCount);
                }));
                
            }
        }

        private RelayCommand generateTestCommand;

        public RelayCommand GenerateTestCommand
        {
            get
            {
                

                return generateTestCommand ?? (generateTestCommand = new RelayCommand(obj =>
                {
                    StartEnabled = true;

                    string[] fieldPrototype = new[]
                    {
                        //"F# C",
                        //"    ",
                        //"A #C",
                        //" C##",
                        "    ",
                        "  C#",
                        "  C ",
                        "CFA ",
                    };

                    ActionField = new ActionField(fieldPrototype);
                }));
            }
        }

        private RelayCommand _startCommand;

        public RelayCommand StartCommand
        {
            get { return _startCommand ?? (_startCommand = new RelayCommand(obj =>
            {
                GenerationEnabled = false;
                // StartSolvationProcess(); 
                Animate();
            })); }
        }

        private void StartSolvationProcess()
        {
            Task.Run(() =>
            {
                var solver = new BfsSolver();
                var solution = solver.Solve(ActionField);

                Point previousPoint = null;
                NodeType previousNodeType = NodeType.Agent;
                //foreach (var node in solution.Route.Nodes)
                //{
                //    if (previousPoint != null)
                //    {
                //        ActionField.UpdateFieldNodeType(previousPoint, previousNodeType);
                //    }
                //    previousNodeType = ActionField.Nodes.GetNode(point).NodeType; 
                //    previousPoint = point;

                //    ActionField.UpdateFieldNodeType(point, NodeType.Agent);

                //    Thread.Sleep(1000);
                //}
            });
        }

        private void Animate()
        {
            Task.Run(() =>
            {
                
                for (int i = 0; i < CookiesCount; i++)
                {
                    GotoObject(NodeType.Cookie);
                }

                GotoObject(NodeType.Star);
            });
        }

        private void GotoObject(NodeType nodeType)
        {
            var solver = new BfsSolver();

            
            var solution = solver.FindWay(ActionField, nodeType);

            Node reservedNode = ActionField.Nodes.Single(n => n.NodeType == NodeType.Agent);
            reservedNode.NodeType = NodeType.Gross;
            Node reservedNode2 = null; 
            foreach (var node in solution.Route.Nodes)
            {
                ActionField.UpdateFieldNodeType(node.Point, NodeType.Agent);
                if(reservedNode2 != null)
                {
                    ActionField.UpdateFieldNodeType(reservedNode2.Point, reservedNode2.NodeType);
                }
                else
                {
                    ActionField.UpdateFieldNodeType(reservedNode.Point, NodeType.Gross);

                }
                reservedNode = node;
                reservedNode2 = reservedNode;
                Thread.Sleep(1000);
            }
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
