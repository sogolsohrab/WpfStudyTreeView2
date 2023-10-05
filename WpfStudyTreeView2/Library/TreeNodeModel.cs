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
        Well,
        Rock,
        Polygon,
        WellStrategy,
        WellMembers,
        RockMembers,
        PolygonMembers,
        WellStrategyMembers
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
            IsParentNode = (type.ToString().Contains("Member")) ? false : true;
        }

        private string GetImage(NodeTypes type)
        {
            string imagePath;

            switch (type)
            {
                case NodeTypes.Well:
                    imagePath = Constants.ImagePath.WellImagePath;
                    break;
                case NodeTypes.Rock:
                    imagePath = Constants.ImagePath.RockImagePath;
                    break;
                case NodeTypes.Polygon:
                    imagePath = Constants.ImagePath.PolygonImagePath;
                    break;
                case NodeTypes.WellStrategy:
                    imagePath = Constants.ImagePath.WellStrategyImagePath;
                    break;
                case NodeTypes.WellMembers:
                    imagePath = Constants.ImagePath.WellMembersImagePath;
                    break;
                case NodeTypes.RockMembers:
                    imagePath = Constants.ImagePath.RockMembersImagePath;
                    break;
                case NodeTypes.WellStrategyMembers:
                    imagePath = Constants.ImagePath.WellStrategyMembersImagePath;
                    break;
                default:
                    imagePath = Constants.ImagePath.DefaultImagePath;
                    break;
            }

            return imagePath;
        }
    }
}
