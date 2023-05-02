using System.Globalization;
using static System.Globalization.DateTimeStyles;

namespace pa5working{
public class ListingUtility

{
private List<Listing> Listings { get; set; }
private List<Trainer> Trainers { get; set; }

public ListingUtility
(List<Listing> listings, List<Trainer> trainers)
{
    Listings = listings;
    Trainers = trainers;
}

public void ManageListingData()
{
int selectedIndex = 0;

while (true)
{
    Console.Clear();
    System.Console.WriteLine("Listing Management");

    // Display menu options
    string[] menuOptions = new string[] { "Add Listing", "Edit Listing", "Delete Listing", "Go Back" };
    for (int i = 0; i < menuOptions.Length; i++)
    {
        if (i == selectedIndex)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            System.Console.WriteLine("  " + menuOptions[i] + "  ");
        }
        else
        {
            System.Console.WriteLine("  " + menuOptions[i] + "  ");
        }
        Console.ResetColor();
    }

    // Handle arrow key input
    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
    switch (keyInfo.Key)
    {
        case ConsoleKey.UpArrow:
            selectedIndex--;
            if (selectedIndex < 0)
            {
                selectedIndex = menuOptions.Length - 1;
            }
            break;
        case ConsoleKey.DownArrow:
            selectedIndex++;
            if (selectedIndex == menuOptions.Length)
            {
                selectedIndex = 0;
            }
            break;
        case ConsoleKey.Enter:
            Console.Clear();
            switch (selectedIndex)
            {
                case 0:
                    AddListing();
                    break;
                case 1:
                    EditListing();
                    break;
                case 2:
                    DeleteListing();
                    break;
                case 3:
                    return; // Go back to the main menu
            }
            System.Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            break;
    }
}
}


private void AddListing()
{
Console.Write("Enter Listing ID: ");
int listingId;
while (!int.TryParse(Console.ReadLine(), out listingId))
{
    Console.Write("Invalid input. Please enter a valid integer value for Listing ID: ");
}

Console.Write("Enter Trainer ID: ");
int trainerId;
while (!int.TryParse(Console.ReadLine(), out trainerId))
{
    Console.Write("Invalid input. Please enter a valid integer value for Trainer ID: ");
}

Trainer trainer = Trainers.FirstOrDefault(t => t.TrainerID == trainerId);
if (trainer == null)
{
    System.Console.WriteLine("Trainer not found.");
    return;
}

Console.Write("Enter Date of the Session (yyyy-MM-dd): ");
DateTime sessionDate;
while (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out sessionDate))
{
    Console.Write("Invalid input. Please enter a valid date in the format yyyy-MM-dd: ");
}

Console.Write("Enter military time of the Session in (HH:mm): ");
TimeSpan sessionTime;
while (!TimeSpan.TryParse(Console.ReadLine(), out sessionTime))
{
    Console.Write("Invalid input. Please enter a valid military time in the format HH:mm: ");
}

Console.Write("Enter Cost of the Session: ");
decimal cost;
while (!decimal.TryParse(Console.ReadLine(), out cost))
{
    Console.Write("Invalid input. Please enter a valid decimal value for Cost: ");
}

Console.Write("Is the listing taken? (yes/no): ");
string isTakenInput = Console.ReadLine().ToLower();
bool isTaken;
while (isTakenInput != "yes" && isTakenInput != "no")
{
    Console.Write("Invalid input. Please enter 'yes' or 'no': ");
    isTakenInput = Console.ReadLine().ToLower();
}
isTaken = isTakenInput == "yes";

Listing newListing = new Listing
{
    ListingID = listingId,
    TrainerID = trainer.TrainerID,
    TrainerName = trainer.TrainerName,
    SessionDate = sessionDate,
    SessionTime = sessionTime,
    Cost = cost,
    IsTaken = isTaken
};

Listings.Add(newListing);
System.Console.WriteLine("Listing added successfully.");
}

private void EditListing()
{
Console.Write("Enter the Listing ID of the listing you want to edit: ");
int listingId;
while (!int.TryParse(Console.ReadLine(), out listingId))
{
    System.Console.WriteLine("Invalid input. Please enter a valid integer for Listing ID.");
}
Listing listingToEdit = Listings.FirstOrDefault(l => l.ListingID == listingId);

if (listingToEdit == null)
{
    System.Console.WriteLine("Listing not found.");
}
else
{
    Console.Write("Enter new Trainer ID (leave empty to keep current value): ");
    string trainerIdInput = Console.ReadLine();
    if (!string.IsNullOrEmpty(trainerIdInput))
    {
        int newTrainerId;
        while (!int.TryParse(trainerIdInput, out newTrainerId))
        {
            System.Console.WriteLine("Invalid input. Please enter a valid integer for Trainer ID.");
            trainerIdInput = Console.ReadLine();
        }

        Trainer newTrainer = Trainers.FirstOrDefault(t => t.TrainerID == newTrainerId);
        if (newTrainer != null)
        {
            listingToEdit.TrainerName = newTrainer.TrainerName;
        }
        else
        {
            System.Console.WriteLine("Trainer not found. Please try again.");
            return;
        }
    }

    Console.Write("Enter new Date of the Session (yyyy-MM-dd, leave empty to keep current value): ");
    string sessionDateInput = Console.ReadLine();
    if (!string.IsNullOrEmpty(sessionDateInput))
    {
        DateTime sessionDate;
        while (!DateTime.TryParseExact(sessionDateInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out sessionDate))
        {
            System.Console.WriteLine("Invalid input. Please enter a valid date in the format yyyy-MM-dd.");
            sessionDateInput = Console.ReadLine();
        }
        listingToEdit.SessionDate = sessionDate;
    }

            Console.Write("Enter new military Time of the Session (HH:mm, leave empty to keep current value): ");
        string sessionTimeInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(sessionTimeInput))
        {
            TimeSpan sessionTime;
            while (!TimeSpan.TryParseExact(sessionTimeInput, "hh\\:mm", CultureInfo.InvariantCulture, out sessionTime))
                {
                Console.WriteLine("Invalid input. Please enter a valid military time in the format HH:mm.");
                sessionTimeInput = Console.ReadLine();
            }
                listingToEdit.SessionTime = sessionTime;
            }

    Console.Write("Enter new Cost of the Session (leave empty to keep current value): ");
    string costInput = Console.ReadLine();
    if (!string.IsNullOrEmpty(costInput))
    {
        decimal cost;
        while (!decimal.TryParse(costInput, out cost))
        {
            System.Console.WriteLine("Invalid input. Please enter a valid decimal number.");
            costInput = Console.ReadLine();
        }
        listingToEdit.Cost = cost;
    }

                Console.Write("Update taken status (yes/no, leave empty to keep current value): ");
            string isTakenInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(isTakenInput))
{
                bool isTaken;
                    while (isTakenInput.ToLower() != "yes" && isTakenInput.ToLower() != "no")
                        {
                    System.Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                        isTakenInput = Console.ReadLine();
                    }
                        listingToEdit.IsTaken = (isTakenInput.ToLower() == "yes");
}


    System.Console.WriteLine("Listing updated successfully.");
}
}


private void DeleteListing()
{
Console.Write("Enter the Listing ID of the listing you want to delete: ");
int listingId;

while (!int.TryParse(Console.ReadLine(), out listingId))
{
    System.Console.WriteLine("Invalid input. Please enter a valid integer for Listing ID.");
}

Listing listingToDelete = Listings.FirstOrDefault(l => l.ListingID == listingId);

if (listingToDelete == null)
{
    System.Console.WriteLine("Listing not found.");
}
else
{
    Listings.Remove(listingToDelete);
    System.Console.WriteLine("Listing deleted successfully.");
}
}


}
}