using System;

namespace GamePadPlus.Models
{
    public class Game
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string Notes { get; set; } = string.Empty;

        public string? ImageFileName { get; set; }

        public Game()
        {
        }

        public Game(string name)
        {
            Name = name;
        }
    }
}
