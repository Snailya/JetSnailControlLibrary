using System.Collections.ObjectModel;

namespace JetSnailControlLibrary.WPF.Demo
{
    public class DataGridData
    {
        static DataGridData()
        {
            Instance = new ObservableCollection<DataViewModel>();
            for (var i = 0; i < 3; i++)
                Instance.Add(new DataViewModel
                {
                    Name = "气动隔膜泵",
                    Model = "Husky 716",
                    Material = "PP",
                    ShortBrand = "Graco",
                    Price=12
                });
            for (var i = 0; i < 3; i++)
                Instance.Add(new DataViewModel
                {
                    Name = "气动隔膜泵",
                    Model = "Husky 507",
                    Material = "SUS304",
                    ShortBrand = "Graco"
                });
            for (var i = 0; i < 3; i++)
                Instance.Add(new DataViewModel
                {
                    Name = "离心泵",
                    Model = "SBS 100-250-125",
                    Material = "SUS304",
                    ShortBrand = "KSB"
                });

            
        }

        public static void GenerateNewData()
        {
            Instance.Clear();

            for (var i = 0; i < 7; i++)
                Instance.Add(new DataViewModel
                {
                    Name = "阀门",
                    Material = "DN15",
                    ShortBrand = "KBS",
                    Price = 12
                });
            for (var i = 0; i < 3; i++)
                Instance.Add(new DataViewModel
                {
                    Name = "离心泵",
                    Model = "SBS 100-250-125",
                    Material = "SUS304",
                    ShortBrand = "KSB"
                });
        }

        public static ObservableCollection<DataViewModel> Instance { set; get; }
    }

    /// <summary>
    ///     Represents the view model for DataGrid Row.
    /// </summary>
    public class DataViewModel
    {
        /// <summary>
        ///     Name for displaying.
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        ///     Model for displaying.
        /// </summary>
        public string Model { set; get; }

        /// <summary>
        ///     Parameter for displaying.
        /// </summary>
        public string Parameter { set; get; }

        /// <summary>
        ///     Material for displaying.
        /// </summary>
        public string Material { set; get; }

        /// <summary>
        ///     Remarks for displaying.
        /// </summary>
        public string Remarks { set; get; }

        /// <summary>
        ///     ShortBrand for displaying.
        /// </summary>
        public string ShortBrand { set; get; }

        /// <summary>
        ///     Price for displaying
        /// </summary>
        public decimal? Price { set; get; }
    }
}