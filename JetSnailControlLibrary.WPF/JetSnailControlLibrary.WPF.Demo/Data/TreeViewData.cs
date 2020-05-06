using System.Collections.ObjectModel;

namespace JetSnailControlLibrary.WPF.Demo
{
    public class TreeViewData
    {
        static TreeViewData()
        {
            Instance = new ObservableCollection<TreeNodeViewModel>
            {
                new TreeNodeViewModel
                {
                    NodeName = "Node 1",
                    Id = 1,
                    Children = new ObservableCollection<TreeNodeViewModel>
                    {
                        new TreeNodeViewModel
                        {
                            NodeName = "Child Node 1",
                            Id = 11,
                            Children = new ObservableCollection<TreeNodeViewModel>
                            {
                                new TreeNodeViewModel
                                {
                                    NodeName = "Grand Child Node 1",
                                    Id = 111
                                }
                            }
                        },
                        new TreeNodeViewModel
                        {
                            NodeName = "Child Node 2",
                            Id = 12
                        }
                    }
                },
                new TreeNodeViewModel
                {
                    NodeName = "Node 2",
                    Id = 2,
                    Children = new ObservableCollection<TreeNodeViewModel>
                    {
                        new TreeNodeViewModel
                        {
                            NodeName = "Child Node 3",
                            Id = 21
                        },
                        new TreeNodeViewModel
                        {
                            NodeName = "Child Node 4",
                            Id = 22
                        }
                    }
                }
            };
        }

        public static ObservableCollection<TreeNodeViewModel> Instance { set; get; }
    }


    public class TreeNodeViewModel
    {
        /// <summary>
        ///     name for display
        /// </summary>
        public string NodeName { set; get; }

        /// <summary>
        ///     node id
        /// </summary>
        public int Id { set; get; }

        /// <summary>
        ///     children nodes
        /// </summary>
        public ObservableCollection<TreeNodeViewModel> Children { set; get; }
    }
}