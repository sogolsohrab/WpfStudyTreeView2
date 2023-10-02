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
    public class TreeViewModel
    {
        public TreeModel SelectedItem { get; set; }

        public ObservableCollection<TreeModel> Items { get; set; }

        public TreeViewModel()
        {
            Items = new ObservableCollection<TreeModel>();
        }
    }
}
