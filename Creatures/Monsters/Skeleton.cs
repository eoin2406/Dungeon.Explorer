﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    // Inherits from monster:
    public class Skeleton : Monster
    {
        // All monsters have unique values. Being minimum HP, maximum HP, minimum attack damage, and maximum attack damage. This allows for randomness:
        public Skeleton() : base("Skeleton", 30, 45, 6, 10)
        {
            GoesFirst = random.Next(4) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Skeleton breaks into pieces.\n Its cursed soul is absorbed by the brave explorer...\n", 1);
            Console.ForegroundColor = ConsoleColor.Gray;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("The frail bones of a Skeleton can be heard rattling...\n");
        }
    }
}