using System;
using System.Collections;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace JetSnailControlLibrary.WPF
{
    [TemplatePart(Name = PartItemsHost, Type = typeof(ItemsControl))]
    public class MultiValueFilterView : BaseFilterView<IMultiValueFilterViewModel>
    {
        #region Method

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes (such as a
        ///     rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get template parts
            mItemsControl = GetTemplateChild(PartItemsHost) as ItemsControl;
            // Binding
            if (mItemsControl != null) mItemsControl.ItemsSource = DataContext as IEnumerable;
        }

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="MultiValueFilterView" /> class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="itemsSource"></param>
        public MultiValueFilterView(PropertyInfo fieldInfo, IEnumerable itemsSource)
        {
            // Add DataContext
            ViewModel = (IMultiValueFilterViewModel) Activator.CreateInstance(
                typeof(MultiValueFilterViewModel<>).MakeGenericType(fieldInfo.PropertyType), fieldInfo,
                itemsSource);
            ViewModel.FilterExPropertyChanged += OnViewModelPropertyChanged;
            DataContext = ViewModel;
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnViewModelPropertyChanged(object sender, EventArgs e)
        {
            // Call the base class event invocation method.
            base.OnViewModelChanged(e);
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// </summary>
        internal const string PartItemsHost = "PART_ItemsHost";

        /// <summary>
        /// </summary>
        private ItemsControl mItemsControl;

        #endregion
    }
}