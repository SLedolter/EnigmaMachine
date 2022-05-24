using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EnigmaMachine {
  public class EnigmaMachine {
    private string name;

    public List<Cylinder> cylinders = new List<Cylinder>();
    public Cylinder entry_wheel, rotor1, rotor2, rotor3, reflector;
    public string Name { get => name; set => name = value; }
    
    public EnigmaMachine(string name, string[] rotorNames, int[] ringPositions, string plugboardConfig) {
      this.Name = name;
      this.entry_wheel = new Cylinder("entry_wheel", 0, EnigmaConfig.TransformSwitchedPlugsToAlphabet(plugboardConfig), 0);
      
      
      
      this.rotor1 = new Cylinder("Rotor1", 1, EnigmaConfig.CYLINDER_1, EnigmaConfig.TURNOVER_1_CYLINDER_1);
      this.rotor2 = new Cylinder("Rotor2", 1, EnigmaConfig.CYLINDER_2, EnigmaConfig.TURNOVER_1_CYLINDER_2);
      this.rotor3 = new Cylinder("Rotor3", 1, EnigmaConfig.CYLINDER_3, EnigmaConfig.TURNOVER_1_CYLINDER_3);
      
      this.reflector = new Cylinder("Reflector", 0, EnigmaConfig.TransformSwitchedPlugsToAlphabet(EnigmaConfig.REFLECTOR_A), 0);

      this.rotor1.ConnectNextRotor(rotor2);
      this.rotor2.ConnectNextRotor(rotor3);
    }

    public void ResetMachine() {
      rotor1.ResetCylinder();
      rotor2.ResetCylinder();
      rotor3.ResetCylinder();
    }

    public char Encoder(char message) {
      char encodedResult = message;

      encodedResult = entry_wheel.Encode(encodedResult);
      encodedResult = rotor1.Encode(encodedResult);
      encodedResult = rotor2.Encode(encodedResult);
      encodedResult = rotor3.Encode(encodedResult);
      encodedResult = reflector.Encode(encodedResult);
      encodedResult = rotor3.Encode(encodedResult);
      encodedResult = rotor2.Encode(encodedResult);
      encodedResult = rotor1.Encode(encodedResult);
      encodedResult = entry_wheel.Encode(encodedResult);

      rotor1.IncreaseRingPositionAndCheckOverturn();

      return encodedResult;
    }
    public char Decoder(char message) {
      char decodedResult = message;

      decodedResult = entry_wheel.Decode(decodedResult);
      decodedResult = rotor1.Decode(decodedResult);
      decodedResult = rotor2.Decode(decodedResult);
      decodedResult = rotor3.Decode(decodedResult);
      decodedResult = reflector.Decode(decodedResult);
      decodedResult = rotor3.Decode(decodedResult);
      decodedResult = rotor2.Decode(decodedResult);
      decodedResult = rotor1.Decode(decodedResult);
      decodedResult = entry_wheel.Decode(decodedResult);

      rotor1.IncreaseRingPositionAndCheckOverturn();

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
