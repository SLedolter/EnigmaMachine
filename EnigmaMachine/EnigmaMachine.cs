﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EnigmaMachine {
  public class EnigmaMachine {
    private string name;

    public List<Cylinder> cylinders = new List<Cylinder>();
    public Cylinder entry_wheel, reflector;
    public string Name { get => name; set => name = value; }
    
    public EnigmaMachine(string name, string[] rotorNames, int[] ringPositions, string plugboardConfig) {
      this.Name = name;
      this.entry_wheel = new Cylinder("entry_wheel", 0, EnigmaConfig.TransformSwitchedPlugsToAlphabet(plugboardConfig), 0);
      
      for(int i = 0; i < rotorNames.Length; i++) {
        cylinders.Add(
          new Cylinder(
            "Rotor" + rotorNames[i], 
            ringPositions[i], 
            (string)typeof(EnigmaConfig).GetField("CYLINDER_"+rotorNames[i]).GetValue(null), 
            (int)typeof(EnigmaConfig).GetField("TURNOVER_1_CYLINDER_"+rotorNames[i]).GetValue(null)
          )
        );
      }
      
      this.reflector = new Cylinder("Reflector", 0, EnigmaConfig.TransformSwitchedPlugsToAlphabet(EnigmaConfig.REFLECTOR_A), 0);

      for(int i = 0; i < cylinders.Count - 1; i++) {
        cylinders[i].ConnectNextRotor(cylinders[i + 1]);
      }
    }

    public void ResetMachine() {
      for(int i = 0; i < cylinders.Count; i++) {
        cylinders[i].ResetCylinder();
      }
    }

    public char Encoder(char message) {
      char encodedResult = message;

      encodedResult = entry_wheel.Encode(encodedResult);
      for(int i = 0; i < cylinders.Count; i++) {
        encodedResult = cylinders[i].Encode(encodedResult);
      }
      encodedResult = reflector.Encode(encodedResult);
      for (int i = cylinders.Count - 1; i >= 0; i--) {
        encodedResult = cylinders[i].Encode(encodedResult);
      }
      encodedResult = entry_wheel.Encode(encodedResult);

      cylinders[0].IncreaseRingPositionAndCheckOverturn();

      return encodedResult;
    }
    public char Decoder(char message) {
      char decodedResult = message;

      decodedResult = entry_wheel.Decode(decodedResult);
      for (int i = 0; i < cylinders.Count; i++) {
        decodedResult = cylinders[i].Decode(decodedResult);
      }
      decodedResult = reflector.Decode(decodedResult);
      for (int i = cylinders.Count - 1; i >= 0; i--) {
        decodedResult = cylinders[i].Decode(decodedResult);
      }
      decodedResult = entry_wheel.Decode(decodedResult);

      cylinders[0].IncreaseRingPositionAndCheckOverturn();

      return decodedResult;
    }
  }

  

  /// <summary>
  /// Model for base cylinder (should be refactored to real base cylinder and other types inheriting from it)
  /// </summary>
  public class Cylinder {
    private string name;
    private int ringPosition;
    private int turnoverPosition;
    private string inputScheme;
    private string outputScheme;
    private int startPosition;

    public Cylinder previousCylinder, nextCylinder;

    public string Name { get => name; set => name = value; }
    public int RingPositionIndex { get => ringPosition; set => ringPosition = value; }
    public int TurnoverPosition { get => turnoverPosition; set => turnoverPosition = value; }
    public string InputScheme { get => inputScheme; set => inputScheme = value; }
    public string OutputScheme { get => outputScheme; set => outputScheme = value; }
    public int StartPosition { get => startPosition; set => startPosition = value; }

    public Cylinder(string name, int startPosition, string outputScheme, int turnoverPosition) {
      this.Name = name;
      this.RingPositionIndex = this.StartPosition = startPosition - 1;
      this.TurnoverPosition = turnoverPosition - 1;
      InputScheme = EnigmaConfig.ALPHABET;
      this.OutputScheme = outputScheme;
    }

    public void ConnectPreviousRotor(Cylinder rotor) {
      previousCylinder = rotor;
      rotor.nextCylinder = this;
    }

    public void ConnectNextRotor(Cylinder rotor) {
      nextCylinder = rotor;
      rotor.previousCylinder = this;
    }

    public void ResetCylinder() {
      RingPositionIndex = StartPosition;
    }

    public char Encode(char original) {
      char result = ' ';
      int offset = 0;

      if (original >= 'A' && original <= 'Z') {
        offset = (InputScheme.IndexOf(original.ToString()) + (RingPositionIndex > -1 ? RingPositionIndex : 0)) % 26;
        result = OutputScheme[offset];
      }
      Debug.Write($"{original} --> {result} {offset}");
      return result;
    }

    public char Decode(char encodedChar) {
      char result = ' ';
      int offset = 0;

      if(encodedChar >= 'A' && encodedChar <= 'Z') {
        offset = (OutputScheme.IndexOf(encodedChar.ToString()) - (RingPositionIndex > -1 ? RingPositionIndex : 0));
        if(offset < 0) {
          offset += 26;
        }
        result = InputScheme[offset];
      }
      Debug.Write($"{encodedChar} --> {result} ");
      return result;
    }

    public void IncreaseRingPositionAndCheckOverturn() {
      RingPositionIndex++;
      Debug.WriteLine($"nRP: {RingPositionIndex} {Name}");
      RingPositionIndex %= 26;

      if (RingPositionIndex == TurnoverPosition) {
        Debug.WriteLine($"{Name} turnover at {TurnoverPosition}({InputScheme[TurnoverPosition]})");
        if (nextCylinder != null) {
          nextCylinder.IncreaseRingPositionAndCheckOverturn();
        }
      }

    }
  }
}
