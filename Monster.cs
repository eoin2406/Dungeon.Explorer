using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace DungeonExplorer
{
    public class Monster : Creature {
        public Room CurrentRoom { get; set; }
        public int AttackDmg { get; set; }
        public static Random random = new Random();
        public Monster(string name, int minHealth, int maxHealth, int minDmg, int maxDmg) : base(name, random.Next(minHealth, maxHealth)) {
            // Debug.Assert is used to check the player has a name
            Debug.Assert(name != null, "Test failed: Player has no name.");
            // Testing to see if the health is a positive integer
            Test.TestForPositiveInteger(minHealth);
            AttackDmg = random.Next(minDmg, maxDmg);
        }
        public bool GoesFirst { get; set; } = false;
        

    }

    public class Orc : Monster {
        public Orc() : base("Orc", 70, 80, 5, 10) {
            GoesFirst = random.Next(3) == 1;
        }
    }
    public class Dragon : Monster {
        public Dragon() : base("Dragon", 150, 200, 15, 25) {
            GoesFirst = random.Next(2) == 1;
        }
    }
    public class Goblin : Monster {
        public Goblin() : base("Goblin", 40, 50, 1, 7) {
            GoesFirst = random.Next(4) == 1;
        }
    }
    public class Mimic : Monster {
        public Mimic() : base("Mimic", 100, 120, 10, 17) {
            GoesFirst = random.Next(2) == 1;
        }
    }
    public class Vampire : Monster {
        public Vampire() : base("Vampire", 60, 75, 6, 12) {
            GoesFirst = random.Next(3) == 1;
        }
    }
    public class Skeleton : Monster {
        public Skeleton() : base("Skeleton", 20, 40, 1, 5) {
            GoesFirst = random.Next(4) == 1;
        }
    }
    public class Minotaur : Monster {
        public Minotaur() : base("Minotaur", 300, 300, 35, 50) {
            GoesFirst = true;
        }
    }

}
