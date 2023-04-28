using static System.Console;

namespace PA5_Provision
{
    class TrainerList
    {
        public List<Trainer> trainers {get; set;}
        public TrainerUtility tu {get; set;}
        private bool loop; //created to simplify the menu

        public TrainerList()
        {
            trainers = new List<Trainer>{};
            tu = new TrainerUtility(trainers);
            tu.ReadFile();
        }

        public void Menu()
        {
            loop = true;
            while(loop){
                interactiveMenu trainerMenu = new interactiveMenu();
                trainerMenu.AddTitle("Manage Trainer Data\nChoose your desired option");
                trainerMenu.AddLine("Add Trainer", AddTrainer);
                trainerMenu.AddLine("Edit Trainer", EditTrainer);
                trainerMenu.AddLine("Delete Trainer", DeleteTrainer);
                trainerMenu.AddLine("Exit", EndLoop);
                trainerMenu.Start();
                
                WriteLine("Press any key to continue\n");
                ReadKey();
            }
        }

        public void EndLoop(){loop = false;} //keeps the menu easy

        public void AddTrainer()
        {
            WriteLine("What is the Trainer's (full) name?");
            string name = (ReadLine() ?? "").ToLower();
            WriteLine("What is the Trainer's email?");
            string email = (ReadLine() ?? "").ToLower();
            WriteLine("What is the Trainer's address?");
            string address = (ReadLine() ?? "").ToLower();
            Trainer newTrainer = new Trainer(name, email, address);
            trainers.Add(newTrainer);
            WriteLine("\nTrainer Created\n");
        }

        public void EditTrainer()
        {
            WriteLine("What is the Trainer's email?");
            string email = (ReadLine() ?? "").ToLower();
            foreach(Trainer t in trainers)
            {
                if(t.email == email)
                {
                    t.Print();
                    WriteLine("Is this the correct Trainer? (y/n)");
                    if((ReadLine() ?? "").ToLower() == "y")
                    {
                        while(true)
                        {
                            interactiveMenu<int> trainerEdit = new interactiveMenu<int>();
                            trainerEdit.AddTitle("Please select the attribute you would like to edit: ");
                            trainerEdit.AddLine($"Name: {t.name}", 1);
                            trainerEdit.AddLine($"Email: {t.email}", 2);
                            trainerEdit.AddLine($"Address: {t.address}", 3);
                            trainerEdit.AddLine($"Id: {t.id}", 4);
                            int editChoice = trainerEdit.Start();

                            switch(editChoice)
                            {
                                case(1):
                                    WriteLine("What would you like to change the (full) name to?");
                                    t.name = (ReadLine() ?? "").ToLower();
                                    break;
                                case(2):
                                    WriteLine("What would you like to change the email to?");
                                    t.email = (ReadLine() ?? "").ToLower();
                                    break;
                                case(3):
                                    WriteLine("What would you like to change the address to?");
                                    t.address = (ReadLine() ?? "").ToLower();
                                    break;
                                case(4):
                                    WriteLine("What would you like to change the id to?");
                                    t.id = (ReadLine() ?? "").ToLower();
                                    break;
                            }
                            WriteLine("Updated!\nWould you like to edit another attribute? (y/n)");
                            string response = (ReadLine() ?? "").ToLower();
                            if(response != "y") return;
                        }
                    }
                    else WriteLine("Searching...");
                }
            }
            WriteLine("Trainer not found");
        }

        public void DeleteTrainer()
        {
            WriteLine("What is the Trainer's email?");
            string email = (ReadLine() ?? "").ToLower();
            foreach(Trainer t in trainers)
            {
                if(t.email == email)
                {
                    t.Print();
                    WriteLine("Is this the correct Trainer? (y/n)");
                    if((ReadLine() ?? "").ToLower() == "y")
                    {
                        trainers.Remove(t);
                        WriteLine("Trainer Deleted\n");
                        return;
                    }
                    else WriteLine("Searching");
                }
            }
            WriteLine("Trainer not found");
        }

        public void CleanUp()
        {
            tu.WriteFile();
        }
    }
}