using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CNF_K_Coloring {
    class Program {
        static void Main(string[] args) {
            string fileInput = args[0];
            string fileOutput = args[1];
            if (!File.Exists(fileInput)) {
                return;
            }

            // input
            string input = File.ReadAllText(fileInput);
            input = input.Replace("\r", "");
            var lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            var info = lines[0].Trim().Split(' ');

            int k = int.Parse(info[0]);
            int n = int.Parse(info[1]);
            var edges = new List<List<int>>();

            for (int i = 0; i < n; i++)
                edges.Add(new List<int>());
            for (int i = 1; i < lines.Length; i++) {
                var vertices = lines[i].Trim().Split(' ');
                int a = int.Parse(vertices[0]) - 1;
                int b = int.Parse(vertices[1]) - 1;

                if (!edges[b].Contains(a))
                    edges[a].Add(b);
            }

            // logic
            int variableCount = n * k;
            int clauseCount = 0;
            string result = "";

            // at least one color
            for (int i = 0; i < n; i++) {
                clauseCount++;
                int variable = i * k + 1;
                result += "\n" + variable;

                for (int color = 1; color < k; color++) {
                    result += " " + (variable + color);
                }

                result += " 0";
            }

            // only one color
            for (int i = 0; i < n; i++) {
                for (int c1 = 1; c1 <= k; c1++) {
                    for (int c2 = c1 + 1; c2 <= k; c2++) {
                        clauseCount++;
                        result += $"\n{-(i * k + c1)} {-(i * k + c2)} 0";
                    }
                }
            }

            // edge checks
            for (int i = 0; i < n; i++) {
                foreach (var ii in edges[i]) {
                    for (int c = 1; c <= k; c++) {
                        int a = i * k + c;
                        int b = ii * k + c;
                        result += $"\n{-a} {-b} 0";
                        clauseCount++;
                    }
                }
            }

            // end
            result = $"p cnf {variableCount} {clauseCount}{result}";
            File.WriteAllText(fileOutput, result);
        }
    }
}
