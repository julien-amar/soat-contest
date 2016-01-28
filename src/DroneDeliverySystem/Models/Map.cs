using System.Collections.Generic;
using System.Drawing;

namespace DroneDeliverySystem.Models
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int TargetCount { get; set; }
        public int DroneCount { get; set; }
        public int MaxMove { get; set; }
        public int TurnCount { get; set; }

        public Point StartPosition { get; set; }

        public List<Point> Targets { get; set; }
        public List<Drone> Drones { get; set; }
    }
}