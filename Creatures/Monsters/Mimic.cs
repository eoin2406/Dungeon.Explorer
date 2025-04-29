using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    // Inherits from monster:
    public class Mimic : Monster
    {
        private Player player;
        // All monsters have unique values. Being minimum HP, maximum HP, minimum attack damage, and maximum attack damage. This allows for randomness:
        public Mimic(Player player) : base("Mimic", 75, 90, 10, 17)
        {
            this.player = player;
            GoesFirst = random.Next(2) == 1;
        }
        public override void Collect()
        {
            base.Collect();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            this.PrintDelay("> The Mimic shatters to pieces.\nIt drops an old key, and the brave explorer absorbs its cursed soul...\n\nMaybe this key can be used for the door in the ruins!", 1);
            Console.ForegroundColor = ConsoleColor.Gray;
            // The mysterious key to the boss door is dropped upon the mimic's defeat:

            if (player != null)
            {
                Misc key = new Misc("Key", "This old key seems similar to a lock on a door you passed by earlier...");
                player.AddMisc(key);
            }
            else
            {
                // Error testing here:
                Console.WriteLine("Error: Player is null and cannot collect key.");
            }

            Room currentRoom = player.CurrentRoom;
            currentRoom.delMonster(this);
                System.Threading.Thread.Sleep(3000);
                Console.Clear();
        }
        public override string GetMonsterNoise()
        {
            return ("You notice a large chest in the centre of the room.\nIts wooden surface shines in the dim light, but something seems off.\n\nThe large chest calls you closer...\n> (Try attack)");
        }
    }
}