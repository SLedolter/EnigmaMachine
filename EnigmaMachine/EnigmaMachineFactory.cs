using System;
using System.Collections.Generic;
using System.Text;

namespace EnigmaMachine {

  public static class EnigmaMachineFactory {
    public static EnigmaMachine Day29Machine() {
      EnigmaMachine enigmaMachine = 
        new EnigmaMachine(
        "Day29Machine",
        new string[] { "1", "4", "3" },
        new int[] { 16, 26, 8 },
        EnigmaConfig.PLUGBOARD_DAY_29,
        "A"
      );

      return enigmaMachine;
    }

    public static EnigmaMachine NoPlugboardMachineWithFixedRingposition() {
      EnigmaMachine enigmaMachine = new EnigmaMachine(
        "NoPlugboardMachine",
        new string[] { "1", "2", "3" },
        new int[] { 0, 0, 0 },
        null,
        "A"
      );
      foreach(Cylinder cylinder in enigmaMachine.cylinders) {
        cylinder.HasFixRingposition = true;
      }
      return enigmaMachine;
    }

    public static EnigmaMachine Rotor3OnlyForTesting() {
      return new EnigmaMachine(
        "Rotor3Only",
        new string[] { "3" },
        new int[] { 16 },
        null,
        null
      );
    }
  }
}
