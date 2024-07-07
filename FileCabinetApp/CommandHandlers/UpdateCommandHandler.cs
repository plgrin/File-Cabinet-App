#pragma warning disable CA1305
using System.Text.RegularExpressions;
using FileCabinetApp.Services;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Handles the 'update' command to update records in the file cabinet.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">The file cabinet service to be used by the handler.</param>
        public UpdateCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <summary>
        /// Handles the specified command request.
        /// </summary>
        /// <param name="request">The command request.</param>
        public override void Handle(AppCommandRequest request)
        {
            if (request.Command.Equals("update", StringComparison.OrdinalIgnoreCase))
            {
                this.Update(request.Parameters);
            }
            else
            {
                base.Handle(request);
            }
        }

        /// <summary>
        /// Updates records in the file cabinet based on the specified parameters.
        /// </summary>
        /// <param name="parameters">The update command parameters.</param>
        private void Update(string parameters)
        {
            var updatePattern = @"set (.*) where (.*)";
            var match = Regex.Match(parameters, updatePattern, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                Console.WriteLine("Invalid update command format.");
                return;
            }

            var setPart = match.Groups[1].Value;
            var wherePart = match.Groups[2].Value;

            var updates = setPart.Split(',')
                .Select(u => u.Split('='))
                .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim().Trim('\''));

            var criteria = wherePart.Split(new string[] { " and " }, StringSplitOptions.None)
                .Select(c => c.Split('='))
                .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim().Trim('\''));

            var records = this.service.GetRecords();

            foreach (var record in records)
            {
                bool matchCriteria = true;
                foreach (var criterion in criteria)
                {
                    switch (criterion.Key.ToLower(System.Globalization.CultureInfo.CurrentCulture))
                    {
                        case "id":
                            matchCriteria &= record.Id == int.Parse(criterion.Value);
                            break;
                        case "firstname":
                            matchCriteria &= record.FirstName.Equals(criterion.Value, StringComparison.OrdinalIgnoreCase);
                            break;
                        case "lastname":
                            matchCriteria &= record.LastName.Equals(criterion.Value, StringComparison.OrdinalIgnoreCase);
                            break;
                        case "dateofbirth":
                            matchCriteria &= record.DateOfBirth == DateTime.Parse(criterion.Value);
                            break;
                        case "age":
                            matchCriteria &= record.Age == short.Parse(criterion.Value);
                            break;
                        case "salary":
                            matchCriteria &= record.Salary == decimal.Parse(criterion.Value);
                            break;
                        case "gender":
                            matchCriteria &= record.Gender == char.Parse(criterion.Value);
                            break;
                    }
                }

                if (matchCriteria)
                {
                    foreach (var update in updates)
                    {
                        switch (update.Key.ToLower(System.Globalization.CultureInfo.CurrentCulture))
                        {
                            case "firstname":
                                record.FirstName = update.Value;
                                break;
                            case "lastname":
                                record.LastName = update.Value;
                                break;
                            case "dateofbirth":
                                record.DateOfBirth = DateTime.Parse(update.Value);
                                break;
                            case "age":
                                record.Age = short.Parse(update.Value);
                                break;
                            case "salary":
                                record.Salary = decimal.Parse(update.Value);
                                break;
                            case "gender":
                                record.Gender = char.Parse(update.Value);
                                break;
                        }
                    }

                    this.service.EditRecord(record.Id, record.FirstName, record.LastName, record.DateOfBirth, record.Age, record.Salary, record.Gender);
                }
            }
        }
    }
}
