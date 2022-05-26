using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine {
  class ConsoleGUI {
    const int COMMAND_AREA_HEIGHT = 4;
    const int PADDING = 1;
    const string MENU_ICON = ">>";

    private EnigmaMachine enigmaMachine;
    private string lastMenuCommand;
    private bool userWantsToExit;
    private string currentMenuShortname = "";

    private string originalMessage, encodedMessage;

    public string LastInput { get => lastMenuCommand; set => lastMenuCommand = value; }
    public bool UserWantsToExit { get => userWantsToExit; set => userWantsToExit = value; }

    public ConsoleGUI(EnigmaMachine enigmaMachine) {
      this.enigmaMachine = enigmaMachine;
    }

    public void ShowStartScreenAndGetInput() {
      lastMenuCommand = "";
      lastMenuCommand = ShowPromptAndReadCommandLine();
    }

    private void PlaceCursorWithinPadding(int x, int y) {
      Console.CursorLeft = PADDING + x;
      Console.CursorTop = PADDING + y;
    }

    public string ShowPromptAndReadCommandLine(string menuPrefix = "") {
      currentMenuShortname = CreateMenuPrefix(menuPrefix);

      Console.Clear();
      ShowInformations(0, 0);
      ShowMessages(Console.WindowWidth / 2, 0);
      DrawCommandSectionAndPlaceInputCursor();
      lastMenuCommand = ReadLine();
      RunUserCommand();

      return lastMenuCommand;
    }

    public ConsoleKeyInfo ShowPromptAndReadCommandChar(string menuPrefix = "") {
      ConsoleKeyInfo command;
      currentMenuShortname = CreateMenuPrefix(menuPrefix);

      Console.Clear();
      ShowInformations(0, 0);
      ShowMessages(Console.WindowWidth/2, 0);
      DrawCommandSectionAndPlaceInputCursor();
      command = ReadChar();

      return command;
    }

    public string CreateMenuPrefix(string menuPrefixIn) {
      string result;

      if (menuPrefixIn.Length > 0) {
        result = menuPrefixIn + " ";
      } else {
        result = "";
      }

      return result;
    }

    private void PlaceInputCursorToPromptSign() {
      PlaceCursorWithinPadding(
        currentMenuShortname.Length + MENU_ICON.Length + 1,
        Console.WindowHeight - COMMAND_AREA_HEIGHT);
    }

    private string ReadLine() {
      PlaceInputCursorToPromptSign();
      return Console.ReadLine();
    }

    private ConsoleKeyInfo ReadChar() {
      PlaceInputCursorToPromptSign();
      return Console.ReadKey();
    }

    private void DrawCommandSectionAndPlaceInputCursor() {
      Console.CursorTop = Console.WindowHeight - COMMAND_AREA_HEIGHT;
      for (int i = 0; i < Console.WindowWidth; i++) {
        Console.CursorLeft = i;
        Console.Write("-");
      }
      PlaceCursorWithinPadding(0, Console.WindowHeight - COMMAND_AREA_HEIGHT);
      Console.Write($"{currentMenuShortname}>> ");
    }

    public void ShowInformations(int x, int y) {
      for(int i = 0; i < enigmaMachine.cylinders.Count; i++) {
        if (!enigmaMachine.cylinders[i].IsActive) {
          continue;
        }
        DrawCylinder(enigmaMachine.cylinders[i], 0, y + (i * 5));
      }
    }

    public void DrawCylinder(Cylinder cylinder, int x, int y) {
      int yIndex = 0;
      PlaceCursorWithinPadding(x + 0, y + yIndex++);
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write($"{cylinder.Name} ({cylinder.CurrentStrikeCount})");
      if(!cylinder.HasFixRingposition) {
        PlaceCursorWithinPadding(x + cylinder.RingPositionIndex, y + yIndex++);
        Console.Write("V");
      }
      PlaceCursorWithinPadding(x + 0, y + yIndex++);
      Console.Write(cylinder.InputScheme);
      if(cylinder.FirstIndex > -1) {
        Console.ForegroundColor = ConsoleColor.Green;
        PlaceCursorWithinPadding(x + cylinder.FirstIndex, y + yIndex-1);
        Console.Write(cylinder.InputScheme[cylinder.FirstIndex]);
      }
      if (cylinder.SecondIndex > -1) {
        Console.ForegroundColor = ConsoleColor.Red;
        PlaceCursorWithinPadding(x + cylinder.SecondIndex, y + yIndex - 1);
        Console.Write(cylinder.InputScheme[cylinder.SecondIndex]);
      }

      Console.ForegroundColor = ConsoleColor.White;
      PlaceCursorWithinPadding(x + 0, y + yIndex++);
      Console.Write(cylinder.OutputScheme);
      if (cylinder.FirstIndex > -1) {
        Console.ForegroundColor = ConsoleColor.Green  ;
        PlaceCursorWithinPadding(x + cylinder.FirstIndex, y + yIndex - 1);
        Console.Write(cylinder.OutputScheme[cylinder.FirstIndex]);
      }
      if (cylinder.SecondIndex > -1) {
        Console.ForegroundColor = ConsoleColor.Red;
        PlaceCursorWithinPadding(x + cylinder.SecondIndex, y + yIndex - 1);
        Console.Write(cylinder.OutputScheme[cylinder.SecondIndex]);
      }
      Console.ForegroundColor = ConsoleColor.White;
    }

    public void ShowMessages(int x, int y) {
      PlaceCursorWithinPadding(x + 0, y + 0);
      Console.Write(originalMessage);
      PlaceCursorWithinPadding(x + 0, y + 1);
      Console.WriteLine(encodedMessage);
    }

    private void RunUserCommand() {
      switch (lastMenuCommand) {
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
        if (result < 'A' || result > 'Z') {
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
