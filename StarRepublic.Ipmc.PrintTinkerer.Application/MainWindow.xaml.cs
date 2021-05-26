using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using log4net.Config;
using StarRepublic.Ipmc.PrintTinkerer.Application.Model;

namespace StarRepublic.Ipmc.PrintTinkerer.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal readonly ViewModel ViewModel;
        internal readonly PrintControl PrintControl;

        public MainWindow()
        {
            XmlConfigurator.Configure();

            InitializeComponent();
            Console.SetOut(new TextBoxWriter(LogTextBox));

            ViewModel = new ViewModel();
            PrintControl = new PrintControl(ViewModel);
            DataContext = ViewModel;

            LoadClusters();
            LoadCredentials();
        }

        private void LoadCredentials()
        {
            Username.Text = ConfigurationManager.AppSettings["Username"];
            Password.Password = ConfigurationManager.AppSettings["Password"];
            Environment.Text = ConfigurationManager.AppSettings["Environment"];
        }

        private void LoadClusters()
        {
            var clusters = ((Hashtable)ConfigurationManager.GetSection("IpmcClusters"))
                .Cast<DictionaryEntry>()
                .Select(n => new IpmcCluster { Name = (string)n.Key, Url = (string)n.Value });

            IpmcClusterComboBox.ItemsSource = clusters;
            IpmcClusterComboBox.SelectedIndex = 0;
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            var ipmcCluster = (IpmcCluster)IpmcClusterComboBox.SelectedItem;
            await PrintControl.ConnectAndInitialize(ipmcCluster.Url, Username.Text, Password.Password, Environment.Text);

            PrintObjectsComboBox.SelectedIndex = 0;
            EditionsComboBox.SelectedIndex = 0;
        }

        private async void TreeView_OnExpanded(object sender, RoutedEventArgs e)
        {
            if (!(e.OriginalSource is TreeViewItem treeViewItem))
                return;

            var hierarchicalEntity = (HierarchicalEntity)treeViewItem.DataContext;

            await PrintControl.LoadChildren(hierarchicalEntity);
        }

        private async void Get_Click(object sender, RoutedEventArgs e)
        {
            var printObject = (PrintObject)PrintObjectsComboBox.SelectedItem;
            var edition = (EditionViewModel)EditionsComboBox.SelectedItem;
            var article = (ArticleViewModel)TreeView.SelectedItem;

            AttributeSetResultTextBox.Text = await PrintControl.GetPrintObjectText(printObject, edition, article);
        }
    }
}

