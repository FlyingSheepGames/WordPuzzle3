using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;
using WordPuzzles.Puzzle;
using WordPuzzles.Puzzle.Legacy;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.NetFramework.Utility
{
    [TestFixture]
    public class WeekOfPuzzlesTest
    {
        private static readonly string ExpectedText = @"<?xml version=""1.0"" encoding=""utf-8""?>
<WeekOfPuzzles xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <MondayWordSquare>
    <Lines>
      <string>acorn</string>
      <string>curio</string>
      <string>orals</string>
      <string>rille</string>
      <string>nosed</string>
    </Lines>
    <Size>5</Size>
    <Repository>
      <IgnoreCache>true</IgnoreCache>
    </Repository>
    <Clues>
      <string>first clue</string>
      <string>second clue</string>
      <string>third clue</string>
      <string>fourth clue</string>
      <string>fifth clue</string>
    </Clues>
    <Theme>Theme</Theme>
  </MondayWordSquare>
  <TuesdayVowelMovement>
    <Clue>The * * their *.</Clue>
    <Solution>The MICE MISS their MOOSE.</Solution>
    <FinalConsonant>s</FinalConsonant>
    <InitialConsonant>m</InitialConsonant>
    <Theme>Theme</Theme>
  </TuesdayVowelMovement>
  <WednesdayALittleAlliteration>
    <Clue>Convey vegetable automobile</Clue>
    <Solution>carry carrot car</Solution>
    <Theme>VegetableWeek</Theme>
  </WednesdayALittleAlliteration>
  <SelectedWords>
    <string />
    <string />
    <string />
    <string />
    <string />
  </SelectedWords>
  <Theme>WeeklyTheme</Theme>
  <MondayOfWeekPosted>0001-01-01</MondayOfWeekPosted>
</WeekOfPuzzles>";
        [TestFixture]
        public class Serialize
        {

            [Test]
            public void EmptyObject_CreatesExpectedFile()
            {
                WeekOfPuzzles weekOfPuzzles = new WeekOfPuzzles();
                string fileName = $"EmptyObject_example_{Process.GetCurrentProcess().Id}.xml";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                weekOfPuzzles.Serialize(fileName);

                var actualText = File.ReadAllText(fileName);

                Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?>
<WeekOfPuzzles xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <SelectedWords>
    <string />
    <string />
    <string />
    <string />
    <string />
  </SelectedWords>
  <MondayOfWeekPosted>0001-01-01</MondayOfWeekPosted>
</WeekOfPuzzles>", 
                    actualText);
            }

            [Test]
            public void EmptyObject_WithDate_CreatesExpectedFile()
            {
                WeekOfPuzzles weekOfPuzzles = new WeekOfPuzzles {MondayOfWeekPosted = new DateTime(2019, 2, 25)};
                string fileName = $"EmptyObject_example_{Process.GetCurrentProcess().Id}.xml";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                weekOfPuzzles.Serialize(fileName);

                var actualText = File.ReadAllText(fileName);

                Assert.AreEqual(@"<?xml version=""1.0"" encoding=""utf-8""?>
<WeekOfPuzzles xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <SelectedWords>
    <string />
    <string />
    <string />
    <string />
    <string />
  </SelectedWords>
  <MondayOfWeekPosted>2019-02-25</MondayOfWeekPosted>
</WeekOfPuzzles>",
                    actualText);
            }


            [Test]
            public void PopulatedObject_CreatesExpectedFile()
            {
                WeekOfPuzzles weekOfPuzzles = new WeekOfPuzzles {Theme = "WeeklyTheme"};
                WordSquare mondayWordSquare = new WordSquare("_____")
                {
                    Clues = new []{"first clue", "second clue", "third clue", "fourth clue", "fifth clue"}, 
                    Theme = "Theme"
                };
                mondayWordSquare.SetWordAtIndex("acorn", 0);
                mondayWordSquare.SetWordAtIndex("curio", 1);
                int indexToSet = 2;

                mondayWordSquare.SetWordAtIndex("orals", indexToSet);
                mondayWordSquare.SetWordAtIndex("rille", 3);
                mondayWordSquare.SetWordAtIndex("nosed", 4);

                weekOfPuzzles.MondayWordSquare = mondayWordSquare;

                VowelMovement tuesdayVowelMovementPuzzle = new VowelMovement("The MICE MISS their MOOSE.")
                {
                    InitialConsonant = "m", 
                    FinalConsonant = "s", 
                    Theme = "Theme"
                };
                
                weekOfPuzzles.TuesdayVowelMovement = tuesdayVowelMovementPuzzle;

                ALittleAlliteration wednesdayAlliterationPuzzle = new ALittleAlliteration()
                {
                    Clue = "Convey vegetable automobile", 
                    Solution = "carry carrot car", 
                    Theme = "VegetableWeek"
                };

                weekOfPuzzles.WednesdayALittleAlliteration = wednesdayAlliterationPuzzle;


                string fileName = $"EmptyObject_example_{Process.GetCurrentProcess().Id}.xml";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                weekOfPuzzles.Serialize(fileName);

                var actualText = File.ReadAllText(fileName);
                Console.WriteLine(actualText);
                Assert.AreEqual(ExpectedText,actualText);

            }
        }

        [TestFixture]
        public class Deserialize
        {
            [Test]
            public void ReturnsExpectedObject()
            {
                string fileName = $"testcase_{Process.GetCurrentProcess().Id}.xml";
                if (!File.Exists(fileName))
                {
                    File.WriteAllText(fileName, ExpectedText);
                }
                WeekOfPuzzles weekOfPuzzles = new WeekOfPuzzles();
                weekOfPuzzles.Deserialize(fileName);

                Assert.IsNotNull(weekOfPuzzles.MondayWordSquare);
                Assert.IsNotNull(weekOfPuzzles.TuesdayVowelMovement);
                Assert.IsNotNull(weekOfPuzzles.WednesdayALittleAlliteration);
                Assert.AreEqual("WeeklyTheme", weekOfPuzzles.Theme);
            }

            [Test]
            public void WithoutDate_SetsMondayToMinValue()
            {
                string fileName = $"testcase_{Process.GetCurrentProcess().Id}.xml";
                if (!File.Exists(fileName))
                {
                    File.WriteAllText(fileName, ExpectedText);
                }
                WeekOfPuzzles weekOfPuzzles = new WeekOfPuzzles();
                weekOfPuzzles.Deserialize(fileName);

                Assert.AreEqual(DateTime.MinValue, weekOfPuzzles.MondayOfWeekPosted);
            }


            [Test]
            public void EmptyObject_WithDate_CreatesExpectedFile()
            {
                const string EMPTY_OBJECT_WITH_DATE = @"<?xml version=""1.0"" encoding=""utf-8""?>
<WeekOfPuzzles xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <SelectedWords>
    <string />
    <string />
    <string />
    <string />
    <string />
  </SelectedWords>
  <MondayOfWeekPosted>2019-02-25</MondayOfWeekPosted>
</WeekOfPuzzles>";
                string fileName = $"EmptyObject_example_{Process.GetCurrentProcess().Id}.xml";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                File.WriteAllText(fileName, EMPTY_OBJECT_WITH_DATE);

                WeekOfPuzzles weekOfPuzzles = new WeekOfPuzzles();
                weekOfPuzzles.Deserialize(fileName);
        
                Assert.AreEqual(new DateTime(2019, 2, 25), weekOfPuzzles.MondayOfWeekPosted);

            }

        }
    }
}