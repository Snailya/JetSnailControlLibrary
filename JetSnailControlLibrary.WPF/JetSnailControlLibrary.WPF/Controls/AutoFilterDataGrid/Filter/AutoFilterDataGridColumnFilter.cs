using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     A control in datagrid column header for adding filter features.
    /// </summary>
    [TemplatePart(Name = PartPopup)]
    public class AutoFilterDataGridColumnFilter : Control
    {
        #region Constructor

        /// <summary>
        ///     Override default style key and provide control template in generic.
        /// </summary>
        static AutoFilterDataGridColumnFilter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AutoFilterDataGridColumnFilter),
                new FrameworkPropertyMetadata(typeof(AutoFilterDataGridColumnFilter)));
        }

        #endregion

        #region Method

        /// <summary>
        ///     When overridden in a derived class, is invoked whenever application code or internal processes (such as a
        ///     rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" />.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get DataGrid to apply filterEx later
            mDataGrid = VisualHelper.TryFindParent<AutoFilterDataGrid>(this);

            // Get template parts
            mPopup = GetTemplateChild(PartPopup) as Popup;
            if (mPopup == null) return;

            // Get binding path
            var bindingPath = string.Empty;
            var columnHeader = VisualHelper.TryFindParent<DataGridColumnHeader>(this);
            if (!(columnHeader?.Column is DataGridBoundColumn boundColumn)) return;
            if (boundColumn.Binding is Binding binding) bindingPath = binding.Path.Path;
            if (string.IsNullOrEmpty(bindingPath) || bindingPath.Contains(".")) return;

            // Get PropertyInfo of the binding
            mDataGridItemsSource = VisualHelper.TryFindParent<DataGrid>(columnHeader).ItemsSource;
            mFieldInfo = mDataGridItemsSource.GetType().GetGenericArguments()[0].GetProperty(bindingPath);

            // XXX: currently only show filter if it is of type string or nothing to filter
            if (mFieldInfo.PropertyType != typeof(string))
            {
                Visibility = Visibility.Hidden;
                return;
            }

            // Create viewmodel and set bindings
            mViewModel = new FilterViewHostViewModel(mFieldInfo, mDataGridItemsSource);
            
            mViewModel.FilterViewHostViewModelChanged += OnFilterViewHostViewModelChanged;
            mPopup.DataContext = mViewModel;
            mPopup.Opened += OnOpened;
            mPopup.Closed += OnClosed;
        }

        private PropertyInfo mFieldInfo;

        private void OnFilterViewHostViewModelChanged(object sender, EventArgs e)
        {
            if (!mDataGrid.FilterExes.ContainsKey(mFieldInfo.Name))
                mDataGrid.FilterExes.Add(mFieldInfo.Name, mViewModel.Views.Select(v => v.ViewModel.FilterEx).ToList());

            RefreshView();
        }

        private void RefreshView()
        {
            // Get the view
            var view = CollectionViewSource.GetDefaultView(mDataGrid.ItemsSource);
            if (view != null)
                // Create a filter
                view.Filter = delegate(object item)
                {
                    // Show the current object
                    var result = true;

                    // Loop filters
                    foreach (var columnFilterExes in mDataGrid.FilterExes.Where(columnFilterExes =>
                        columnFilterExes.Value.ToList().Any(filterEx => !filterEx.IsMatch(item))))
                        result = false;

                    // Return if it's visible or not
                    return result;
                };
        }

        /// <summary>
        ///     store previous to caches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpened(object sender, EventArgs e)
        {
            foreach (var view in mViewModel.Views) view.ViewModel.Cache();
        }

        /// <summary>
        ///     Cancel un-applied selection when popup lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnClosed(object sender, EventArgs e)
        {
            foreach (var view in mViewModel.Views) view.ViewModel.Cancel();
        }

        #endregion

        #region Private Properties

        internal const string PartPopup = "PART_Popup";

        private IEnumerable mDataGridItemsSource;
        private Popup mPopup;
        private AutoFilterDataGrid mDataGrid;
        private FilterViewHostViewModel mViewModel;

        #endregion
    }
}