using System;

namespace EnigmaMachine
{
  class Program
  {
    private static EnigmaMachine enigmaMachine;
    private static ConsoleGUI consoleGUI;

    static void Main(string[] args)
    {
      enigmaMachine = new EnigmaMachine("MyEnigmaMachine");
      consoleGUI = new ConsoleGUI(enigmaMachine);

      do {
        Console.Clear();
        consoleGUI.ShowStartScreenAndGetInput();
      } while (!consoleGUI.UserWantsToExit);
    }

    static void ShowUserInterface(EnigmaMachine enigmaMachine) {
      Console.CursorLeft = 0;
      Console.CursorTop = 0;
      Console.Error.WriteLine($"Name: {enigmaMachine.Name}");
    }
  }
}
