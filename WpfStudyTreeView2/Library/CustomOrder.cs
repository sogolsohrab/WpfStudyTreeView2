using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WpfStudyTreeView2.Library
{
    public class CustomOrder
    {
        public static void OrderModel(ObservableCollection<TreeModel> collection)
        {
            List<TreeModel> listOfCollection = collection.OrderBy(x => PadNumbers(x.Title)).ToList();

            for (int i = 0; i < listOfCollection.Count; i++)
            {
                collection.Move(collection.IndexOf(listOfCollection[i]), i);
            }
        }

        private static string PadNumbers(string input)
        {
            return Regex.Replace(input, "[0-9]+", match => match.Value.PadLeft(20, '0'));
        }

    }
}
