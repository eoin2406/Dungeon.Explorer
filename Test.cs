using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    internal class Test
    {
        // Debug.Assert is used to check if the health of the creature is more than 0. This method is called within the Player and Monster classes:
        public static void TestForPositiveInteger(int value)
        {
            Debug.Assert(value > 0, "Error: Value wasn't a positive integer");
        }
        // Debug.Assert is used to check if the value is greater than zero:
        public static void TestForZeroOrAbove(int value)
        {
            Debug.Assert(value >= 0, "Error: Value wasn't a positive integer or 0");
        }
        // Debug.Assert is used to check if the player's name is at least 1 character in length. This method is called within the Game class when the player chooses their name:
        public static void TestForPlayerNameLength(string value)
        {
            Debug.Assert(value.Length > 0, "Error: Player name must be at least one character in length");
        }

    }
}
