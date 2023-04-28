namespace PA5_Provision{

    class optionBase
    {
        public string title {get; set;}
        
        public optionBase()
        {
            title = "";
        }
    }

    class menuOption : optionBase
    {
        public Action method {get; set;}

        public menuOption(string title, Action method)
        {
            this.title = title;
            this.method = method;
        } 
    }

    class menuOption <T> : optionBase
    {
        public T passed {get; set;}
        public Action<T> methodTake {get; set;} //takes type T
        public Func<T> methodReturn {get; set;} //returns type T
        public Func<T,T> methodTakeReturn {get; set;} //takes and returns type T
        public int type {get; set;} //indicates what the menuOption needs to do
        //? after declaration allows for these types to be nullable, no need to initalize empty

        public menuOption(string title, T passed)
        {
            this.title = title;
            this.passed = passed;
            type = 1;
        }

        public menuOption(string title, Func<T> method)
        {
            this.title = title;
            // this.passed = default(T); //should never be called in theory
            // methodTake = BlankMethod;
            this.methodReturn = method;
            // methodTakeReturn = BlankMethodTakeReturn;
            type = 2;
        }

        public menuOption(string title, Action<T> method, T passed)
        {
            this.title = title;
            this.passed = passed;
            this.methodTake = method;
            // methodReturn = BlankMethod;
            // methodTakeReturn = BlankMethodTakeReturn;
            type = 3;
        }

        public menuOption(string title, Func<T,T> method, T passed)
        {
            this.title = title;
            this.passed = passed;
            // methodTake = BlankMethod;
            // methodReturn = BlankMethod;
            methodTakeReturn = method;
            type = 4;
        }

        // private void BlankMethod(T? input){}
        // private T? BlankMethod(){return default(T);}
        // private T? BlankMethodTakeReturn(T input){return default(T);}
        //Blank methods SHOULD never be called upon in interactiveMenu, but helps ease my mind
    }

    class menuOption <T1, T2> : optionBase
    {
        public T1 passed {get; set;}
        public Func<T1, T2> method {get; set;}

        public menuOption(string title, Func<T1,T2> method, T1 passed)
        {
            this.title = title;
            this.passed = passed;
            this.method = method;
        }
    }
}