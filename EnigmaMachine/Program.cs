using System;

namespace EnigmaMachine
{
  class Program
  {
    private static EnigmaMachine enigmaMachine;
    private static ConsoleGUI consoleGUI;

    static void Main(string[] args)
    {
      enigmaMachine = new EnigmaMachine(
        "MyEnigmaMachine", 
        new string[] { "1", "4", "3" }, 
        new int[] { 16, 26, 8}, 
        EnigmaConfig.PLUGBOARD_DAY_29
      );
      consoleGUI = new ConsoleGUI(enigmaMachine);

      do {
        Console.Clear();
        consoleGUI.ShowStartScreenAndGetInput();
      } while (!consoleGUI.UserWantsToExit);
    }
  }
}
