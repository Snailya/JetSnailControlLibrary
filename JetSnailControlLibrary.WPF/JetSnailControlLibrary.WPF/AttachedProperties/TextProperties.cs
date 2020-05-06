using System.Windows;
using System.Windows.Controls;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Any attached property related to text.
    /// </summary>
    public class TextProperties
    {
        public static readonly DependencyProperty LeadingIconProperty =
            DependencyProperty.RegisterAttached("LeadingIcon", typeof(string),
                typeof(TextProperties),
                new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty TrailingIconProperty =
            DependencyProperty.RegisterAttached("TrailingIcon", typeof(string),
                typeof(TextProperties),
                new FrameworkPropertyMetadata(string.Empty));

        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.RegisterAttached("PlaceHolder", typeof(string),
                typeof(TextProperties),
                new FrameworkPropertyMetadata(string.Empty));

        /// <summary>
        ///     Sets the leading icon string
        /// </summary>
        public static void SetLeadingIcon(DependencyObject obj, string value)
        {
            obj.SetValue(LeadingIconProperty, value);
        }

        /// <summary>
        ///     Gets the leading icon string
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static string GetLeadingIcon(DependencyObject obj)
        {
            return (string) obj.GetValue(LeadingIconProperty);
        }

        /// <summary>
        ///     Sets the leading icon string
        /// </summary>
        public static void SetTrailingIcon(DependencyObject obj, string value)
        {
            obj.SetValue(TrailingIconProperty, value);
        }

        /// <summary>
        ///     Gets the leading icon string
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static string GetTrailingIcon(DependencyObject obj)
        {
            return (string) obj.GetValue(TrailingIconProperty);
        }

        /// <summary>
        ///     Sets the leading icon string
        /// </summary>
        public static void SetPlaceHolder(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceHolderProperty, value);
        }

        /// <summary>
        ///     Gets the leading icon string
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static string GetPlaceHolder(DependencyObject obj)
        {
            return (string) obj.GetValue(PlaceHolderProperty);
        }
    }

    public class PasswordProperties
    {
        public static readonly DependencyProperty MonitorPasswordProperty =
            DependencyProperty.RegisterAttached("MonitorPassword", typeof(bool),
                typeof(PasswordProperties),
                new UIPropertyMetadata(
                    false, OnMonitorPasswordPropertyChanged));

        public static readonly DependencyProperty HasTextProperty = DependencyProperty.RegisterAttached(
            "HasText", typeof(bool), typeof(PasswordProperties), new UIPropertyMetadata(
                false));

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password", typeof(string), typeof(PasswordProperties), new FrameworkPropertyMetadata(
                string.Empty));


        private static void OnMonitorPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Make sure it is a password box
            if (!(d is PasswordBox passwordBox)) return;

            // Remove any previous events
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            // If the caller set MonitorPassword to true...
            if (!(bool) e.NewValue) return;

            // Set default value
            SetHasText(passwordBox);

            // Start listening out for password changes
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }


        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {

            // Set the attached HasText value
            SetHasText(sender as PasswordBox);

            // Set the attached Password value
            SetPassword(sender as PasswordBox);
        }


        /// <summary>
        ///     Sets the monitor password flag
        /// </summary>
        public static void SetMonitorPassword(DependencyObject obj, bool value)
        {
            obj.SetValue(MonitorPasswordProperty, value);
        }

        /// <summary>
        ///     Gets the monitor password flag
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetMonitorPassword(DependencyObject obj)
        {
            return (bool) obj.GetValue(MonitorPasswordProperty);
        }

        /// <summary>
        ///     Sets the has text flag
        /// </summary>
        public static void SetHasText(DependencyObject obj)
        {
            obj.SetValue(HasTextProperty, ((PasswordBox) obj).SecurePassword.Length > 0);
        }


        /// <summary>
        ///     Gets the has text flag
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static bool GetHasText(DependencyObject obj)
        {
            return (bool) obj.GetValue(HasTextProperty);
        }

        /// <summary>
        ///     Sets the password
        /// </summary>
        public static void SetPassword(DependencyObject obj)
        {
            obj.SetValue(PasswordProperty, ((PasswordBox)obj).Password);
        }


        /// <summary>
        ///     Gets the password
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(PasswordBox))]
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }
    }
}