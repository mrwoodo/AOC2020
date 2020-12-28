using System.Collections.Generic;
using System.Linq;

namespace AOC2020
{
    public class Day25 : DayBase, ITwoPartQuestion
    {
        private const int SECRET = 20201227;

        public List<int> PublicKeys;

        public Day25()
        {
            PublicKeys = InputFileAsIntList;
            Run(() => Part1(), () => Part2());
        }

        public string Part1()
        {
            var loopSizeKey = GetLoopSize(PublicKeys[0]);
            var encryptionKey = GetEncryptionKey(PublicKeys[1], loopSizeKey);

            return $"Encryption key = {encryptionKey}";
        }

        public long GetLoopSize(long publicKey)
        {
            long val = 1;
            long loopSize = 0;
            long subjectNumber = 7;

            while (val != publicKey)
            {
                loopSize++;
                val *= subjectNumber;
                val %= SECRET;
            }

            return loopSize;
        }

        public long GetEncryptionKey(long subjectNumber, long loopSize)
        {
            long result = 1;

            for (long l = 0; l < loopSize; l++)
            {
                result *= subjectNumber;
                result %= SECRET;
            }

            return result;
        }

        public string Part2()
        {
            return "";
        }
    }
}