﻿using System;

namespace EnigmaMachine
{
  class Program
  {
    private static EnigmaMachine enigmaMachine;
    private static ConsoleGUI consoleGUI;

    static void Main(string[] args)
    {
      enigmaMachine = EnigmaMachineFactory.Rotor3OnlyForTesting();

      consoleGUI = new ConsoleGUI(enigmaMachine);

      do {
        Console.Clear();
        consoleGUI.ShowStartScreenAndGetInput();
      } while (!consoleGUI.UserWantsToExit);
    }
  }
}
