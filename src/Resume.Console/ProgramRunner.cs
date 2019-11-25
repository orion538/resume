using Microsoft.Extensions.Logging;
using Resume.Domain.Mediators;
using System.IO;

namespace Resume.Console
{
    public interface IProgramRunner
    {
        void Execute();
    }

    public class ProgramRunner : IProgramRunner
    {
        private readonly IResumeMediator _resumeMediator;
        private readonly IProgramDescription _programDescription;
        private readonly ILogger<ProgramRunner> _logger;

        public ProgramRunner(IResumeMediator resumeMediator, IProgramDescription programDescription, ILogger<ProgramRunner> logger)
        {
            _resumeMediator = resumeMediator;
            _programDescription = programDescription;
            _logger = logger;
        }

        public void Execute()
        {
            Description();

            _logger.LogInformation("Press a key to start running the application logic");
            System.Console.ReadKey(); // Momentarily pause by waiting for a key to be pressed before exiting the application.
            System.Console.Clear();

            Logic();

            _logger.LogInformation("Finished executing the program\r\n" +
                                   "--------------------------------------------------------------------------\r\n" +
                                   "Press a key to close the program.\r\n");
            System.Console.ReadKey(); // Momentarily pause by waiting for a key to be pressed before exiting the application.
        }

        private void Description()
        {
            _programDescription.Create();
        }

        private void Logic()
        {
            _logger.LogInformation("Executing the program\r\n" +
                                   "--------------------------------------------------------------------------\r\n" +
                                   "Please provide the path to the resume you want to convert into a pdf file:\r\n");

            var userInput = $"{System.Console.ReadLine()}";

            while (true)
            {
                if (File.Exists(userInput))
                {
                    _resumeMediator.GenerateResume(userInput);
                    break;
                }

                _logger.LogError("File does not exist. Please try again.");

                userInput = $"{System.Console.ReadLine()}";
            }
        }
    }
}
