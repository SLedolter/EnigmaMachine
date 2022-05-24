using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EnigmaMachine {
  public class EnigmaMachine {
    private string name;
    Cylinder entry_wheel, rotor1, rotor2, rotor3, reflector;

    public string Name { get => name; set => name = value; }
    
    public EnigmaMachine(string name) {
      this.Name = name;
      this.entry_wheel = new Cylinder("entry_wheel", 0, EnigmaConfig.TransformSwitchedPlugsToAlphabet(EnigmaConfig.PLUGBOARD_1), 0);
      this.rotor1 = new Cylinder("Rotor1", 1, EnigmaConfig.CYLINDER_1, EnigmaConfig.TURNOVER_1_CYLINDER_1);
      this.rotor2 = new Cylinder("Rotor2", 1, EnigmaConfig.CYLINDER_2, EnigmaConfig.TURNOVER_1_CYLINDER_2);
      this.rotor3 = new Cylinder("Rotor3", 1, EnigmaConfig.CYLINDER_3, EnigmaConfig.TURNOVER_1_CYLINDER_3);
      this.reflector = new Cylinder("Reflector", 0, EnigmaConfig.REFLECTOR_A_SOLUTION, 0);

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
  class Cylinder {
    private string name;
    private int ringPosition;
    private int turnoverPosition;
    private string inputScheme;
    private string outputScheme;
    private int startPosition;

    public Cylinder previousCylinder, nextCylinder;

    public Cylinder(string name, int startPosition, string outputScheme, int turnoverPosition) {
      this.name = name;
      this.ringPosition = this.startPosition = startPosition - 1;
      this.turnoverPosition = turnoverPosition - 1;
      inputScheme = EnigmaConfig.ALPHABET;
      this.outputScheme = outputScheme;
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
      ringPosition = startPosition;
    }

    public char Encode(char original) {
      char result = ' ';

      if (original >= 'A' && original <= 'Z') {
        int offset = (inputScheme.IndexOf(original.ToString()) + (ringPosition > -1 ? ringPosition : 0)) % 26;
        result = outputScheme[offset];
      }
      Debug.Write($"{original} --> {result} ");
      return result;
    }

    public char Decode(char encodedChar) {
      char result = ' ';

      if(encodedChar >= 'A' && encodedChar <= 'Z') {
        int offset = (outputScheme.IndexOf(encodedChar.ToString()) + (ringPosition > -1 ? ringPosition : 0)) % 26;
        result = inputScheme[offset];
      }
      Debug.Write($"{encodedChar} --> {result} ");
      return result;
    }

    public void IncreaseRingPositionAndCheckOverturn() {
      ringPosition++;
      Debug.WriteLine($"nRP: {ringPosition} {name}");
      if (ringPosition > 26) {
        ringPosition %= 26;
      }

      if (ringPosition == turnoverPosition) {
        Debug.WriteLine($"{name} turnover at {turnoverPosition}({inputScheme[turnoverPosition]})");
        if (nextCylinder != null) {
          nextCylinder.IncreaseRingPositionAndCheckOverturn();
        }
      }

    }
  }
}
