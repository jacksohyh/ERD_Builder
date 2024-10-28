using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERD_Builder.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Projects { get; set; } = new ObservableCollection<string>();
        private string _selectedProject;

        public string SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        public void AddProject(string projectName)
        {
            Projects.Add(projectName);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
