using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows.Input;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Represents an view model for FilterViewHost binding.
    /// </summary>
    public class FilterPaneViewModel
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="FilterPaneViewModel" /> class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="itemsSource"></param>
        public FilterPaneViewModel(PropertyInfo fieldInfo, IEnumerable itemsSource)
        {
            // Binding commands
            ApplyFilterCommand = new RelayCommand(param => ApplyFilterCommandExecute());
            ClearCommand = new RelayCommand(param => ClearCommandExecute());

            // Binding views and view models
            InitializeViews(fieldInfo, itemsSource);
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Represents a collection of <see cref="IFilterView" /> the filter view host hosts.
        /// </summary>
        public IList<IFilterView> Views { get; private set; }

        #endregion

        #region Event

        /// <summary>
        ///     Notify when new filter expressions are applied.
        /// </summary>
        public event EventHandler FilterApplied = (sender, e) => { };

        #endregion

        #region Command

        /// <summary>
        ///     The command to apply current filter
        /// </summary>
        public ICommand ApplyFilterCommand { get; set; }

        /// <summary>
        ///     The command to apply current filter
        /// </summary>
        public ICommand ClearCommand { get; set; }

        #endregion

        #region Method

        /// <summary>
        ///     Initialize views and view models for view host
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="itemsSource"></param>
        private void InitializeViews(PropertyInfo fieldInfo, IEnumerable itemsSource)
        {
            // Add multivalue filter view
            Views = new List<IFilterView> {new MultiValueFilterView(fieldInfo, itemsSource)};

            //// Add string filter view if it's of type string
            //if (fieldInfo.PropertyType == typeof(string))
            //    Views.Add(new StringFilterView(fieldInfo, string.Empty));
        }

        /// <summary>
        ///     store current filterEx and refresh AutoFilterDataGrid's view
        /// </summary>
        public void ApplyFilterCommandExecute()
        {
            foreach (var view in Views) view.ViewModel.Apply();
            FilterApplied(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Clear any applied filterEx.
        /// </summary>
        public void ClearCommandExecute()
        {
            foreach (var view in Views) view.ViewModel.Clear();

            ApplyFilterCommandExecute();
        }

        #endregion
    }
}