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
            var edges = new Dictionary<int, HashSet<int>>();
            for (int i = 1; i <= n; i++)
                edges.Add(i, new HashSet<int>());
            for (int i = 1; i < lines.Length; i++) {
                var vertices = lines[i].Trim().Split(' ');
                int a = int.Parse(vertices[0]);
                int b = int.Parse(vertices[1]);

                edges[a].Add(b);
                edges[b].Add(a);
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
                var check = new Dictionary<int, HashSet<int>>();
                for (int ii = 0; ii < k; ii++)
                    check.Add(ii, new HashSet<int>());
                int variable = i * k + 1;
                for (int a = 0; a < k; a++) {
                    for (int b = 0; b < k; b++) {
                        if (a == b)
                            continue;
                        if (check[a].Contains(b))
                            continue;

                        int color1 = variable + a;
                        int color2 = variable + b;

                        result += $"\n{-color1} {-color2} 0";
                        clauseCount++;

                        check[a].Add(b);
                        check[b].Add(a);
                    }
                }
            }

            // edge checks
            var check2 = new Dictionary<int, HashSet<int>>();
            for (int i = 0; i < n; i++)
                check2.Add(i, new HashSet<int>());
            for (int a = 0; a < n; a++) {
                for (int b = 0; b < n; b++) {
                    if (!edges[a].Contains(b))
                        continue;
                    if (check2[a].Contains(b))
                        continue;

                    for (int color = 0; color < k; color++) {
                        int var1 = a * k + 1 + color;
                        int var2 = b * k + 1 + color;
                        result += $"\n{-var1} {-var2} 0";
                        clauseCount++;
                    }

                    check2[a].Add(b);
                    check2[b].Add(a);
                }
            }

            // end
            result = $"p cnf {variableCount} {clauseCount}{result}";
            File.WriteAllText(fileOutput, result);
        }
    }
}
