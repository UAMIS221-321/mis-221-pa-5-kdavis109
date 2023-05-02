namespace pa5working
{
    public class Trainer
    {
        public int TrainerID { get; set; }
        public string TrainerName { get; set; }
        public string MailingAddress { get; set; }
        public string TrainerEmailAddress { get; set; }

public static List<Trainer> ReadTrainersFromFile(string filename)
{
    List<Trainer> trainers = new List<Trainer>();
    using (StreamReader reader = new StreamReader(filename))
    {
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] parts = line.Split('#');
            Trainer trainer = new Trainer
            {
                TrainerID = int.Parse(parts[0]),
                TrainerName = parts[1],
                MailingAddress = parts[2],
                TrainerEmailAddress = parts[3]
            };
            trainers.Add(trainer);
        }
    }
    return trainers;
}

    public static void WriteTrainersToFile(string filename, List<Trainer> trainers)  //Write to file
{
    using (StreamWriter writer = new StreamWriter(filename))
    {
        foreach (Trainer trainer in trainers)
        {
            writer.WriteLine("{0}#{1}#{2}#{3}", trainer.TrainerID, trainer.TrainerName, trainer.MailingAddress, trainer.TrainerEmailAddress);
        }
        
    }
}
}
}
