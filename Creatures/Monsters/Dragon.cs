using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonExplorer
{  
    // Inherits from monster:
    public class Dragon : Monster
    {
        // All monsters have unique values. Being minimum HP, maximum HP, minimum attack damage, and maximum attack damage. This allows for randomness:
        public Dragon() : base("Dragon", 80, 100, 15, 25)
        {
            GoesFirst = random.Next(2) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Dragon has been slain.\n Its fiery soul is absorbed by the brave explorer...\n", 1);
            Console.ForegroundColor = ConsoleColor.White;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("A Dragon is watching you. Its ginormous eyes staring deep into yours...\n");
        }
    }
}