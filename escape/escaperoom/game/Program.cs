using System;
using libs;

class Program
{
    static void Main(string[] args)
    {
        Console.CursorVisible = false;
        var engine = GameEngine.Instance;

        string dialogFilePath = "Data/dialogs.json";
        bool isHardMode = ShowDialogue(dialogFilePath, 1);

        engine.Setup(isHardMode);

        var inputHandler = InputHandler.Instance;

        while (true)
        {
            engine.Render();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            inputHandler.Handle(keyInfo);
        }
    }

    static bool ShowDialogue(string filePath, int startId)
    {
        var dialogManager = new DialogueManager(filePath);
        int currentId = startId;

        while (currentId != -1)
        {
            var dialog = dialogManager.GetDialogueById(currentId);
            if (dialog == null)
            {
                Console.WriteLine("Dialogue not found!");
                break;
            }

            Console.Clear();
            Console.WriteLine(dialog.Text);
            Console.WriteLine();

            for (int i = 0; i < dialog.Responses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dialog.Responses[i].Text}");
            }

            int choice = GetValidInput(dialog.Responses.Count);
            currentId = dialog.Responses[choice - 1].NextId;

      
            if (currentId == -1 && dialog.Text.Contains("Do you want to play on hard mode?"))
            {
                return dialog.Responses[choice - 1].Text.ToLower().Contains("yes");
            }
        }

        Console.Clear();
        return false;
    }

    static int GetValidInput(int numberOfChoices)
    {
        while (true)
        {
            var key = Console.ReadKey(true);

            if (int.TryParse(key.KeyChar.ToString(), out int choice) && choice >= 1 && choice <= numberOfChoices)
            {
                return choice;
            }

            Console.WriteLine("Invalid choice. Please select a valid option.");
        }
    }
}
