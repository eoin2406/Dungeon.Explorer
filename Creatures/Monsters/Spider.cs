using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Spider : Monster
    {
        public Spider() : base("Spider", 60, 80, 6, 12)
        {
            GoesFirst = random.Next(3) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Spider has been slain.\nIts webbed soul is absorbed by the brave explorer...\n",1);
            Console.ForegroundColor = ConsoleColor.White;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("You can hear a Spider scattering around the room...\n");
        }
    }
}