using System.Windows;
using System.Windows.Controls;

namespace JetSnailControlLibrary.WPF.Demo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.AutoFilterDataGrid.DataContext = DataGridData.Instance;
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            this.AutoFilterDataGrid.DataContext = null;
            this.AutoFilterDataGrid.DataContext = DataGridData.Instance;

        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            DataGridData.GenerateNewData();

        }
    }
}