using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine {
  public class EnigmaMachine {
    private string name;

    public EnigmaMachine(string name) {
      this.Name = name;
    }

    public string Name { get => name; set => name = value; }
  }

  class Cylinder {
    string name;
    int ringPosition;
    string encoding;

    public int RingPosition { get => ringPosition; set => ringPosition = value; }
    public string Encoding { get => encoding; set => encoding = value; }
    public string Name { get => name; set => name = value; }

    public Cylinder(string name, int startPosition, string encoding) {
      this.Name = name;
      this.RingPosition = startPosition;
      this.Encoding = encoding;
    }
  }
}
