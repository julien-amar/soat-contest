using System;
using System.Linq;
using System.Threading;

namespace DroneDeliverySystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver(@"Resources\BigChallengeInput.txt");

            var allTargets = solver.Map.Targets.ToList();

            foreach (var drone in solver.Map.Drones)
            {
                var pathes = solver.GetShortestPath(4);

                drone.Targets.AddRange(pathes);
            }

            for (int turn = 0; turn < solver.Map.TurnCount; ++turn)
            {
                foreach (var drone in solver.Map.Drones)
                {
                    var droneTargets = drone.Targets
                        .Where(x => !x.Delivered)
                        .ToList();

                    var move = solver.GenerateMove(
                        drone,
                        drone.Position,
                        droneTargets,
                        allTargets);

                    drone.TTL++;
                    drone.Moves.Add(move);

                    var target = droneTargets.FirstOrDefault();

                    if (target != null && drone.Position == target.Position)
                    {
                        drone.Delivered++;

                        target.Delivered = true;

                        allTargets.Remove(target.Position);
                    }
                }
            }

            foreach (var drone in solver.Map.Drones)
            {
                Console.WriteLine("Drone #{0}: {1} [{2}] (TTL: {3})",
                    drone.Index,
                    drone.Delivered,
                    String.Join(", ", drone.Targets.Select(x => x.Position)),
                    drone.TTL + 40 * drone.Delivered);
            }

            solver.WriteOutput("output.txt");

            var score = solver.UploadSolution(
                new Uri("http://soat-challenge.cloudapp.net"),
                "/judge/upload",
                "8507B2FEF8C809BD5CE2D2F1C563703B");

            Console.WriteLine("Score: {0}", score);
        }
    }
}
