﻿using Rivers;
using Rivers.Analysis.Traversal;
using System.Linq;

namespace AOC2020
{
    public class Day07 : DayBase, ITwoPartQuestion
    {
        private readonly Graph graph = new Graph();
        private const string SEARCH = "shiny gold";
        private int Part2Amount = 0;

        public Day07()
        {
            foreach (var line in InputFileAsStringList)
            {
                var c = line.IndexOf("contain");
                var parentBag = line.Substring(0, c).Replace("bags", "").Trim();
                var children = line[(c + 8)..];
                var array = children.Replace(" bags", "").Replace(" bag", "").Replace(".", "").Split(", ");

                //From node
                if (!graph.Nodes.Contains(parentBag))
                    graph.Nodes.Add(parentBag);

                foreach (var child in array.Where(i => i != "no other"))
                {
                    var quantity = int.Parse(child.Substring(0, child.IndexOf(" ")));
                    var childBag = child[(child.IndexOf(" ") + 1)..];

                    //To node
                    if (!graph.Nodes.Contains(childBag))
                        graph.Nodes.Add(childBag);

                    //Edge
                    var e = graph.Edges.Add(parentBag, childBag);
                    e.UserData["quantity"] = quantity;
                }
            }

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var paths = 0;
            foreach (var node in graph.Nodes.Where(n => n.Name != SEARCH))
                if (node.DepthFirstSearch(n => n.Name == SEARCH) != null)
                    paths++;

            return $"Paths to {SEARCH} : {paths}";
        }

        public string Part2()
        {
            foreach (var e in graph.Nodes[SEARCH].OutgoingEdges)
                InspectBag(e);

            return $"Bags inside {SEARCH} : {Part2Amount}";
        }

        public void InspectBag(Edge edge)
        {
            var quantity = (int)(edge.UserData["quantity"]);
            Part2Amount += quantity;

            //Console.WriteLine($"{edge.Source.Name} => {edge.Target.Name}, {amount}x");
            for (int i = 0; i < quantity; i++)
            {
                foreach (var innerBag in edge.Target.OutgoingEdges)
                    InspectBag(innerBag);
            }
        }
    }
}