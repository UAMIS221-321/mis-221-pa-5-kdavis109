namespace pa5working
{
public class ReportUtility
{
private List<Booking> Bookings { get; set; }
private List<Listing> Listings { get; set; }
private string ListingsFilename { get; set; }

public ReportUtility(List<Booking> bookings, List<Listing> listings, string listingsFilename)
{
    Bookings = bookings;
    Listings = listings;
    ListingsFilename = listingsFilename;
}
public void RunReports()
{
int selectedIndex = 0;

while (true)
{
Console.Clear();
System.Console.WriteLine("Report Management");
System.Console.WriteLine();

// Print menu options
string[] options = { "Individual Customer Sessions", "Historical Customer Sessions", "Historical Revenue Report", "Go Back" };
for (int i = 0; i < options.Length; i++)
{
    if (i == selectedIndex)
    {
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;
    }
    System.Console.WriteLine(options[i]);
    Console.ResetColor();
}

// Get user input
ConsoleKeyInfo key;
bool isValidKey;
do
{
    System.Console.WriteLine();
    Console.Write("Enter your selection: ");
    key = Console.ReadKey(true);
    isValidKey = key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.Enter;
} while (!isValidKey);

switch (key.Key)
{
    case ConsoleKey.UpArrow:
        selectedIndex = (selectedIndex == 0) ? options.Length - 1 : selectedIndex - 1;
        break;
    case ConsoleKey.DownArrow:
        selectedIndex = (selectedIndex == options.Length - 1) ? 0 : selectedIndex + 1;
        break;
    case ConsoleKey.Enter:
        Console.Clear();
        switch (selectedIndex)
        {
                    case 0:
        // Individual Customer Sessions report
        while (true)
        {
            Console.Write("Enter the customer email address or press 'Enter' to go back: ");
            string email = Console.ReadLine();
            if (string.IsNullOrEmpty(email))
            {
                break;
            }

            var customerSessions = Bookings.Where(b => b.CustomerEmail == email).ToList();

            if (customerSessions.Count == 0)
            {
                System.Console.WriteLine("There are no sessions for the entered email. Please try again.");
                continue;
            }

            System.Console.WriteLine("\nIndividual Customer Sessions:");
            foreach (var session in customerSessions)
            {
                System.Console.WriteLine($"- Session ID: {session.SessionID}, Trainer ID: {session.TrainerID}, Trainer Name: {session.TrainerName}, Date: {session.TrainingDate.ToString("MM/dd/yyyy")}, Time: {session.SessionTime.ToString("hh\\:mm")}, Status: {session.Status}");



            }

            System.Console.WriteLine("\nPress 'S' to save the report to a file or any other key to continue.");
            var saveReportKey = Console.ReadKey(true);
            if (saveReportKey.Key == ConsoleKey.S)
            {
                SaveIndividualCustomerSessionsReport(customerSessions, email);
            }
            break;
        }
        break;


            case 1:
                // Historical Customer Sessions report
                var groupedSessions = Bookings.GroupBy(b => b.CustomerEmail).OrderBy(g => g.Key);

                System.Console.WriteLine("\nHistorical Customer Sessions:");
                foreach (var group in groupedSessions)
                {
                    System.Console.WriteLine($"Customer Email: {group.Key}");
                    System.Console.WriteLine($"Total Sessions: {group.Count()}");
                    foreach (var session in group.OrderBy(s => s.TrainingDate))
                    {
                        System.Console.WriteLine($"- Session ID: {session.SessionID}, Trainer ID: {session.TrainerID}, Trainer Name: {session.TrainerName}, Date: {session.TrainingDate.ToString("MM/dd/yyyy")}, Time: {session.SessionTime.ToString("hh\\:mm")}, Status: {session.Status}");



                    }
                    System.Console.WriteLine();
                }

                System.Console.WriteLine("\nPress 'S' to save the report to a file or any other key to continue.");
                var saveHistoricalReportKey = Console.ReadKey(true);
                if (saveHistoricalReportKey.Key == ConsoleKey.S)
                {
                    SaveHistoricalCustomerSessionsReport(groupedSessions);
                }
                break;
            case 2:
// Historical Revenue report
var groupedSessionsByMonth = Bookings
    .GroupBy(b => b.TrainingDate.ToString("yyyy-MM"))
    .OrderBy(g => g.Key);

System.Console.WriteLine("\nHistorical Revenue Report:");
foreach (var group in groupedSessionsByMonth)
{
    decimal revenue = group.Sum(b => b.Cost); // Use the Cost property instead of SessionCost
    System.Console.WriteLine($"Month: {group.Key}, Revenue: ${revenue}");
}

System.Console.WriteLine("\nPress 'S' to save the report to a file or any other key to continue.");
var saveHistoricalRevenueReportKey = Console.ReadKey(true);
if (saveHistoricalRevenueReportKey.Key == ConsoleKey.S)
{
    SaveHistoricalRevenueReport(groupedSessionsByMonth);
}
break;
            case 3:
                // Go back to the main menu
                return;
            default:
                // Invalid selection
                System.Console.WriteLine("Invalid selection. Please try again.");
                System.Console.WriteLine("Press any key to continue.");
                Console.ReadKey(true);
                break;
        }
        break;
}
}
}

private void SaveIndividualCustomerSessionsReport(List<Booking> customerSessions, string email)
{
    string filename = $"Individual_Customer_Sessions_{email.Replace("@", "_at_")}.txt";
    using (StreamWriter writer = new StreamWriter(filename))
    {
        writer.WriteLine("Individual Customer Sessions:");
        foreach (var session in customerSessions)
        {
            writer.WriteLine($"- Session ID: {session.SessionID}, Trainer ID: {session.TrainerID}, Trainer Name: {session.TrainerName}, Date: {session.TrainingDate.ToString("M/d/yyyy")}, Time: {session.SessionTime.ToString("hh\\:mm")}, Status: {session.Status}");


        }
    }
    System.Console.WriteLine($"Report saved as {filename}");
}

private void SaveHistoricalCustomerSessionsReport(IEnumerable<IGrouping<string, Booking>> groupedSessions)
{
string filename = "Historical_Customer_Sessions.txt";
using (StreamWriter writer = new StreamWriter(filename))
{
writer.WriteLine("Historical Customer Sessions:");
foreach (var group in groupedSessions)
{
    writer.WriteLine($"Customer Email: {group.Key}");
    writer.WriteLine($"Total Sessions: {group.Count()}");
    foreach (var session in group.OrderBy(s => s.TrainingDate))
    {
        writer.WriteLine($"- Session ID: {session.SessionID}, Trainer ID: {session.TrainerID}, Trainer Name: {session.TrainerName}, Date: {session.TrainingDate.ToString("M/d/yyyy")}, Time: {session.SessionTime.ToString("hh\\:mm")}, Status: {session.Status}");




    }
    writer.WriteLine();
}
}
System.Console.WriteLine($"Report saved as {filename}");
}


private void SaveHistoricalRevenueReport(IEnumerable<IGrouping<string, Booking>> groupedSessionsByMonth)
{
    string filename = "Historical_Revenue_Report.txt";
    using (StreamWriter writer = new StreamWriter(filename))
    {
        writer.WriteLine("Historical Revenue Report:");
        foreach (var group in groupedSessionsByMonth)
        {
            decimal revenue = group.Sum(b => b.Cost); // Use the Cost property
            writer.WriteLine($"Month: {group.Key}, Revenue: ${revenue}");
        }
    }
    System.Console.WriteLine($"Report saved as {filename}");
}

    }
}
