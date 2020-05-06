using System;
using System.Reflection;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Represents an filter expression.
    /// </summary>
    public abstract class BaseFilterEx : IFilterEx
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseFilterEx" /> class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        protected BaseFilterEx(PropertyInfo fieldInfo)
        {
            if (fieldInfo == null) throw new ArgumentNullException(nameof(fieldInfo));
            FieldInfo = fieldInfo;
        }

        /// <summary>
        ///     Gets the object representing the public field property that the filter expression to apply on.
        /// </summary>
        public PropertyInfo FieldInfo { get; }

        /// <summary>
        ///     Indicates whether the filter expression finds a match to the input parameter.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public abstract bool IsMatch(object input);
    }
}