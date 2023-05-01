using System.Globalization;

namespace pa5working
{
    public class Booking
    {
        public int SessionID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime TrainingDate { get; set; }
        public TimeSpan SessionTime { get; set; }
        public int TrainerID { get; set; }
        public string TrainerName { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }

        public static List<Booking> ReadBookingsFromFile(string filename)
        {
            List<Booking> bookings = new List<Booking>();

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split('#');
                        Booking booking = new Booking
                        {
                            SessionID = int.Parse(parts[0]),
                            CustomerName = parts[1],
                            CustomerEmail = parts[2],
                            TrainingDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            SessionTime = TimeSpan.Parse(parts[4]),
                            TrainerID = int.Parse(parts[5]),
                            TrainerName = parts[6],
                            Status = parts[7],
                            Cost = decimal.Parse(parts[8])
                        };
                        bookings.Add(booking);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found: {filename}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }

            return bookings;
        }

        public static void WriteBookingsToFile(string filename, List<Booking> bookings)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    foreach (var booking in bookings)
                    {
                        string line = $"{booking.SessionID}#{booking.CustomerName}#{booking.CustomerEmail}#{booking.TrainingDate.ToString("yyyy-MM-dd")}#{booking.SessionTime.ToString(@"hh\:mm")}#{booking.TrainerID}#{booking.TrainerName}#{booking.Status}#{booking.Cost}";

                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing file: {ex.Message}");
            }
        }
    }
}