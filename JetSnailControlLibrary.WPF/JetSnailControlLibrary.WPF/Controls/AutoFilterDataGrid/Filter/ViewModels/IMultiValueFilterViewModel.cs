using System.Collections;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Defines a contract for <see cref="MultiValueFilterViewModel{T}" /> class.
    /// </summary>
    public interface IMultiValueFilterViewModel : IFilterViewModel
    {
        /// <summary>
        ///     A collection used to generate the content for <see cref="MultiValueFilterView" /> control.
        /// </summary>
        IEnumerable ItemsSource { get; }
    }
}