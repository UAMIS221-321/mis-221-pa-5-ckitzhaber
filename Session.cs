using static System.Console;

namespace PA5_Provision
{

    class Session
    {
        public DateTime date {get; set;}
        public string time {get; set;}
        public string customerName {get; set;}
        public string customerEmail {get; set;}
        public string trainerName {get; set;}
        public string trainerId {get; set;}
        public double price {get; set;}
        public string status{get; set;} //cancelled, unbooked, booked, completed
        public string id {get; set;}

        public Session(DateTime date, string time, string trainerName, string trainerId, double price)
        {
            this.date = date;
            this.time = time.ToLower();
            this.customerName = " ";
            this.customerEmail = " "; //these are set when booked
            this.trainerName = trainerName.ToLower();
            this.trainerId = trainerId;
            this.price = price;
            status = "unbooked";
            id = Guid.NewGuid().ToString();
        }

        public Session(string[] info)
        {
            string[] dateParts = info[0].Split('/');
            date = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[0]), int.Parse(dateParts[1]));
            time  = info[1].ToLower();
            customerName = info[2].ToLower();
            customerEmail = info[3].ToLower();
            trainerName = info[4].ToLower();
            trainerId = info[5];
            price = double.Parse(info[6]);
            status = info[7];
            id = info[8];
        }

        public void Print()
        {
            WriteLine($"Date: {date}, Time: {time}\nCustomer Name: {customerName}, Customer Email: {customerEmail}\nTrainer Name: {trainerName}\n Price: {price}, Status: {status}\n Id: {id}");
        }

        public override string ToString()
        {
            return $"{(this.date).ToString("MM/dd/yyyy")}#{time}#{customerName}#{customerEmail}#{trainerName}#{trainerId}#{price}#{status}#{id}";
        }
    }

}