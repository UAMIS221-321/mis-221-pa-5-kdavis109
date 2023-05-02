namespace pa5working
{
    public class BookingUtility
    {
        private List<Booking> Bookings { get; set; }
        private List<Listing> Listings { get; set; }

        public BookingUtility(List<Booking> bookings, List<Listing> listings)
        {
            Bookings = bookings;
            Listings = listings;
        }

        public void ManageCustomerBookingData()
        {
            int selectedIndex = 0;
            while (true)
            {
                Console.Clear();
                System.Console.WriteLine("Booking Management");
                System.Console.WriteLine();

                // Print menu options
                string[] options = { "View Available Training Sessions", "Book a Session", "Complete a Session", "Cancel a Session", "Go Back" };
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
                ConsoleKeyInfo key = Console.ReadKey(true);
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
                                ViewAvailableTrainingSessions();
                                break;
                            case 1:
                                BookSession();
                                break;
                            case 2:
                                CompleteSession();
                                break;
                            case 3:
                                CancelSession();
                                break;
                            case 4:
                                return; // Go back to the main menu
                            default:
                                System.Console.WriteLine("Invalid choice. Please try again.");
                                break;
                        }
                        System.Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    default:
                        break;
                }
            }
        }

private void ViewAvailableTrainingSessions()
{
    Console.Clear();
    System.Console.WriteLine("╔═════════════════════════════════════════════════════════════╗");
    System.Console.WriteLine("║                 Available Training Sessions                 ║");
    System.Console.WriteLine("╟─────────────────────────────────────────────────────────────╢");
    System.Console.WriteLine("║    ID    |       Trainer      |     Date   | Time  | Cost   ║");
    System.Console.WriteLine("╟──────────┼────────────────────┼────────────┼───────┼────────║");

    foreach (Listing listing in Listings.Where(l => !l.IsTaken))
    {
        System.Console.WriteLine($"║{listing.ListingID,-10}|{listing.TrainerName,-20}|{listing.SessionDate.ToShortDateString(),-12}|{listing.SessionTime.ToString("hh\\:mm"),-7}|${listing.Cost,-7}║");
    }

    System.Console.WriteLine("╚══════════╧════════════════════╧════════════╧═══════╧════════╝");
}

private void CompleteSession()
{
    Console.Write("\nEnter the ID of the session you want to complete: ");
    int sessionId;
    while (!int.TryParse(Console.ReadLine(), out sessionId))
    {
        Console.WriteLine("Invalid input. Please enter a valid integer.");
    }

    Booking booking = Bookings.FirstOrDefault(b => b.SessionID == sessionId && b.Status == "Booked");

    if (booking != null && DateTime.Now >= booking.TrainingDate)
    {
        booking.Status = "Completed";

        // Prompt for feedback
        Console.Write("Would you like to leave feedback for your trainer? (Y/N): ");
        string feedbackChoice = Console.ReadLine().ToUpper();
        while (feedbackChoice != "Y" && feedbackChoice != "N")
        {
            Console.WriteLine("Invalid input. Please enter 'Y' or 'N'.");
            feedbackChoice = Console.ReadLine().ToUpper();
        }
        
        if (feedbackChoice == "Y")
        {
            Console.Write("Enter your feedback: ");
            string feedbackText = Console.ReadLine();

            // Save feedback to file
            using (StreamWriter sw = File.AppendText("feedback.txt"))
            {
                sw.WriteLine($"Trainer ID: {booking.TrainerID}");
                sw.WriteLine($"Feedback: {feedbackText}");
                sw.WriteLine();
            }
            Console.WriteLine("Thank you for your feedback!");
        }
        else
        {
            Console.WriteLine("Thank you for completing the session!");
        }
    }
    else
    {
        System.Console.WriteLine("Booking not found or already completed/cancelled, or the session date has not yet passed.");
    }
}

    
private void BookSession()
{
    Console.Write("\nEnter the ID of the session you want to book: ");
    if (!int.TryParse(Console.ReadLine(), out int sessionId))
    {
        System.Console.WriteLine("Invalid input. Please enter a valid integer for Session ID.");
        return;
    }

    Listing listing = Listings.FirstOrDefault(l => l.ListingID == sessionId && !l.IsTaken);

    if (listing == null)
    {
        System.Console.WriteLine("Session not found or already taken.");
        return;
    }

    Console.Write("Enter your name: ");
    string customerName = Console.ReadLine();
    if (string.IsNullOrEmpty(customerName))
    {
        System.Console.WriteLine("Invalid input. Name cannot be empty.");
        return;
    }

    string customerEmail = "";
    while (true)
    {
        Console.Write("Enter your email: ");
        customerEmail = Console.ReadLine();
        if (IsValidEmail(customerEmail))
        {
            break;
        }
        System.Console.WriteLine("Invalid input. Please enter a valid email address.");
    }

    decimal cost = listing.Cost;

    // Add discount code feature
   Console.Write("Enter discount code (if applicable): ");
string discountCode = Console.ReadLine();
while (!string.IsNullOrEmpty(discountCode))
{
    // Apply 20% discount if code is valid
    if (discountCode.ToLower() == "newstart")
    {
        cost *= 0.8m;
        break; // exit the while loop if discount applied successfully
    }
    else
    {
        System.Console.WriteLine("Invalid discount code.");
        Console.Write("Enter discount code (if applicable): ");
        discountCode = Console.ReadLine();
    }
}

    Booking newBooking = new Booking
    {
        SessionID = sessionId,
        CustomerName = customerName,
        CustomerEmail = customerEmail,
        TrainingDate = listing.SessionDate,
        SessionTime = listing.SessionTime,
        TrainerID = listing.TrainerID,
        TrainerName = listing.TrainerName,
        Status = "Booked",
        Cost = cost
    };

    Bookings.Add(newBooking);
    listing.IsTaken = true;

    System.Console.WriteLine("Session booked successfully.");
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

       private void CancelSession()
{
    Console.Write("\nEnter the ID of the session you want to cancel: ");
    int sessionId;
    while (!int.TryParse(Console.ReadLine(), out sessionId))
    {
        System.Console.WriteLine("Invalid input. Please enter a valid integer for Session ID.");
    }

    Booking booking = Bookings.FirstOrDefault(b => b.SessionID == sessionId && b.Status == "Booked");

    if (booking != null && DateTime.Now <= booking.TrainingDate)
    {
        booking.Status = "Cancelled";
        Listing listing = Listings.FirstOrDefault(l => l.ListingID == sessionId);
        if (listing != null)
        {
            listing.IsTaken = false;
        }
        System.Console.WriteLine("Session cancelled.");
    }
    else
    {
        System.Console.WriteLine("Booking not found or already completed/cancelled, or the session date has already passed.");
    }
}

    }
}
