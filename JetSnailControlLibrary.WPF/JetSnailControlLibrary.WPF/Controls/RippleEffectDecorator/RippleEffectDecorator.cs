using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace JetSnailControlLibrary.WPF
{
    public class RippleEffectDecorator : ContentControl
    {
        // Using a DependencyProperty as the backing store for HighlightBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorThemeProperty =
            DependencyProperty.Register("ColorTheme", typeof(EffectOnType), typeof(RippleEffectDecorator),
                new PropertyMetadata(EffectOnType.OnSurface));

        // Using a DependencyProperty as the backing store for HighlightBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(RippleEffectDecorator),
                new PropertyMetadata(new CornerRadius(0)));


        static RippleEffectDecorator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RippleEffectDecorator),
                new FrameworkPropertyMetadata(typeof(RippleEffectDecorator)));
        }

        public EffectOnType ColorTheme
        {
            get => (EffectOnType) GetValue(ColorThemeProperty);
            set => SetValue(ColorThemeProperty, value);
        }

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var ellipse = GetTemplateChild("PART_ellipse") as Ellipse;
            var grid = GetTemplateChild("PART_grid") as Grid;

            var RippleEffect = grid.FindResource("RippleEffect") as Storyboard;

            AddHandler(MouseDownEvent, new RoutedEventHandler((sender, e) =>
            {
                var animation = RippleEffect;

                // stop growing of grid when ripple reaches border.
                grid.Width = ActualWidth;
                grid.Height = ActualHeight;

                var targetWidth = Math.Max(ActualWidth, ActualHeight) * 2;
                var mousePosition = (e as MouseButtonEventArgs).GetPosition(this);
                var startMargin = new Thickness(mousePosition.X, mousePosition.Y, 0, 0);
                //set initial margin to mouse position
                ellipse.Margin = startMargin;
                //set the to value of the animation that animates the width to the target width
                (animation.Children[0] as DoubleAnimation).To = targetWidth;
                //set the to and from values of the animation that animates the distance relative to the container (grid)
                (animation.Children[1] as ThicknessAnimation).From = startMargin;
                (animation.Children[1] as ThicknessAnimation).To = new Thickness(mousePosition.X - targetWidth / 2,
                    mousePosition.Y - targetWidth / 2, 0, 0);
                ellipse.BeginStoryboard(animation);
            }), true);
        }
    }

    public enum EffectOnType
    {
        OnSurface,
        OnPrimary
    }
}