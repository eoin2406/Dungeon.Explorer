using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Dragon : Monster
    {
        public Dragon() : base("Dragon", 100, 120, 15, 25)
        {
            GoesFirst = random.Next(2) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("The Dragon has fallen. Its cursed soul absorbed by the brave explorer...\n", 1);
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