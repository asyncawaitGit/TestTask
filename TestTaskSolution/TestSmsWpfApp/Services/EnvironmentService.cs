using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using TestSmsWpfApp.Models;

namespace TestSmsWpfApp.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EnvironmentService> _logger;
        private readonly string _commentsPath = "comments.json";

        public EnvironmentService(
            IConfiguration configuration,
            ILogger<EnvironmentService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public IReadOnlyCollection<EnvironmentVariableModel> LoadVariables()
        {
            var comments = LoadComments();

            var names = _configuration
                .GetSection("EnvironmentVariables")
                .Get<string[]>() ?? [];

            var result = new List<EnvironmentVariableModel>();

            foreach (var name in names)
            {
                var value = Environment.GetEnvironmentVariable(
                    name,
                    EnvironmentVariableTarget.Machine) ?? string.Empty;

                result.Add(new EnvironmentVariableModel
                {
                    Name = name,
                    Value = value,
                    Comment = comments.GetValueOrDefault(name, string.Empty)
                });
            }

            return result;
        }

        private Dictionary<string, string> LoadComments()
        {
            if (!File.Exists(_commentsPath))
                return new Dictionary<string, string>();

            var json = File.ReadAllText(_commentsPath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                   ?? new Dictionary<string, string>();
        }

        public void SaveComment(EnvironmentVariableModel variable)
        {
            var comments = LoadComments();
            comments[variable.Name] = variable.Comment;

            File.WriteAllText(
                _commentsPath,
                JsonSerializer.Serialize(comments, new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
        }

        public void SaveVariable(EnvironmentVariableModel variable)
        {
            Environment.SetEnvironmentVariable(
                variable.Name,
                variable.Value,
                EnvironmentVariableTarget.Machine);

            _logger.LogInformation(
                "Переменная {Name} изменена. Новое значение: {Value}",
                variable.Name,
                variable.Value);
        }
    }
}
