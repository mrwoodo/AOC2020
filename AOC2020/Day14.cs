using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day14 : DayBase, ITwoPartQuestion
    {
        public Day14()
        {
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var mask = "";
            var memory = new Dictionary<long, long>();

            foreach (var line in InputFileAsStringList)
            {
                if (line.StartsWith("mask"))
                    mask = line.Replace("mask = ", "");
                else
                {
                    var s = line.Split("] = ");
                    var idx = int.Parse(s[0].Replace("mem[", ""));
                    var val = long.Parse(s[1]);

                    memory[idx] = ApplyVer1Mask(val, mask);
                }
            }

            return $"Sum of memory = {memory.Values.Sum()}";
        }

        private long ApplyVer1Mask(long val, string mask)
        {
            var valBinStr = Convert.ToString(val, 2).PadLeft(36, '0');
            var result = "";

            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == 'X')
                    result += valBinStr[i];
                else
                    result += mask[i];
            }

            return Convert.ToInt64(result, 2);
        }

        public string Part2()
        {
            var mask = "";
            var memory = new Dictionary<long, long>();

            foreach (var line in InputFileAsStringList)
            {
                if (line.StartsWith("mask"))
                    mask = line.Replace("mask = ", "");
                else
                {
                    var s = line.Split("] = ");
                    var idx = int.Parse(s[0].Replace("mem[", ""));
                    var val = long.Parse(s[1]);

                    ApplyVer2Mask(ref memory, idx, val, mask);
                }
            }

            return $"Sum of memory = {memory.Values.Sum()}";
        }

        private void ApplyVer2Mask(ref Dictionary<long, long> memory, int idx, long val, string mask)
        {
            var maskedDigitsCount = mask.Count(i => i == 'X');
            var memoryPadded = Convert.ToString(idx, 2).PadLeft(36, '0');

            for (int i = 0; i < Math.Pow(2, maskedDigitsCount); i++)
            {
                var sequence = Convert.ToString(i, 2).PadLeft(maskedDigitsCount, '0');
                var outPutMask = new StringBuilder(mask);
                var count = 0;

                for (int j = 0; j < outPutMask.Length; j++)
                {
                    if (outPutMask[j] == 'X')
                    {
                        outPutMask[j] = sequence[count];
                        count++;
                    }
                    else if (outPutMask[j] == '0')
                        outPutMask[j] = memoryPadded.Substring(j, 1)[0];
                }

                memory[Convert.ToInt64(outPutMask.ToString(), 2)] = val;
            }
        }
    }
}