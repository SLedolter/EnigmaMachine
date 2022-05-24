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
        new string[] { "I", "IV", "III" }, 
        new int[] { 16, 26, 08}, 
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
