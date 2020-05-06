using System.Collections.Generic;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Defines a contract for <see cref="EqualBaseFilterEx{T}" /> class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMultiValueFilterEx<T> : IFilterEx
    {
        /// <summary>
        ///     Gets the available patterns for equality checks.
        /// </summary>
        IList<T> Patterns { get; }
    }
}