using FileCabinetApp.CommandHandlers;
using FileCabinetApp;
using System.Text.RegularExpressions;

public class UpdateCommandHandler : ServiceCommandHandlerBase
{
    public UpdateCommandHandler(IFileCabinetService service)
        : base(service)
    {
    }

    public override void Handle(AppCommandRequest commandRequest)
    {
        if (commandRequest.Command.Equals("update", StringComparison.InvariantCultureIgnoreCase))
        {
            this.Update(commandRequest.Parameters);
        }
        else
        {
            base.Handle(commandRequest);
        }
    }

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
                switch (criterion.Key.ToLower())
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
                        // Add other criteria as needed
                }
            }

            if (matchCriteria)
            {
                foreach (var update in updates)
                {
                    switch (update.Key.ToLower())
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
                            // Add other updates as needed
                    }
                }

                this.service.EditRecord(record.Id, record.FirstName, record.LastName, record.DateOfBirth, record.Age, record.Salary, record.Gender);
            }
        }
    }
}
