using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Represents a filter expression using equality checks.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EqualBaseFilterEx<T> : BaseFilterEx, IMultiValueFilterEx<T>
    {
        #region Private Properties

        /// <summary>
        ///     Available patterns to check for equality.
        /// </summary>
        private readonly ObservableCollection<T> mPatterns = new ObservableCollection<T>();

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="EqualBaseFilterEx{T}" /> class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        public EqualBaseFilterEx(PropertyInfo fieldInfo) : base(fieldInfo)
        {
            if (fieldInfo.PropertyType != typeof(T))
                throw new ArgumentException(
                    "Invalid property type, the return type is not matching the class generic type");
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets the available patterns for equality checks.
        /// </summary>
        /// <value></value>
        public IList<T> Patterns => mPatterns;

        #endregion

        #region Method

        /// <summary>
        ///     Indicates whether the filter expression finds a match to the input parameter.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>
        ///     <c>true</c> if the specified input is a match; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsMatch(object input)
        {
            // Show all if nothing selected
            if (Patterns.Count == 0) return true;

            if (input == null) return false;
            var value = (T) FieldInfo.GetValue(input, null);
            return mPatterns.Contains(value);
        }

        #endregion
    }
}