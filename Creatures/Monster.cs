using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading;

namespace DungeonExplorer
{
    // Monster inherits from Creature as it is a Derived Class. This means every monster will take the attributes from creature:
    public class Monster : Creature, ICollectable
    {
        public Room CurrentRoom { get; set; }
        public int AttackDmg { get; set; }
        public static Random random = new Random();
        public Monster(string name, int minHealth, int maxHealth, int minDmg, int maxDmg) : base(name, random.Next(minHealth, maxHealth))
        {
            // Debug.Assert is used to check the player has a name:
            Debug.Assert(name != null, "Test failed: Player has no name.");
            // Testing to see if the monster health is a positive integer:
            Test.TestForPositiveInteger(minHealth);
            AttackDmg = random.Next(minDmg, maxDmg);
        }
        public void PrintDelay(string text, int delay)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.Write("\n");
        }
        // The Collect() method from the ICollectable interface is used here:
        public override void Collect()
        {
            {
                base.Collect();
                Thread.Sleep(4000);
                Console.Clear();
                // The end-game statistics are updated for the monster' soul collected:
                Statistics.CollectedMonster();
            }
        }
        public bool GoesFirst { get; set; } = false;
        public virtual string GetMonsterNoise()
        {
            return ("The monster attacked you");
        }
    }
}