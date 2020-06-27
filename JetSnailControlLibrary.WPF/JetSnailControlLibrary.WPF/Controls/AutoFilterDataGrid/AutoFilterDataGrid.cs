using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Data;

namespace JetSnailControlLibrary.WPF
{
    public class AutoFilterDataGrid : DataGrid
    {

        public ICollectionView DefaultView;
        public Predicate<object> PreviousFilter;
        public string PreviousFilterAppliedOn;


        public CollectionViewSource Source;

        /// <summary>
        ///     This dictionary will have a list of all applied filters
        /// </summary>
        public Dictionary<string, IList<IFilterEx>> AppliedFilterExpressions { get; } =
            new Dictionary<string, IList<IFilterEx>>();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // cache the default view and add collection change event
            Source = new CollectionViewSource {Source = ItemsSource};
        }



        public event EventHandler ItemsReset;
    }
}