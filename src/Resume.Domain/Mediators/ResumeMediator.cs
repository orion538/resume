using Microsoft.Extensions.Logging;
using Resume.Pdf;
using System.Text.Json;

namespace Resume.Domain.Mediators
{
    public interface IResumeMediator
    {
        void GenerateResume(string pathToFile);
    }

    public class ResumeMediator : IResumeMediator
    {
        private readonly IResumeGenerator _resumeGenerator;
        private readonly ILogger<ResumeMediator> _logger;

        public ResumeMediator(IResumeGenerator resumeGenerator, ILogger<ResumeMediator> logger)
        {
            _resumeGenerator = resumeGenerator;
            _logger = logger;
        }

        public void GenerateResume(string pathToFile)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var resume = JsonParser.GetModelFromJson<Data.Json.Resume>(options, pathToFile);
            _logger.LogInformation("Mapped resume to an object.");

            CreatePdfDocument(resume);
            _logger.LogInformation("Created PDF.");
        }

        private void CreatePdfDocument(Data.Json.Resume resume)
        {
            _resumeGenerator.Create(resume);
        }
    }
}
