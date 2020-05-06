using System.Windows.Controls;
using System.Windows.Input;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     A class stores library level commands.
    /// </summary>
    public static class StaticCommands
    {
        static StaticCommands()
        {
            // Register CommandBinding for Textbox.
            CommandManager.RegisterClassCommandBinding(typeof(TextBox),
                new CommandBinding(ClearTextCommand, ClearTextCommandExecuted, ClearTextCommandCanExecute));
        }

        public static RoutedUICommand ClearTextCommand { set; get; } =
            new RoutedUICommand("A command to clear Textbox's text", "ClearTextCommand", typeof(StaticCommands));

        private static void ClearTextCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((TextBox) sender).Text = string.Empty;
        }

        private static void ClearTextCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}