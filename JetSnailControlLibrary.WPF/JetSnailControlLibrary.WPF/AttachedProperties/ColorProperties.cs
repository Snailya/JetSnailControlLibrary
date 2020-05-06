using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Any attached property related to color.
    /// </summary>
    public class ColorProperties
    {
        #region ----------Primary----------

        #region Theme 主题模式

        public static readonly DependencyProperty ThemeProperty =
            DependencyProperty.RegisterAttached("Theme", typeof(ThemeType),
                typeof(ColorProperties),
                new FrameworkPropertyMetadata(ThemeType.Light));

        /// <summary>
        ///     Sets the theme mode
        /// </summary>
        public static void SetTheme(DependencyObject obj, ThemeType value)
        {
            obj.SetValue(ThemeProperty, value);
        }

        /// <summary>
        ///     Gets the theme mode
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        [AttachedPropertyBrowsableForType(typeof(TextBlock))]
        [AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
        [AttachedPropertyBrowsableForType(typeof(TreeView))]
        public static ThemeType GetTheme(DependencyObject obj)
        {
            return (ThemeType) obj.GetValue(ThemeProperty);
        }

        #endregion

        #endregion

        #region ----------Background----------

        #region OverlayBackgroundBrush 状态指示层

        public static readonly DependencyProperty OverlayBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("OverlayBackgroundBrush", typeof(Brush),
                typeof(ColorProperties),
                new FrameworkPropertyMetadata(Brushes.White,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        ///     Sets the brush used to draw the background when mouse over
        /// </summary>
        public static void SetOverlayBackgroundBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(OverlayBackgroundBrushProperty, value);
        }

        /// <summary>
        ///     Gets the brush used to draw the background when mouse over
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Control))]
        public static Brush GetOverlayBackgroundBrush(DependencyObject obj)
        {
            return (Brush) obj.GetValue(OverlayBackgroundBrushProperty);
        }

        #endregion


        #region HoverBackgroundBrush 鼠标悬停时背景色

        public static readonly DependencyProperty HoverBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("HoverBackgroundBrush", typeof(Brush),
                typeof(ColorProperties),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        ///     Sets the brush used to draw the background when mouse over
        /// </summary>
        public static void SetHoverBackgroundBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(HoverBackgroundBrushProperty, value);
        }

        /// <summary>
        ///     Gets the brush used to draw the background when mouse over
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        [AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
        [AttachedPropertyBrowsableForType(typeof(TreeView))]
        public static Brush GetHoverBackgroundBrush(DependencyObject obj)
        {
            return (Brush) obj.GetValue(HoverBackgroundBrushProperty);
        }

        #endregion

        #region PressedBackgroundBrush 鼠标按下时背景色

        public static readonly DependencyProperty PressedBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("PressedBackgroundBrush", typeof(Brush),
                typeof(ColorProperties),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        ///     Sets the brush used to draw the background when mouse click
        /// </summary>
        public static void SetPressedBackgroundBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedBackgroundBrushProperty, value);
        }

        /// <summary>
        ///     Gets the brush used to draw the background when mouse click
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static Brush GetPressedBackgroundBrush(DependencyObject obj)
        {
            return (Brush) obj.GetValue(PressedBackgroundBrushProperty);
        }

        #endregion

        #region SelectedBackgroundBrush 选中时背景色

        public static readonly DependencyProperty SelectedBackgroundBrushProperty =
            DependencyProperty.RegisterAttached("SelectedBackgroundBrush", typeof(Brush),
                typeof(ColorProperties),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        ///     Sets the brush used to draw the background when item selected
        /// </summary>
        public static void SetSelectedBackgroundBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(SelectedBackgroundBrushProperty, value);
        }

        /// <summary>
        ///     Gets the brush used to draw the background when item selected
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        [AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
        [AttachedPropertyBrowsableForType(typeof(TreeView))]
        public static Brush GetSelectedBackgroundBrush(DependencyObject obj)
        {
            return (Brush) obj.GetValue(SelectedBackgroundBrushProperty);
        }

        #endregion

        #endregion

        #region ----------Border----------

        #region HoverBorderBrush 鼠标悬停时边框色

        public static readonly DependencyProperty HoverBorderBrushProperty =
            DependencyProperty.RegisterAttached("HoverBorderBrush", typeof(Brush),
                typeof(ColorProperties),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        ///     Sets the brush used to draw the border when mouse over
        /// </summary>
        public static void SetHoverBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(HoverBorderBrushProperty, value);
        }

        /// <summary>
        ///     Gets the brush used to draw the border when mouse over
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        [AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
        [AttachedPropertyBrowsableForType(typeof(TreeView))]
        public static Brush GetHoverBorderBrush(DependencyObject obj)
        {
            return (Brush) obj.GetValue(HoverBorderBrushProperty);
        }

        #endregion

        #region PressedBorderBrush 鼠标按下时边框色

        public static readonly DependencyProperty PressedBorderBrushProperty =
            DependencyProperty.RegisterAttached("PressedBorderBrush", typeof(Brush),
                typeof(ColorProperties),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        ///     Sets the brush used to draw the border when mouse click
        /// </summary>
        public static void SetPressedBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(PressedBorderBrushProperty, value);
        }

        /// <summary>
        ///     Gets the brush used to draw the border when mouse click
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static Brush GetPressedBorderBrush(DependencyObject obj)
        {
            return (Brush) obj.GetValue(PressedBorderBrushProperty);
        }

        #endregion

        #region SelectedBorderBrush 选中时边框色

        public static readonly DependencyProperty SelectedBorderBrushProperty =
            DependencyProperty.RegisterAttached("SelectedBorderBrush", typeof(Brush),
                typeof(ColorProperties),
                new FrameworkPropertyMetadata(Brushes.Transparent,
                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        ///     Sets the brush used to draw the border when mouser over
        /// </summary>
        public static void SetSelectedBorderBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(SelectedBorderBrushProperty, value);
        }

        /// <summary>
        ///     Gets the brush used to draw the border when mouse over
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        [AttachedPropertyBrowsableForType(typeof(TreeViewItem))]
        [AttachedPropertyBrowsableForType(typeof(TreeView))]
        public static Brush GetSelectedBorderBrush(DependencyObject obj)
        {
            return (Brush) obj.GetValue(SelectedBorderBrushProperty);
        }

        #endregion

        #endregion
    }

    public enum ThemeType
    {
        Light,
        Dark
    }
}