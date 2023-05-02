using System.Globalization;

namespace pa5working
{
public class Listing
{
    public int ListingID { get; set; }
    public int TrainerID { get; set; }
    public string TrainerName { get; set; }
    public DateTime SessionDate { get; set; }
    public TimeSpan SessionTime { get; set; }
    public decimal Cost { get; set; }
    public bool IsTaken { get; set; }

    public static List<Listing> ReadListingsFromFile(string filename, List<Trainer> trainers)
    {
        List<Listing> listings = new List<Listing>();

try
{
    using (StreamReader reader = new StreamReader(filename))
    {
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] data = line.Split('#');

            int id = int.Parse(data[0]);
            int trainerId = int.Parse(data[1]);
            Trainer trainer = trainers.FirstOrDefault(t => t.TrainerID == trainerId);
            if (trainer == null)
            {
                Console.WriteLine($"Error: Trainer with ID {trainerId} not found.");
                continue;
            }
            DateTime sessionDate = DateTime.ParseExact(data[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            TimeSpan sessionTime = TimeSpan.Parse(data[3]);
            decimal cost = decimal.Parse(data[4]);
            bool isTaken = bool.Parse(data[5]);

            Listing listing = new Listing
            {
                ListingID = id,
                TrainerID = trainerId,
                TrainerName = trainer.TrainerName,
                SessionDate = sessionDate,
                SessionTime = sessionTime,
                Cost = cost,
                IsTaken = isTaken
            };

            listings.Add(listing);
        }
    }
}
catch (FileNotFoundException)
{
    Console.WriteLine($"Error: {filename} not found.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

return listings;
}

public static void WriteListingsToFile(string filename, List<Listing> listings)
{
try
{
    using (StreamWriter writer = new StreamWriter(filename))
    {
        foreach (Listing listing in listings)
        {
            string line = $"{listing.ListingID}#{listing.TrainerID}#{listing.SessionDate.ToString("yyyy-MM-dd")}#{listing.SessionTime.ToString("hh\\:mm")}#{listing.Cost}#{listing.IsTaken}";
            writer.WriteLine(line);
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
}
}
}