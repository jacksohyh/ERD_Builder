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

namespace ERD_Builder.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

        }

        private void NewProjectButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the NameProject popup window
            NameProject nameProjectPopup = new NameProject();
            bool? result = nameProjectPopup.ShowDialog();

            // If the user clicks OK and enters a valid project name
            if (result == true && !string.IsNullOrWhiteSpace(nameProjectPopup.ProjectName))
            {
                Trace.WriteLine($"New Project Created: {nameProjectPopup.ProjectName}");

            }
        }
    }
}