using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DroneDeliverySystem.Models;

namespace DroneDeliverySystem.Helpers
{
    public class DistanceHelper
    {
        public static Target GetDistance(Map map, Point source, Point target)
        {
            var distanceWithOverflow = map.Width
                - Math.Max(source.X, target.X)
                + Math.Min(source.X, target.X);

            var distanceWithoutOverflow = Math.Abs(source.X - target.X);

            var useOverflow = distanceWithOverflow < distanceWithoutOverflow;

            var distanceVertical = Math.Abs(source.Y - target.Y);

            var distance = Math.Min(distanceWithOverflow, distanceWithoutOverflow) + distanceVertical;

            return new Target()
            {
                Position = target,
                Distance = distance,
                OverflowPath = useOverflow,
            };
        }
    }
}
