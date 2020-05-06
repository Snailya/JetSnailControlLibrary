using System.ComponentModel;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Represents an available pattern for filter expression, used as an item of ItemsSource for MultiValueFilterView
    ///     binding.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiValueFilterItem<T> : IMultiValueFilterItem<T>
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiValueFilterItem{T}" /> class.
        /// </summary>
        /// <param name="value"></param>
        public MultiValueFilterItem(T value)
        {
            Value = value;
        }

        #endregion

        #region Event

        /// <summary>
        ///     Declare an property changed event when IsSelected value changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

        #region Private Properties

        /// <summary>
        ///     A boolean representing whether this item is selected by user.
        /// </summary>
        private bool mIsSelected;

        /// <summary>
        ///     An object representing this item's value.
        /// </summary>
        object IMultiValueFilterItem.Value => Value;

        #endregion

        #region Public Proeprties

        /// <summary>
        ///     A boolean representing whether this item is selected by user.
        ///     Raise PropertyChangedEvent if value changed.
        /// </summary>
        public bool IsSelected
        {
            get => mIsSelected;
            set
            {
                if (mIsSelected == value) return;

                mIsSelected = value;

                // To notify multivaluefilter view
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        /// <summary>
        ///     An object of type T that represents this item's value.
        /// </summary>
        public T Value { get; }

        #endregion
    }
}