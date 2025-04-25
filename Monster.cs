using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace DungeonExplorer
{

    // Monster inherits from Creature as it is a Derived Class. This means every monster will take the attributes from creature:
    public class Monster : Creature
    {
        public Room CurrentRoom { get; set; }
        public int AttackDmg { get; set; }
        public static Random random = new Random();
        public Monster(string name, int minHealth, int maxHealth, int minDmg, int maxDmg) : base(name, random.Next(minHealth, maxHealth))
        {
            // Debug.Assert is used to check the player has a name
            Debug.Assert(name != null, "Test failed: Player has no name.");
            // Testing to see if the health is a positive integer
            Test.TestForPositiveInteger(minHealth);
            AttackDmg = random.Next(minDmg, maxDmg);
        }
        public bool GoesFirst { get; set; } = false;
        public virtual string GetMonsterNoise()
        {
            return ("The monster attacked you");
        }
        

    }
    // Monster names are displayed as a string. Their minumum and maximum health values are stored here, as well as their minimum and maximum damage values: 
    public class Orc : Monster
    {
        public Orc() : base("Orc", 70, 80, 5, 10)
        {
            GoesFirst = random.Next(3) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("A Orc shrieks");
        }
    }
    public class Dragon : Monster
    {
        public Dragon() : base("Dragon", 150, 200, 15, 25)
        {
            GoesFirst = random.Next(2) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("A Dragon roars");
        }
    }
    public class Goblin : Monster
    {
        public Goblin() : base("Goblin", 40, 50, 1, 7)
        {
            GoesFirst = random.Next(4) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("A Goblin screeches");
        }
    }
    public class Mimic : Monster {
        public Mimic() : base("Mimic", 100, 120, 10, 17)
        {
            GoesFirst = random.Next(2) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("A Mimic whimpers");
        }
    }
    public class Vampire : Monster
    {
        public Vampire() : base("Vampire", 60, 75, 6, 12)
        {
            GoesFirst = random.Next(3) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("A Vampire laughs");
        }
    }
    public class Skeleton : Monster
    {
        public Skeleton() : base("Skeleton", 20, 40, 1, 5)
        {
            GoesFirst = random.Next(4) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("A Skeleton rattles");
        }
    }
    public class Minotaur : Monster {
        public Minotaur() : base("Minotaur", 300, 300, 35, 50)
        {
            GoesFirst = true;
        }
        public override string GetMonsterNoise()
        {
            return ("A Minotaur groans");
        }
    }

}