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
    public enum NodeTypes
    {
        Wells,
        Rocks,
        Polygons,
        WellStrategies,
        WellsChild,
        RocksChild,
        PolygonsChild,
        WellStrategiesChild
    }

    public class TreeNodeModel : PropertyChangedBase
    {
        private string name;
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
        }

        public string DisplayedImagePath { get; set; }

        public NodeTypes NodeType { get; set; }

        public bool IsParentNode { get; set; }

        public ObservableCollection<TreeNodeModel> Items { get; set; }

        public TreeNodeModel(string value, NodeTypes type)
        {
            Name = value;
            NodeType = type;
            DisplayedImagePath = GetImage(type);
            Items = new ObservableCollection<TreeNodeModel>();

            if (type == NodeTypes.Wells || type == NodeTypes.Rocks || type == NodeTypes.Polygons || type == NodeTypes.WellStrategies)
            {
                IsParentNode = true;
            }
            else
            {
                IsParentNode = false;
            }
        }

        private string GetImage(NodeTypes type)
        {
            string imagePath;

            switch (type)
            {
                case NodeTypes.Wells:
                    imagePath = @"/Resources/Wells.jpg";
                    break;
                case NodeTypes.Rocks:
                    imagePath = @"/Resources/Rocks.jpg";
                    break;
                case NodeTypes.Polygons:
                    imagePath = @"/Resources/Polygons.jpg";
                    break;
                case NodeTypes.WellStrategies:
                    imagePath = @"/Resources/WellStrategies.jpg";
                    break;
                case NodeTypes.WellsChild:
                    imagePath = @"/Resources/WellsM.jpg";
                    break;
                case NodeTypes.RocksChild:
                    imagePath = @"/Resources/RocksM.jpg";
                    break;
                case NodeTypes.WellStrategiesChild:
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
