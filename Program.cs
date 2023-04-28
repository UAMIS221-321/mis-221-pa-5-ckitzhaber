using static System.Console;
using System.Media;

namespace PA5_Provision
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string trainLike = @"
 _____ ____      _    ___ _   _      _     ___ _  _______ 
|_   _|  _ \    / \  |_ _| \ | |    | |   |_ _| |/ / ____|
  | | | |_) |  / _ \  | ||  \| |    | |    | || ' /|  _| 
  | | |  _ <  / ___ \ | || |\  |    | |___ | || . \| |___
  |_| |_| \_\/_/   \_\___|_| \_|    |_____|___|_|\_\_____|
  ";
            string aChampion = @"
    _        ____ _   _    _    __  __ ____ ___ ___  _   _ 
   / \      / ___| | | |  / \  |  \/  |  _ \_ _/ _ \| \ | |
  / _ \    | |   | |_| | / _ \ | |\/| | |_) | | | | |  \| |
 / /_\ \   \ |___|  _  |/ ___ \| |  | |  __/| | |_| | |\  |
/_/   \_\   \____|_| |_/_/   \_\_|  |_|_|  |___\___/|_| \_|";

            BackgroundColor = ConsoleColor.White;
            ForegroundColor = ConsoleColor.Magenta;
            bool loopFlag = true;
            TrainerList tl = new TrainerList();
            SessionList sl = new SessionList();
            Reporting reports = new Reporting();

            LogIn();

            while(loopFlag)
            {
                interactiveMenu<int> mainMenu = new interactiveMenu<int>();
                mainMenu.AddTitle($"{trainLike}{aChampion}\n\nChoose an option below to get started");
                mainMenu.AddLine(" - Manage Trainer Data", 1);
                mainMenu.AddLine(" - Manage Listing Data", 2);
                mainMenu.AddLine(" - Manage Customer Booking Data", 3);
                mainMenu.AddLine(" - Run Reports", 4);
                mainMenu.AddLine(" - Exit", 5);
                int userChoice = mainMenu.Start();

                switch(userChoice)
                {
                    case(1): tl.Menu(); break;
                    case(2): sl.ListingMenu(tl); break;
                    case(3): sl.BookingMenu(); break;
                    case(4): reports.Menu(sl); break;
                    case(5): loopFlag = false; break;
                }
                WriteLine("Press any key");
                ReadKey();
            }
            tl.CleanUp();
            sl.CleanUp();
        }

        static void LogIn()
        {
            Clear();
            WriteLine("Operator Log-In");
            while(true)
            {
                WriteLine("Please provide your email and password to log into the system");

                WriteLine("\nEmail:");
                string email = ReadLine() ?? "";
                WriteLine("\nPassword:");
                string password = ReadLine() ?? "";//use continue in try-catch
                

                StreamReader logIn = new StreamReader("textfiles/login.txt");
                
                while(!logIn.EndOfStream)
                {
                    string inLine = logIn.ReadLine() ?? "";
                    string[] userPass = inLine.Split('#');
                    if(userPass[0] == email && userPass[1] == password) 
                    {
                        WriteLine("\nLogged-In!\nPress any key to continue");
                        ReadKey();
                        return;
                    }
                }
                WriteLine("Credentials Invalid, please try again");
            }
        }
    }
}

/*
Extras:
Interactive Menu class creation and implementation!
Console Background and text color change
Large Text Title
Log-In using email and password
Reporting based on time of session
Reporting based on trainer
Test Logs
Removed warnings using "null coalescing operator", ensuring non-null variables
*/