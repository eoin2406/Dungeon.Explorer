using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;

namespace DungeonExplorer
{
    public class Player : Creature
    {
        public bool playerHasKey = false;
        public Room CurrentRoom { get; set; }
        public List<Item> Inventory { get; set; }
        public Player(string name, int health) : base(name, health)
        {
            // Debug.Assert is used to check the player has a name
            Debug.Assert(name != null, "Test failed: Player has no name.");
            // Testing to see if the health is a positive integer
            Test.TestForPositiveInteger(health);
            Inventory = new List<Item>();
        }

        public void AddWeapon(Weapon weapon)
        {
            if (Inventory.OfType<Weapon>().Count() == 3)
            {
                // This string is shown when the user picks up too many weapons. I made the inventory have a limit on how many weapons you can carry at once:
                Console.WriteLine("You cannot carry anymore weapons, due to a weight limit!\nWould you like to remove your weakest weapon?\n(Enter \"yes\" or \"no\")\n");

                string input = Console.ReadLine()?.ToLower().Trim();

                // If the player types "yes", their weakest weapon will be removed from their inventory:
                if (input == "yes")
                {
                    var weapons = ChooseWeapons();

                    if (weapons.Count > 0)
                    {
                        var WeakWeapon = weapons.Last();
                        Inventory.Remove(WeakWeapon);
                        // This string is printed when you remove your weak weapon and pick up the newly discovered one:
                        Console.WriteLine($"\nYour weakest weapon ({WeakWeapon.Name}) was discarded, and you picked up the {weapon.Name}!\n");

                        Inventory.Add(weapon);
                    }
                    else
                    {
                        Console.WriteLine("\nYou have no weapons to dispose of...\n");
                    }
                }
                else
                {
                    // This string is printed if the user chooses not to pick up the newly discovered weapon:
                    Console.WriteLine("\nYou chose not to discard your weakest weapon. The newly discovered weapon was not added...\n");
                }
            }
            else
            {
                // Removes the fists as a weapon if another weapon is collected:
                Inventory.Add(weapon);

                if (weapon.Name != "Fists")
                {
                    RemoveFists();
                }
            }

        }
        // PrintDelay method is called to be used for the next part:
        private void PrintDelay(string text, int delay)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.Write("\n");
        }
        // Method used to remove the fists once a weapon has been collected:
        private void RemoveFists()
        {
            var fists = Inventory.OfType<Weapon>().FirstOrDefault(w => w.Name.Equals("Fists", StringComparison.OrdinalIgnoreCase));

            if (fists != null)
            {
                Inventory.Remove(fists);
                this.PrintDelay("A weapon! No need for my bare knuckles anymore.", 1);
                Thread.Sleep(2000);
                Console.Clear();
            }
        }

        public List<Weapon> ChooseWeapons()
        {
            var NumberedWeapons = Inventory.OfType<Weapon>().ToList();
            var WeaponsSorted = from Weapon weapon in NumberedWeapons orderby weapon.AttackDmg descending select weapon;
            return WeaponsSorted.ToList();
        }

        // Adds potions to inventory:
        public void AddPotion(Potion potion)
        {
            Inventory.Add(potion);
        }
        // Adds miscellaneous to inventory:
        public void AddMisc(Misc misc)
        {
            Inventory.Add(misc);
        }

        // Inventory is stored as a list:
        public List<Item> GetInventory()
        {
            return Inventory;
        }

        public void SetInventory(List<Item> inventory)
        {
            Inventory = inventory;
        }

        public List<Weapon> GetWeapons()
        {
            //Checks if the player has any weapons:
            var NumberedWeapons = Inventory.OfType<Weapon>().Select(weapon => weapon).ToList();
            var WeaponsSorted = from Weapon weapon in NumberedWeapons orderby weapon.AttackDmg descending select weapon;
            List<Weapon> WeaponsSortedList = WeaponsSorted.ToList();
            if (WeaponsSortedList.Count == 0)
            {
                Console.WriteLine("Apart from your trusty fists, you are currently carrying no weapons.");
                return null;
            }
            return WeaponsSortedList;
        }

        public List<Potion> GetPotions()
        {
            // Checks if the player has any potions:
            var potions = Inventory.OfType<Potion>().Select(Potion => Potion).ToList();
            var PotionsSorted = from Potion potion in potions orderby potion.HealingFactor descending select potion;
            List<Potion> PotionsList = PotionsSorted.ToList();
            if (PotionsList.Count == 0)
            {
                return null;
            }
            return PotionsList;
        }
        
        public List<Misc> GetMiscs()
        {
            return Inventory.OfType<Misc>().ToList();
        }

        // Heals the player by consuming their potion/s:
        public void drinkPotion(Potion potion)
        {
            Health += potion.HealingFactor;
        }

        // Uses miscellaneous items (the key for the boss room):
        public void useMisc(Misc misc)
        {
            Inventory.Remove(misc);
        }

        // Sets the player in a room:
        public void SetCurrentRoom(Room room)
        {
            CurrentRoom = room;
        }
        private Weapon GetStrongestWeapon()
        {
            Weapon strongestWeapon = Inventory.OfType<Weapon>().ToList()[0];
            foreach (var weapon in Inventory.OfType<Weapon>().ToList())
            {
                if (weapon.AttackDmg > strongestWeapon.AttackDmg)
                {
                    strongestWeapon = weapon;
                }
            }
            return strongestWeapon;
        }
        
        // Player combat below:
        public void Combat(List<Monster> monsters, Player player)
        {
            Random random = new Random();
            Console.Clear();
            Monster monster = monsters[0];

            // Shows the player's name vs the monster's name:
            Console.WriteLine($"{player.Name} vs the {monster.Name}");
            int roundNum = 1;

            // Displays the round number:
            while (player.IsAlive() && monster.IsAlive())
            {
                Console.WriteLine("- - - - - - - - - -");
                Console.WriteLine($"Round {roundNum}");

                bool isCritHit = random.Next(100) < 10;

                // The monster has a random chance of attacking before the player:
                if (monster.GoesFirst)
                {
                    Console.WriteLine($"{monster.Name} attacks {player.Name} for {monster.AttackDmg} DMG.");
                    player.TakeDamage(monster.AttackDmg);
                    Statistics.TakenDamage(monster.AttackDmg);

                    if (!player.IsAlive())
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"\n{player.Name} was slain by {monster.Name}");
                        Console.WriteLine("\nYou lose!\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(2000);
                        Console.WriteLine("Game Over!\nType anything and click ENTER to view your statistics");
                        Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine(Statistics.GameOverStats());
                        Environment.Exit(0);
                    }

                    // This uses the player's strongest weapon by default, as well as allowing for critical hits:
                    int playerDmg = player.GetStrongestWeapon().GetAttackDmg();
                    if (isCritHit)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        playerDmg *= 2;
                        Console.WriteLine($"Critical Hit! {player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.WriteLine($"{player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                    monster.TakeDamage(playerDmg);
                    Statistics.DoneDamage(playerDmg);

                    // If the monster is not alive:
                    if (!monster.IsAlive())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{monster.Name} was defeated!");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        if (monster is ICollectable collectableMonster)
                        {
                            collectableMonster.Collect();
                        }
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                
                else
                {
                    // Uses the strongest weapon by default for attacking monsters:
                    int playerDmg = player.GetStrongestWeapon().GetAttackDmg();
                    if (isCritHit)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        playerDmg *= 2;
                        Console.WriteLine($"Critical Hit! {player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                    Console.WriteLine($"{player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                    monster.TakeDamage(playerDmg);
                    Statistics.DoneDamage(playerDmg);

                    // If the monster is not alive, it is cleared from the room and the player collects its soul:
                    if (!monster.IsAlive())
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{monster.Name} was defeated!");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        monster.Collect();
                        player.CurrentRoom.delMonster(monster);
                        break;
                    }

                    Console.WriteLine($"{monster.Name} attacks {player.Name} for {monster.AttackDmg} DMG.");
                    player.TakeDamage(monster.AttackDmg);
                    Statistics.TakenDamage(monster.AttackDmg);

                    // Game over screen and statistics if the player is not alive:
                    if (!player.IsAlive())
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        PrintDelay($"\n{player.Name} was slain by {monster.Name}", 2);
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(2000);
                        Console.Clear();
                        Console.WriteLine("Game Over!\nType anything and click ENTER to view your statistics");
                        Console.ReadLine();
                        Console.Clear();
                        Console.WriteLine(Statistics.GameOverStats());
                        Environment.Exit(0);
                    }
                }

                Console.WriteLine($"{player.Name}: {player.GetHealth()} HP | {monster.Name}: {monster.GetHealth()} HP");
                Thread.Sleep(2000);
                roundNum += 1;
            }
        }
    }

}