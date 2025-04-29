using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    // Mysterious key is used to enter the boss room:
    internal class MysteriousKey : Item
    {
        public MysteriousKey() : base("Mysterious Key", "Key", "A strange key that might open something important.")
        {
        }
    }
}
