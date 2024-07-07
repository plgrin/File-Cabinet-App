#pragma warning disable S108
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Base class for command handlers.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler? nextHandler;

        /// <summary>
        /// Sets the next handler in the chain.
        /// </summary>
        /// <param name="handler">The next handler.</param>
        /// <returns>The current handler for chaining.</returns>
        public ICommandHandler SetNext(ICommandHandler handler)
        {
            this.nextHandler = handler;
            return handler;
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public virtual void Handle(AppCommandRequest request)
        {
            if (this.nextHandler != null)
            {
                this.nextHandler.Handle(request);
            }
            else
            {
                this.PrintMissedCommandInfo(request.Command);
            }
        }

        /// <summary>
        /// Prints a message indicating that the command was not recognized.
        /// </summary>
        /// <param name="command">The unrecognized command.</param>
        protected void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            var suggestions = GetSuggestions(command);
            if (suggestions.Any())
            {
                Console.WriteLine("The most similar command(s) are:");
                foreach (var suggestion in suggestions)
                {
                    Console.WriteLine($"\t{suggestion}");
                }
            }

            Console.WriteLine();
        }

        private static IEnumerable<string> GetSuggestions(string command)
        {
            var availableCommands = new string[]
            {
                "help", "exit", "stat", "create", "list", "find", "export", "import", "purge", "insert", "delete", "update",
            };

            var threshold = 3; // Maximum allowed edit distance
            return availableCommands
                .Select(c => new { Command = c, Distance = LevenshteinDistance(command, c) })
                .Where(c => c.Distance <= threshold)
                .OrderBy(c => c.Distance)
                .Take(3) // Take top 3 closest commands
                .Select(c => c.Command);
        }

        private static int LevenshteinDistance(string source, string target)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.IsNullOrEmpty(target) ? 0 : target.Length;
            }

            if (string.IsNullOrEmpty(target))
            {
                return source.Length;
            }

            var sourceLength = source.Length;
            var targetLength = target.Length;
            var distance = new int[sourceLength + 1, targetLength + 1];

            for (var i = 0; i <= sourceLength; distance[i, 0] = i++)
            {
            }

            for (var j = 0; j <= targetLength; distance[0, j] = j++)
            {
            }

            for (var i = 1; i <= sourceLength; i++)
            {
                for (var j = 1; j <= targetLength; j++)
                {
                    var cost = target[j - 1] == source[i - 1] ? 0 : 1;
                    distance[i, j] = Math.Min(
                        Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
                        distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceLength, targetLength];
        }
    }
}
