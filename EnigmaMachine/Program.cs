using System;

namespace EnigmaMachine
{
  class Program
  {
    public static EnigmaMachine enigmaMachine;

    const int COMMAND_AREA_HEIGHT = 4;

    static void Main(string[] args)
    {
      string userInput;
      
      enigmaMachine = new EnigmaMachine("MyEnigmaMachine");
      
      do {
        Console.Clear();
        ShowUserInterface(enigmaMachine);
        userInput = ShowAndReadCommandPrompt();
        RunUserCommand(userInput);
      } while (userInput != "exit");
    }

    static void ShowUserInterface(EnigmaMachine enigmaMachine) {
      Console.CursorLeft = 0;
      Console.CursorTop = 0;
      Console.Error.WriteLine($"Name: {enigmaMachine.Name}");
    }

    static string ShowAndReadCommandPrompt() {
      string command;

      Console.CursorTop = Console.WindowHeight - COMMAND_AREA_HEIGHT;
      for (int i = 0; i < Console.WindowWidth; i++) {
        Console.CursorLeft = i;
        Console.Write("-");
      }

      Console.CursorLeft = 0;
      Console.CursorTop = Console.WindowHeight - COMMAND_AREA_HEIGHT + 2;
      Console.Write(">> ");
      command = Console.ReadLine();
      return command;
    }

    static void RunUserCommand(string userInput) {
      switch (userInput) {
        case "1":
          EncodeLetters();
          break;
      }
    }

    static void EncodeLetters() {
      ConsoleKey input;

      do {
        input = Console.ReadKey().Key;
      } while (input != ConsoleKey.OemMinus);
    }
  }
}
