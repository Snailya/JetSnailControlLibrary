using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace JetSnailControlLibrary.WPF
{
    public class ControlProperties
    {
        #region AllowNaturalSortProperty

        /// <summary>
        ///     The AttachedProperty for the UseNaturalSort property.
        /// </summary>
        public static readonly DependencyProperty UseNaturalSortProperty =
            DependencyProperty.RegisterAttached("UseNaturalSort", typeof(bool), typeof(ControlProperties),
                new PropertyMetadata(default(bool), OnUseNaturalSortPropertyChanged));

        /// <summary>
        ///     UseNaturalSort property getter.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static bool GetUseNaturalSort(DependencyObject element)
        {
            return (bool) element.GetValue(UseNaturalSortProperty);
        }

        /// <summary>
        ///     UseNaturalSort property setter.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="value"></param>
        public static void SetUseNaturalSort(DependencyObject element, bool value)
        {
            element.SetValue(UseNaturalSortProperty, value);
        }

        /// <summary>
        ///     Add a natural sorting handle for DataGrid if OnUseNaturalSort is set to true.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnUseNaturalSortPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((bool) e.NewValue == false)
                return;

            if (!(d is DataGrid dataGrid)) return;

            dataGrid.Sorting -= HandleNaturalSorting;
            dataGrid.Sorting += HandleNaturalSorting;
        }

        /// <summary>
        ///     Create an IComparer for multi DataGrid columns and assign to CustomSort property of DataGrid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HandleNaturalSorting(object sender, DataGridSortingEventArgs e)
        {
            var dataGrid = (AutoFilterDataGrid) sender;

            var listCollectionView = (ListCollectionView) CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);

            listCollectionView.CustomSort = new DataGridColumnNaturalComparer(dataGrid.SortedColumns);

            e.Handled = true;
        }

        #endregion

        #region IsBusyProperty

        /// <summary>
        ///     The IsBusy attached property for a anything that wants to flag if the control is busy.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.RegisterAttached("IsBusy", typeof(bool),
                typeof(ControlProperties),
                new UIPropertyMetadata(false));


        /// <summary>
        ///     Sets the IsBusy
        /// </summary>
        public static void SetIsBusy(DependencyObject obj, bool value)
        {
            obj.SetValue(IsBusyProperty, value);
        }

        /// <summary>
        ///     Gets the IsBusy
        /// </summary>
        public static bool GetIsBusy(DependencyObject obj)
        {
            return (bool) obj.GetValue(IsBusyProperty);
        }

        #endregion

        #region GroupByProperty

        /// <summary>
        ///     The GroupBy attached property for a anything that wants a grouped view to display.
        /// </summary>
        public static readonly DependencyProperty GroupByProperty =
            DependencyProperty.RegisterAttached("GroupBy", typeof(string),
                typeof(ControlProperties),
                new UIPropertyMetadata(string.Empty, OnGroupByPropertyChanged));

        /// <summary>
        ///     Add group description to the view when GroupBy property changed.
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnGroupByPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Make sure it is a list box
            if (!(d is ListBox listBox)) return;

            var myView = (CollectionView) CollectionViewSource.GetDefaultView(listBox.ItemsSource);
            if (myView == null) return;

            myView.GroupDescriptions.Clear();

            if (string.IsNullOrEmpty((string) e.NewValue)) return;

            if (!myView.CanGroup) return;
            var groupDescription
                = new PropertyGroupDescription((string) e.NewValue);
            myView.GroupDescriptions.Add(groupDescription);
        }

        /// <summary>
        ///     Sets the GroupBy property
        /// </summary>
        public static void SetGroupBy(DependencyObject obj, string value)
        {
            obj.SetValue(GroupByProperty, value);
        }

        /// <summary>
        ///     Gets the GroupBy property
        /// </summary>
        public static string GetGroupBy(DependencyObject obj)
        {
            return (string) obj.GetValue(GroupByProperty);
        }

        #endregion

        #region AttachedTemplateProperty for combobox

        /// <summary>
        ///     The AttachedTemplate attached property for a Combobox that that want's to has a different template of selected
        ///     item.
        /// </summary>
        public static readonly DependencyProperty AttachedTemplateProperty =
            DependencyProperty.RegisterAttached("AttachedTemplate", typeof(ControlTemplate),
                typeof(ControlProperties),
                new FrameworkPropertyMetadata(null));


        /// <summary>
        ///     Sets the AttachedTemplate.
        /// </summary>
        public static void SetAttachedTemplate(DependencyObject obj, Style value)
        {
            obj.SetValue(AttachedTemplateProperty, value);
        }

        /// <summary>
        ///     Gets the AttachedTemplate.
        /// </summary>
        public static Style GetAttachedTemplate(DependencyObject obj)
        {
            return (Style) obj.GetValue(AttachedTemplateProperty);
        }

        #endregion
    }
}