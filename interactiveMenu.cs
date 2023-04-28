using static System.Console;

namespace PA5_Provision{

    class menuBase
    {
        protected string title;

        protected menuBase()
        {
            title = "";
        }

        protected int IndexWrap(int position, int size)
        { //returns the position user keys to as a number in the original range, fixes negative number modulus division
            int tempIndex = position % size;
            if(tempIndex < 0){ //puts negative number back to positive
                return tempIndex + size;
            }
            return tempIndex;
        }

        public void appearancePicker() //let the user pick how things look
        {//inherited by all versions of the menu
            List<menuOption<ConsoleColor>> colorOptions = new List<menuOption<ConsoleColor>>(); //utilizes our built class
            ConsoleColor[] colors = (ConsoleColor[]) ConsoleColor.GetValues(typeof(ConsoleColor)); //pulled this right from the C# documentation

            Clear();
            WriteLine("Welcome to the color picker. Warning: Some Text-Background color pairs may not display well\n");
            WriteLine("Choose your background color: ");
            foreach(ConsoleColor c in colors)
            {
                colorOptions.Add(new menuOption<ConsoleColor>(c.ToString(), c));
                BackgroundColor = c;
                WriteLine(c);
            }
            BackgroundColor = ConsoleColor.Black; //start out standard
            WriteLine("___________________________\n");
            CursorVisible = false;

            int position = 0;
            Write(colorOptions[position].title);
            while(true)
            {
                ConsoleKeyInfo input = ReadKey(true);
                if(input.Key.ToString() == "UpArrow")
                {
                    position--;
                    WriteBGFromArray(colorOptions, position);
                }

                if(input.Key.ToString() == "DownArrow")
                {
                    position++;
                    WriteBGFromArray(colorOptions, position);
                }

                if(input.Key.ToString() == "Enter")
                {
                    WriteLine();
                    break;
                }
            }
            
            WriteLine("Choose your text color: ");
            foreach(ConsoleColor c in colors)
            {
                ForegroundColor = c;
                WriteLine(c);
            }

            ForegroundColor = ConsoleColor.White; //start out standard
            WriteLine("___________________________\n");
            position = 0;
            Write(colorOptions[position].title);
            while(true)
            {
                ConsoleKeyInfo input = ReadKey(true);
                if(input.Key.ToString() == "UpArrow")
                {
                    position--;
                    WriteFGFromArray(colorOptions, position);
                }

                if(input.Key.ToString() == "DownArrow")
                {
                    position++;
                    WriteFGFromArray(colorOptions, position);
                }

                if(input.Key.ToString() == "Enter")
                {
                    WriteLine();
                    CursorVisible = true;
                    return;
                }
            }
        }

        private void WriteBGFromArray(List<menuOption<ConsoleColor>> options, int position)
        { //these methods are specifically for the display function. Could not standard other methods because of the differing menuOption type
            BackgroundColor = options[IndexWrap(position, options.Count)].passed;
            Write("\r" + new string(' ', WindowWidth-1) + "\r");
            Write(options[IndexWrap(position, options.Count)].title);
        }

        private void WriteFGFromArray(List<menuOption<ConsoleColor>> options, int position)
        {
            ForegroundColor = options[IndexWrap(position, options.Count)].passed;
            Write("\r" + new string(' ', WindowWidth-1) + "\r");
            Write(options[IndexWrap(position, options.Count)].title);
        }
    }

    class interactiveMenu : menuBase
    { //this version allows the user to pass any void, no parameter method
        public List<menuOption> options {get; set;}
        
        public interactiveMenu()
        {
            options = new List<menuOption>{};
            title = "";
        }

        public void AddTitle(string title)
        {
            this.title = title;
        }

        public void AddLine(string line, Action passedMethod)
        { //add menu option with a void, no parameter function
            options.Add(new menuOption(line, passedMethod));
        }

        public void AddList(IEnumerable<string> list)
        { //adds menu options of any IEnumberable (eg. array, list, linkedlist, etc)
            foreach(String s in list) {AddTitle(s);}
        }

        public void AddExit()
        {
            options.Add(new menuOption("Exit Menu", ExitFunction));
        }

        public void ExitFunction()
        {
            WriteLine("Press any key to continue");
            ReadKey();
        }

        public void Start()
        {
            Clear();
            if(title != "") WriteLine(title);
            foreach(menuOption m in options)
            {
                WriteLine(m.title);
            }
            WriteLine("___________________________\n");
            CursorVisible = false;

            int position = 0;
            Write(options[position].title);
            while(true)
            {
                ConsoleKeyInfo input = ReadKey(true);
                if(input.Key.ToString() == "UpArrow")
                {
                    position--;
                    WriteFromArray(options, position);
                }

                if(input.Key.ToString() == "DownArrow")
                {
                    position++;
                    WriteFromArray(options, position);
                }

                if(input.Key.ToString() == "Enter")
                {
                    WriteLine();
                    int realPos = IndexWrap(position, options.Count);
                    options[realPos].method();
                    CursorVisible = true;
                    return;
                }
            }
        }

        private void WriteFromArray(List<menuOption> options, int position)
        { //prints out option from array over the previous, highlights option briefly
            Write("\r" + new string(' ', WindowWidth-1) + "\r");
            BackgroundColor = ConsoleColor.Black;
            Write(options[IndexWrap(position, options.Count)].title);
            Thread.Sleep(225);
            Write("\r");
            BackgroundColor = ConsoleColor.White;
            Write(options[IndexWrap(position, options.Count)].title);
        }
    }

    class interactiveMenu <T> : menuBase
    {//this version allows the user to set a type T each menuOption method to return and/or take
     //also allows users to pass a return value of type T (no method)
        private List<menuOption<T>> options {get; set;}
        
        public interactiveMenu()
        {//T is implicit
            options = new List<menuOption<T>>{};
            title = "";
        }

        public void AddLine(string line, T returnInfo)
        {//returns passed information
            options.Add(new menuOption<T>(line, returnInfo));
        }

        public void AddLine(string line, Func<T> passedMethod)
        {//returns type T from passedMethod
            options.Add(new menuOption<T>(line, passedMethod));
        }

        public void AddLine(string line, Action<T> passedMethod, T parameter)
        {//returns void from method, passed type T
            options.Add(new menuOption<T>(line, passedMethod, parameter));
        }

        public void AddLine(string line, Func<T,T> passedMethod, T parameter)
        {//returns and takes type T
             options.Add(new menuOption<T>(line, passedMethod, parameter));
        }

        public void AddExit()
        { //addExit will execute no methods, but code will continue to run
            options.Add(new menuOption<T>("Exit Menu", ExitFunction, default(T))); //un-used value
        }

        public void ExitFunction(T empty)
        {
            WriteLine("Press any key to continue");
            ReadKey();
        }

        public void AddTitle(string title)
        {
            this.title = title;
        }
        
        public T Start()
        {
            Clear();
            if(title != "") WriteLine(title);
            foreach(menuOption<T> m in options)
            {
                WriteLine(m.title);
            }
            WriteLine("___________________________\n");
            CursorVisible = false;

            int position = 0;
            Write(options[position].title);
            while(true)
            {
                ConsoleKeyInfo input = ReadKey(true);
                if(input.Key.ToString() == "UpArrow")
                {
                    position--;
                    WriteFromArray(options, position);
                }

                if(input.Key.ToString() == "DownArrow")
                {
                    position++;
                    WriteFromArray(options, position);
                }

                if(input.Key.ToString() == "Enter")
                {
                    WriteLine(); //spacing
                    int realPos = IndexWrap(position, options.Count);
                    CursorVisible = true;

                    switch(options[realPos].type)
                    {
                        case (1):
                            return options[realPos].passed;  //returns the passed information
                        case (2):
                            return options[realPos].methodReturn(); //returns the result of the passed (no parameter) function
                        case (3):
                            options[realPos].methodTake(options[realPos].passed); //calls the void method using the passed information as a parameter
                            return options[realPos].passed; //returns the passed information --> this allows the user to potentially get back its changed the value after it is used in the method
                        case (4):
                            return options[realPos].methodTakeReturn(options[realPos].passed); //returns result T of function using passed (type T) parameter
                    }
                }
            }
        }

        private void WriteFromArray(List<menuOption<T>> options, int position)
        {
            Write("\r" + new string(' ', WindowWidth-1) + "\r");
            BackgroundColor = ConsoleColor.Black;
            Write(options[IndexWrap(position, options.Count)].title);
            Thread.Sleep(225);
            Write("\r");
            BackgroundColor = ConsoleColor.White;
            Write(options[IndexWrap(position, options.Count)].title);
        }
    }

    class interactiveMenu <T1, T2> : menuBase //T1 is input, T2 is output (matches the Func class order)
    {//this version allows the user to specify two different types for each menuOption to take and return
        private List<menuOption<T1, T2>> options {get; set;} //order of generics matter

        public interactiveMenu()
        {//T is implicit
            options = new List<menuOption<T1, T2>>{};
            title = "";
        }

        public void AddLine(string line, Func<T1, T2> method, T1 passed)
        {
            options.Add(new menuOption<T1, T2>(line, method, passed));
        }

        public void AddTitle(string title)
        {
            this.title = title;
        }

        public void AddExit()
        { //addExit will execute no methods, but code will continue to run
            options.Add(new menuOption<T1, T2>("Exit Menu", ExitFunction, default(T1))); //un-used value
        }

        public T2 ExitFunction(T1 empty)
        {
            WriteLine("Press any key to continue");
            ReadKey();
            return default(T2);
        }
        
        public T2 Start()
        {
            Clear();
            if(title != "") WriteLine(title);
            foreach(menuOption<T1, T2> m in options)
            {
                WriteLine(m.title);
            }
            WriteLine("___________________________\n");
            CursorVisible = false;

            int position = 0;
            Write(options[position].title);
            while(true)
            {
                ConsoleKeyInfo input = ReadKey(true);
                if(input.Key.ToString() == "UpArrow")
                {
                    position--;
                    WriteFromArray(options, position);
                }

                if(input.Key.ToString() == "DownArrow")
                {
                    position++;
                    WriteFromArray(options, position);
                }

                if(input.Key.ToString() == "Enter")
                {
                    WriteLine();
                    CursorVisible = true;
                    int realPos = IndexWrap(position, options.Count);
                    return options[realPos].method(options[realPos].passed); //returns T2 output from T1 input in passed method
                }
            }
        }

        private void WriteFromArray(List<menuOption<T1, T2>> options, int position)
        {
            Write("\r" + new string(' ', WindowWidth-1) + "\r");
            BackgroundColor = ConsoleColor.Black;
            Write(options[IndexWrap(position, options.Count)].title);
            Thread.Sleep(225);
            Write("\r");
            BackgroundColor = ConsoleColor.White;
            Write(options[IndexWrap(position, options.Count)].title);
        }
    } 
}