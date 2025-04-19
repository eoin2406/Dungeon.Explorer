using System;

public class Statistics
{
	// List for the number of rooms the player has explored during the game's duration:
	private static RoomsExplored;
	// List for the damage the player has done to monsters during the game's duration:
	private static List<int> DamageDone = new List<int>();
	// List for the damage the player has received during the game's duration:
	private static List<int> DamageTaken = new List<int>();
	public Statistics()

		{
		// Rooms explored by the player begins at 0 and will increase for every room they enter:
		RoomsExplored = 0;
	}

	public static void RoomsExplored()
	{
		// This will add one to the RoomsExplored list for every room the player enters:
		RoomsExplored += 1;
		return;
	}
	// The damage dealt by the player is stored in a list:
	public static void DamageDone(int damage)
	{
		// This will store the damage the player has done to monsters:
		DamageDone.Add(damage);
		return;
	}
	public static void DamageTaken(int damage)
	{
		// This will store the damage the player has taken from monsters:
		DamageTaken.Add(damage);
		return;
	}
	private static int ListSummary(List<int> list)
	{
		return list.Sum();
	}
	private static int ListNum(List<int> list)
	{
		return list.Count();
	}
	private static float ListAverage(List<int> list)
	{
		int listsummary = ListSummary(list);
		int listnum = ListNum(list);
		return listsummary / listnum;
	}
	public static string GameOverStats()
	{
		// Overall game statistics are displayed to the user. This includes the damage dealt, taken, amount of attacks, average damage and number of rooms:
		string statistics = ($"You dealt {ListSummary(DamageDone)} damage. You attacked {ListNum(DamageDone)} times, and your average damage per attack was: {ListAverage(DamageDone)}. \n");
        ($"You took {ListSummary(DamageTaken)} damage. You were attacked {ListNum(DamageTaken)} times, and the average damage to you per attack was: {ListAverage(DamageTaken)}. \n");
		($"You explored {RoomsExplored} rooms.");
		return statistics;
	}




}
