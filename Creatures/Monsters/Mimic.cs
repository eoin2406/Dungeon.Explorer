using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public class Mimic : Monster
    {
        public Mimic() : base("Mimic", 75, 90, 10, 17)
        {
            GoesFirst = random.Next(2) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Mimic shatters to pieces.\nAfter collecting the key, the brave explorer absorbs its cursed soul...\n", 1);
            Console.ForegroundColor = ConsoleColor.White;
            System.Threading.Thread.Sleep(3000);
            Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("You notice a Mimic lurking, trying its best to fool unfortunate explorers...\n");
        }
    }
}