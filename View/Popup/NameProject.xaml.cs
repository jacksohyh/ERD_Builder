using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERD_Builder.View.Popup
{
    /// <summary>
    /// Interaction logic for NameProject.xaml
    /// </summary>
    public partial class NameProject : Window
    {
        public string ProjectName { get; private set; }

        public NameProject()
        {
            InitializeComponent();
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Capture the input project name
            ProjectName = ProjectNameTextBox.Text;

            // Close the dialog with a positive result if the name is valid
            if (!string.IsNullOrWhiteSpace(ProjectName))
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid project name.");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Close the dialog without returning a result
            this.DialogResult = false;
            this.Close();
        }
    }
}
