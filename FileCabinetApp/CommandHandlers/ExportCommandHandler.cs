using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler for the "export" command.
    /// </summary>
    public class ExportCommandHandler : CommandHandlerBase
    {
        private const string ExportCommandText = "export";
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The file cabinet service to use for exporting records.</param>
        public ExportCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Handles the "export" command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.ToLower() == ExportCommandText)
            {
                this.Export(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        private void Export(string parameters)
        {
            var inputs = parameters.Split(' ', 2);
            if (inputs.Length < 2)
            {
                Console.WriteLine("Invalid parameters. Usage: export <format> <filename>");
                return;
            }

            var format = inputs[0];
            var path = inputs[1];

            if (format.Equals("csv", StringComparison.OrdinalIgnoreCase))
            {
                this.ExportCsv(path);
            }
            else if (format.Equals("xml", StringComparison.OrdinalIgnoreCase))
            {
                this.ExportXml(path);
            }
            else
            {
                Console.WriteLine($"Export in {format} format is not supported.");
            }
        }

        private void ExportCsv(string path)
        {
            if (File.Exists(path))
            {
                Console.Write($"File is exist - rewrite {path}? [Y/n] ");
                var answer = Console.ReadLine();
                if (answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            try
            {
                using var writer = new StreamWriter(path);
                var snapshot = this.service.MakeSnapshot();
                snapshot.SaveToCsv(writer);
                Console.WriteLine($"All records are exported to file {path}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }

        private void ExportXml(string path)
        {
            if (File.Exists(path))
            {
                Console.Write($"File is exist - rewrite {path}? [Y/n] ");
                var answer = Console.ReadLine();
                if (answer.Equals("n", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            try
            {
                using var writer = new StreamWriter(path);
                var snapshot = this.service.MakeSnapshot();
                snapshot.SaveToXml(writer);
                Console.WriteLine($"All records are exported to file {path}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export failed: {ex.Message}");
            }
        }
    }
}
