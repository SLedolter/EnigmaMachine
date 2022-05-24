using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine {
  public static class EnigmaConfig {
    public const string ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public const string PLUGBOARD_1 = "AD CN ET FL GI JV KZ PU QY WX";

    public const string CYLINDER_1 = "EKMFLGDQVZNTOWYHXUSPAIBRCJ";
    public const int TURNOVER_1_CYLINDER_1 = 17;
    
    public const string CYLINDER_2 = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
    public const int TURNOVER_1_CYLINDER_2 = 5;

    public const string CYLINDER_3 = "BDFHJLCPRTXVZNYEIWGAKMUSQO";
    public const int TURNOVER_1_CYLINDER_3 = 22;

    public const string CYLINDER_4 = "ESOVPZJAYQUIRHXLNFTGKDCMWB";
    public const int TURNOVER_1_CYLINDER_4 = 10;

    public const string CYLINDER_5 = "VZBRGITYUPSDNHLXAWMJQOFECK";
    public const int TURNOVER_1_CYLINDER_5 = 26;

    public const string CYLINDER_6 = "JPGVOUMFYQBENHZRDKASXLICTW";
    public const int TURNOVER_1_CYLINDER_6 = 26;
    public const int TURNOVER_2_CYLINDER_6 = 13;

    public const string CYLINDER_7 = "NZJHGRCXMYSWBOUFAIVLPEKQDT";
    public const int TURNOVER_1_CYLINDER_7 = 26;
    public const int TURNOVER_2_CYLINDER_7 = 13;

    public const string CYLINDER_8 = "FKQHTLXOCBJSPDZRAMEWNIUYGV";
    public const int TURNOVER_1_CYLINDER_8 = 26;
    public const int TURNOVER_2_CYLINDER_8 = 13;

    public const string CYLINDER_BETA_M4 = "LEYJVCNIXWPBQMDRTAKZGFUHOS";
    public const string CYLINDER_GAMMA_M4 = "FSOKANUERHMBTIYCWLQPZXVGJD";

    public const string REFLECTOR_A = "AE  BJ  CM  DZ  FL  GY  HX  IV  KW  NR  OQ  PU  ST";
    public const string REFLECTOR_A_SOLUTION = "EJMZALYXVBWFCRQUONTSPIKHGD";

    public const string REFLECTOR_B = "AY  BR  CU  DH  EQ  FS  GL  IP  JX  KN  MO  TZ  VW";
    public const string REFLECTOR_B_SOLUTION = "YRUHQSLDPXNGOKMIEBFZCWVJAT";

    public const string REFLECTOR_C = "AF  BV  CP  DJ  EI  GO  HY  KR  LZ  MX  NW  QT  SU";
    public const string REFLECTOR_C_SOLUTION = "FVPJIAOYEDRZXWGCTKUQSBNMHL";

    public const string REFLECTOR_B_NARROW_SOLUTION = "ENKQAUYWJICOPBLMDXZVFTHRGS";

    public const string REFLECTOR_C_NARROW_SOLUTION = "RDOBJNTKVEHMLFCWZAXGYIPSUQ";

    public static string TransformSwitchedPlugsToAlphabet(string plugboardOrder) {
      string result = EnigmaConfig.ALPHABET;
      int index1, index2;
      char char1, char2;

      foreach (string pair in plugboardOrder.Split(" ")) {
        index1 = result.IndexOf(pair[0]);
        char1 = result[index1];
        index2 = result.IndexOf(pair[1]);
        char2 = result[index2];
        result = result
          .Remove(index1, 1)
          .Insert(index1, char2.ToString())
          .Remove(index2, 1)
          .Insert(index2, char1.ToString());
      }

      return result;
    }
  }
}
