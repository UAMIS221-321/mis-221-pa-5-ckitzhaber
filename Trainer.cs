namespace PA5_Provision
{

    class Trainer
    {
        public string name{get; set;}
        public string email {get; set;}
        public string address {get; set;} //mailing address
        public string id {get; set;} //guid

        public Trainer(string name, string email, string address)
        {
            this.name = name.ToLower();
            this.email = email.ToLower();
            this.address = address.ToLower();
            id = Guid.NewGuid().ToString();
        }

        public Trainer(string[] info)
        {
            name = info[0].ToLower();
            email = info[1].ToLower();
            address = info[2].ToLower();
            id = info[3];
        }

        public void Print()
        {
            Console.WriteLine($"Name: {name}, Email: {email}\nAddress: {address}\nId: {id}");
        }

        public override string ToString()
        {
            return $"{name}#{email}#{address}#{id}";
        }
    }

}