using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine {
  public class EnigmaMachine {
    private string name;
    Cylinder entry_wheel, rotor1, rotor2, rotor3, reflector;

    public string Name { get => name; set => name = value; }

    public EnigmaMachine(string name) {
      this.Name = name;
      this.rotor1 = new Cylinder("Rotor1", 1, EnigmaConfig.CYLINDER_1);
      this.rotor2 = new Cylinder("Rotor2", 1, EnigmaConfig.CYLINDER_2);
      this.rotor3 = new Cylinder("Rotor3", 1, EnigmaConfig.CYLINDER_3);
    }

    public char Encoder(char original) {
      char result = original;
      
      result = rotor1.Encode(result);
      result = rotor2.Encode(result);
      result = rotor3.Encode(result);
      
      return result;
    }
  }

  class Cylinder {
    string name;
    int ringPosition;
    string inputScheme;
    string outputScheme;

    public int RingPosition { get => ringPosition; set => ringPosition = value; }
    public string Encoding { get => outputScheme; set => outputScheme = value; }
    public string Name { get => name; set => name = value; }
    public string Alphabet { get => inputScheme; set => inputScheme = value; }

    public Cylinder(string name, int startPosition, string encoding) {
      this.Name = name;
      this.RingPosition = startPosition;
      this.Encoding = encoding;
      inputScheme = EnigmaConfig.ALPHABET;
    }

    public char Encode(char original) {
      char result = outputScheme[inputScheme.IndexOf(original.ToString())];
      return result;
    }
  }

  public static class EnigmaConfig {
    public const string ALPHABET =   "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string CYLINDER_1 = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
    public const string CYLINDER_2 = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
    public const string CYLINDER_3 = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
    public const string CYLINDER_4 = "ESOVPZJAYQUIRHXLNFTGKDCMWB";
    public const string CYLINDER_5 = "VZBRGITYUPSDNHLXAWMJQOFECK";
    public const string CYLINDER_6 = "JPGVOUMFYQBENHZRDKASXLICTW";
    public const string CYLINDER_7 = "NZJHGRCXMYSWBOUFAIVLPEKQDT";
    public const string CYLINDER_8 = "FKQHTLXOCBJSPDZRAMEWNIUYGV";
    public const string CYLINDER_BETA_M4 = "LEYJVCNIXWPBQMDRTAKZGFUHOS";
    public const string CYLINDER_GAMMA_M4 = "FSOKANUERHMBTIYCWLQPZXVGJD";

    public const string REFLECTOR_A = "AE  BJ  CM  DZ  FL  GY  HX  IV  KW  NR  OQ  PU  ST";
    public const string REFLECTOR_A_SOLUTION = "EJMZALYXVBWFCRQUONTSPIKHGD";

    public const string REFLECTOR_B = "AY  BR  CU  DH  EQ  FS  GL  IP  JX  KN  MO  TZ  VW";
    public const string REFLECTOR_B_SOLUTION = "YRUHQSLDPXNGOKMIEBFZCWVJAT";

    public const string REFLECTOR_C = "AF  BV  CP  DJ  EI  GO  HY  KR  LZ  MX  NW  QT  SU";
    public const string REFLECTOR_C_SOLUTION = "FVPJIAOYEDRZXWGCTKUQSBNMHL";

    public const string REFLECTOR_B_NARROW = "AE  BJ  CM  DZ  FL  GY  HX  IV  KW  NR  OQ  PU  ST";
    public const string REFLECTOR_B_NARROW_SOLUTION = "ENKQAUYWJICOPBLMDXZVFTHRGS";

    public const string REFLECTOR_C_NARROW = "AE  BJ  CM  DZ  FL  GY  HX  IV  KW  NR  OQ  PU  ST";
    public const string REFLECTOR_C_NARROW_SOLUTION = "RDOBJNTKVEHMLFCWZAXGYIPSUQ";
  }
}
