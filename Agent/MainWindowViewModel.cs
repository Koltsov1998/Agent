using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Agent.Annotations;
using Agent.Models;

namespace Agent
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            ActionField = new ActionField(8, 8);

            Task.Run(() =>
            {
                for (int i = 0; i < 60; i++)
                {
                    Time += TimeSpan.FromSeconds(1);
                    Thread.Sleep(1000);
                }
            });
        }

        private ActionField _actionField;

        public ActionField ActionField
        {
            get { return _actionField; }
            set
            {
                OnPropertyChanged();
                _actionField = value;
            }
        }

        private DateTime _time;

        public DateTime Time
        {
            set
            {
                OnPropertyChanged();
                _time = value;
            }
            get { return _time; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
