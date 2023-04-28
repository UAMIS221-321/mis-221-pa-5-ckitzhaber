using static System.Console;

namespace PA5_Provision
{
    class TrainerUtility
    {
        public List<Trainer> trainers {get; set;}

        public TrainerUtility(List<Trainer> trainers)
        {
            this.trainers = trainers;
        }

        public void ReadFile()
        {
            StreamReader infile = new StreamReader("textfiles/trainers.txt");

            while(!infile.EndOfStream)
            {
                string inLine = infile.ReadLine() ?? "";
                string[] trainerSections = inLine.Split('#');
                Trainer fileTrainers = new Trainer(trainerSections);
                trainers.Add(fileTrainers);
            }
            infile.Close();
        }

        public void WriteFile()
        {
            StreamWriter outfile = new StreamWriter("textfiles/trainers.txt");
            foreach(Trainer t in trainers) {outfile.WriteLine(t.ToString());}
            outfile.Close();
        }
    }
}