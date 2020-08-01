using System;
using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.NetFramework.Utility
{
    [TestFixture]
    public class ThemeRepositoryTest
    {
        [TestFixture]
        public class Constructor
        {
            [Test]
            public void PopulatesInternalThemeList()
            {
                ThemeRepository repository = new ThemeRepository();

                var listOfThemes = repository.Themes;
                Assert.Less(0, listOfThemes.Count, "Expected at least one theme.");
                Console.WriteLine(listOfThemes.Count);
                //Don't expect any empty themes
                foreach (var wordList in listOfThemes)
                {
                    Console.WriteLine($"Theme {wordList.Key} has {wordList.Value.Count} words.");
                    Assert.Less(0, wordList.Value.Count, $"Theme {wordList.Key} was empty!");
                }
            }
        }

    }
}