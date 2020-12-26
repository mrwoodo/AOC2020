using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AOC2020
{
    public class Day25 : DayBase, ITwoPartQuestion
    {
        public List<int> PublicKeys;

        public Day25()
        {
            PublicKeys = (from i in InputFile.Split("\r\n")
                         select int.Parse(i)).ToList();

            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var loopSizeKey = GetLoopSize(PublicKeys[0]);
            //var loopSizeDoor = GetLoopSize(PublicKeys[1]);
            var encryptionKey = GetEncryptionKey(PublicKeys[1], loopSizeKey);

            return $"Encryption key = {encryptionKey}";
        }

        public long GetLoopSize(long publicKey)
        {
            long val = 1;
            long loopSize = 0;

            while (val != publicKey)
            {
                loopSize++;
                val *= 7;
                val %= 20201227;
            }

            return loopSize;
        }

        public long GetEncryptionKey(long subjectNumber, long loopSize)
        {
            long result = 1;

            for (long l = 0; l < loopSize; l++)
            {
                result *= subjectNumber;
                result %= 20201227;
            }

            return result;
        }

        public string Part2()
        {
            return "";
        }
    }
}