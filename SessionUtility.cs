using static System.Console;

namespace PA5_Provision
{
    class SessionUtility
    {
        public List<Session> sessions {get; set;}

        public SessionUtility(List<Session> sessions)
        {
            this.sessions = sessions;
        }

        public void ReadFile()
        {
            StreamReader infile = new StreamReader("textfiles/listings.txt");

            while(!infile.EndOfStream)
            {
                string[] sessionSections = (infile.ReadLine() ?? "").Split('#');
                Session fileSessions = new Session(sessionSections);
                sessions.Add(fileSessions);
            }
            infile.Close();
        }

        public void WriteFile()
        {
            foreach(Session s in sessions.ToList())
            { //cleanup sessions
                if(s.status == "completed" || s.status == "cancelled") sessions.Remove(s);
            }

            StreamWriter listingOutfile = new StreamWriter("textfiles/listings.txt");
            foreach(Session s in sessions) //write remaining sessions (unbooked & booked) to file
            {
                listingOutfile.WriteLine(s.ToString());
            }
            listingOutfile.Close();
        }
    }
}