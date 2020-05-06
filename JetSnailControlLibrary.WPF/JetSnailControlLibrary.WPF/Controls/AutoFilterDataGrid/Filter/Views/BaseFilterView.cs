using System;
using System.Windows.Controls;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Represents an base view which can not implement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseFilterView<T> : Control, IFilterView
        where T : class, IFilterViewModel
    {

        public IFilterViewModel ViewModel { get; set; }

        /// <summary>
        ///     Declare an property changed event when viewmodel changed.
        /// </summary>
        public event EventHandler ViewModelChanged = (sender, e) => { };

        /// <summary>
        ///     The event-invoking method that derived classes can override.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnViewModelChanged(EventArgs e)
        {
            ViewModelChanged(this, e);
        }
    }
}