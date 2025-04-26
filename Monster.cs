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
    // These seven monsters can be found at random in rooms when the player is exploring:
    public class Spider : Monster
    {
        public Spider() : base("Spider", 60, 80, 5, 10)
        {
            GoesFirst = random.Next(3) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("You can hear a Spider scattering around the room...\n");
        }
    }
    public class Dragon : Monster
    {
        public Dragon() : base("Dragon", 100, 120, 15, 25)
        {
            GoesFirst = random.Next(2) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("A Dragon is watching you. Its ginormous eyes staring deep into yours...\n");
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
            return ("A puny Goblin screeches at you...\n");
        }
    }
    public class Mimic : Monster
    {
        public Mimic() : base("Mimic", 75, 90, 10, 17)
        {
            GoesFirst = random.Next(2) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("You notice a Mimic lurking, trying its best to fool unfortunate explorers...\n");
        }
    }
    public class Vampire : Monster
    {
        public Vampire() : base("Vampire", 50, 70, 6, 12)
        {
            GoesFirst = random.Next(3) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("You hear a Vampire laughing...\n");
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
            return ("The frail bones of a Skeleton can be heard rattling...\n");
        }
    }
    public class Hound : Monster
    {
        public Hound() : base("Hound", 30, 40, 4, 10)
        {
            GoesFirst = random.Next(4) == 1;
        }
        public override string GetMonsterNoise()
        {
            return ("You hear the barking of a fierce Hound...\n");
        }
    }
    // This is the boss monster. It cannot spawn in rooms, and can only be battled in the boss room. Its minimum and maximum health are the same every game:
    public class Minotaur : Monster {
        public Minotaur() : base("Minotaur", 300, 300, 35, 50)
        {
            GoesFirst = true;
        }
        public override string GetMonsterNoise()
        {
            return ("The Minotaur roars at you, fueled by anger and hatred...\n");
        }
    }

}