using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OpenPaint.Collections
{
    class NotifyUIElementCollection:UIElementCollection, INotifyCollectionChanged
    {

        public NotifyUIElementCollection(UIElement visualParent, FrameworkElement logicalParent)
            :base(visualParent, logicalParent)
        {

        }
 
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected void NotifyCollectionChanged(NotifyCollectionChangedAction action)
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action));
            }
        }

        public override int Add(System.Windows.UIElement element)
        {
            int result = base.Add(element);

            NotifyCollectionChanged(NotifyCollectionChangedAction.Add);

            return result;
        }
    }
}
