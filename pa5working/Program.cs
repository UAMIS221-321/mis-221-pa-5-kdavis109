using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace pa5working
{
    class Program
    {
        const string TrainersFilename = "trainers.txt";
        const string ListingsFilename = "listings.txt";
        const string TransactionsFilename = "transactions.txt";

        static void Main(string[] args)
        {
            var trainers = Trainer.ReadTrainersFromFile(TrainersFilename);
            var listings = Listing.ReadListingsFromFile(ListingsFilename, trainers);
            var bookings = Booking.ReadBookingsFromFile(TransactionsFilename);

            TrainerUtility trainerUtility = new TrainerUtility(trainers);
            ListingUtility listingUtility = new ListingUtility(listings, trainers);
            BookingUtility bookingUtility = new BookingUtility(bookings, listings);
            ReportUtility reportUtility = new ReportUtility(bookings, listings, ListingsFilename);

            int choice = 1;
            bool exit = false;
            do
            {
                Console.Clear();
                System.Console.WriteLine(@"

██████╗ ███████╗██████╗ ███████╗ ██████╗ ███╗   ██╗ █████╗ ██╗         ███████╗██╗████████╗███╗   ██╗███████╗███████╗███████╗    ███╗   ███╗ █████╗ ███╗   ██╗ █████╗  ██████╗ ███████╗██████╗     
██╔══██╗██╔════╝██╔══██╗██╔════╝██╔═══██╗████╗  ██║██╔══██╗██║         ██╔════╝██║╚══██╔══╝████╗  ██║██╔════╝██╔════╝██╔════╝    ████╗ ████║██╔══██╗████╗  ██║██╔══██╗██╔════╝ ██╔════╝██╔══██╗    
██████╔╝█████╗  ██████╔╝███████╗██║   ██║██╔██╗ ██║███████║██║         █████╗  ██║   ██║   ██╔██╗ ██║█████╗  ███████╗███████╗    ██╔████╔██║███████║██╔██╗ ██║███████║██║  ███╗█████╗  ██████╔╝    
██╔═══╝ ██╔══╝  ██╔══██╗╚════██║██║   ██║██║╚██╗██║██╔══██║██║         ██╔══╝  ██║   ██║   ██║╚██╗██║██╔══╝  ╚════██║╚════██║    ██║╚██╔╝██║██╔══██║██║╚██╗██║██╔══██║██║   ██║██╔══╝  ██╔══██╗    
██║     ███████╗██║  ██║███████║╚██████╔╝██║ ╚████║██║  ██║███████╗    ██║     ██║   ██║   ██║ ╚████║███████╗███████║███████║    ██║ ╚═╝ ██║██║  ██║██║ ╚████║██║  ██║╚██████╔╝███████╗██║  ██║    
╚═╝     ╚══════╝╚═╝  ╚═╝╚══════╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═╝╚══════╝    ╚═╝     ╚═╝   ╚═╝   ╚═╝  ╚═══╝╚══════╝╚══════╝╚══════╝    ╚═╝     ╚═╝╚═╝  ╚═╝╚═╝  ╚═══╝╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═╝    
                                                                                                                                                                                                   
                                                                                            
");
// Display menu options
string[] options = { "Manage Trainer Data", "Manage Listing Data", "Manage Customer Booking Data", "Run Reports", "Exit Application" };
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
                trainerUtility.ManageTrainerData();
                break;
            case 2:
                listingUtility.ManageListingData();
                break;
            case 3:
                bookingUtility.ManageCustomerBookingData();
                break;
            case 4:
                reportUtility.RunReports();
                break;
            case 5:
                System.Console.WriteLine("Exiting the application...");
                Trainer.WriteTrainersToFile(TrainersFilename, trainers);
                Listing.WriteListingsToFile(ListingsFilename, listings);
                Booking.WriteBookingsToFile(TransactionsFilename, bookings);
                exit = true;
                break;
            default:
                System.Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
        break;
}
            } while(!exit);
        }
    }
}
