using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
            _draggedItemIndicator = GetTemplateChild("DG_Popup") as Popup;
            _scrollViewer = GetTemplateChild("DG_ScrollViewer") as ScrollViewer;

            _defaultIsReadOnlyValue = IsReadOnly;

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

            _dragTrigger.Interval = new TimeSpan(0, 0, 0, 0, 800);
            _dragTrigger.Tick += _dragTrigger_Tick;

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

        private Popup _draggedItemIndicator;
        private ScrollViewer _scrollViewer;
        private DataGridRow _draggingRow;

        private bool _isDragging;
        private bool _isEditing;
        private bool _defaultIsReadOnlyValue;

        // This event will be used for tracking if the MouseUp has been received
        private readonly DispatcherTimer _dragTrigger = new DispatcherTimer();

        /// <summary>
        ///     The DependencyProperty for the CanUserDrag property.
        /// </summary>
        public static readonly DependencyProperty CanUserDragProperty =
            DependencyProperty.Register("CanUserDrag", typeof(bool), typeof(AutoFilterDataGrid),
                new FrameworkPropertyMetadata(true));

        /// <summary>
        ///     A dependency property that represents whether user can drag and drop row.
        /// </summary>
        public bool CanUserDrag
        {
            get => (bool) GetValue(CanUserDragProperty);
            set => SetValue(CanUserDragProperty, value);
        }

        /// <summary>
        ///     The DependencyProperty for the DraggedItem property.
        /// </summary>
        public static readonly DependencyProperty DraggingItemProperty =
            DependencyProperty.Register("DraggingItem", typeof(object), typeof(AutoFilterDataGrid));

        /// <summary>
        ///     A dependency property that represents the object being dragging.
        /// </summary>
        public object DraggingItem
        {
            get => GetValue(DraggingItemProperty);
            set => SetValue(DraggingItemProperty, value);
        }

        /// <summary>
        ///     Move the popup with mouse position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            // prevent if CanUserDrag is false
            if (!CanUserDrag) return;

            // only if DraggedItem exist and left button of mouse is pressed while moving, treat it as drag event
            if (_draggingRow == null || e.LeftButton != MouseButtonState.Pressed) return;


            // update position
            var pos = e.GetPosition(_scrollViewer);
            (pos.X, pos.Y) = (0, pos.Y - _draggedItemIndicator.Height / 2);
            _draggedItemIndicator.PlacementRectangle =
                new Rect(pos, new Size(_draggedItemIndicator.Width, _draggedItemIndicator.Height));

            // the popup control will move with our mouse, use SelectedItem of DataGrid as indicator
            // if the mouse is not over the DataGridRow, it can't be a drag event.
            var row = VisualHelper.TryFindByPoint<DataGridRow>((UIElement) sender,
                e.GetPosition(this));
            SelectedItem = row?.Item;

            // NEXT, FOCUS ON MOUSE LEFT BUTTON UP
        }

        /// <summary>
        ///     Create a popup content with item for dragging to notify user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // prevent if CanUserDrag is false
            if (!CanUserDrag) return;

            _isDragging = false;

            // if the user is editing or filtering the DataGrid, do not treat this as a drag event.
            if (_isEditing || IsFiltering) return;

            // if the DataGrid is sorted by columns, why drag to reorder it?
            if (_sorted) return;

            // let's find the clicked row, if the mouse is not over the DataGridRow, it can't be a drag event.
            _draggingRow = VisualHelper.TryFindByPoint<DataGridRow>((UIElement) sender,
                Mouse.GetPosition(this));
            if (_draggingRow == null) return;


            // it's now very likely a drag event after rule out editing, filtering or sorted, in addition to the click happens on the DataGridRow. But it still could be a click on button rather than perform dragging. Drag event is only triggered after long pressed.
            _dragTrigger.Start();
        }

        private void _dragTrigger_Tick(object sender, EventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed) return;

            // if mouse has already moved over other row , it's not a drag event
            if (!ReferenceEquals(_draggingRow, VisualHelper.TryFindByPoint<DataGridRow>(this,
                Mouse.GetPosition(this))))
                return;

            // Therefore, to make it a drag event, we should combine move event. However, we still store this row as DraggedItem in case that it is surely a drag event. 
            DraggingItem = _draggingRow.Item;

            // dragging start

            // also initialize a DataGrid to host this DraggedItem in popup control
            // add dragged item to popup
            var border = new Border
            {
                Child = new TextBlock {Text = DraggingItem.ToString()},
                Background = _draggingRow.Background,
                BorderThickness = new Thickness(1),
                BorderBrush = _draggingRow.BorderBrush,
                Padding = _draggingRow.Padding
            };
            _draggedItemIndicator.Child = border;

            // set popup size
            _draggedItemIndicator.Width = _scrollViewer.ViewportWidth;
            _draggedItemIndicator.Height = RowHeight;

            // starting dragging event and capture mouse
            _isDragging = true;
            IsReadOnly = true;
            Mouse.Capture(this);

            // display popup as indicator
            var pos = Mouse.GetPosition(_scrollViewer);
            (pos.X, pos.Y) = (0, pos.Y - _draggedItemIndicator.Height / 2);
            _draggedItemIndicator.PlacementRectangle = new Rect(pos,
                new Size(_draggedItemIndicator.Width, _draggedItemIndicator.Height));

            // show indicator and hide origin row
            _draggedItemIndicator.IsOpen = true;
            _draggingRow.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        ///     Drop the dragging item to mouse position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragTrigger.Stop();

            // prevent if CanUserDrag is false
            if (!CanUserDrag) return;

            // if it's dragging event
            if (_isDragging)
                // if the current DataGridRow is DraggedItem, nothing happens
                if (SelectedItem != null && !ReferenceEquals(DraggingItem, SelectedItem))
                    // otherwise, delete the DraggedItem from ItemsSource and add to new position
                    if (ItemsSource is IList itemsSource)
                    {
                        // get index
                        var draggingItemIndex = itemsSource.IndexOf(DraggingItem);
                        var targetIndex = itemsSource.IndexOf(SelectedItem);

                        if (draggingItemIndex < targetIndex)
                            targetIndex -= 1;

                        // remove origin one
                        itemsSource.Remove(DraggingItem);

                        // insert at the target's location
                        itemsSource.Insert(targetIndex, DraggingItem);
                        // select the new one
                        SelectedItem = DraggingItem;
                    }

            // fresh view
            if (ItemsSource != null)
                CollectionViewSource.GetDefaultView(ItemsSource).Refresh();

            ResetDragDrop();
        }

        /// <summary>
        ///     Closes the popup and resets the grid to read-enabled mode.
        /// </summary>
        private void ResetDragDrop()
        {
            Mouse.Capture(null);

            _isEditing = false;
            _isDragging = false;

            // set IsReadOnly back to user set
            IsReadOnly = _defaultIsReadOnlyValue;

            _draggingRow = null;
            DraggingItem = null;

            _draggedItemIndicator.IsOpen = false;
        }

        #endregion
    }
}