using libs;

class Program
{    
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        var engine = GameEngine.Instance;

        bool isHardMode = AskYesNoQuestion("You are about to gain knowledge but abandon your sanity, win the game but lose in life. Do you want to play on hard mode?");
        // bool skipTutorial = AskYesNoQuestion("Would you like to skip the tutorial?");

       
        // engine.Setup(isHardMode, skipTutorial);
             engine.Setup(isHardMode);

        var inputHandler = InputHandler.Instance;

     
        while (true)
        {
            engine.Render();
            

       
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            inputHandler.Handle(keyInfo);
        }
    }

   static bool AskYesNoQuestion(string question)
{
    while (true)
    {
        Console.Write(question + " (y/n) ");
        var key = Console.ReadKey(true);

        switch (key.Key)
        {
            case ConsoleKey.Y:
                Console.WriteLine("Yes");
                return true;

            case ConsoleKey.N:
                Console.WriteLine("No");
                return false;

            case ConsoleKey.Escape:
                Console.WriteLine("Exiting question...");
                 Environment.Exit(0);
                return false;

            default:
                Console.WriteLine("Invalid input. Please answer with 'y' for Yes or 'n' for No.");
                break;
        }
    }
}

}



