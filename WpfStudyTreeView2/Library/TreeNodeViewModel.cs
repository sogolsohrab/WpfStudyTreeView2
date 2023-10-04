using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WpfStudyTreeView2.Library
{
    public class TreeNodeViewModel
    {
        public TreeNodeModel SelectedItem { get; set; }

        public ObservableCollection<TreeNodeModel> Items { get; set; }

        public TreeNodeViewModel()
        {
            Items = new ObservableCollection<TreeNodeModel>();
        }
    }
}
