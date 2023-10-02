using System;
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

        public TreeViewModel MyViewModel = new TreeViewModel();
        public TreeModel Wells { get; set; }
        public TreeModel Rocks { get; set; }
        public TreeModel Polygones { get; set; }
        public TreeModel WellStrategies { get; set; }
        
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
            // Wells
            Wells = new TreeModel("Wells", Types.Wells);
            MyViewModel.Items.Add(Wells);
            Wells.Items.Add(new TreeModel("R1_W012", Types.WellsChild));
            Wells.Items.Add(new TreeModel("R2_W1", Types.WellsChild));
            Wells.Items.Add(new TreeModel("R1_W1", Types.WellsChild));
            Wells.Items.Add(new TreeModel("R1_W7", Types.WellsChild));
            Wells.Items.Add(new TreeModel("R2_W11", Types.WellsChild));
            Wells.Items.Add(new TreeModel("R2_W03", Types.WellsChild));
            Wells.Items.Add(new TreeModel("R2_W123456789123456789", Types.WellsChild));

            // Rocks
            Rocks = new TreeModel("Rocks", Types.Rocks);
            MyViewModel.Items.Add(Rocks);
            Rocks.Items.Add(new TreeModel("R4", Types.RocksChild));
            Rocks.Items.Add(new TreeModel("R2", Types.RocksChild));
            Rocks.Items.Add(new TreeModel("R1", Types.RocksChild));
            Rocks.Items.Add(new TreeModel("R3", Types.RocksChild));
            Rocks.Items.Add(new TreeModel("R5", Types.RocksChild));
            Rocks.Items.Add(new TreeModel("R6", Types.RocksChild));
            Rocks.Items.Add(new TreeModel("R7", Types.RocksChild));

            // Polygones
            Polygones = new TreeModel("Polygones", Types.Polygones);
            MyViewModel.Items.Add(Polygones);

            // WellStrategies
            WellStrategies = new TreeModel("WellStrategies", Types.WellStrategies);
            MyViewModel.Items.Add(WellStrategies);
            WellStrategies.Items.Add(new TreeModel("WS1", Types.WellStrategiesChild));
            WellStrategies.Items.Add(new TreeModel("WS2", Types.WellStrategiesChild));
            WellStrategies.Items.Add(new TreeModel("WS3", Types.WellStrategiesChild));
            WellStrategies.Items.Add(new TreeModel("WS4", Types.WellStrategiesChild));
            WellStrategies.Items.Add(new TreeModel("WS5", Types.WellStrategiesChild));
            WellStrategies.Items.Add(new TreeModel("WS6", Types.WellStrategiesChild));

            // Ordering
            SortTree();
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
            MyTree.ItemContainerStyle = GetStyle();
            MyTree.ItemsSource = MyViewModel.Items;
            MyTree.SelectedItemChanged += OnSelectedItemChanged;
        }

        private HierarchicalDataTemplate GetDataTemplate()
        {
            //create the data template
            HierarchicalDataTemplate dataTemplate = new HierarchicalDataTemplate();
            dataTemplate.ItemsSource = new Binding() { Path = new PropertyPath("Items") };

            //create stack pane;
            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "parentStackpanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            // create Image 
            FrameworkElementFactory image = new FrameworkElementFactory(typeof(Image));
            image.SetValue(Image.MarginProperty, new Thickness(2));
            image.SetValue(Image.WidthProperty, 16.0);
            image.SetValue(Image.HeightProperty, 16.0);
            image.SetBinding(Image.SourceProperty, new Binding() { Path = new PropertyPath("DisplayedImagePath") });
            stackPanel.AppendChild(image);

            // create textBlock
            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.MarginProperty, new Thickness(5));
            textBlock.SetValue(TextBlock.FontSizeProperty, 12.0);
            textBlock.SetBinding(TextBlock.TextProperty, new Binding() { Path = new PropertyPath("Title") });
            stackPanel.AppendChild(textBlock);

            //set the visual tree of the data template
            dataTemplate.VisualTree = stackPanel;

            return dataTemplate;
        }

        private Style GetStyle()
        {
            var style = new Style {TargetType = typeof(TreeViewItem)};

            var eventSetter = new EventSetter(PreviewMouseRightButtonDownEvent, new MouseButtonEventHandler(OnPreviewMouseRightButtonDown));

            style.Setters.Add(eventSetter);
           
            return style;
        }


        private void OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

            ContextMenu contextMenu = new ContextMenu();

            if (MyViewModel.SelectedItem.ParantType)
            {
                contextMenu.Items.Add(CreateMenuItem("Rename", renameMenuItem_Click));
            }
            else 
            {
                contextMenu.Items.Add(CreateMenuItem("Rename", renameMenuItem_Click));
                contextMenu.Items.Add(CreateMenuItem("Delete", deleteMenuItem_Click));
            }
            (sender as TreeViewItem).ContextMenu = contextMenu;
        }

        private static MenuItem CreateMenuItem(String header, RoutedEventHandler routedEventHandler)
        {
            MenuItem menuItem = new MenuItem();
            menuItem.Header = header;
            menuItem.Click += routedEventHandler;

            return menuItem;
        }

        private void ControlPanelsVisibilty(Border selectedBorder)
        {
            Border visibleBorder;
            List<Border> bordersList = new List<Border>() { HomePanel, RenamePanel, DeletePanel };
            foreach (var border in bordersList)
            {
                border.Visibility = Visibility.Collapsed;
            }


            if (selectedBorder == RenamePanel)
            {
                visibleBorder = RenamePanel;
            }
            else if (selectedBorder == DeletePanel)
            {
                visibleBorder = DeletePanel;
            }
            else
            {
                visibleBorder = HomePanel;
            }

            visibleBorder.Visibility = Visibility.Visible;
        }
        #endregion Private Methods


        #region Events

        #region SelectedItemChanged
        private void OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MyViewModel.SelectedItem = e.NewValue as TreeModel;
        }
        #endregion SelectedItemChanged


        #region Delete Events
        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ControlPanelsVisibilty(DeletePanel);
        }

        private void btnAbortDelete_Click(object sender, RoutedEventArgs e)
        {
            ControlPanelsVisibilty(HomePanel);
        }

        private void btnProceedDelete_Click(object sender, RoutedEventArgs e)
        {

            if (MyViewModel.SelectedItem.ParantType)
            {
                MyViewModel.Items.Remove(MyViewModel.SelectedItem);
            }
            else
            {
                Wells.Items.Remove(MyViewModel.SelectedItem);
                Rocks.Items.Remove(MyViewModel.SelectedItem);
                WellStrategies.Items.Remove(MyViewModel.SelectedItem);
                Polygones.Items.Remove(MyViewModel.SelectedItem);
            }

            ControlPanelsVisibilty(HomePanel);

        }
        #endregion Delete Events


        #region Rename Events
        private void renameMenuItem_Click(object sender, RoutedEventArgs e)
        {

            ControlPanelsVisibilty(RenamePanel);

            if (MyViewModel.SelectedItem.Title != null)
            {
                txtRename.Text = MyViewModel.SelectedItem.Title;
            }
            else
            {
                txtRename.Text = "";
            }

        }

        private void btnAbortRename_Click(object sender, RoutedEventArgs e)
        {
            lblError.Content = string.Empty;
            ControlPanelsVisibilty(HomePanel);
        }

        private void btnProceedRename_Click(object sender, RoutedEventArgs e)
        {

            if (
                (txtRename.Text.Length > 1) &&
                (!MyViewModel.Items.Any(x => x.Title == txtRename.Text)) &&
                (!Wells.Items.Any(x => x.Title == txtRename.Text)) &&
                (!Rocks.Items.Any(x => x.Title == txtRename.Text)) &&
                (!WellStrategies.Items.Any(x => x.Title == txtRename.Text))
             )
            {
                MyViewModel.SelectedItem.Title = txtRename.Text;
                ControlPanelsVisibilty(HomePanel);

                SortTree();
                lblError.Content = string.Empty;
            }
            else if (txtRename.Text.Length < 2)
            {
                lblError.Content = "**Please choose a name with more than 2 characters!";
                ControlPanelsVisibilty(RenamePanel);
            }
            else
            {
                lblError.Content = "**duplicated name Please choose a new one!";
                ControlPanelsVisibilty(RenamePanel);
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
