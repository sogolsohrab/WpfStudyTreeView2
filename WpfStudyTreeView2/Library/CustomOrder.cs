using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WpfStudyTreeView2.Comparer;

namespace WpfStudyTreeView2.Library
{
    public static class CustomOrder
    {
        public static void OrderModel(ObservableCollection<TreeNodeModel> collection)
        {
            List<TreeNodeModel> listOfCollection = collection.OrderBy(x => x.Name, new NaturalStringComparer()).ToList();

            for (int i = 0; i < listOfCollection.Count; i++)
            {
                collection.Move(collection.IndexOf(listOfCollection[i]), i);
            }
        }
    }
}
