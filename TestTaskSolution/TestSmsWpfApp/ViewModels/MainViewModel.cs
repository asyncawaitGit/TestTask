using System.Collections.ObjectModel;
using TestSmsWpfApp.Models;
using TestSmsWpfApp.Services;

namespace TestSmsWpfApp.ViewModels
{
    public class MainViewModel
    {
        private readonly IEnvironmentService _environmentService;

        public ObservableCollection<EnvironmentVariableModel> Variables { get; }

        public MainViewModel(IEnvironmentService environmentService)
        {
            _environmentService = environmentService;

            Variables = new ObservableCollection<EnvironmentVariableModel>(
                _environmentService.LoadVariables());
        }

        public void SaveAll()
        {
            foreach (var variable in Variables)
            {
                _environmentService.SaveVariable(variable);
            }
        }

        public void SaveVariable(EnvironmentVariableModel variable)
        {
            _environmentService.SaveVariable(variable);
        }

        public void SaveComment(EnvironmentVariableModel variable)
        {
            _environmentService.SaveComment(variable);
        }
    }
}
