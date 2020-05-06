using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Represents an view model for MultiValueFilterView binding.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiValueFilterViewModel<T> : IMultiValueFilterViewModel
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiValueFilterViewModel{T}" /> class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="itemsSource"></param>
        public MultiValueFilterViewModel(PropertyInfo fieldInfo, IEnumerable itemsSource)
        {
            // Set filter expression
            _mEqualBaseFilterEx = new EqualBaseFilterEx<T>(fieldInfo);

            // set itemsSource
            var distinctValues = itemsSource.Cast<dynamic>().Select(i => fieldInfo.GetValue(i, null)).Distinct();
            if (distinctValues.Count() <= 1) return;

            // generate filter item
            foreach (var value in distinctValues) mItemsSource.Add(new MultiValueFilterItem<T>((T) value));
        }

        #endregion

        #region Event

        /// <summary>
        ///     Declare an property changed event when BaseFilterEx's pattern changed.
        /// </summary>
        public event EventHandler FilterExPropertyChanged = (sender, e) =>
        {
            Debug.WriteLine("Firing FilterExPropertyChanged");
        };

        #endregion


        #region Private Properties

        /// <summary>
        ///     A collection used to generate the content for <see cref="MultiValueFilterView" /> control.
        /// </summary>
        private readonly ObservableCollection<MultiValueFilterItem<T>> mItemsSource =
            new ObservableCollection<MultiValueFilterItem<T>>();

        /// <summary>
        ///     A collection used to restore user selected caches for <see cref="MultiValueFilterView" /> control.
        /// </summary>
        private readonly ObservableCollection<MultiValueFilterItem<T>> mPrevious =
            new ObservableCollection<MultiValueFilterItem<T>>();

        /// <summary>
        ///     An object of type <see cref="EqualBaseFilterEx{T}" /> representing equality filter expression to use.
        /// </summary>
        private readonly EqualBaseFilterEx<T> _mEqualBaseFilterEx;

        #endregion

        #region Method

        /// <summary>
        ///     Restore.
        /// </summary>
        public void Cancel()
        {
            if (mPrevious.Count == 0) return;

            mItemsSource.Clear();
            foreach (var item in mPrevious)
                mItemsSource.Add(new MultiValueFilterItem<T>(item.Value) {IsSelected = item.IsSelected});
        }

        /// <summary>
        ///     Store previous selection.
        /// </summary>
        public void Cache()
        {
            mPrevious.Clear();

            foreach (var item in mItemsSource)
                mPrevious.Add(new MultiValueFilterItem<T>(item.Value) {IsSelected = item.IsSelected});
        }

        /// <summary>
        ///     Update BaseFilterEx.
        /// </summary>
        public void Apply()
        {
            _mEqualBaseFilterEx.Patterns.Clear();

            foreach (var item in mItemsSource)
                if (item.IsSelected)
                    _mEqualBaseFilterEx.Patterns.Add(item.Value);

            mPrevious.Clear();
        }

        /// <summary>
        ///     Clear and Update BaseFilterEx
        /// </summary>
        public void Clear()
        {
            foreach (var item in mItemsSource)
                item.IsSelected = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     An object of type <see cref="IFilterEx" /> representing which filter expression to use.
        /// </summary>
        public IFilterEx FilterEx => _mEqualBaseFilterEx;


        /// <summary>
        ///     A collection used to generate the content for <see cref="MultiValueFilterView" /> control.
        /// </summary>
        public IEnumerable ItemsSource => mItemsSource;

        #endregion
    }
}