using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using DroneDeliverySystem.Helpers;
using DroneDeliverySystem.Models;
using System.Diagnostics;

namespace DroneDeliverySystem
{
    public class Solver
    {
        public Map Map { get; }

        public Solver(string inputFile)
        {
            Map = new Map();

            var input = File.OpenText(inputFile);

            var size = input.ReadLine().Split(' ').Select(int.Parse).ToArray();

            Map.Width = size[0];
            Map.Height = size[1];

            var infos = input.ReadLine().Split(' ').Select(int.Parse).ToArray();

            Map.TargetCount = infos[0];
            Map.DroneCount = infos[1];
            Map.MaxMove = infos[2];
            Map.TurnCount = infos[3];

            var start = input.ReadLine().Split(' ').Select(int.Parse).ToArray();

            Map.StartPosition = new Point(start[1], start[0]);

            Map.Targets = Enumerable.Range(0, Map.TargetCount)
                .Select(i => input.ReadLine().Split(' ').Select(int.Parse).ToArray())
                .Select(i => new Point(i[1], i[0]))
                .ToList();

            Map.Drones = Enumerable.Range(0, Map.DroneCount)
                .Select(d => new Drone
                {
                    Index = d,
                    Position = Map.StartPosition,
                })
                .ToList();
        }

        public List<Target> GetShortestPath(int maxPathLength)
        {
            var pathes = new List<Target>()
            {
                new Target()
                {
                    Position = Map.StartPosition,
                    Distance = 0
                }
            };

            for (int i = 0; i < maxPathLength; ++i)
            {
                var lastTarget = pathes.Last();

                if (Map.Targets.Any())
                {
                    var shortestTarget = (
                            from target in Map.Targets
                            let path = DistanceHelper.GetDistance(Map, lastTarget.Position, target)
                            orderby path.Distance
                            select path)
                            .First();

                    pathes.Add(shortestTarget);

                    Map.Targets.Remove(shortestTarget.Position);
                }
            }

            pathes.RemoveAt(0);

            return pathes;
        }

        public Moves GenerateMove(Drone drone, Point startPosition, List<Target> pathes, List<Point> avoidTargets)
        {
            var moves = new List<Moves>();

            avoidTargets = avoidTargets
                .Where(at => !pathes.Select(x => x.Position).Contains(at))
                .ToList();

            if (!pathes.Any())
            {
                return Moves.Stand;
            }

            var path = pathes.First();

            var shortestPath = AStar.FindPath(
                startPosition,
                n => PathFinder.GetNeightboors(Map, n, avoidTargets),
                (src, dst) => 0,
                h => DistanceHelper.GetDistance(Map, h, path.Position).Distance,
                end => end == path.Position)
                .ToList();

            if (shortestPath.Any())
            {
                shortestPath.Reverse();

                drone.Position = shortestPath
                    .Skip(1)
                    .First();

                return MoveHelper.GetMovesFromPath(Map, shortestPath)
                    .First();
            }


            return Moves.Stand;
        }


        public void WriteOutput(string outputFilename)
        {
            var result = string.Join("\n", Map.Drones
                .Select(drone =>
                    drone.Targets.Count +
                    " " +
                    string.Join(" ", drone.Moves.Select(m => Convert.ToInt32(m)))
            ));

            File.WriteAllText(outputFilename, result);
        }

        public string UploadSolution(Uri baseAddress, string judgeUploadUrl, string sessionToken)
        {
            HttpContent outputFileStreamContent = new StreamContent(File.OpenRead(@"Resources\output.txt"));
            HttpContent codeFileStreamContent = new StreamContent(File.OpenRead(@"Resources\Dummy.zip"));

            var cookieContainer = new CookieContainer();

            cookieContainer.Add(baseAddress, new Cookie("JSESSIONID", sessionToken));

            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(outputFileStreamContent, "solution", "output.txt");
                formData.Add(codeFileStreamContent, "code", "Dummy.zip");

                var response = client.PostAsync(judgeUploadUrl, formData).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var result = response.Content.ReadAsStringAsync().Result;

                result = result.Substring(result.IndexOf("Score :"));
                result = result.Substring(result.IndexOf(">") + 1);
                result = result.Substring(result.IndexOf(">") + 1);
                result = result.Substring(0, result.IndexOf("<"));

                return result;
            }
        }
    }
}
