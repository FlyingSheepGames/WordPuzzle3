using System;
using NUnit.Framework;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class UnrelatedMatter
    {
        [Test]
        public void TwoStrangeDice()
        {
            int[] observations = new int[13];
            foreach (int firstDie in new int[] { 1, 2, 2, 3, 3, 4})
            {
                foreach (int secondDie in new int[] {1, 3, 4, 5, 6, 8})
                {
                    observations[firstDie + secondDie]++;
                }
            }

            for (int total = 2; total < 13; total++)
            {
                Console.WriteLine($"{total} : {observations[total]}");
            }
        }

        
    }
}