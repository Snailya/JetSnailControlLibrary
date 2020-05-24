using System;
using System.Diagnostics;
using System.Reflection;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Represents an view model for MultiValueFilterView binding.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StringFilterViewModel : IStringFilterViewModel
    {
        #region Private Properties

        /// <summary>
        ///     An object of type <see cref="StringBaseFilterEx" /> representing equality filter expression to use.
        /// </summary>
        private readonly StringBaseFilterEx _mStringBaseFilterEx;

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiValueFilterViewModel{T}" /> class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="subString"></param>
        public StringFilterViewModel(PropertyInfo fieldInfo, string subString)
        {
            // Set filter expression
            _mStringBaseFilterEx = new StringBaseFilterEx(fieldInfo, subString) {Substring = subString};
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

        #region Method

        public void Cancel()
        {
            // todo
        }

        public void Cache()
        {
            //
        }

        public void Apply()
        {
            //
        }

        public void Clear()
        {
            //
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     An object of type <see cref="IFilterEx" /> representing which filter expression to use.
        /// </summary>
        public IFilterEx FilterEx => _mStringBaseFilterEx;

        /// <summary>
        ///     The substring to check for contain
        /// </summary>
        public string Substring
        {
            get => _mStringBaseFilterEx.Substring;
            set
            {
                if (value == _mStringBaseFilterEx.Substring) return;
                _mStringBaseFilterEx.Substring = value;

                // Raise FilterExPropertyChanged Event
                FilterExPropertyChanged?.Invoke(_mStringBaseFilterEx, new EventArgs());
            }
        }

        #endregion
    }
}