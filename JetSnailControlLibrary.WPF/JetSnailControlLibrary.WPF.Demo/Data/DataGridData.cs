using System.Collections;
using System.Collections.ObjectModel;

namespace JetSnailControlLibrary.WPF.Demo
{
    public class DataGridData
    {
        public IList CurrentSelections;

        static DataGridData()
        {
            Instance = new ObservableCollection<DataViewModel>();
            Instance.Add(new DataViewModel()
            {
                Col1 = "string1",
                Col2 = 1
            });
            Instance.Add(new DataViewModel()
            {
                Col1 = "string2",
                Col2 = 2
            });
            Instance.Add(new DataViewModel()
            {
                Col1 = "string3",
                Col2 = 3
            });

        }

        public static ObservableCollection<DataViewModel> Instance { set; get; }

        public static void GenerateNewData()
        {
            Instance.Clear();
        }
    }

    /// <summary>
    ///     Represents the view model for DataGrid Row.
    /// </summary>
    public class DataViewModel
    {
        /// <summary>
        ///     Name for displaying.
        /// </summary>
        public string Col1 { set; get; }

        /// <summary>
        ///     Model for displaying.
        /// </summary>
        public int Col2 { set; get; }
    }
}