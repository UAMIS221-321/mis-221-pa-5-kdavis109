namespace pa5working
{
public class TrainerUtility
{
    private List<Trainer> Trainers { get; set; }

    public TrainerUtility(List<Trainer> trainers)
    {
        Trainers = trainers;
    }

    public void ManageTrainerData()
    {
        int choice = 1;
        while (true)
        {
            Console.Clear();
            System.Console.WriteLine("Trainer Management");
            System.Console.WriteLine("Use the arrow keys to scroll through the menu options, then press Enter to select an option:");
            System.Console.WriteLine();

            // Display menu options
            string[] options = { "Add Trainer", "Edit Trainer", "Delete Trainer", "Go Back" };
            for (int i = 0; i < options.Length; i++)
            {
                if (choice == i + 1)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("> ");
                }
                else
                {
                    Console.Write("  ");
                }
                System.Console.WriteLine(options[i]);
                Console.ResetColor();
            }

            // Get user input
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    choice--;
                    if (choice < 1)
                    {
                        choice = options.Length;
                    }
                    break;
                case ConsoleKey.DownArrow:
                    choice++;
                    if (choice > options.Length)
                    {
                        choice = 1;
                    }
                    break;
                case ConsoleKey.Enter:
                    switch (choice)
                    {
                        case 1:
                            AddTrainer();
                            break;
                        case 2:
                            EditTrainer();
                            break;
                        case 3:
                            DeleteTrainer();
                            break;
                        case 4:
                            return; // Go back to the main menu
                        default:
                            System.Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                    break;
            }
        }
    }

    private void AddTrainer()
    {
        Console.Clear();
        System.Console.WriteLine("Add Trainer");

        int trainerId = 0;
        while (trainerId == 0)
        {
            Console.Write("Enter Trainer ID: ");
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                trainerId = result;
            }
            else
            {
                System.Console.WriteLine("Invalid input. Please enter a valid integer value.");
            }
        }

        Console.Write("Enter Trainer Name: ");
        string trainerName = Console.ReadLine();

        Console.Write("Enter Mailing Address: ");
        string mailingAddress = Console.ReadLine();

        string trainerEmail = "";
        while (!IsValidEmail(trainerEmail))
        {
            Console.Write("Enter Trainer Email Address: ");
            trainerEmail = Console.ReadLine();

            if (!IsValidEmail(trainerEmail))
            {
                System.Console.WriteLine("Invalid email address. Please enter a valid email address.");
            }
        }

        Trainer newTrainer = new Trainer
        {
            TrainerID = trainerId,
            TrainerName = trainerName,
            MailingAddress = mailingAddress,
            TrainerEmailAddress = trainerEmail
        };

        Trainers.Add(newTrainer);
        System.Console.WriteLine("Trainer added successfully.");

        System.Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }
    private void EditTrainer()
{
Console.Clear();
System.Console.WriteLine("Edit Trainer");

int trainerId;
while (true)
{
    Console.Write("Enter the Trainer ID of the trainer you want to edit: ");
    if (!int.TryParse(Console.ReadLine(), out trainerId))
    {
        System.Console.WriteLine("Invalid input. Please enter a valid integer.");
        continue;
    }
    Trainer trainerToEdit = Trainers.FirstOrDefault(t => t.TrainerID == trainerId);

    if (trainerToEdit == null)
    {
        System.Console.WriteLine("Trainer not found.");
    }
    else
    {
        Console.Write("Enter new Trainer Name (leave empty to keep current value): ");
        string trainerName = Console.ReadLine();
        if (!string.IsNullOrEmpty(trainerName))
        {
            trainerToEdit.TrainerName = trainerName;
        }

        Console.Write("Enter new Mailing Address (leave empty to keep current value): ");
        string mailingAddress = Console.ReadLine();
        if (!string.IsNullOrEmpty(mailingAddress))
        {
            trainerToEdit.MailingAddress = mailingAddress;
        }

        Console.Write("Enter new Trainer Email Address (leave empty to keep current value): ");
        string trainerEmail = Console.ReadLine();
        while (!string.IsNullOrEmpty(trainerEmail) && !IsValidEmail(trainerEmail))
        {
            System.Console.WriteLine("Invalid input. Please enter a valid email address (e.g. example@example.com).");
            trainerEmail = Console.ReadLine();
        }
        if (!string.IsNullOrEmpty(trainerEmail))
        {
            trainerToEdit.TrainerEmailAddress = trainerEmail;
        }

        System.Console.WriteLine("Trainer updated successfully.");
    }

    System.Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
    return;
}
}

private void DeleteTrainer()
{
    Console.Clear();
    System.Console.WriteLine("Delete Trainer");

    Console.Write("Enter the Trainer ID of the trainer you want to delete: ");
    int trainerId;
    while (!int.TryParse(Console.ReadLine(), out trainerId))
    {
        System.Console.WriteLine("Invalid input. Please enter a valid integer.");
    }
    Trainer trainerToDelete = Trainers.FirstOrDefault(t => t.TrainerID == trainerId);

    if (trainerToDelete == null)
    {
        System.Console.WriteLine("Trainer not found.");
    }
    else
    {
        Trainers.Remove(trainerToDelete);
        System.Console.WriteLine("Trainer deleted successfully.");
    }

    System.Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

private bool IsValidEmail(string email)
{
    try
    {
        var addr = new System.Net.Mail.MailAddress(email);
        return addr.Address == email;
    }
    catch
    {
        return false;
    }
}
    }}