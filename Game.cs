using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Media;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

/* IDEAS:

    Timed event DONE (Fix timer not working now)
    Save/Load ability
    Add some way to get the key
    Scoring system
    Statistics Class DONE
    Chest turns into mimic
    Campfire heals (random room) (one use only) (item hidden in ashes)
    Mirror room (Exits swapped)
    Locked door (exit) requires key DONE
    Boss battle DONE    
    System sleep DONE
    Random suffix messages DONE
    PrintDelay DONE
NEED TO FIX IT SO ALL MONSTERS DROP SOULS
NEED TO FIX IT SO WHEN SOULS ARE COLLECTED THE MONSTER IS REMOVED FROM THE ROOM
NEED TO MAKE IT NOT SHOW THE 0HP OF THE MONMNSTER WHEN YOU DEFEAT IT AND COLLECT SOUL
MAKE STATISTICS COME UP WITHOUT ANY AWKWARDNESS WHEN YOU LOSE THE GAME
HIDDEN LAIR BROKE
    
*/

namespace DungeonExplorer
{
    internal class Game
    {
        private List<Room> Rooms;
        private Player player;
        private Room bossRoom;
        private Room ruins;
        public Game()
        {
            Rooms = new List<Room>();
            player = new Player("Name", 150);
            InitialiseRooms();
            player.CurrentRoom = Rooms[0];

        }

        private void PrintDelay(string text, int delay)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.Write("\n");
        }

        List<string> Suffixes = new List<string>
        {
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

        private void InitialiseRooms()
        {
            // Room names and their descriptions. Each room has an individual, unique description:
            Room cave = new Room("a Cave Mouth", "You make your way down through the uneven ground, a horrible chill goes down your spine.");
            Room hall = new Room("the Great Hall", "The hall is faintly lit by candlelight. You imagine how lively this place must have been in the past.");
            Room chamber = new Room("a Chamber", "Everything feels so still... as if time itself has paused.");
            Room mirrors = new Room("the Room of Mirrors", "You enter a room filled with mirrors. You begin to feel very disoriented.");
            Room treasury = new Room("the Treasury", "Precious gems, ancient relics are everywhere. In the centre of the room, a large chest rests on a pedestal.");
            Room library = new Room("the Library", "You feel a strange sensation upon entering the room. The large bookshelves tower over you.");
            Room cellar = new Room("a Cellar", "A damp, musty smell floods the room. The only the light to guide you comes from the ladder hatch above.");
            Room walls = new Room("the Crushing walls Corridor", "The walls are open. This room is now safe!");
            Room altar = new Room("an Altar", "The moonlight illuminates the large stone altar. You feel it calling you...");
            ruins = new Room("a Ruin", "Remains of a place once magnificent lay sprawled across the hard ground. A grand spruce door stands completely intact.\n\n>>> The door requires a key to open, perhaps it is hidden somewhere in the dungeon...\n\n>>> (Try \"use\" if you find the key!)");
            bossRoom = new Room("a Hidden Lair", "Add boss here + functionality");

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

            // Only the Mimic spawns in the treasury room. The Mimic drops the key to the boss room:
            Mimic mimic = new Mimic(player);
            treasury.AddMonster(mimic);

            bossRoom.AddMonster(new Minotaur());

            // Randomly assign a room to each monster (apart from the mimic and minotaur as they are needed for the key for the boss door and boss room:
            List<Monster> monsters = new List<Monster>
            {
                new Dragon(),
                new Spider(),
                new Goblin(),
                new Vampire(),
                new Skeleton(),
                new Hound(),

            };

            Random random = new Random();
            foreach (var monster in monsters)
            {
                List<Room> availableRooms = Rooms.Where(room => room != treasury && room != bossRoom && room != walls).ToList();
                int roomNum = random.Next(availableRooms.Count);
                availableRooms[roomNum].AddMonster(monster);
            }

            // These are the weapons and the potions. They all have their own name, description and number for how much damage they deal:
            Weapon sword = new Weapon("Broadsword", "A plain broadsword", 18);
            Weapon club = new Weapon("Club", "An old club", 20);
            Weapon stick = new Weapon("Stick", "Just a stick", 10);
            Weapon bow = new Weapon("Bow", "A sturdy longbow", 24);
            Weapon longsword = new Weapon("Longsword", "A mighty longsword", 27);

            Potion small = new Potion("Lesser Healing", "Heals 15 HP", 15);
            Potion medium = new Potion("Medium Healing", "Heals 30 HP", 30);
            Potion large = new Potion("Greater Healing", "Heals 40 HP", 40);

            cave.SetItems(new List<Item>
            {
                stick,
                small,
            });
            chamber.SetItems(new List<Item>
            {
                club,
            });
            ruins.SetItems(new List<Item>
            {
                sword,
            });
            cellar.SetItems(new List<Item>
            {
                bow,
                medium
            });
            // Player begins with fists. This is so they can attack monsters without collecting a weapon - preventing any possible errors:
                Weapon fists = new Weapon("Fists", "Your fists", 5);
            player.AddWeapon(fists);
        }
        // This prints the monsters found in the current room:
        private void PrintMonsters(List<Monster> monsters)
        {
            // When no monsters are in a room:
            if (monsters == null || monsters.Count == 0)
            {
                Console.WriteLine("The room feels still. You cannot hear any monsters...\n");
                return;
            }
            foreach (var monster in monsters)
            {
                if (monster != null)
                {
                    string monsterName = monster.Name;
                    if(!string.IsNullOrEmpty(monsterName))
                    {
                        Console.WriteLine(monster.GetMonsterNoise());
                    }
                    else
                    {
                        Console.WriteLine("ERROR: MonsterName empty.");
                    }
                }
            }
        }
        // This prints the weapons found within the current room. It uses a random suffix at the end to make the game more interesting:
        private void PrintWeapons(List<Weapon> weapons)
        {
            if (weapons == null || weapons.Count == 0)
            {
                return;
            }
            Random random = new Random();

            foreach (var weapon in weapons)
            {
                string randomSuffix = Suffixes[random.Next(Suffixes.Count)];
                Console.WriteLine($"> A {weapon.Name}{randomSuffix}");
            }
        }
        // This prints the potions found within the current room:
        private void PrintPotions(List<Potion> potions)
        {
            if (potions == null || potions.Count == 0)
            {
                return;
            }
            foreach (var potion in potions)
            {
                Console.WriteLine($"> You can see a potion of {potion.Name}.");
            }
        }

        // This prints the exits that can be found within the current room:
        private void PrintExits(Dictionary<string, Room> exits)
        {
            Console.Write("\n- Exits are visible ");

            var keys = exits.Keys.ToList();
            
            if (exits.Count == 1)
            {
                Console.WriteLine($"to the {keys[0]}.");
            }
            else if (exits.Count == 2)
            {
                Console.WriteLine($"to the {keys[0]} and {keys[1]}.");
            }
            else
            {
                for (var i = 0; i < keys.Count - 1; i++)
                {
                    Console.Write($"to the {keys[i]}, ");
                }
                Console.Write($"and {keys[keys.Count - 1]}.\n");
            }
        }
        private void HandleWallEvent(Room room)
        {
            int timerLine = 5;
            // This is a random quicktime event in one of the rooms. If you fail the challenge, the walls close in on the player and they lose:
            Console.Clear();
            Console.Write("Quick! You must disable the mechanism that is forcing the walls to close in on you!\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("(Type the alphabet as fast as you can to disable the mechanism)");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("\nPress ENTER to start the quicktime event...");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Type the alphabet (lowercase)");

            bool isTimeUp = false;
            DateTime startTime = DateTime.Now;
            // The player has 15 seconds to complete the quicktime event:
            DateTime endTime = startTime.AddSeconds(15);

            // This is the correct string the user should input to pass the quicktime event:
            string correctInput = "abcdefghijklmnopqrstuvwxyz";
            // This will allow the user's player input to be stored whilst they are typing:
            string userInput = "";

            int timerLineY = Console.CursorTop;
            int typingLineY = timerLineY + 1;
            // This will handle the timer, once the quicktime event is over, the timer will disappear:
            bool timerCompleted = false;

            Task.Run(() =>
            {
                while (DateTime.Now < endTime && !isTimeUp)
                {
                    TimeSpan timeLeft = endTime - DateTime.Now;

                    Console.SetCursorPosition(0, timerLineY);
                    Console.Write(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, timerLineY);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"The walls close in: {timeLeft.Seconds} seconds...");
                    Console.ForegroundColor = ConsoleColor.White;
                    // Using Thread.Sleep, I can make the timer update every second:
                    Thread.Sleep(1000);

                    if (timeLeft.TotalSeconds <= 0 || isTimeUp) break;
                }
                // Once the time is up, the timer will be flagged as completed:
                timerCompleted = true;
            });
            while (DateTime.Now < endTime && !isTimeUp)
            {
                if(Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).KeyChar;

                    // If the key entered is a letter, it will be added to the player's input on-screen:
                    if (char.IsLetter(key))
                    {
                        userInput += key.ToString().ToLower();
                        Console.SetCursorPosition(0, typingLineY);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition(0, typingLineY);
                        Console.Write(userInput);
                    }
                    else if (key == 8 && userInput.Length > 0)
                    {
                        userInput = userInput.Substring(0, userInput.Length - 1);

                        Console.SetCursorPosition(0, typingLineY);
                        Console.Write(new string(' ', Console.WindowWidth));
                        Console.SetCursorPosition (0, typingLineY);
                        Console.Write(userInput);
                    }
                    if (userInput == correctInput)
                    {
                        isTimeUp = true;
                        break;
                    }    
                }
            }
            // If the player fails the quicktime event:
            if (!isTimeUp || userInput != correctInput)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Clear();
                PrintDelay($"The walls close in and the brave adventurer {player.Name} is crushed...", 1);
                Thread.Sleep(3000);
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Game Over!\n\nType anything and click ENTER to view your statistics");
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine(Statistics.GameOverStats());
                Environment.Exit(0);
            }
            // If the player completes the quicktime event:
            Console.Clear();
            Console.ForegroundColor= ConsoleColor.Green;
            PrintDelay("The walls come to a screeching halt. You did it!", 2);
            Thread.Sleep(3000);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            room.EventTriggered = true;
        }

        private void handleMimicEvent(Room room)
        {
        }

        private void handleKeyEvent(Room room)
        {
            room.EventTriggered = true;
            Console.Clear();
            PrintDelay("...", 1000);
            PrintDelay($"{player.Name} presents the mysterious key to the door...", 1);
            PrintDelay("...", 1000);
            PrintDelay("The door swings open, and a strong force pulls you inside...", 1);
            Thread.Sleep(1000);

            // Move player to boss room!
            player.SetCurrentRoom(bossRoom);

            // Ensure bossRoom has only the Minotaur
            bossRoom.Monsters.Clear();
            bossRoom.AddMonster(new Minotaur());

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            PrintDelay("The room shakes as a loud roar erupts from the creature's mouth.", 1);
            Thread.Sleep(1000);
            PrintDelay("You question if this was the right decision...", 1);
            Thread.Sleep(2000);

            var monsters = player.CurrentRoom.Monsters;

            PrintMonsters(monsters);

            // If the player beats the boss room, this is displayed:
            player.Combat(monsters, player);
            if (player.IsAlive())
            {
                Console.Clear();
                Console.WriteLine("You bested the dungeons and defeated the Minotaur.\n\nYou win!");
                Thread.Sleep(2000);
                Console.WriteLine("Type anything and click ENTER to view your statistics");
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine(Statistics.GameOverStats());
                
                Environment.Exit(0);
            }
        }



        public void Start()
        {
            // This text is printed at the beginning of the game. The program asks the player for their name:
            PrintDelay("Explorer, before you attempt to explore the dungeons, you must tell me your name: ", 1);
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
            PrintDelay($"I see, your name is {userName}!\nGood luck, {userName}. You will need it...\n", 1);
            // A list of player commands is displayed to the user to show them their available options:
            PrintDelay($"============== User Commands: ===============\n\n\"attack\" to use your weapon\n\"heal\" to increase your HP\n\"N\", \"S\", \"E\", \"W\" to navigate through rooms\n\"inv\" to view your inventory\n\"pick\" to collect items\n\"use\" to use items\n\"help\" to display a list of commands\n\n=============================================", 1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            // The user can type anything, or leave the space empty. They must press enter to begin the game. This is so the game does not begin automatically and allows the user a moment of time to understand the player commands:
            PrintDelay("\nType anything and click ENTER to begin...", 1);
            Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
            
            bool GameInProgress = true;
            while (GameInProgress)
            {
                // Checks if the player is in the crushing walls corridor:
                if (player.CurrentRoom.Name == "the Crushing walls Corridor" && !player.CurrentRoom.EventTriggered)
                {
                    // Runs the event if they are:
                    HandleWallEvent(player.CurrentRoom);
                }

                if (player.CurrentRoom.Name == "the Treasury" && !player.CurrentRoom.EventTriggered)
                {
                    handleMimicEvent(player.CurrentRoom);
                }
                if (player.CurrentRoom.Name == "a Hidden Lair" && bossRoom.EventTriggered == false)
                {
                    PrintDelay($"{player.Name} attempts to force the door open, to no avail.", 1);
                    PrintDelay("There must be some other way to get through.", 1);
                    Thread.Sleep(2000);
                    Console.Clear();
                    player.SetCurrentRoom(ruins);
                }

                // Display player name, HP, current room, any monsters found within the room, as well as any items:
                Console.Write($"{player.Name} ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"({player.GetHealth()} HP) ");
                Console.ResetColor();
                Console.WriteLine($"is in {player.CurrentRoom.Name}:");
                Console.WriteLine($"{player.CurrentRoom.Description}");
                PrintExits(player.CurrentRoom.GetExits());
                Console.WriteLine();
                var monsters = player.CurrentRoom.Monsters;
                var roomWeapons = player.CurrentRoom.GetItems().OfType<Weapon>().ToList();
                var roomPotions = player.CurrentRoom.GetItems().OfType<Potion>().ToList();
                PrintMonsters(monsters);
                PrintWeapons(roomWeapons);
                PrintPotions(roomPotions);
                // A string that tells you to enter "help" for a list of user commands. This is to prevent any confusion for the player:
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\n(Type \"help\" to view a list of user commands)\n");
                Console.ResetColor();

                // User input:
                string input = ExplorerInput();

                // Corresponding action to decision. This for navigating the rooms in directions:
                if (input == "n" || input == "s" || input == "w" || input == "e")
                {
                    var exits = player.CurrentRoom.GetExits();
                    if (exits.ContainsKey(input.ToUpper()))
                    {
                        player.CurrentRoom = exits[input.ToUpper()];
                        Statistics.RoomsExplored();
                        Console.Clear();
                        continue;
                    }
                    else
                    {
                        PrintDelay("\nThere is no exit in that direction...", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                }
                else if (input == "attack")
                {
                    if (monsters.Count == 0)
                    {
                     // If there are no monsters in a room, the player is unable to attack, and this string is printed:
                        PrintDelay("You can't attack; there are no monsters here.", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                    else
                    {
                        player.Combat(monsters, player);
                    }
                }
                else if (input == "inv")
                {
                    Console.Clear();
                    Console.WriteLine("============== Inventory: ==============\n\nType \"weapons\" to view your weapons.\nType \"potions\" to view your potions.\nType \"misc\" to view miscellaneous items.\n\n========================================");
                    var weapons = player.GetWeapons();
                    var potions = player.GetPotions();
                    var miscs = player.GetMiscs();
                    string choice = Console.ReadLine();
                    if (choice == "weapons")
                    {
                        Console.Clear();
                        Console.WriteLine("======================= Weapons: =======================\n");
                        if (weapons.Count == 0)
                        {
                            Console.WriteLine("None.");
                        }
                        else
                        {
                            // OrderBy LINQ is used here to sort the weapons in the inventory by weakest to strongest:
                            var sortedWeapons = weapons.OrderBy(weapon => weapon.GetAttackDmg()).ToList();

                            foreach (var weapon in sortedWeapons)
                            {
                                Console.WriteLine($"- {weapon.GetSummary()}");
                            }
                            Console.Write("\n========================================================\n\n");
                        }
                    }
                    else if (choice == "potions")
                    {
                        Console.Clear();
                        Console.WriteLine("======================= Potions: =======================\n");
                        if (potions == null || potions.Count == 0)
                        {
                            Console.WriteLine("You currently have no potions in your inventory...\n");
                            Console.WriteLine("\n========================================================\n");
                        }
                        else
                        {
                            foreach (var potion in potions)
                            {
                                //Console.WriteLine($"- {potion.Name}: {potion.HealingFactor} HP");
                                Console.WriteLine($"- {potion.GetSummary()}");
                            }
                            Console.Write("\n========================================================\n\n");
                        }
                    }
                    else if (choice == "misc")
                    {
                        Console.Clear();
                        Console.WriteLine("======================= Miscellaneous: =======================\n");
                        if (miscs.Count == 0)
                        {
                            Console.WriteLine("None.");
                        }
                        else
                        {
                            foreach (var misc in miscs)
                            {
                             Console.WriteLine($"- {misc.Name}");
                            }
                        }
                        Console.WriteLine("\n==============================================================\n");
                    }

                }
                else if (input == "pick")
                {
                    var currentItems = player.CurrentRoom.GetItems();
                    if (currentItems.Count == 0)
                    {
                        // If there are no items in the room and the player attempts to collect an item, this string will be printed:
                        PrintDelay("\nYou scramble around the room in attempt to find something, but there's nothing there.", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                    else
                    {
                        // This string is printed if the player uses the "pick" command and collects items in a room:
                        PrintDelay("\nYou gather everything you can see, in hope that it comes in handy...", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                        foreach (var potion in roomPotions)
                        {
                            player.AddPotion(potion);
                            currentItems.Remove(potion);
                        }
                        foreach (var weapon in roomWeapons)
                        {
                            player.AddWeapon(weapon);
                            currentItems.Remove(weapon);
                        }
                        player.CurrentRoom.SetItems(currentItems);
                    }

                }
                else if (input == "heal")
                {
                    var currentItems = player.GetInventory();
                    if (currentItems.OfType<Potion>().Count() == 0)
                    {
                        // If the player has no potions and uses the "heal" command, this string will be printed:
                        PrintDelay("\nYou reach for your potions, only to find you have none.", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                    else
                    {
                        var potions = player.GetPotions();
                        foreach (var potion in potions)
                        {
                            player.drinkPotion(potion);
                            currentItems.Remove(potion);
                        }
                        player.SetInventory(currentItems);
                        // This string is printed if the player uses the "heal" command with potion/s in their inventory:
                        PrintDelay("\nYou drink all of your potions. You feel pumped!", 1);
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                }
                // Displays help menu:
                else if (input == "help")
                    {
                    GetHelp();
                    }
                // Asks the player what item they want to use from their inventory:
                else if (input == "use")
                {
                    var miscs = player.GetMiscs();
                        Console.WriteLine("What item do you want to use?");
                        Console.WriteLine("=================================");
                        Console.WriteLine("Items in Inventory: ");
                    if (miscs.Count == 0)
                    {
                        Console.WriteLine("None.");
                    }
                    else
                    {
                        foreach (var misc in miscs)
                        {
                            Console.WriteLine($"- {misc.Name}");
                        }
                    }
                        Console.WriteLine("=================================");
                        string useInput = Console.ReadLine();
                    if (useInput == "key" && player.CurrentRoom.Name == "a Ruin")
                    {
                        if (player.GetWeapons().Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"{player.Name} feels dreadfully unprepared. They step back and look elsewhere.");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                        handleKeyEvent(player.CurrentRoom);
                        }
                    }
                }

                if (input == "quit")
                {
                    Console.WriteLine(Statistics.GameOverStats());
                    GameInProgress = false;
                }
            }
        }
        // This allows the player to see all of the valid user inputs:
        private void GetHelp()
        {
            Console.Clear();
           Console.WriteLine("============= User Commands: ===============\n\n\"attack\" to use your weapon\n\"heal\" to increase your HP\n\"N\", \"S\", \"E\", \"W\" to navigate through rooms\n\"inv\" to view your inventory\n\"pick\" to collect items\n\"use\" to use items\n\"help\" to display a list of commands\n\n============================================\n", 1);
        }
        private string ExplorerInput()
        {
            // Below are all of the valid inputs that the player can use throughout the duration of the game:
            string[] validInputs = {"inv", "pick", "heal", "attack", "n", "s", "e", "w", "quit", "use", "help"};

            while (true)
            {
                // This string is printed when the game is awaiting user input:
                Console.WriteLine("\nWhat do you do?");
                string input = Console.ReadLine()?.ToLower().Trim();

                if (Array.Exists(validInputs, cmd => cmd == input))
                {
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