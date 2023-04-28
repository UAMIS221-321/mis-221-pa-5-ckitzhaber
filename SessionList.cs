using static System.Console;

namespace PA5_Provision
{
    class SessionList
    {
        public List<Session> sessions {get; set;}
        public SessionUtility su {get; set;}
        private bool loop;

        public SessionList()
        {
            sessions = new List<Session>{};
            su = new SessionUtility(sessions);
            su.ReadFile();
        }

        public void ListingMenu(TrainerList trainers)
        {
            loop = true;
            while(loop){
                interactiveMenu<TrainerList> lm = new interactiveMenu<TrainerList>();
                lm.AddTitle("Manage Session Data. Select you desired option");
                lm.AddLine(" - Add Session", AddSession, trainers);
                lm.AddLine(" - Edit Session", EditSession, trainers);
                lm.AddLine(" - Delete Session", DeleteSession, trainers);
                lm.AddLine(" - Exit", EndLoop, trainers);
                lm.Start();

                WriteLine("Press any key to continue\n");
                ReadKey();
            }
        }

        public void EndLoop(TrainerList trainers){loop = false;} //keeps the menu easy
        public void EndLoop() {loop = false;}

        public void BookingMenu()
        {
            loop = true;
            while(loop){
                interactiveMenu lm = new interactiveMenu();
                lm.AddTitle("Manage Customer Booking Data. Select you desired option");
                lm.AddLine(" - View Available Training Sessions", PrintAvailable);
                lm.AddLine(" - Book a Session", BookSession);
                lm.AddLine(" - Exit", EndLoop);
                lm.Start();

                WriteLine("Press any key to continue\n");
                ReadKey();
            }
        }

        public void AddSession(TrainerList trainers)
        {
            WriteLine("What is the Session's date? (Use MM/DD/YYYY format)");
            string dateInput = (ReadLine() ?? "");
            string[] dateParts = dateInput.Split('/');

            DateTime date = new DateTime(Int32.Parse(dateParts[2]), Int32.Parse(dateParts[0]), Int32.Parse(dateParts[1]));
            WriteLine("What time is the Session? (Use 24-hour time [HH:MM])");
            string time = (ReadLine() ?? "");
            WriteLine("What is the Session's price? (Type the number)");
            double price = double.Parse((ReadLine() ?? ""));

            string trainerName, trainerId;
            while(true){//validate name and pull id
                WriteLine("What is the name of the Session's trainer?");
                trainerName = (ReadLine() ?? "").ToLower();
                trainerId = FindTrainerId(trainerName, trainers);
                if(trainerId != "") break;
            }

            Session newSession = new Session(date, time, trainerName, trainerId, price);
            sessions.Add(newSession);
            WriteLine("\nSession Created\n");
        }

        public void AddTransaction(Session s)
        {
            StreamWriter transactionOutfile = new StreamWriter("textfiles/transactions.txt", true);
            transactionOutfile.WriteLine(s.ToString());
            transactionOutfile.Close();
        }

        public void EditSession(TrainerList trainers)
        {
            WriteLine("What is the Session Trainer's name?");
            string trainerName = (ReadLine() ?? "").ToLower();

            WriteLine("What is the Session's date? (Use MM/DD/YYYY format)");
            string dateInput = (ReadLine() ?? "");
            string[] dateParts = dateInput.Split('/');
            DateTime date = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[0]), int.Parse(dateParts[1]));


            foreach(Session s in sessions)
            {
                if(s.trainerName == trainerName && s.date == date)
                {
                    s.Print();
                    WriteLine("Is this the correct Session? (y/n)");
                    if((ReadLine() ?? "").ToLower() == "y")
                    {
                        while(true)
                        {
                            interactiveMenu<int> trainerEdit = new interactiveMenu<int>();
                            trainerEdit.AddTitle("Please select the attribute you would like to edit: ");
                            trainerEdit.AddLine($"Date: {s.date}", 1);
                            trainerEdit.AddLine($"Time: {s.time}", 2);
                            trainerEdit.AddLine($"Trainer: {s.trainerName}", 3);
                            trainerEdit.AddLine($"Price: {s.price}", 4);
                            trainerEdit.AddLine($"Status: {s.status}", 5);
                            trainerEdit.AddLine($"ID: {s.id}", 6);

                            int editChoice = trainerEdit.Start();

                            switch(editChoice)
                            {
                                case(1):
                                    WriteLine("What would you like to change the Session's date to? (Use MM/DD/YYYY format)");
                                    string[] dateP = (ReadLine() ?? "").Split('/');
                                    s.date = new DateTime(int.Parse(dateP[2]), int.Parse(dateP[0]), int.Parse(dateP[1]));
                                    break;
                                case(2):
                                    WriteLine("When would you like to change the Session's time to? (Use 24-hour time [HH:MM])");
                                    s.time = (ReadLine() ?? "");
                                    break;
                                case(3):
                                    string name, trainerId;
                                    while(true)
                                    {//validate name and pull id
                                        WriteLine("What is the name of the new Trainer?");
                                        name = (ReadLine() ?? "").ToLower();
                                        trainerId = FindTrainerId(trainerName, trainers);
                                        if(trainerId != "") break;
                                    }
                                    s.trainerName = name;
                                    s.trainerId = trainerId;
                                    break;
                                case(4):
                                    WriteLine("What would you like to change the Session's Price to?");
                                    s.price = double.Parse((ReadLine() ?? ""));
                                    break;
                                case(5):
                                    while(true){
                                        WriteLine("Enter the number corresponding to your desired option");
                                        WriteLine("1 - Cancel Session");
                                        WriteLine("2 - Book Session");
                                        WriteLine("3 - Session Completed");
                                        string sessionChoice = (ReadLine() ?? "");

                                        if(sessionChoice == "1"){s.status = "cancelled";}
                                        else if(sessionChoice == "2"){s.status = "booked";}
                                        else if(sessionChoice == "3")
                                        {
                                            s.status = "completed";
                                            AddTransaction(s);
                                        }
                                        else{
                                            WriteLine("Invalid Input");
                                            continue;
                                        }
                                        break;
                                    }
                                    break;
                                case(6):
                                    WriteLine("What would you like to change the Session's id to?");
                                    s.id = (ReadLine() ?? "");
                                    break;
                            }
                            WriteLine("Updated!\nWould you like to edit another attribute? (y/n)");
                            string response = (ReadLine() ?? "");
                            if(response != "y") return;
                        }
                    }
                    else WriteLine("Searching...");
                }
            }
            WriteLine("Session not found");
        }

        public void DeleteSession(TrainerList trainers) //unused reference, but helps the menu
        {
            WriteLine("What is the Session Trainer's (full) name");
            string Trainername = (ReadLine() ?? "").ToLower();

            WriteLine("What is the Session's date? (Use MM/DD/YYYY format)");
            string dateInput = (ReadLine() ?? "");
            string[] dateParts = dateInput.Split('/');
             DateTime date = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[0]), int.Parse(dateParts[1]));

            foreach(Session s in sessions)
            {
                if(s.trainerName == Trainername && s.date == date)
                {
                    s.Print();
                    WriteLine("Is this the correct Session? (y/n)");
                    if((ReadLine() ?? "").ToLower() == "y")
                    {
                        sessions.Remove(s);
                        WriteLine("Session Deleted!");
                        return;
                    }
                    else WriteLine("Searching...");
                }
            }
            WriteLine("Session not found");
        }

        public void BookSession()
        {
            WriteLine("What is the Session Trainer's name?");
            string trainerName = (ReadLine() ?? "").ToLower();

            WriteLine("What is the Session's date? (Use MM/DD/YYYY format)");
            string dateInput = (ReadLine() ?? "");
            string[] dateParts = dateInput.Split('/');
            DateTime date = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[0]), int.Parse(dateParts[1]));
            
            foreach(Session s in sessions)
            {
                if(s.trainerName == trainerName && s.date == date)
                {
                    s.Print();
                    WriteLine("Is this the correct Session? (y/n)");
                    if((ReadLine() ?? "").ToLower() == "y")
                    {
                        WriteLine("What is the Customer's Name?");
                        s.customerName = (ReadLine() ?? "").ToLower();
                        WriteLine("What is the Customer's Email?");
                        s.customerEmail = (ReadLine() ?? "").ToLower();
                        s.status = "booked";
                        return;
                    }
                    else WriteLine("Searching...");
                }
            }
            WriteLine("Session not found");
        }

        public void PrintAvailable()
        {
            foreach(Session s in sessions)
            {
                if(s.status == "unbooked")
                {
                    WriteLine($"Date: {s.date}, Time: {s.time}\nTrainer: {s.trainerName}, Price : ${s.price}");
                }
            }
        }

        public string FindTrainerId(string trainerName, TrainerList trainerList){
            List<Trainer> trainers = trainerList.trainers;
            foreach(Trainer t in trainers)
            {
                if(t.name == trainerName)
                {
                    t.Print();
                    WriteLine("Is this the correct Trainer? (y/n)");
                    if((ReadLine() ?? "").ToLower() == "y")
                    {
                        return t.id;
                    }
                    else{
                        WriteLine("Searching...");
                    }
                }
            }
            WriteLine("Trainer not found, please try again.\nIf you are trying to add a new trainer, create their profile before assigning to a session");
            return "";
        }

        public void CleanUp()
        {
            su.WriteFile();
        }
    }
}