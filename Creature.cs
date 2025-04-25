using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Creature is the superclass/parent class of Monster and Player as they both inherit the attributes from this class:
namespace DungeonExplorer
{
    // The IDamageable interface is used in the Creature class:
    public abstract class Creature : IDamageable
    {
        public string Name {get; set;}
        protected int Health {get; set;}

        // These are where the name of the creature and the creature's health value will be set in the derived classes that inherit from this class:
        public Creature(string name, int health)
        {
            Name = name;
            Health = health;
        }

        // The GetHealth method returns the health of the creature:
        public int GetHealth()
        {
            return Health;
        }

        // The TakeDamage method reduces the health value by the damage value:
        public void TakeDamage(int damage)
        {
            Health -= damage;
        }

        // The IsAlive method checks if the creature is alive by seeing if their health is less than 0:
        public bool IsAlive()
        {
            return Health > 0;
        }
    }
}