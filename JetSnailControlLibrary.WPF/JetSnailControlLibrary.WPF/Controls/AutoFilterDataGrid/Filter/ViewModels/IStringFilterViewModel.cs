namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Defines a common contract for viewmodel interfaces.
    /// </summary>
    public interface IStringFilterViewModel : IFilterViewModel
    {
        /// <summary>
        ///     The substring to check for contain
        /// </summary>
        string Substring { get; set; }
    }
}