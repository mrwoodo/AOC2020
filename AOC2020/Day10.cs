using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AOC2020
{
    public class Day10 : DayBase, ITwoPartQuestion
    {
        public List<int> Devices = new List<int>();

        public Day10()
        {
            Devices = (from line in InputFile.Split("\r\n")
                       select int.Parse(line)).OrderBy(i => i).ToList();

            Devices.Insert(0, 0);
            Devices.Add(Devices.Last() + 3);

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var Differences = new Dictionary<int, int>
            {
                [1] = 0,
                [2] = 0,
                [3] = 0
            };

            for (int i = 1; i < Devices.Count; i++)
            {
                var prev = Devices[i - 1];
                var curr = Devices[i];

                Differences[curr - prev]++;
            }

            return $"{Differences[1]}x 1 jolt differences, {Differences[3]}x 3 jolt differences = {Differences[1] * Differences[3]}";
        }

        public string Part2()
        {
            return "";
        }
    }
}

/*

0   1   4   5   6   7   10  11  12  15  16  19  22        1   3   1   1   1   3   1   1   3   1   3   3
0   1   4   5   6   7   10      12  15  16  19  22        1   3   1   1   1   3       2   3   1   3   3
0   1   4   5       7   10  11  12  15  16  19  22        1   3   1       2   3   1   1   3   1   3   3
0   1   4   5       7   10      12  15  16  19  22        1   3   1       2   3       2   3   1   3   3
0   1   4       6   7   10  11  12  15  16  19  22        1   3       2   1   3   1   1   3   1   3   3
0   1   4       6   7   10      12  15  16  19  22        1   3       2   1   3       2   3   1   3   3
0   1   4           7   10  11  12  15  16  19  22        1   3           3   3   1   1   3   1   3   3
0   1   4           7   10      12  15  16  19  22        1   3           3   3       2   3   1   3   3

                                                          x   x    ---------  x    ---    x   x   x   x
                                                                        4             2


4n

111     111
12      111
21      111
3       111

111     12
12      12
21      12
3       12

111     21
12      21
21      21
3       21

111     3
12      3
21      3
3       3









[0]: 0
[1]: 1                  1
[2]: 2                  1
[3]: 3                  1   3
[4]: 4                  1   1
[5]: 7                  3   3
[6]: 8                  1
[7]: 9                  1
[8]: 10                 1   3
[9]: 11                 1   1
[10]: 14                3   3
[11]: 17                3   3
[12]: 18                1
[13]: 19                1
[14]: 20                1   3
[15]: 23                3   3
[16]: 24                1
[17]: 25                1   2
[18]: 28                3   3
[19]: 31                3   3
[20]: 32                1
[21]: 33                1
[22]: 34                1   3
[23]: 35                1   1
[24]: 38                3   3
[25]: 39                1   1
[26]: 42                3   3
[27]: 45                3   3
[28]: 46                1
[29]: 47                1
[30]: 48                1   3
[31]: 49                1   1
[32]: 52                3   3



[1]: 1                  1
[2]: 2                  1
[3]: 3                  1   3

[6]: 8                  1
[7]: 9                  1
[8]: 10                 1   3

[12]: 18                1
[13]: 19                1
[14]: 20                1   3

[16]: 24                1
[17]: 25                1   2

[20]: 32                1
[21]: 33                1
[22]: 34                1   3

[28]: 46                1
[29]: 47                1
[30]: 48                1   3





*/

