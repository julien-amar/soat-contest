using System.Collections.Generic;
using System.Drawing;

namespace DroneDeliverySystem.Models
{
    public enum Moves
    {
        Stand = 0,
        Left = 1,
        Up = 2,
        Right = 3,
        Down = 4,
    }

    public class Drone
    {
        public int Index { get; set; }

        public Point Position { get; set; }

        public List<Target> Targets { get; private set; } = new List<Target>();

        public List<Moves> Moves { get; private set; } = new List<Moves>();

        public int Delivered { get; set; }

        public int TTL { get; set; }
    }
}