using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EnigmaMachine {
  public class EnigmaMachine {
    private string name;

    public List<Cylinder> cylinders = new List<Cylinder>();
    public string Name { get => name; set => name = value; }

    public EnigmaMachine(string name, string[] rotorNames, int[] ringPositions, string plugboardConfig) {
      this.Name = name;

      cylinders.Add(
        new Cylinder("Entry Wheel", 0, EnigmaConfig.TransformSwitchedPlugsToAlphabet(plugboardConfig), 0)
      );

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
      cylinders.Add(new Cylinder("Reflector", 0, EnigmaConfig.TransformSwitchedPlugsToAlphabet(EnigmaConfig.REFLECTOR_A), 0));

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

      for(int i = 0; i < cylinders.Count; i++) {
        if (!cylinders[i].IsActive) {
          continue;
        }
        encodedResult = cylinders[i].Encode(encodedResult, true); 
      }
      
      for (int i = cylinders.Count - 2; i >= 0; i--) {
        if (!cylinders[i].IsActive) {
          continue;
        }
        encodedResult = cylinders[i].Encode(encodedResult, false);
      }

      cylinders[1].IncreaseStrikeCountAndRingPositionAndCheckOverturn();

      return encodedResult;
    }
    public char Decoder(char message) {
      char decodedResult = message;

      for (int i = 0; i < cylinders.Count; i++) {
        if (!cylinders[i].IsActive) {
          continue;
        }
        decodedResult = cylinders[i].Decode(decodedResult, true);
      }

      for (int i = cylinders.Count - 2; i >= 0; i--) {
        if (!cylinders[i].IsActive) {
          continue;
        }
        decodedResult = cylinders[i].Decode(decodedResult, false);
      }

      cylinders[1].IncreaseStrikeCountAndRingPositionAndCheckOverturn();

      return decodedResult;
    }
  }

  

  /// <summary>
  /// Model for base cylinder (should be refactored to real base cylinder and other types inheriting from it)
  /// </summary>
  public class Cylinder {
    private string name;
    private int currentStrikeCount;
    private int ringPositionIndex;
    private int turnoverPosition;
    private string inputScheme;
    private string outputScheme;
    private int startPosition;
    private bool isActive = true;
    private bool hasFixRingposition = false;
    private int firstIndex = -1;
    private int secondIndex = -1;

    public Cylinder previousCylinder, nextCylinder;

    public string Name { get => name; set => name = value; }
    public int RingPositionIndex { get => ringPositionIndex; set => ringPositionIndex = value; }
    public int TurnoverPosition { get => turnoverPosition; set => turnoverPosition = value; }
    public string InputScheme { get => inputScheme; set => inputScheme = value; }
    public string OutputScheme { get => outputScheme; set => outputScheme = value; }
    public int StartPosition { get => startPosition; set => startPosition = value; }
    public bool IsActive { get => isActive; set => isActive = value; }
    public bool HasFixRingposition { get => hasFixRingposition; set => hasFixRingposition = value; }
    public int FirstIndex { get => firstIndex; set => firstIndex = value; }
    public int SecondIndex { get => secondIndex; set => secondIndex = value; }
    public int CurrentStrikeCount { get => currentStrikeCount; set => currentStrikeCount = value; }

    public Cylinder(string name, int startPosition, string outputScheme, int turnoverPosition) {
      this.Name = name;
      this.RingPositionIndex = this.StartPosition = startPosition - 1;
      this.TurnoverPosition = turnoverPosition - 1;
      this.CurrentStrikeCount = 0;
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
      firstIndex = -1;
      CurrentStrikeCount = 0;
    }

    public char Encode(char original, bool beforeReflector) {
      char result = ' ';
      int offset = 0;

      if (original >= 'A' && original <= 'Z') {
        offset = (InputScheme.IndexOf(original.ToString()) + (RingPositionIndex > -1 ? CurrentStrikeCount : 0)) % 26;
        result = OutputScheme[offset];
        if(beforeReflector) {
          FirstIndex = offset;
        } else {
          SecondIndex = offset;
        }
      }
      Debug.Write($"{original} --> {result} {offset}");
      return result;
    }

    public char Decode(char encodedChar, bool beforeReflector) {
      char result = ' ';
      int offset = 0;

      if(encodedChar >= 'A' && encodedChar <= 'Z') {
        offset = (OutputScheme.IndexOf(encodedChar.ToString()) - (RingPositionIndex > -1 ? CurrentStrikeCount : 0));
        if(offset < 0) {
          offset += 26;
        }
        result = InputScheme[offset];
        if (beforeReflector) {
          FirstIndex = offset;
        } else {
          SecondIndex = offset;
        }
      }
      Debug.Write($"{encodedChar} --> {result} ");
      return result;
    }

    public void IncreaseStrikeCountAndRingPositionAndCheckOverturn() {
      RingPositionIndex++;
      CurrentStrikeCount++;
      Debug.WriteLine($"nRP: {RingPositionIndex} {Name}");
      RingPositionIndex %= 26;
      CurrentStrikeCount %= 26;

      if (RingPositionIndex == TurnoverPosition) {
        Debug.WriteLine($"{Name} turnover at {TurnoverPosition}({InputScheme[TurnoverPosition]})");
        if (nextCylinder != null) {
          nextCylinder.IncreaseStrikeCountAndRingPositionAndCheckOverturn();
        }
      }

    }
  }
}
