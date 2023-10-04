﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfStudyTreeView2.Library;

namespace WpfStudyTreeView2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public TreeNodeViewModel MyViewModel { get; set; } 
        public TreeNodeModel Wells { get; set; }
        public TreeNodeModel Rocks { get; set; }
        public TreeNodeModel Polygons { get; set; }
        public TreeNodeModel WellStrategies { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
            LoadTree();
            FillTree();
        }

        #region Private Methods
        private void LoadTree()
        {
            MyViewModel = new TreeNodeViewModel();

            // Wells
            Wells = new TreeNodeModel("Wells", NodeTypes.Wells);
            MyViewModel.Items.Add(Wells);
            string[] wellMembers = { "R1_W7", "R1_W12345678912", "R1_W123456789111", "R2_W1", "R2_W11", "R2_W03", "R2_W123456789123456789", "R1_W1", "R1_W012" };
            CreateMembers(Wells, wellMembers, NodeTypes.WellsChild);

            // Rocks
            Rocks = new TreeNodeModel("Rocks", NodeTypes.Rocks);
            MyViewModel.Items.Add(Rocks);
            string[] rockMembers = { "R4", "R2", "R1", "R3", "R7", "R6", "R5"};
            CreateMembers(Rocks, rockMembers, NodeTypes.RocksChild);

            // Polygones
            Polygons = new TreeNodeModel("Polygons", NodeTypes.Polygons);
            MyViewModel.Items.Add(Polygons);

            // WellStrategies
            WellStrategies = new TreeNodeModel("WellStrategies", NodeTypes.WellStrategies);
            MyViewModel.Items.Add(WellStrategies);
            string[] wellStrategieMembers = { "WS6", "WS2", "WS4", "WS1", "WS3", "WS5" };
            CreateMembers(WellStrategies, wellStrategieMembers, NodeTypes.WellStrategiesChild);

            SortTree();
        }

        private void CreateMembers(TreeNodeModel parent, string[] membersNameArray, NodeTypes membersType)
        {
            foreach (string memberName in membersNameArray)
            {
                parent.Items.Add(new TreeNodeModel(memberName, membersType));
            }
        }

        private void SortTree()
        {
            CustomOrder.OrderModel(Wells.Items);
            CustomOrder.OrderModel(Rocks.Items);
            CustomOrder.OrderModel(WellStrategies.Items);
            CustomOrder.OrderModel(MyViewModel.Items);
        }

        private void FillTree()
        { 
            MyTree.ItemTemplate = GetDataTemplate();
            MyTree.ItemContainerStyle = CreateStyle();
            MyTree.ItemsSource = MyViewModel.Items;
            MyTree.SelectedItemChanged += OnSelectedItemChanged;
        }

        private HierarchicalDataTemplate GetDataTemplate()
        {
            
            HierarchicalDataTemplate dataTemplate = new HierarchicalDataTemplate();
            dataTemplate.ItemsSource = new Binding() { Path = new PropertyPath("Items") };

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "parentStackPanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            
            FrameworkElementFactory image = new FrameworkElementFactory(typeof(Image));
            image.SetValue(Image.MarginProperty, new Thickness(2));
            image.SetValue(Image.WidthProperty, 16.0);
            image.SetValue(Image.HeightProperty, 16.0);
            image.SetBinding(Image.SourceProperty, new Binding() { Path = new PropertyPath("DisplayedImagePath") });
            stackPanel.AppendChild(image);

            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.MarginProperty, new Thickness(5));
            textBlock.SetValue(TextBlock.FontSizeProperty, 12.0);
            textBlock.SetBinding(TextBlock.TextProperty, new Binding() { Path = new PropertyPath("Name") });
            stackPanel.AppendChild(textBlock);

            dataTemplate.VisualTree = stackPanel;

            return dataTemplate;
        }

        private Style CreateStyle()
        {
            var style = new Style {TargetType = typeof(TreeViewItem)};
            var eventSetter = new EventSetter(PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(OnPreviewMouseRightButtonDown));
            style.Setters.Add(eventSetter);
           
            return style;
        }


        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MyViewModel.SelectedItem == null)
            {
                return;
            }

            ContextMenu contextMenu = new ContextMenu();
            MenuItem renameMenuItem = CreateMenuItem("Rename", @"/Resources/RenameIcon.jpg", RenameMenuItem_Click);
            MenuItem deleteMenuItem = CreateMenuItem("Delete", @"/Resources/DeleteIcon.jpg", DeleteMenuItem_Click);

            if (MyViewModel.SelectedItem.IsParentNode)
            {
                contextMenu.Items.Add(renameMenuItem);
            }
            else 
            {
                contextMenu.Items.Add(renameMenuItem);
                contextMenu.Items.Add(deleteMenuItem);
            }
            (sender as TreeViewItem).ContextMenu = contextMenu;
        }

        private static MenuItem CreateMenuItem(String header, String imagePath, RoutedEventHandler routedEventHandler)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = header;
            menuItem.Click += routedEventHandler;
            menuItem.Icon = new System.Windows.Controls.Image
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Relative))
            };
            return menuItem;
        }

        private void ControlPanelsVisibility(Border selectedBorder)
        {
            
            List<Border> bordersList = new List<Border>() { HomePanel, RenamePanel, DeletePanel };
            foreach (var border in bordersList)
            {
                border.Visibility = Visibility.Collapsed;
            }

            selectedBorder.Visibility = Visibility.Visible;

        }
        #endregion Private Methods


        #region Events

        #region SelectedItemChanged
        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MyViewModel.SelectedItem = e.NewValue as TreeNodeModel;
        }
        #endregion SelectedItemChanged


        #region Delete Events
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ControlPanelsVisibility(DeletePanel);
        }

        private void BtnAbortDelete_Click(object sender, RoutedEventArgs e)
        {
            ControlPanelsVisibility(HomePanel);
        }

        private void BtnProceedDelete_Click(object sender, RoutedEventArgs e)
        {

            if (MyViewModel.SelectedItem.IsParentNode)
            {
                MyViewModel.Items.Remove(MyViewModel.SelectedItem);
            }
            else
            {
                Wells.Items.Remove(MyViewModel.SelectedItem);
                Rocks.Items.Remove(MyViewModel.SelectedItem);
                WellStrategies.Items.Remove(MyViewModel.SelectedItem);
                Polygons.Items.Remove(MyViewModel.SelectedItem);
            }

            ControlPanelsVisibility(HomePanel);

        }
        #endregion Delete Events


        #region Rename Events
        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {

            ControlPanelsVisibility(RenamePanel);

            if (MyViewModel.SelectedItem.Name != null)
            {
                txtRename.Text = MyViewModel.SelectedItem.Name;
            }
            else
            {
                txtRename.Text = "";
            }

        }

        private void BtnAbortRename_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = string.Empty;
            ControlPanelsVisibility(HomePanel);
        }

        private void BtnProceedRename_Click(object sender, RoutedEventArgs e)
        {

            if (
                (txtRename.Text.Length > 1) &&
                (!MyViewModel.Items.Any(x => x.Name == txtRename.Text)) &&
                (!Wells.Items.Any(x => x.Name == txtRename.Text)) &&
                (!Rocks.Items.Any(x => x.Name == txtRename.Text)) &&
                (!WellStrategies.Items.Any(x => x.Name == txtRename.Text))
             )
            {
                MyViewModel.SelectedItem.Name = txtRename.Text;
                ControlPanelsVisibility(HomePanel);

                SortTree();
                lblError.Content = string.Empty;
            }
            else if (txtRename.Text.Length < 2)
            {
                lblError.Content = "**Please choose a name with more than 2 characters!";
                ControlPanelsVisibility(RenamePanel);
            }
            else
            {
                lblError.Content = "**duplicated name Please choose a new one!";
                ControlPanelsVisibility(RenamePanel);
            }
            
            txtRename.Clear();
        }
        #endregion Rename Events

        #endregion Events


        #region Window style Events
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        // State change
        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }
        #endregion Window style Events

    }
}
