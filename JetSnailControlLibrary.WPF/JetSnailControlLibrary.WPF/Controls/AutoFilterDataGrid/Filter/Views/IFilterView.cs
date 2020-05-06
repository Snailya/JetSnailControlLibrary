namespace JetSnailControlLibrary.WPF
{
    public interface IFilterView
    {
        /// <summary>
        ///     An object of type <see cref="IFilterViewModel" /> representing view's viewmodel.
        /// </summary>
        IFilterViewModel ViewModel { set; get; }
    }
}