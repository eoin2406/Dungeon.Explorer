using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Skeleton : Monster
    {
        public Skeleton() : base("Skeleton", 20, 40, 1, 5)
        {
            GoesFirst = random.Next(4) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Skeleton breaks into pieces.\n Its cursed soul is absorbed by the brave explorer...\n", 1);
            Console.ForegroundColor = ConsoleColor.White;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("The frail bones of a Skeleton can be heard rattling...\n");
        }
    }
}