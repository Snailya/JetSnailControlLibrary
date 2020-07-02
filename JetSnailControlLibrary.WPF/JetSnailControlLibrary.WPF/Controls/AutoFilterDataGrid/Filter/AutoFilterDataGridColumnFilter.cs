using System;
using System.Collections;
using System.Collections.Generic;
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
        ///     This method raises whenever DataGrid's ItemsSource is reset. Re-generate view model for filter pane when this
        ///     raises.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            #region Get Elements

            // Get DataGrid to apply filterEx later
            _dataGrid = VisualHelper.TryFindParent<AutoFilterDataGrid>(this);
            _dataGridColumnHeader = VisualHelper.TryFindParent<DataGridColumnHeader>(this);
            _filterPane = GetTemplateChild(PartPopup) as Popup;

            if (_filterPane == null) return;

            // Get binding path
            var bindingPath = string.Empty;
            if (!(_dataGridColumnHeader?.Column is DataGridBoundColumn boundColumn)) return;
            if (boundColumn.Binding is Binding binding) bindingPath = binding.Path.Path;
            if (string.IsNullOrEmpty(bindingPath) || bindingPath.Contains(".")) return;

            // Get PropertyInfo of the binding
            _columnInfo = _dataGrid.ItemsSource.GetType().GetGenericArguments()[0].GetProperty(bindingPath);
            if (_columnInfo == null) return;

            #endregion

            // clear all previous sort
            _dataGrid.SortedColumns.Clear();

            // clear all applied filters because we are going to re-generate viewmodel for filter pane
            var view = CollectionViewSource.GetDefaultView(_dataGrid.ItemsSource);
            _dataGrid.AppliedFilterExpressions.Clear();
            view.Filter = null;

            InitializeFilterPaneViewModel();
            _filterPane.Opened += OnFilterPaneOpened;
            _filterPane.Closed += OnFilterPaneClosed;
        }


        private void InitializeFilterPaneViewModel()
        {
            UpdateFilterPaneViewModel(_dataGrid.ItemsSource);
        }

        private void UpdateFilterPaneViewModel(IEnumerable source)
        {
            var viewModel = new FilterPaneViewModel(_columnInfo, source);
            viewModel.FilterApplied += OnFilterApplied;
            _filterPane.DataContext = viewModel;
        }

        /// <summary>
        ///     Actions taking on filter expression applied.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilterApplied(object sender, EventArgs e)
        {
            if (!_dataGrid.AppliedFilterExpressions.ContainsKey(_columnInfo.Name))
                _dataGrid.AppliedFilterExpressions.Add(_columnInfo.Name,
                    ((FilterPaneViewModel) _filterPane.DataContext).Views.Select(v => v.ViewModel.FilterEx).ToList());

            RefreshView();
        }

        private void RefreshView()
        {
            // Get the view
            var view = CollectionViewSource.GetDefaultView(_dataGrid.ItemsSource);
            if (view == null) return;

            // Create a filter
            view.Filter = delegate(object item)
            {
                // Show the current object
                var result = true;

                // Loop filters
                foreach (var columnFilterExes in _dataGrid.AppliedFilterExpressions.Where(columnFilterExes =>
                    columnFilterExes.Value.ToList().Any(filterEx => !filterEx.IsMatch(item))))
                    result = false;

                // Return if it's visible or not
                return result;
            };

            _dataGrid.PreviousFilter = view.Filter;
        }

        /// <summary>
        ///     store previous to caches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFilterPaneOpened(object sender, EventArgs e)
        {
            _dataGrid.IsFiltering = true;
            foreach (var view in ((FilterPaneViewModel) _filterPane.DataContext).Views) view.ViewModel.Cache();
        }

        /// <summary>
        ///     Cancel un-applied selection when popup lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnFilterPaneClosed(object sender, EventArgs e)
        {
            if (_filterPane.DataContext == null) return;
            foreach (var view in ((FilterPaneViewModel) _filterPane.DataContext).Views) view.ViewModel.Cancel();
            _dataGrid.IsFiltering = false;
        }

        #endregion

        #region Private Properties

        internal const string PartPopup = "PART_Popup";

        private AutoFilterDataGrid _dataGrid;
        private DataGridColumnHeader _dataGridColumnHeader;
        private Popup _filterPane;

        private PropertyInfo _columnInfo;
        private List<dynamic> _source;

        #endregion
    }
}