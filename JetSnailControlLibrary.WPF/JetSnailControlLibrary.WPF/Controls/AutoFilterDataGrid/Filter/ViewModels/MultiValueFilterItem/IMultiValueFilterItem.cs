using System.ComponentModel;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Defines a contract for <see cref="IMultiValueFilterItem{T}" /> interface.
    /// </summary>
    public interface IMultiValueFilterItem : INotifyPropertyChanged
    {
        /// <summary>
        ///     A boolean representing whether this item is selected by user.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        ///     An object representing this item's value.
        /// </summary>
        object Value { get; }
    }

    /// <summary>
    ///     Defines a contract for <see cref="MultiValueFilterItem{T}" /> class.
    /// </summary>
    public interface IMultiValueFilterItem<out T> : IMultiValueFilterItem
    {
        /// <summary>
        ///     Mark this as "new" so that the compiler knows that
        ///     this is intended to be the new method that
        ///     does not conflict with the old method of the same name but different return type.
        /// </summary>
        new T Value { get; }
    }
}