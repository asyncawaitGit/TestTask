using TestSmsWpfApp.Models;

namespace TestSmsWpfApp.Services
{
    public interface IEnvironmentService
    {
        IReadOnlyCollection<EnvironmentVariableModel> LoadVariables();
        void SaveVariable(EnvironmentVariableModel variable);
        void SaveComment(EnvironmentVariableModel variable);
    }
}
