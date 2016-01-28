using DroneDeliverySystem.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace DroneDeliverySystem.Helpers
{
    public static class PathFinder
    {
        public static IEnumerable<Point> GetNeightboors(Map map, Point point, IEnumerable<Point> avoid)
        {
            var left = new Point(point.X - 1, point.Y);
            var right = new Point(point.X + 1, point.Y);
            var top = new Point(point.X, point.Y - 1);
            var bot = new Point(point.X, point.Y + 1);

            if (left.X == -1)
                left = new Point(map.Width - 1, point.Y);
            else if (right.X == map.Width)
                right = new Point(0, point.Y);

            if (!avoid.Contains(left))
                yield return left;

            if (!avoid.Contains(right))
                yield return right;

            if (!avoid.Contains(top))
                yield return top;

            if (!avoid.Contains(bot))
                yield return bot;
        }
    }
}
