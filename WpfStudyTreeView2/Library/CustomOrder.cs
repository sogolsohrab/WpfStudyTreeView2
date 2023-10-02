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
        public static void OrderModel(ObservableCollection<TreeModel> collection)
        {
            List<TreeModel> listOfCollection = collection.OrderBy(x => x.Title, new NaturalStringComparer()).ToList();

            for (int i = 0; i < listOfCollection.Count; i++)
            {
                collection.Move(collection.IndexOf(listOfCollection[i]), i);
            }
        }
    }
}
