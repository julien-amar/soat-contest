using System.Drawing;

namespace DroneDeliverySystem.Models
{
    public class Target
    {
        public bool Delivered { get; set; }

        public Point Position { get; set; }

        public int Distance { get; set; }

        public bool OverflowPath { get; set; }
    }
}