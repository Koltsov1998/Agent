using System.ComponentModel;
using System.Runtime.CompilerServices;
using Agent.Annotations;
using Agent.Models;

namespace Agent.Components
{
    public class VisualFieldViewModel : INotifyPropertyChanged
    {
        private VisualField _visual;
        private ActionField _actionField;

        public ActionField ActionField
        {
            set
            {
                OnPropertyChanged();
                _actionField = value;
                //_visual.RenderActionField(value);
            }
            get { return _actionField; }
        }

        public VisualFieldViewModel(VisualField visual)
        {
            _visual = visual;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
