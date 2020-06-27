using System.Windows;
using System.Windows.Controls;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     A view host for filter views.
    /// </summary>
    public class FilterViewHost : ItemsControl
    {
        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes (such as a
        ///     rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Set Binding
            ItemsSource = (DataContext as FilterPaneViewModel)?.Views;
        }

        #region Constructor

        /// <summary>
        ///     static constructor of the <see cref="FilterViewHost" /> class.
        /// </summary>
        static FilterViewHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FilterViewHost),
                new FrameworkPropertyMetadata(typeof(FilterViewHost)));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterViewHost" /> class.
        /// </summary>
        public FilterViewHost()
        {
            DefaultStyleKey = typeof(FilterViewHost);
        }

        #endregion
    }
}