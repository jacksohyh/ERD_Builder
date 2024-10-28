using System.ComponentModel;

namespace ERD_Builder.ViewModel
{
    public class ProjectPageViewModel : INotifyPropertyChanged
    {
        private string _projectName;
        public string projectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(projectName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
