using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

/* IDEAS:

    Timed event DONE (Fix timer not working now)
    Save/Load ability
    Add some way to get the key
    Scoring system
    Statistics Class
    Chest turns into mimic
    Campfire heals (random room) (one use only) (item hidden in ashes)
    Mirror room (Exits swapped)
    Locked door (exit) requires key DONE
    Boss battle DONE    
    System sleep DONE
    Random suffix messages DONE
    PrintDelay DONE
    
*/

namespace DungeonExplorer
{
    internal class Game
    {
        private List<Room> Rooms;
        private Player player;
        private Room bossRoom;
        private Room ruins;
        public Game() {
            Rooms = new List<Room>();
            player = new Player("Name", 100);
            InitialiseRooms();
            player.CurrentRoom = Rooms[0];

        }

        private void PrintDelay(string text, int delay) {
            foreach (char c in text) {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.Write("\n");
        }

        List<string> Suffixes = new List<string> {
            " is neatly placed in the corner.",
            " lies at your feet.",
            "! Could be useful.",
            " has been forgotten here.",
            " is left for the taking.",
            " calls you closer",
            " rests on the lap of a fallen soldier.",
            " is strung from the ceiling.",
            "! That should come in handy.",
            " looks as if it was left behind."
        };

        private void InitialiseRooms(){
            // Room names and their descriptions. Each room has an individual, unique description:
            Room cave = new Room("Cave Mouth", "You make your way down through the uneven ground, with a chill down your spine.");
            Room hall = new Room("Hall", "The hall is faintly lit, only by candlelight. You picture how lively this place must have been at some point.");
            Room chamber = new Room("Chamber", "Everything feels so still... as if time itself has paused.");
            Room mirrors = new Room("Room of Mirrors", "You enter a room filled with mirrors, feeling increasingly disoriented.");
            Room treasury = new Room("Treasury", "Precious gems, ancient relics all on display. In the centre of the room, an encased sword rests on a pedastal.");
            Room library = new Room("Library", "You feel a strange sensation upon entering the room. The large bookshelves tower over you.");
            Room cellar = new Room("Cellar", "A damp, musty smell floods the room. With only the light from the ladder hatch above, you fear you aren't alone down here.");
            Room walls = new Room("Crushing walls Corridor", "You let out a sigh of relief. That could've been bad!");
            Room altar = new Room("Altar", "The moonlight illuminates the large stone altar. You feel it calling you...");
            ruins = new Room("Ruins", "Remains of a place once magnificent lay sprawled across the hard ground. Strangely enough, a wooden door remains completely intact. Where could it lead?");
            bossRoom = new Room("Hidden Lair", "Add boss here + functionality");

            // Add navigation. W = West. S = South. N = North. E = East:
            cave.AddExit("W", ruins);
            cave.AddExit("S", hall);
            hall.AddExit("N", cave);
            hall.AddExit("E", chamber);
            hall.AddExit("S",mirrors);
            chamber.AddExit("W", hall);
            mirrors.AddExit("N", hall);
            mirrors.AddExit("W", treasury);
            treasury.AddExit("E", mirrors);
            treasury.AddExit("W", library);
            library.AddExit("E", treasury);
            library.AddExit("S", cellar);
            library.AddExit("N", walls);
            cellar.AddExit("N", library);
            walls.AddExit("S", library);
            walls.AddExit("N", altar);
            altar.AddExit("S", walls);
            altar.AddExit("E", ruins);
            ruins.AddExit("W", altar);
            ruins.AddExit("N", bossRoom);
            ruins.AddExit("E", cave);

            Rooms.Add(cave);
            Rooms.Add(hall);
            Rooms.Add(chamber);
            Rooms.Add(mirrors);
            Rooms.Add(treasury);
            Rooms.Add(library);
            Rooms.Add(cellar);
            Rooms.Add(walls);
            Rooms.Add(altar);
            Rooms.Add(ruins);
            Rooms.Add(bossRoom);

            treasury.AddMonster(new Mimic());
            bossRoom.AddMonster(new Minotaur());

            // Randomly assign a room to each monster:
            List<Monster> monsters = new List<Monster> {
                new Dragon(),
                new Orc(),
                new Goblin(),
                new Vampire(),
                new Skeleton(),
              //new Mimic(),

            };

            Random random = new Random();
            foreach (var monster in monsters) {
                int roomNum = random.Next(Rooms.Count);
                Rooms[roomNum].AddMonster(monster);
            }

            // These are the weapons and the potions. They all have their own name, description and number for how much damage they deal:
            Weapon sword = new Weapon("Broadsword", "A plain broadsword", 12);
            Weapon club = new Weapon("Club", "An old club", 17);
            Weapon stick = new Weapon("Stick", "Just a stick", 1);
            Weapon bow = new Weapon("Bow", "A sturdy longbow", 24);
            Weapon longsword = new Weapon("Longsword", "A mighty longsword", 25);

            Potion small = new Potion("Lesser Healing", "Heals 5 HP", 5);
            Potion medium = new Potion("Medium Healing", "Heals 10 HP", 10);
            Potion large = new Potion("Greater Healing", "Heals 20 HP", 20);

            cave.SetItems(new List<Item> {
                stick,
                small,
            });
            chamber.SetItems(new List<Item> {
                club,
            });
            ruins.SetItems(new List<Item> {
                sword,
            });
            cellar.SetItems(new List<Item> {
                bow,
                medium
            });

            Misc key = new Misc("Mysterious Key", "I wonder what this is used for?");
            player.AddMisc(key);
            Weapon fists = new Weapon("Fists", "Your fists", 1);
            player.AddWeapon(fists);
        }

        private void PrintMonsters(List<Monster> monsters) {
            if (monsters == null || monsters.Count == 0) {
                return;
            }
            foreach (var monster in monsters) {
                if (monster != null) {
                    string monsterName = monster.Name;
                    if(!string.IsNullOrEmpty(monsterName)) {
                        Console.WriteLine(monster.GetMonsterNoise());
                    } else {
                        Console.WriteLine("DEBUG: MonsterName empty.");
                    }
                }
            }
        }

        private void PrintWeapons(List<Weapon> weapons) {
            if (weapons == null || weapons.Count == 0) {
                return;
            }
            Random random = new Random();

            foreach (var weapon in weapons) {
                string randomSuffix = Suffixes[random.Next(Suffixes.Count)];
                Console.WriteLine($"A {weapon.Name}{randomSuffix}");
            }
        }
        
        private void PrintPotions(List<Potion> potions) {
            if (potions == null || potions.Count == 0) {
                return;
            }


            foreach (var potion in potions) {
                Console.WriteLine($"You can see a potion of {potion.Name}.");
            }
        }

        private void PrintExits(Dictionary<string, Room> exits) {
            Console.Write("Exits are visible ");

            var keys = exits.Keys.ToList();
            
            if (exits.Count == 1) {
                Console.WriteLine($"to the {keys[0]}");
            } else if (exits.Count == 2) {
                Console.WriteLine($"to the {keys[0]} and {keys[1]}");
            } else {
                for (var i = 0; i < keys.Count - 1; i++) {
                    Console.Write($"to the {keys[i]}, ");
                }
                Console.Write($"and {keys[keys.Count - 1]}\n");
            }
        }
        private void HandleWallEvent(Room room)
        {
            // This is a random quicktime event in one of the rooms. If you fail the challenge, the walls close in on the player and they lose:
            Console.Clear();
            Console.WriteLine("Quick! You must disable the mechanism forcing the walls to close in on you!");
            Console.WriteLine("Type the alphabet as fast as you can to survive!");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Press ENTER to start quicktime event.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Quicktime event: Type the alphabet (no spaces, all lowercase)");

            bool isTimeUp = false;
            DateTime startTime = DateTime.Now;
            DateTime endTime = startTime.AddSeconds(10); 

            while (!isTimeUp && DateTime.Now < endTime)
            {
                if (Console.KeyAvailable)
                    {string quicktimeInput = Console.ReadLine()?.Trim();

                    if (quicktimeInput == "abcdefghijklmnopqrstuvwxyz")
                    {
                        isTimeUp = true;
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        // This string is printed if the player completes the quicktime event:
                        Console.WriteLine("The walls come to a screeching halt. You did it!");
                        Thread.Sleep(5000);
                        Console.ForegroundColor = ConsoleColor.White;
                        room.EventTriggered = true;
                        break;
                    }}
            }

            if (!isTimeUp)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Clear();
                // This string is printed if the player fails the quicktime event:
                Console.WriteLine("The walls crush you...");
                Console.WriteLine($"\n\n {player.Name} was slain!");
                Thread.Sleep(5000);
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
        
        private void handleMimicEvent(Room room) {
            Console.WriteLine("Test/Placholder. TO DO!"); /////////////NEEEEEEEEEEEEEEEEEEEEDS WORK
        }

        private void handleKeyEvent(Room room) {
            bossRoom.EventTriggered = true;
            player.SetCurrentRoom(bossRoom);
            Console.Clear();
            PrintDelay("...", 1000);
            PrintDelay($"{player.Name} presents the mysterious key to the door...", 1);
            PrintDelay("...", 1000);
            PrintDelay("The door swings open, and a strong force pulls you inside...", 1);
            Thread.Sleep(1000);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            PrintDelay("The room shakes as a loud roar erupts from the creature's mouth.", 1);
            Thread.Sleep(1000);
            PrintDelay("You question if this was the right decision...", 1);
            Thread.Sleep(1000);

            var monsters = player.CurrentRoom.Monsters;

            player.Combat(monsters, player);

        }


        public void Start() {

            // This text is printed at the beginning of the game. The program asks the player for their name:
            PrintDelay("Before you embark on the journey of a lifetime, please tell me your name: ", 1);
            string userName = Console.ReadLine();
            // Debug.Assert is used to see if the player's name is more than 0 characters. If it is, an error message will be displayed to the player and the game will restart from the beginning again:
            Test.TestForPlayerNameLength(userName);
            if (userName.Length == 0)
            {
                Start();
                return;
            }
            player.Name = userName;
            Console.Clear();
            // The player has chosen their name. The game has started and the introduction begins to play:
            PrintDelay($"{userName} began their epic adventure through the deep dungeons...\n\n", 1);
            // A list of player commands is displayed to the user to show them their available options throughout the game's duration:
            PrintDelay($"Commands:\n\"attack\" to use your weapon\n\"heal\" to increase your HP\n\"N\", \"S\", \"E\", \"W\" to navigate through rooms\n\"inv\" to view your inventory\n\"pick\" to collect items\n\"use\" to use items", 1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            // The user can type anything, or leave the space empty. They must press enter to begin the game. This is so the game does not begin automatically and allows the user a moment of time to understand the player commands:
            PrintDelay("\nType anything and click ENTER to begin.", 1);
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            
            bool GameInProgress = true;
            while (GameInProgress) {
                // Wall crushing event check
                if (player.CurrentRoom.Name == "Crushing walls Corridor" && !player.CurrentRoom.EventTriggered) {
                    HandleWallEvent(player.CurrentRoom);
                }

                if (player.CurrentRoom.Name == "Treasury" && !player.CurrentRoom.EventTriggered) {
                    handleMimicEvent(player.CurrentRoom);
                }

                if (player.CurrentRoom.Name == "Hidden Lair" && bossRoom.EventTriggered == false) {
                    PrintDelay($"{player.Name} attempts to force the door open, to no avail.", 1);
                    PrintDelay("There must be some other way to get through.", 1);
                    Console.Clear();
                    Thread.Sleep(2000);
                    player.SetCurrentRoom(ruins);
                }

                // Display current room information:        
                Console.WriteLine($"{player.Name} ({player.GetHealth()} HP) is in a {player.CurrentRoom.Name}.");
                Console.WriteLine($"{player.CurrentRoom.Description}");
                PrintExits(player.CurrentRoom.GetExits());
                var monsters = player.CurrentRoom.Monsters;
                var roomWeapons = player.CurrentRoom.GetItems().OfType<Weapon>().ToList();
                var roomPotions = player.CurrentRoom.GetItems().OfType<Potion>().ToList();
                PrintMonsters(monsters);
                PrintWeapons(roomWeapons);
                PrintPotions(roomPotions);

                // User input:
                string input = ExplorerInput();

                // Corresponding action to decision. This for navigating the rooms in directions:
                if (input == "n" || input == "s" || input == "w" || input == "e") {
                    var exits = player.CurrentRoom.GetExits();
                    if (exits.ContainsKey(input.ToUpper())) {
                        player.CurrentRoom = exits[input.ToUpper()];
                        Console.Clear();
                        continue;
                    }
                } else if (input == "attack") {
                    if (monsters.Count == 0) {
                     // If there are no monsters in a room, the player is unable to attack, and this string is printed:
                        PrintDelay("You can't attack; there are no monsters here.", 1);
                    } else {
                        player.Combat(monsters, player);
                    }
                } else if (input == "inv") {
                    var weapons = player.GetWeapons();
                    var potions = player.GetPotions();
                    var miscs = player.GetMiscs();
                    Console.WriteLine("Type \"weapons\" to view your weapons, \"potions\" to view your potions, or \"misc\" to view miscellaneous items:");
                    string choice = Console.ReadLine();
                    if (choice == "weapons") { 
                        Console.WriteLine("=================================");
                        Console.WriteLine("Weapons in Inventory: ");
                        if (weapons.Count == 0) {
                            Console.WriteLine("None.");
                        } else {
                            foreach (var weapon in weapons){
                                //Console.WriteLine($"- {weapon.Name}: {weapon.AttackDmg} DMG");
                                Console.WriteLine($"- {weapon.GetSummary()}");
                            }
                            Console.Write("\n");
                        }

                    } else if (choice == "potions") {
                        Console.WriteLine("=================================");
                        Console.WriteLine("Potions in Inventory: ");
                        if (potions.Count == 0) {
                            Console.WriteLine("None.");
                        } else {
                            foreach (var potion in potions) {
                                //Console.WriteLine($"- {potion.Name}: {potion.HealingFactor} HP");
                                Console.WriteLine($"- {potion.GetSummary()}");
                            }
                            Console.Write("\n");
                        }
                    } else if (choice == "misc") {
                        Console.WriteLine("=================================");
                        Console.WriteLine("Items in Inventory: ");
                        if (miscs.Count == 0) {
                            Console.WriteLine("None.");
                        } else {
                            foreach (var misc in miscs) {
                                Console.WriteLine($"- {misc.Name}");
                            }
                        }
                        Console.WriteLine("=================================");
                    }

                } else if (input == "pick") {
                    var currentItems = player.CurrentRoom.GetItems();
                    if (currentItems.Count == 0) {
                        // If there are no items in the room and the player attempts to collect an item, this string will be printed:
                        PrintDelay("You scramble around the room in attempt to find something, but there's nothing there.", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                    } else {
                        // This string is printed if the player uses the "pick" command and collects items in a room:
                        PrintDelay("You gather everything you can see, in hope that it comes in handy.", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                        foreach (var potion in roomPotions) {
                            player.AddPotion(potion);
                            currentItems.Remove(potion);
                        }
                        foreach (var weapon in roomWeapons) {
                            player.AddWeapon(weapon);
                            currentItems.Remove(weapon);
                        }
                        player.CurrentRoom.SetItems(currentItems);
                    }

                } else if (input == "heal") {
                    var currentItems = player.GetInventory();
                    if (currentItems.OfType<Potion>().Count() == 0) {
                        // If the player has no potions and uses the "heal" command, this string will be printed:
                        PrintDelay("You reach for your potions, only to find you have none.", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                    } else {
                        var potions = player.GetPotions();
                        foreach (var potion in potions) {
                            player.drinkPotion(potion);
                            currentItems.Remove(potion);
                        }
                        player.SetInventory(currentItems);
                        // This string is printed if the player uses the "heal" command with potion/s in their inventory:
                        PrintDelay("You drink all of your potions. You feel pumped!", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                } else if (input == "use") {
                    var miscs = player.GetMiscs();
                    Console.WriteLine("What item do you want to use?");
                    Console.WriteLine("=================================");
                        Console.WriteLine("Items in Inventory: ");
                        if (miscs.Count == 0) {
                            Console.WriteLine("None.");
                        } else {
                            foreach (var misc in miscs) {
                                Console.WriteLine($"- {misc.Name}");
                            }
                        }
                        Console.WriteLine("=================================");
                        string useInput = Console.ReadLine();
                        if (useInput == "key" && player.CurrentRoom.Name == "Ruins") {
                            if (player.GetWeapons().Count == 0) {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($"{player.Name} feels dreadfully unprepared. They step back and look elsewhere.");
                                Console.ForegroundColor = ConsoleColor.White;
                            } else {
                                handleKeyEvent(player.CurrentRoom);
                            }
                        }
                    }


                if (input == "quit") {
                    GameInProgress = false;
                }
            }
        }

        private string ExplorerInput()
        {
            // Below are all of the valid inputs that the player can use throughout the duration of the game:
            string[] validInputs = {"inv", "pick", "heal", "attack", "n", "s", "e", "w", "quit", "use"};

            while (true) {
                // This string is printed when the game is awaiting user input:
                Console.WriteLine("\nWhat do you do?");
                string input = Console.ReadLine()?.ToLower().Trim();

                if (Array.Exists(validInputs, cmd => cmd == input)) {
                    return input;
                }

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                // Error-handling to make sure the game continues gracefully if the user enters an invalid input:
                Console.WriteLine("Invalid command. Try again.");
                Console.ForegroundColor = ConsoleColor.White;

            }
        }
    }
}