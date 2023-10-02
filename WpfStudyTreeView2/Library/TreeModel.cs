using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WpfStudyTreeView2.Properties;

namespace WpfStudyTreeView2.Library
{
    public enum Types
    {
        Wells,
        Rocks,
        Polygones,
        WellStrategies,
        WellsChild,
        RocksChild,
        PolygonesChild,
        WellStrategiesChild
    }

    public class TreeModel : PropertyChangedBase
    {
        private string title;
        public string Title
        {
            get => this.title;
            set
            {
                this.title = value;
                OnPropertyChanged("Title");
            }
        }

        public string DisplayedImagePath { get; set; }

        public Types ModelType { get; set; }

        public bool ParantType { get; set; }

        public ObservableCollection<TreeModel> Items { get; set; }

        public TreeModel(string value, Types type)
        {
            Title = value;
            ModelType = type;
            DisplayedImagePath = setImage(type);
            Items = new ObservableCollection<TreeModel>();

            if (type == Types.Wells || type == Types.Rocks || type == Types.Polygones || type == Types.WellStrategies)
            {
                ParantType = true;
            }
            else
            {
                ParantType = false;
            }
        }

        private string setImage(Types type)
        {
            string imagePath;

            switch (type)
            {
                case Types.Wells:
                    imagePath = @"/Resources/Wells.jpg";
                    break;
                case Types.Rocks:
                    imagePath = @"/Resources/Rocks.jpg";
                    break;
                case Types.Polygones:
                    imagePath = @"/Resources/Polygones.jpg";
                    break;
                case Types.WellStrategies:
                    imagePath = @"/Resources/WellStrategies.jpg";
                    break;
                case Types.WellsChild:
                    imagePath = @"/Resources/WellsM.jpg";
                    break;
                case Types.RocksChild:
                    imagePath = @"/Resources/RocksM.jpg";
                    break;
                case Types.WellStrategiesChild:
                    imagePath = @"/Resources/WellStrategiesM.jpg";
                    break;
                default:
                    imagePath = @"/Resources/new.jpg";
                    break;
            }

            return imagePath;
        }
    }
}
