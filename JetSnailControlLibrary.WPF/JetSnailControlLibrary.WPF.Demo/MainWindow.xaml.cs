using System;
using System.Diagnostics;
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
            AutoFilterDataGrid.DataContext = DataGridData.Instance;
        }

        private void ButtonBase1_OnClick(object sender, RoutedEventArgs e)
        {
            AutoFilterDataGrid.DataContext = null;
            AutoFilterDataGrid.DataContext = DataGridData.Instance;
        }

        private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
        {
            DataGridData.GenerateNewData();
        }

        private void ButtonBase3_OnClick(object sender, RoutedEventArgs e)
        {
            var index = new Random().Next();
            DataGridData.Instance.Add(new DataViewModel()
            {
                Col1 = $"string{index}",
                Col2 = index
            });
        }
    }
}