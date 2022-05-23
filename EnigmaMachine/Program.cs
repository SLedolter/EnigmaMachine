using System;

namespace EnigmaMachine
{
  class Program
  {
    static void Main(string[] args)
    {
      EnigmaMachine enigmaMachine = new EnigmaMachine("MyEnigmaMachine");
      Console.Error.WriteLine($"Name: {enigmaMachine.Name}");
    }
  }
}
