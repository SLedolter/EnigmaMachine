using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine {
  class ConsoleGUI {
    const int COMMAND_AREA_HEIGHT = 4;
    const string MENU_ICON = ">>";

    private string lastInput;
    private bool userWantsToExit;
    private string currentMenuShortname = "";

    public string LastInput { get => lastInput; set => lastInput = value; }
    public bool UserWantsToExit { get => userWantsToExit; set => userWantsToExit = value; }

    public ConsoleGUI() { }

    public void ShowStartScreenAndGetInput() {
      lastInput = "";
      lastInput = ShowPromptAndReadCommandLine();
    }

    public string ShowPromptAndReadCommandLine(string menuPrefix = "") {
      currentMenuShortname = CreateMenuPrefix(menuPrefix);

      Console.Clear();
      DrawCommandSectionAndPlaceInputCursor();
      lastInput = ReadLine();
      RunUserCommand();

      return lastInput;
    }

    public ConsoleKey ShowPromptAndReadCommandChar(string menuPrefix = "") {
      ConsoleKey command;
      currentMenuShortname = CreateMenuPrefix(menuPrefix);

      Console.Clear();
      DrawCommandSectionAndPlaceInputCursor();
      command = ReadChar();

      return command;
    }

    public string CreateMenuPrefix(string menuPrefixIn) {
      string result;

      if(menuPrefixIn.Length > 0) {
        result = menuPrefixIn + " ";
      } else {
        result = "";
      }

      return result;
    }

    private void PlaceInputCursorToPromptSign() {
      Console.CursorLeft = currentMenuShortname.Length + MENU_ICON.Length + 1;
      Console.CursorTop = Console.WindowHeight - COMMAND_AREA_HEIGHT + 2;
    }

    private string ReadLine() {
      PlaceInputCursorToPromptSign();
      return Console.ReadLine();
    }

    private ConsoleKey ReadChar() {
      PlaceInputCursorToPromptSign();
      return Console.ReadKey().Key ;
    }

    private void DrawCommandSectionAndPlaceInputCursor() {
      Console.CursorTop = Console.WindowHeight - COMMAND_AREA_HEIGHT;
      for (int i = 0; i < Console.WindowWidth; i++) {
        Console.CursorLeft = i;
        Console.Write("-");
      }
      Console.CursorLeft = 0;
      Console.CursorTop = Console.WindowHeight - COMMAND_AREA_HEIGHT + 2;
      Console.Write($"{currentMenuShortname}>> ");
    }

    private void RunUserCommand() {
      switch (lastInput) {
        case "1":
          EncodeLetters();
          break;
        case "exit":
          userWantsToExit = true;
          break;
      }
    }

    public void EncodeLetters() {
      ConsoleKey input;
      
      do {
        input = ShowPromptAndReadCommandChar("Enc");
      } while (input != ConsoleKey.OemMinus);
    }
  }
}
