using Microsoft.EntityFrameworkCore;
using W9_assignment_template.Data;
using W9_assignment_template.Models;

namespace W9_assignment_template.Services;

public class GameEngine
{
    private readonly GameContext _context;

    public GameEngine(GameContext context)
    {
        _context = context;
    }

    public void DisplayRooms()
    {
        var rooms = _context.Rooms.Include(r => r.Characters).ToList();

        foreach (var room in rooms)
        {
            Console.WriteLine($"Room: {room.Name} - {room.Description}");
            foreach (var character in room.Characters)
            {
                Console.WriteLine($"    Character: {character.Name}, Level: {character.Level}");
            }
        }
    }

    public void DisplayCharacters()
    {
        var characters = _context.Characters.ToList();
        if (characters.Any())
        {
            Console.WriteLine("\nCharacters:");
            foreach (var character in characters)
            {
                Console.WriteLine($"Character ID: {character.Id}, Name: {character.Name}, Level: {character.Level}, Room ID: {character.RoomId}");
            }
        }
        else
        {
            Console.WriteLine("No characters available.");
        }
    }

    public void AddRoom()
    {
        Console.Write("Enter room name: ");
        var name = Console.ReadLine();

        Console.Write("Enter room description: ");
        var description = Console.ReadLine();

        var room = new Room
        {
            Name = name,
            Description = description
        };

        _context.Rooms.Add(room);
        _context.SaveChanges();

        Console.WriteLine($"Room '{name}' added to the game.");
    }

    public void AddCharacter()
    {
        Console.Write("Enter character name: ");
        var name = Console.ReadLine();

        Console.Write("Enter character level: ");
        var level = int.Parse(Console.ReadLine());

        Console.Write("Enter room ID for the character: ");
        var roomId = int.Parse(Console.ReadLine());

        if (!_context.Rooms.Any(room => room.Id == roomId))
        {
            return;
        }

        var room =_context.Rooms.FirstOrDefault(room => room.Id == roomId);

        var character = new Character
        {
            Name = name,
            Level = level,
            RoomId = roomId,
            Room = room
        };

        room.Characters.Add(character);

        _context.Characters.Add(character);
        _context.SaveChanges();
    }

    public void FindCharacter()
    {
        Console.Write("Enter character name to search: ");
        var name = Console.ReadLine();

        var character = _context.Characters.FirstOrDefault(character => character.Name == name);

        if (character is null) {
            Console.WriteLine($"No character by character name {name}");
            return;
        }

        Console.WriteLine($"Found character ({character.Id}) {character.Name}");
    }

}