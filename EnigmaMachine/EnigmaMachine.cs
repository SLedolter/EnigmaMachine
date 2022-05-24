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
      this.rotor1 = new Cylinder("Rotor1", 1, EnigmaConfig.CYLINDER_1, EnigmaConfig.TURNOVER_1_CYLINDER_1);
      this.rotor2 = new Cylinder("Rotor2", 1, EnigmaConfig.CYLINDER_2, EnigmaConfig.TURNOVER_1_CYLINDER_2);
      this.rotor3 = new Cylinder("Rotor3", 1, EnigmaConfig.CYLINDER_3, EnigmaConfig.TURNOVER_1_CYLINDER_3);
      this.reflector = new Cylinder("Reflector", 0, EnigmaConfig.REFLECTOR_A_SOLUTION, 0);

      this.rotor1.nextCylinder = rotor2;
      this.rotor1.nextCylinder = rotor3;
    }

    public void ResetMachine() {
      rotor1.ResetCylinder();
      rotor2.ResetCylinder();
      rotor3.ResetCylinder();
    }

    public char Encoder(char message) {
      char encodedResult = message;

      Debug.Write($"(R1) {encodedResult} --> ");
      encodedResult = rotor1.Encode(encodedResult);
      Debug.Write($"{encodedResult}\n{encodedResult} --> ");
      //result = rotor2.Encode(result);
      //Debug.Write($"result}\n(R2) {result} --> ");
      //result = rotor3.Encode(result);
      //Debug.Write($"result}\n(R3) {result} --> ");
      encodedResult = reflector.Encode(encodedResult);
      Debug.Write($"{encodedResult}\n(UKW) {encodedResult} --> ");
      //result = rotor3.Encode(result);
      //Debug.Write($"result}\n(R3) {result} --> ");
      //result = rotor2.Encode(result);
      //Debug.Write($"result}\n(R2) {result} --> ");
      encodedResult = rotor1.Encode(encodedResult);
      Debug.Write($"{encodedResult}\n(R1) {encodedResult} --> ");

      //rotor1.IncreaseRingPositionAndCheckOverturn();

      return encodedResult;
    }
    public char Decoder(char message) {
      char decodedResult = message;

      Debug.Write($"(R1) {decodedResult} --> ");
      decodedResult = rotor1.Decode(decodedResult);
      Debug.Write($"{decodedResult}\n{decodedResult} --> ");
      //result = rotor2.Encode(result);
      //Debug.Write($"result}\n(R2) {result} --> ");
      //result = rotor3.Encode(result);
      //Debug.Write($"result}\n(R3) {result} --> ");
      decodedResult = reflector.Decode(decodedResult);
      Debug.Write($"{decodedResult}\n(UKW) {decodedResult} --> ");
      //result = rotor3.Encode(result);
      //Debug.Write($"result}\n(R3) {result} --> ");
      //result = rotor2.Encode(result);
      //Debug.Write($"result}\n(R2) {result} --> ");
      decodedResult = rotor1.Decode(decodedResult);
      Debug.Write($"{decodedResult}\n(R1) {decodedResult} --> ");

      //rotor1.IncreaseRingPositionAndCheckOverturn();

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
      return result;
    }

    public char Decode(char encodedChar) {
      char result = ' ';

      if(encodedChar >= 'A' && encodedChar <= 'Z') {
        int offset = (outputScheme.IndexOf(encodedChar.ToString()) + (ringPosition > -1 ? ringPosition : 0)) % 26;
        result = inputScheme[offset];
      }

      return result;
    }

    public void IncreaseRingPositionAndCheckOverturn() {
      ringPosition++;
      Debug.WriteLine($"nRP: {ringPosition} {name}");
      if (ringPosition > 26) {
        ringPosition %= 26;
      }

      if (ringPosition == turnoverPosition) {
        Debug.WriteLine($"{name} overTurn at {turnoverPosition}{inputScheme[turnoverPosition]}");
        if (nextCylinder != null) {
          nextCylinder.IncreaseRingPositionAndCheckOverturn();
        }
      }

    }
  }
}
