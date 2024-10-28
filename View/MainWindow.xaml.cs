using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ERD_Builder.View.Popup;
using System.Diagnostics;
using ERD_Builder.ViewModel;

namespace ERD_Builder.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _mainViewModel = new MainViewModel();
            this.DataContext = _mainViewModel;

        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            //Name project popup
            NameProject nameProjectPopup = new NameProject();
            bool? result = nameProjectPopup.ShowDialog();

            

            if (result == true && !string.IsNullOrWhiteSpace(nameProjectPopup.ProjectName))
            {
                Trace.WriteLine($"New Project Created: {nameProjectPopup.ProjectName}");
                //_mainViewModel.AddProject(nameProjectPopup.ProjectName);
                               

                //Open project page if success and pass in project name
                ProjectPage projectWindow = new ProjectPage(nameProjectPopup.ProjectName);
                projectWindow.Show();

            }
        }
    }
}