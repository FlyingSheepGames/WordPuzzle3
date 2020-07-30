using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class BirthdayFinderTest
    {
        [TestFixture]
        public class FindPeopleForDate
        {
            [Test]
            public void June8th_FindsRichardFleeshman()
            {
                BirthdayFinder finder = new BirthdayFinder();

                List<Person> results = finder.FindPeopleForDate(6, 8);
                Assert.Less(0, results.Count, "Expected at least one result");

                bool foundRichard = false;
                foreach (var person in results)
                {
                    if (person.Name == "Richard Fleeshman")
                    {
                        foundRichard = true;
                        List<string> quotes = person.Quotes;
                        Assert.Less(0, quotes.Count, "Expected at least one quote.");
                        bool foundExpectedQuote = false;
                        foreach (var quote in quotes)
                        {
                            if (quote == "I'm quite looking forward to the fact that people know me as Richard rather than Craig.")
                            {
                                foundExpectedQuote = true;
                            }
                            Console.WriteLine(quote);
                        }
                        Assert.IsTrue(foundExpectedQuote, "Expected to find quote.");
                    }
                }

                Assert.IsTrue(foundRichard, "Found expected person.");
 

            }
        }
    }
}