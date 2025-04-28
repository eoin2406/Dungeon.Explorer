using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    public class Room
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Dictionary<string, Room> Exits { get; set; }
        public List<Monster> Monsters { get; set; }
        public List<Item> Items { get; set; }
        public bool EventTriggered { get; set; }
        public bool Locked { get; set; }

        public Room(string name, string description)
        {
            Name = name;
            Description = description;
            Exits = new Dictionary<string, Room>();
            Monsters = new List<Monster>();
            Items = new List<Item>();
            EventTriggered = false;
            Locked = true;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void AddExit(string direction, Room room)
        {
            Exits[direction] = room;
        }
        public Dictionary<string, Room> GetExits()
        {
            return Exits;
        }
        public void AddMonster(Monster monster)
        {
            Monsters.Add(monster);
        }

        public void delMonster(Monster monster)
        {
            Monsters.Remove(monster);
        }

        public string GetDescription()
        {
            return Description;
        }

        public void SetItems(List<Item> items)
        {
            Items = items;
        }

        public List<Item> GetItems()
        {
            return Items;
        }
    }
}