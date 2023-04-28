using static System.Console;

namespace PA5_Provision
{
    class Reporting
    {
        public List<Session> reportSessions {get; set;}
        public bool loop;

        public Reporting()
        {
            reportSessions = new List<Session>{};
        }

        public void Menu(SessionList activeSessions)
        {
            ReadFile(); //get what's currently on file
            loop = true;
            while(loop){
                interactiveMenu rm = new interactiveMenu();
                rm.AddTitle("Reporting!\nPlease select your desired option");
                rm.AddLine(" - Individual Customer Sessions", CustomerSessions);
                rm.AddLine(" - Historical Customer Sessions", HistoricalSessions);
                rm.AddLine(" - Historical Revenue Report", RevenueReport);
                rm.AddLine(" - List Sesssions by Time of Day", SessionByTime);
                rm.AddLine(" - List Sessions by Trainer", SessionsByTrainer);
                rm.AddLine(" - Exit", EndLoop);
                rm.Start();

                WriteLine("Press any key to continue\n");
                ReadKey();
            }
        }
        public void EndLoop(){loop = false;}

        public void ReadFile()
        {
            StreamReader infile = new StreamReader("textfiles/transactions.txt");

            while(!infile.EndOfStream)
            {
                string[] sessionSections = (infile.ReadLine() ?? "").Split('#');
                Session fileSession = new Session(sessionSections);
                reportSessions.Add(fileSession);
            }
            infile.Close();
        }

        public void CustomerSessions()
        {
            WriteLine("What is the customer's email?");
            string email = (ReadLine() ?? "");

            foreach(Session s in reportSessions)
            {
                if(s.customerEmail == email)
                {
                    s.Print();
                }
            }
            
            WriteLine("Would you like to write this information to a file? (y/n)");
            string wFile = (ReadLine() ?? "");
            if(wFile == "y")
            {
                WriteLine("What would you like to name the file?");
                string fileName = (ReadLine() ?? "");
                StreamWriter outfile = new StreamWriter($"{fileName}.txt");
                foreach(Session s in reportSessions)
                {
                    if(s.customerEmail == email)
                    {
                        outfile.WriteLine(s.ToString());
                    }
                }
                outfile.Close();
            }
        }

        public void HistoricalSessions()
        {
            for (int i = 0; i < reportSessions.Count-1; i++) //sort by customer name
            {
                for (int j = 1; j < reportSessions.Count; j++)
                {
                    if(String.Compare(reportSessions[j].customerName, reportSessions[i].customerName) < 0)
                    {
                        Session temp = reportSessions[i];
                        reportSessions[i] = reportSessions[j];
                        reportSessions[j] = temp;
                    }
                }
            }

            for (int i = 0; i < reportSessions.Count-1; i++) //sort customer name sections by date
            {
                int switchCount = 0;
                for (int j = 0; j < reportSessions.Count-1; j++)
                {
                    if(reportSessions[j].customerName == reportSessions[j+1].customerName && DateTime.Compare(reportSessions[j+1].date, reportSessions[j].date) < 0)
                    { //bubble sort within customer sections
                        Session temp = reportSessions[j];
                        reportSessions[j] = reportSessions[j+1];
                        reportSessions[j+1] = temp;
                        switchCount++;
                    }
                }
                if(switchCount == 0) break; //cuts of bubble sort if no swaps made
            }

            //Gives total of instances for every customer and prints all
            string cbName = reportSessions[0].customerName;
            int count = 0;
            for (int i = 0; i < reportSessions.Count-1; i++)
            {
                reportSessions[i].Print();
                count++;
                if(reportSessions[i+1].customerName != cbName)
                {
                    WriteLine($"Customer: {cbName}, Total = {count}");
                    cbName = reportSessions[i+1].customerName;
                    count = 0;
                }
                WriteLine();
            }
            if(reportSessions[reportSessions.Count-1].customerName != reportSessions[reportSessions.Count-2].customerName)
            {
                reportSessions[reportSessions.Count-1].Print();
                WriteLine($"Customer: {reportSessions[reportSessions.Count-1].customerName} Total = 1");
            }
            else
            {
                reportSessions[reportSessions.Count-1].Print();
                WriteLine($"Customer: {reportSessions[reportSessions.Count-1].customerName} Total = {count+1}");
            }


            WriteLine("Would you like to write this information to a file? (y/n)");
            string wFile = (ReadLine() ?? "");
            if(wFile == "y")
            {
                WriteLine("What would you like to name the file?");
                string fileName = (ReadLine() ?? "");
                StreamWriter outfile = new StreamWriter($"{fileName}.txt");
                
                string custName = reportSessions[0].customerName;
                int counter = 0;
                for (int i = 0; i < reportSessions.Count-1; i++)
                {
                    outfile.WriteLine(reportSessions[i].ToString());
                    counter++;
                    if(reportSessions[i+1].customerName != custName)
                    {
                        outfile.WriteLine($"Customer: {custName}, Total = {count}");
                        custName = reportSessions[i+1].customerName;
                        counter = 0;
                    }
                }
                if(reportSessions[reportSessions.Count-1].customerName != reportSessions[reportSessions.Count-2].customerName)
                {
                    outfile.WriteLine($"{(reportSessions[reportSessions.Count-1].date).ToString("MM/dd/yyyy")}#{reportSessions[reportSessions.Count-1].time}#{reportSessions[reportSessions.Count-1].customerName}#{reportSessions[reportSessions.Count-1].customerEmail}#{reportSessions[reportSessions.Count-1].trainerName}#{reportSessions[reportSessions.Count-1].trainerId}#{reportSessions[reportSessions.Count-1].price}#{reportSessions[reportSessions.Count-1].status}#{reportSessions[reportSessions.Count-1].id}");
                    outfile.WriteLine($"Customer: {reportSessions[reportSessions.Count-1].customerName} Total = 1");
                }
                else
                {
                    outfile.WriteLine($"{(reportSessions[reportSessions.Count-1].date).ToString("MM/dd/yyyy")}#{reportSessions[reportSessions.Count-1].time}#{reportSessions[reportSessions.Count-1].customerName}#{reportSessions[reportSessions.Count-1].customerEmail}#{reportSessions[reportSessions.Count-1].trainerName}#{reportSessions[reportSessions.Count-1].trainerId}#{reportSessions[reportSessions.Count-1].price}#{reportSessions[reportSessions.Count-1].status}#{reportSessions[reportSessions.Count-1].id}");
                    outfile.WriteLine($"Customer: {reportSessions[reportSessions.Count-1].customerName} Total = {counter+1}");
                }
                outfile.Close();
            }
        }

        public void RevenueReport()
        {
            for (int i = 0; i < reportSessions.Count-1; i++)
            {
                for (int j = 1; j < reportSessions.Count; j++)
                {
                    if(DateTime.Compare(reportSessions[j].date, reportSessions[i].date) < 0)
                    {
                        Session temp = reportSessions[i];
                        reportSessions[i] = reportSessions[j];
                        reportSessions[j] = temp;
                    }
                }
            }

            // now the list is in chronological order
            double yearTotal = 0;
            double monthTotal = 0;
            for (int i = 0; i < reportSessions.Count-1; i++)
            {
                monthTotal += reportSessions[i].price;
                yearTotal += reportSessions[i].price;
                if((reportSessions[i].date).Month != (reportSessions[i+1].date).Month)
                {
                    WriteLine(reportSessions[i].date.Month.ToString() + "/" + reportSessions[i].date.Year.ToString() + " Total: " + monthTotal);
                    monthTotal = 0;
                }

                if((reportSessions[i].date).Year != (reportSessions[i+1].date).Year)
                {
                    WriteLine(reportSessions[i].date.Year.ToString() + " Total: " + yearTotal);
                    yearTotal = 0;
                }
            }
            
            //month fencepost
            if(reportSessions[reportSessions.Count-1].date.Month != reportSessions[reportSessions.Count-2].date.Month)
            {
                WriteLine(reportSessions[reportSessions.Count-1].date.Month.ToString() + "/" + reportSessions[reportSessions.Count-1].date.Year.ToString() + " Total: " + reportSessions[reportSessions.Count-1].price);
            }
            else
            {
                WriteLine(reportSessions[reportSessions.Count-1].date.Month.ToString() + "/" + reportSessions[reportSessions.Count-1].date.Year.ToString() + " Total: " + (monthTotal + reportSessions[reportSessions.Count-1].price));
            }
            //year fencepost
            if(reportSessions[reportSessions.Count-1].date.Year != reportSessions[reportSessions.Count-2].date.Year)
            {
                WriteLine(reportSessions[reportSessions.Count-1].date.Year.ToString() +  " Total: " + reportSessions[reportSessions.Count-1].price);
            }
            else
            {
                WriteLine(reportSessions[reportSessions.Count-1].date.Year.ToString() + " Total: " + (yearTotal + reportSessions[reportSessions.Count-1].price));
            }

            WriteLine("Would you like to write this information to a file? (y/n)");
            string wFile = (ReadLine() ?? "");
            if(wFile == "y")
            {
                WriteLine("What would you like to name the file?");
                string fileName = (ReadLine() ?? "");
                StreamWriter outfile = new StreamWriter($"{fileName}.txt");
                for (int i = 1; i < reportSessions.Count; i++)
                {
                    if((reportSessions[i].date).Month != (reportSessions[i-1].date).Month)
                    {
                        outfile.WriteLine(reportSessions[i].date.Year.ToString() + reportSessions[i].date.Month.ToString() + " Total: " + monthTotal);
                        monthTotal = 0;
                    }

                    if((reportSessions[i].date).Year != (reportSessions[i-1].date).Year)
                    {
                        outfile.WriteLine(reportSessions[i].date.Year.ToString() + " Total: " + yearTotal);
                        yearTotal = 0;
                    }
                    monthTotal += reportSessions[i].price;
                    yearTotal += reportSessions[i].price;
                }
                //month fencepost
                if(reportSessions[reportSessions.Count-1].date.Month != reportSessions[reportSessions.Count-2].date.Month)
                {
                    outfile.WriteLine(reportSessions[reportSessions.Count-1].date.Month.ToString() + "/" + reportSessions[reportSessions.Count-1].date.Year.ToString() + " Total: " + reportSessions[reportSessions.Count-1].price);
                }
                else
                {
                    outfile.WriteLine(reportSessions[reportSessions.Count-1].date.Month.ToString() + "/" + reportSessions[reportSessions.Count-1].date.Year.ToString() + " Total: " + (monthTotal + reportSessions[reportSessions.Count-1].price));
                }
                //year fencepost
                if(reportSessions[reportSessions.Count-1].date.Year != reportSessions[reportSessions.Count-2].date.Year)
                {
                    outfile.WriteLine(reportSessions[reportSessions.Count-1].date.Year.ToString() +  " Total: " + reportSessions[reportSessions.Count-1].price);
                }
                else
                {
                    outfile.WriteLine(reportSessions[reportSessions.Count-1].date.Year.ToString() + " Total: " + (yearTotal + reportSessions[reportSessions.Count-1].price));
                }
                outfile.Close();   
            }
        }

        public void SessionByTime()
        {
            for (int i = 0; i < reportSessions.Count-1; i++)
            {
                for (int j = 1; j < reportSessions.Count; j++)
                {
                    string[] time1 = reportSessions[i].time.Split(':');
                    string[] time2 = reportSessions[j].time.Split(':');

                    if(int.Parse(time1[0]) > int.Parse(time2[0]) && int.Parse(time1[1]) > int.Parse(time2[1]))
                    { //sort by time
                        Session temp = reportSessions[i];
                        reportSessions[i] = reportSessions[j];
                        reportSessions[j] = temp;
                    }
                }
            }

            int count = 0;
            for (int i = 0; i < reportSessions.Count-1; i++)
            {
                count++;
                if(reportSessions[i].time != reportSessions[i+1].time)
                {
                    WriteLine($"{reportSessions[i].time} Session: {count}");
                    count = 0;
                }
            }

            if(reportSessions[reportSessions.Count].time == reportSessions[reportSessions.Count-1].time)
            {
                WriteLine($"{reportSessions[reportSessions.Count].time} Session: {count+1}");
            }
            else
            {
                WriteLine($"{reportSessions[reportSessions.Count-1].time} Session: {count}");
                WriteLine($"{reportSessions[reportSessions.Count].time} Session: {1}");
            }

            WriteLine("Would you like to write this information to a file? (y/n)");
            string wFile = (ReadLine() ?? "");
            if(wFile == "y")
            {
                WriteLine("What would you like to name the file?");
                string fileName = (ReadLine() ?? "");
                StreamWriter outfile = new StreamWriter($"{fileName}.txt");
                count = 0;
                for (int i = 0; i < reportSessions.Count-1; i++)
                {
                    count++;
                    if(reportSessions[i].time != reportSessions[i+1].time)
                    {
                        outfile.WriteLine($"{reportSessions[i].time} Sessions: {count}");
                        count = 0;
                    }
                }
                if(reportSessions[reportSessions.Count].time == reportSessions[reportSessions.Count-1].time)
                {
                    outfile.WriteLine($"{reportSessions[reportSessions.Count].time} Session: {count+1}");
                }
                else
                {
                    outfile.WriteLine($"{reportSessions[reportSessions.Count-1].time} Session: {count}");
                    outfile.WriteLine($"{reportSessions[reportSessions.Count].time} Session: {1}");
                }
                outfile.Close();
            }
        }

        public void SessionsByTrainer()
        {
            for (int i = 0; i < reportSessions.Count-1; i++) //sort by customer name
            {
                for (int j = 1; j < reportSessions.Count; j++)
                {
                    if(String.Compare(reportSessions[j].trainerName, reportSessions[i].trainerName) < 0)
                    {
                        Session temp = reportSessions[i];
                        reportSessions[i] = reportSessions[j];
                        reportSessions[j] = temp;
                    }
                }
            }

            int count = 0;
            for (int i = 0; i < reportSessions.Count-1; i++)
            {
                count++;
                reportSessions[i].Print();
                if(reportSessions[i].trainerName != reportSessions[i+1].trainerName)
                {
                    WriteLine($"{reportSessions[i].trainerName} Total: {count}");
                    count = 0;
                }
            }

            if(reportSessions[reportSessions.Count].trainerName == reportSessions[reportSessions.Count-1].trainerName)
            {
                WriteLine($"{reportSessions[reportSessions.Count].trainerName} Session: {count+1}");
            }
            else
            {
                WriteLine($"{reportSessions[reportSessions.Count-1].trainerName} Session: {count}");
                WriteLine($"{reportSessions[reportSessions.Count].trainerName} Session: {1}");
            }

            WriteLine("Would you like to write this information to a file? (y/n)");
            string wFile = (ReadLine() ?? "");
            if(wFile == "y")
            {
                WriteLine("What would you like to name the file?");
                string fileName = (ReadLine() ?? "");
                StreamWriter outfile = new StreamWriter($"{fileName}.txt");
                
                count = 0;
                for (int i = 0; i < reportSessions.Count-1; i++)
                {
                    count++;
                    reportSessions[i].Print();
                    if(reportSessions[i].trainerName != reportSessions[i+1].trainerName)
                    {
                        outfile.WriteLine($"{reportSessions[i].trainerName} Total: {count}");
                        count = 0;
                    }
                }

                if(reportSessions[reportSessions.Count].trainerName == reportSessions[reportSessions.Count-1].trainerName)
                {
                    outfile.WriteLine($"{reportSessions[reportSessions.Count].trainerName} Session: {count+1}");
                }
                else
                {
                    outfile.WriteLine($"{reportSessions[reportSessions.Count-1].trainerName} Session: {count}");
                    outfile.WriteLine($"{reportSessions[reportSessions.Count].trainerName} Session: {1}");
                }
                outfile.Close();
            }
        }
    }
}