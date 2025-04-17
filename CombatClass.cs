namespace DungeonExplorer {
    public static class CombatClass {
        public static void Start(Player player, Monster monster) {
            Console.WriteLine($"{player.Name} vs {monster.Name}");

            while (player.IsAlive() && monster.IsAlive()) {
                if (monster.GoesFirst) {
                    int monsterDmg = monster.AttackDmg;
                    Console.WriteLine($"{monster.Name} attacks {player.Name} for {monsterDmg} DMG.");
                    player.TakeDamage(monsterDmg);

                    if (!player.IsAlive()) {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"{player.Name} was slain by {monster.Name}");
                        Console.WriteLine("\nYou lose!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(2000);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }

                    int playerDmg = player.getStrongestWeapon().GetAttackDmg();
                    Console.WriteLine($"{player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                    monster.TakeDamage(playerDmg);

                    if (!monster.IsAlive()) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{monster.Name} was defeated!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                } else {
                    int playerDmg = player.getStrongestWeapon().GetAttackDmg();
                    Console.WriteLine($"{player.Name} attacks {monster.Name} for {playerDmg} DMG.");
                    monster.TakeDamage(playerDmg);

                    if (!monster.IsAlive()) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"{monster.Name} was defeated!");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }

                    int monsterDmg = monster.AttackDmg;
                    Console.WriteLine($"{monster.Name} attacks {player.Name} for {monsterDmg} DMG.");
                    player.TakeDamage(monsterDmg);

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

                Console.WriteLine($"{player.Name}: {player.getHealth()} HP | {monster.Name}: {monster.GetHealth()} HP");
            }
        }
    }
}
