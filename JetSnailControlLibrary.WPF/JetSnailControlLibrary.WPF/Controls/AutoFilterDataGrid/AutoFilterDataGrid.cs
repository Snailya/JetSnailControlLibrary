using System.Collections.Generic;
using System.Windows.Controls;

namespace JetSnailControlLibrary.WPF
{
    public class AutoFilterDataGrid : DataGrid
    {
        /// <summary>
        ///     This dictionary will have a list of all applied filters
        /// </summary>
        public Dictionary<string, IList<IFilterEx>> FilterExes { get; } = new Dictionary<string, IList<IFilterEx>>();
    }
}