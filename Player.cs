using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;

namespace DungeonExplorer
{
    public class Player : Creature {
        public Room CurrentRoom { get; set; }
        public List<Item> Inventory { get; set; }
        public Player(string name, int health) : base(name, health) {
            // Debug.Assert is used to check the player has a name
            Debug.Assert(name != null, "Test failed: Player has no name.");
            // Testing to see if the health is a positive integer
            Test.TestForPositiveInteger(health);
            Inventory = new List<Item>();
        }

        public void AddWeapon(Weapon weapon) {
            Inventory.Add(weapon);
        }

        public void AddPotion(Potion potion) {
            Inventory.Add(potion);
        }
        public void AddMisc(Misc misc) {
            Inventory.Add(misc);
        }

        public List<Item> GetInventory() {
            return Inventory;
        }

        public void SetInventory(List<Item> inventory) {
            Inventory = inventory;
        }

        public List<Weapon> GetWeapons() {
            return Inventory.OfType<Weapon>().ToList();
        }

        public List<Potion> GetPotions() {
            return Inventory.OfType<Potion>().ToList();
        }
        
        public List<Misc> GetMiscs() {
            return Inventory.OfType<Misc>().ToList();
        }

        public void drinkPotion(Potion potion) {
            Health += potion.HealingFactor;
        }

        public void useMisc(Misc misc) {
            Inventory.Remove(misc);
        }

        public void SetCurrentRoom(Room room) {
            CurrentRoom = room;
        }

        /* TO DO
        
        NEED TO USE LINQ FOR SORTING FOR MARKS
        
        
        DON'T FORGET
        
        */
        private Weapon GetStrongestWeapon() {
            Weapon strongestWeapon = Inventory.OfType<Weapon>().ToList()[0];
            foreach (var weapon in Inventory.OfType<Weapon>().ToList()) {
                if (weapon.AttackDmg > strongestWeapon.AttackDmg) {
                    strongestWeapon = weapon;
                }
            }
            return strongestWeapon;
        }

        public void Combat(List<Monster> monsters, Player player) {
            Random random = new Random();
            Console.Clear();
            // Maybe add LINQ to sort monsters by weakest first?
            Monster monster = monsters[0];

            Console.WriteLine($"{player.Name} vs the {monster.Name}");
            int roundNum = 1;

            while (player.IsAlive() && monster.IsAlive()) {
                Console.WriteLine("- - - - - - - - - -");
                Console.WriteLine($"Round {roundNum}");

                bool isCritHit = random.Next(100) < 10;

                if (monster.GoesFirst) {
                    Console.WriteLine($"{monster.Name} attacks {player.Name} for {monster.AttackDmg} DMG.");
                    player.TakeDamage(monster.AttackDmg);

                    if (!player.IsAlive()) {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"{player.Name} was slain by {monster.Name}");
                        Console.WriteLine("\nYou lose!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(2000);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }

                    int playerDmg = player.GetStrongestWeapon().GetAttackDmg();
                    if (isCritHit) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        playerDmg *= 2;
                        Console.WriteLine($"Critical Hit! {player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine($"{player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                    monster.TakeDamage(playerDmg);

                    if (!monster.IsAlive()) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{monster.Name} was defeated!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                } else {
                    int playerDmg = player.GetStrongestWeapon().GetAttackDmg();
                    if (isCritHit) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        playerDmg *= 2;
                        Console.WriteLine($"Critical Hit! {player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine($"{player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                    monster.TakeDamage(playerDmg);

                    if (!monster.IsAlive()) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{monster.Name} was defeated!");
                        Console.ForegroundColor = ConsoleColor.White;
                        player.CurrentRoom.delMonster(monster);
                        Thread.Sleep(2000);
                        Console.Clear();
                        break;
                    }

                    Console.WriteLine($"{monster.Name} attacks {player.Name} for {monster.AttackDmg} DMG.");
                    player.TakeDamage(monster.AttackDmg);

                    if (!player.IsAlive()) {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"{player.Name} was slain by {monster.Name}");
                        Console.WriteLine("\nYou lose!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(2000);
                        Console.ReadLine();
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

