using System;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Defines a common contract for viewmodel interfaces.
    /// </summary>
    public interface IFilterViewModel
    {
        /// <summary>
        ///     An object of type <see cref="IFilterEx" /> representing which filter expression to use.
        /// </summary>
        IFilterEx FilterEx { get; }

        /// <summary>
        ///     Declare an property changed event when BaseFilterEx's pattern changed.
        /// </summary>
        event EventHandler FilterExPropertyChanged;

        /// <summary>
        ///     Restore.
        /// </summary>
        void Cancel();

        /// <summary>
        ///     Store previous.
        /// </summary>
        void Cache();

        /// <summary>
        ///     Update BaseFilterEx.
        /// </summary>
        void Apply();

        /// <summary>
        ///     Clear and Update BaseFilterEx
        /// </summary>
        void Clear();
    }
}