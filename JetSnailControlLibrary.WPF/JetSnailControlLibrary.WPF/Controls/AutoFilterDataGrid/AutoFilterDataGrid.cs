using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     A control in DataGrid column header for adding filter features.
    /// </summary>
    [TemplatePart(Name = "DG_Popup")]
    [TemplatePart(Name = "DG_ScrollViewer")]
    public class AutoFilterDataGrid : DataGrid
    {
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // get popup control
            _popup = GetTemplateChild("DG_Popup") as Popup;
            _scrollViewer = GetTemplateChild("DG_ScrollViewer") as ScrollViewer;

            // cache the default view and add collection change event
            Source = new CollectionViewSource {Source = ItemsSource};
        }


        #region Constructor

        public AutoFilterDataGrid()
        {
            #region Sort

            Sorting += OnSorting;

            #endregion

            #region Drag and Drop

            BeginningEdit += (s1, e1) =>
            {
                _isEditing = true;
                //in case we are in the middle of a drag/drop operation, cancel it...
                if (_isDragging) ResetDragDrop();
            };

            CellEditEnding += (s2, e2) => { _isEditing = false; };
            RowEditEnding += (s3, e3) => { _isEditing = false; };

            PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            MouseLeftButtonUp += OnMouseLeftButtonUp;
            MouseMove += OnMouseMove;

            #endregion
        }

        #region Filter

        /// <summary>
        ///     This dictionary will have a list of all applied filters
        /// </summary>
        public Dictionary<string, IList<IFilterEx>> AppliedFilterExpressions { get; } =
            new Dictionary<string, IList<IFilterEx>>();

        public bool IsFiltering;

        #endregion

        #region Sort

        private bool _sorted;

        /// <summary>
        ///     The DependencyProperty for the SortIndicatorColor property.
        /// </summary>
        public static readonly DependencyProperty SortIndicatorColorProperty =
            DependencyProperty.Register("SortIndicatorColor", typeof(Color), typeof(AutoFilterDataGrid),
                new FrameworkPropertyMetadata(Colors.Transparent));

        /// <summary>
        ///     A dependency property that represents the color used for indicate sorting order.
        /// </summary>
        public Color SortIndicatorColor
        {
            get => (Color) GetValue(SortIndicatorColorProperty);
            set => SetValue(SortIndicatorColorProperty, value);
        }

        /// <summary>
        ///     Cache the columns on sorting, and update indicator's color.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSorting(object sender, DataGridSortingEventArgs e)
        {
            var column = e.Column;

            if (SortedColumns.Contains(column))
                SortedColumns.Remove(column);
            else
                SortedColumns.Add(column);

            _sorted = SortedColumns.Count != 0;

            UpdateSortIndicator();
        }

        /// <summary>
        ///     Indicate sort order by opacity of SortIndicatorColor property.
        /// </summary>
        private void UpdateSortIndicator()
        {
            // find column headers from visual
            var columnHeaders = VisualHelper.TryFindChildren<DataGridColumnHeader>(this);

            // loop headers, set header background color if it is cached in SortColumns property;
            // set to transparent if not.
            foreach (var columnHeader in columnHeaders)
                if (SortedColumns.Contains(columnHeader.Column))
                {
                    var i = SortedColumns.IndexOf(columnHeader.Column) + 1;

                    var brush = new SolidColorBrush(SortIndicatorColor) {Opacity = i / (double) columnHeaders.Count};
                    columnHeader.Background = brush;
                }
                else
                {
                    columnHeader.Background = new SolidColorBrush(Colors.Transparent);
                }
        }

        #endregion

        #endregion

        #region Public Properties

        public Predicate<object> PreviousFilter;

        public CollectionViewSource Source;

        public List<DataGridColumn> SortedColumns = new List<DataGridColumn>();

        #endregion

        #region Drag and Drop

        private Popup _popup;
        private ScrollViewer _scrollViewer;

        private bool _isDragging;
        private bool _isEditing;

        /// <summary>
        ///     The DependencyProperty for the DraggedItem property.
        /// </summary>
        public static readonly DependencyProperty DraggedItemProperty =
            DependencyProperty.Register("DraggedItem", typeof(object), typeof(AutoFilterDataGrid));

        /// <summary>
        ///     A dependency property that represents the object being dragging.
        /// </summary>
        public object DraggedItem
        {
            get => GetValue(DraggedItemProperty);
            set => SetValue(DraggedItemProperty, value);
        }

        /// <summary>
        ///     Move the popup with mouse position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging || e.LeftButton != MouseButtonState.Pressed) return;

            // delete selected item from DataGrid
            (ItemsSource as IList)?.Remove(DraggedItem);

            //display the popup if it hasn't been opened yet
            if (!_popup.IsOpen)
            {
                //switch to read-only mode
                IsReadOnly = true;

                //make sure the popup is visible
                _popup.IsOpen = true;
            }

            // set popup size
            _popup.Width = _scrollViewer.ViewportWidth;
            _popup.Height = RowHeight;

            // set placement rectangle
            var pos = e.GetPosition(_scrollViewer);
            pos.X = 0;
            pos.Y = pos.Y - _popup.Height / 2;
            _popup.PlacementRectangle = new Rect(pos, new Size(_popup.Width, _popup.Height));

            //make sure the row under is being selected
            var position = e.GetPosition(_scrollViewer);
            var row = VisualHelper.TryFindByPoint<DataGridRow>(_scrollViewer, position);
            if (row != null) SelectedItem = row.Item;
        }

        /// <summary>
        ///     Create a popup content with item for dragging to notify user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_isEditing || _sorted || IsFiltering) return;

            // find the clicked row
            var row = VisualHelper.TryFindByPoint<DataGridRow>((UIElement) sender,
                e.GetPosition(this));

            if (row == null) return;

            // set flag that indicates we're capturing mouse movements
            _isDragging = true;

            // store to dependency property so that can be used by other control
            DraggedItem = row.Item;

            // add dragged item to popup
            var border = new Border();
            _popup.Child = border;

            border.Child = new AutoFilterDataGrid
            {
                Opacity = 0.9,
                HeadersVisibility = DataGridHeadersVisibility.None,
                ItemsSource = new ObservableCollection<object> {row.Item},
                HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden
            };
        }

        /// <summary>
        ///     Drop the dragging item to mouse position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDragging || _isEditing || _sorted || SelectedItem == null) return;

            var targetItem = SelectedItem;

            if (targetItem == null || !ReferenceEquals(DraggedItem, targetItem))
            {
                //remove the source from the list
                if (ItemsSource is IList itemsSource)
                {
                    //get target index
                    var targetIndex = itemsSource.IndexOf(targetItem);

                    //move source at the target's location
                    itemsSource.Insert(targetIndex, DraggedItem);

                    // fresh view
                    CollectionViewSource.GetDefaultView(itemsSource).Refresh();
                }

                //select the dropped item
                SelectedItem = DraggedItem;
            }

            //reset
            ResetDragDrop();
        }

        /// <summary>
        ///     Closes the popup and resets the grid to read-enabled mode.
        /// </summary>
        private void ResetDragDrop()
        {
            _popup.IsOpen = false;
            _isDragging = false;
            _isEditing = false;
            IsReadOnly = false;
        }

        #endregion
    }
}