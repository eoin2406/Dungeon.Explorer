using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public abstract class Item : ICollectable
    {
        public string Name {get; set;}
        public string Type {get; set;}
        public string Description {get; set;}
        public Item(string name, string type, string description)
        {
            Name = name;
            Type = type;
            Description = description;
        }

        // The ICollectable interface is used here, as well as the Collect() method associated with it:
        public virtual void Collect()
        {
            Console.WriteLine($"You have collected a {Name}. {Description}");
        }
        public virtual string GetSummary()
        {
            return ($"This item is a {Type}.");
        }
    }

    public class Weapon : Item
    {
        public int AttackDmg {get; set;}
        public Weapon(string name, string description, int attackDmg) : base(name, "Weapon", description)
        {
            AttackDmg = attackDmg;
        }
        public int GetAttackDmg()
        {
            return AttackDmg;
        }
        public override string GetSummary()
        {
            return ($"{Name} ({AttackDmg} attack damage)");
        }
    }

    public class Potion : Item
    {
        public int HealingFactor {get; set;}
        public Potion(string name, string description, int healingFactor) : base(name, "Potion", description)
        {
            HealingFactor = healingFactor;
        }
        public override string GetSummary()
        {
            return ($"{Name} potion.");
        }
    }

    public class Misc : Item
    {
        public Misc(string name, string description) : base(name, "Misc", description)
        {
        }
    }

}