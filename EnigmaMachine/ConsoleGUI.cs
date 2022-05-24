using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine {
  class ConsoleGUI {
    const int COMMAND_AREA_HEIGHT = 4;
    const string MENU_ICON = ">>";

    private EnigmaMachine enigmaMachine;
    private string lastInput;
    private bool userWantsToExit;
    private string currentMenuShortname = "";

    private string originalMessage, encodedMessage;

    public string LastInput { get => lastInput; set => lastInput = value; }
    public bool UserWantsToExit { get => userWantsToExit; set => userWantsToExit = value; }

    public ConsoleGUI(EnigmaMachine enigmaMachine) {
      this.enigmaMachine = enigmaMachine;
    }

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

    public ConsoleKeyInfo ShowPromptAndReadCommandChar(string menuPrefix = "") {
      ConsoleKeyInfo command;
      currentMenuShortname = CreateMenuPrefix(menuPrefix);

      Console.Clear();
      DrawCommandSectionAndPlaceInputCursor();
      ShowMessages();
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

    private ConsoleKeyInfo ReadChar() {
      PlaceInputCursorToPromptSign();
      return Console.ReadKey() ;
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

    public void ShowMessages() {
      Console.CursorTop = 5;
      Console.CursorLeft = 0;
      Console.Error.Write(originalMessage);
      Console.CursorTop = 7;
      Console.CursorLeft = 0;
      Console.Error.WriteLine(encodedMessage);
    }

    private void RunUserCommand() {
      switch (lastInput) {
        case "1":
          EncodeLetters();
          break;
        case "2":
          DecodeLetters();
          break;
        case "exit":
          userWantsToExit = true;
          break;
      }
    }

    public void EncodeLetters() {
      originalMessage = encodedMessage = "";

      ConsoleKeyInfo userInput;

      while ((userInput = ShowPromptAndReadCommandChar("Enc")).Key != ConsoleKey.OemMinus) {
        char result;
        result = char.ToUpper(userInput.KeyChar);
        if(result < 'A' || result > 'Z') {
          continue;
        }

        originalMessage += result;
        encodedMessage += enigmaMachine.Encoder(result);
      }

      enigmaMachine.ResetMachine();
      originalMessage = encodedMessage = "";
    }

    public void DecodeLetters() {
      originalMessage = encodedMessage = "";

      ConsoleKeyInfo userInput;

      while ((userInput = ShowPromptAndReadCommandChar("Dec")).Key != ConsoleKey.OemMinus) {
        char result;
        result = char.ToUpper(userInput.KeyChar);
        if (result < 'A' || result > 'Z') {
          continue;
        }

        originalMessage += result;
        encodedMessage += enigmaMachine.Decoder(result);
      }

      enigmaMachine.ResetMachine();
      originalMessage = encodedMessage = "";
    }
  }
}
