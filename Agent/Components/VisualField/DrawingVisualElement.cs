using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Agent.Components
{
    public class DrawingVisualElement : FrameworkElement
    {
        private VisualCollection _children;

        public DrawingVisual drawingVisual;

        public DrawingVisualElement()
        {
            _children = new VisualCollection(this);

            drawingVisual = new DrawingVisual();
            _children.Add(drawingVisual);
        }

        protected override int VisualChildrenCount
        {
            get { return _children.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= _children.Count)
                throw new ArgumentOutOfRangeException();

            return _children[index];
        }
    }
}
