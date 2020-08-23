using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Utility
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
                    Console.WriteLine(person.Year);
                    if (person.Name == "Richard Fleeshman")
                    {
                        foundRichard = true;
                        Assert.AreEqual(1989, person.Year, "Unexpected Year");
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

        [TestFixture]
        public class GetNameAndUrlFromFragment
        {
            [Test]
            public void Examples_ReturnExpectedStrings()
            {
                BirthdayFinder finder = new BirthdayFinder();
                string actualName;
                string actualUrl;
                int actualYear = 0;
                finder.GetNameAndUrlFromFragment(
                    @"/authors/richard-fleeshman-quotes"">Richard Fleeshman</a></td>\n<td>\nActor\n</td>\n<td>1989</td>\n</tr>\n<tr onclick=""window.document.location='/authors/rosanna-pansino-quotes';"">\n"
                    , out actualName, out actualUrl, out actualYear);
                Assert.AreEqual("Richard Fleeshman", actualName);
                Assert.AreEqual("https://www.brainyquote.com/authors/richard-fleeshman-quotes"
                    , actualUrl);
                Assert.AreEqual(1989, actualYear);

                finder.GetNameAndUrlFromFragment(
                    @"/authors/rosanna-pansino-quotes"">Rosanna Pansino</a></td>\n<td>\nEntertainer\n</td>\n<td>1985</td>\n</tr>\n<tr onclick=""window.document.location='/authors/javier-mascherano-quotes';"">\n"
                    , out actualName, out actualUrl, out actualYear);
                Assert.AreEqual("Rosanna Pansino", actualName);
                Assert.AreEqual("https://www.brainyquote.com/authors/rosanna-pansino-quotes"
                    , actualUrl);
                Assert.AreEqual(1985, actualYear);

            }
        }
    }
}