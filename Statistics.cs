﻿using System;
using System.Collections.Generic;
using System.Linq;


public class Statistics
{
	// List for the number of rooms the player has explored during the game's duration:
	private static int ExploredRooms;

	// List for the damage the player has done to monsters during the game's duration:
	private static List<int> DamageDone = new List<int>();

	// List for the damage the player has received during the game's duration:
	private static List<int> DamageTaken = new List<int>();

	// Checks to see how many monster souls were collected with ICollectable:
	private static int MonstersCollected = 0;
	public Statistics()

	{
		// Rooms explored by the player begins at 0 and will increase for every room they enter:
		ExploredRooms = 0;
	}

	public static void RoomsExplored()
	{
		// This will add one to the RoomsExplored list for every room the player enters:
		ExploredRooms += 1;
		return;
	}
	// The damage dealt by the player is stored in a list:
	public static void DoneDamage(int damage)
	{
		// This will store the damage the player has done to monsters:
		DamageDone.Add(damage);
		return;
	}
	public static void TakenDamage(int damage)
	{
		// This will store the damage the player has taken from monsters:
		DamageTaken.Add(damage);
		return;
	}
	// This method is for collecting monster souls:
	public static void CollectedMonster()
		{
			MonstersCollected++;
		}
	// ListSummary, ListNum and ListAverage are all required for the maths that sorts the end-game statistics:
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
	// This is the logic for all of the statistics stored and later displayed at the end of the game:
	public static string GameOverStats()
	{
		string statistics = "";

		// Overall game statistics are displayed to the user. This includes the damage dealt, taken, amount of attacks, average damage, number of rooms and collected monster souls:
		if (ListNum(DamageTaken) > 0 && ListNum(DamageDone) > 0)
		{
			statistics = ($"Statistics:\n\nDamage Dealt:\nYou dealt {ListSummary(DamageDone)} damage.\nYou attacked {ListNum(DamageDone)} times, and your average damage per attack was: {ListAverage(DamageDone)}. \n\n") +
			($"Damage Taken:\nYou took {ListSummary(DamageTaken)} damage.\nYou were attacked {ListNum(DamageTaken)} times, and the average damage to you per attack was: {ListAverage(DamageTaken)}. \n\n") +
			($"Rooms Explored:\nYou explored {ExploredRooms} rooms (Including re-entering rooms).\n\n");
		}
		// "Thanks for playing" is shown as it is an end game screen when these statistics are displayed:
		// If the player does and takes no damage throughout the game's runtime, only the collected souls will be displayed on the statistics screen:
		statistics += ($"Souls Collected:\nYou collected {MonstersCollected} souls from slain foes.\n\nThank you for playing!");
       
		return statistics;
	}
}