using System;
using System.Collections.Generic;
using NUnit.Framework;
using WordPuzzles;

namespace WordPuzzlesTest
{
    [TestFixture]
    public class MonthlyScoreKeeperTest
    {
        [TestFixture]
        public class GetPlayers
        {
            [Test]
            public void January2019_ReturnsExpectedResults()
            {
                MonthlyScoreKeeper keeper = new MonthlyScoreKeeper();
                List<Player> players = keeper.GetPlayers();

                Assert.AreEqual(31, players.Count);

                Player firstPlayer = players[0];
                Assert.AreEqual("ossiangrr", firstPlayer.TwitterHandle);
                Assert.AreEqual(25, firstPlayer.TotalScore);
                Assert.AreEqual(9, firstPlayer.ALittleAlliterationScore);
                Assert.AreEqual(5, firstPlayer.MagicWordSquareScore);
                Assert.AreEqual(11, firstPlayer.VowelMovementScore);
                Assert.AreEqual(false, firstPlayer.AlreadyMentioned);
                players.Sort(Player.SortByTotalScore);
                int previousScore = int.MaxValue;
                foreach (var player in players)
                {
                    Console.WriteLine($"{player.TwitterHandle}:{player.TotalScore}");

                    Assert.LessOrEqual(player.TotalScore, previousScore);
                    previousScore = player.TotalScore;
                }
                Console.WriteLine("----------------------------------");

                previousScore = int.MaxValue;
                players.Sort(Player.SortByMagicWordSquareScore);
                foreach (var player in players)
                {
                    Console.WriteLine($"{player.TwitterHandle}:{player.MagicWordSquareScore}");

                    Assert.LessOrEqual(player.MagicWordSquareScore, previousScore);
                    previousScore = player.MagicWordSquareScore;
                }

                Console.WriteLine("----------------------------------");

                previousScore = int.MaxValue;
                players.Sort(Player.SortByVowelMovementScore);
                foreach (var player in players)
                {
                    Console.WriteLine($"{player.TwitterHandle}:{player.VowelMovementScore}");

                    Assert.LessOrEqual(player.VowelMovementScore, previousScore);
                    previousScore = player.VowelMovementScore;

                }


                Console.WriteLine("----------------------------------");

                previousScore = int.MaxValue;
                players.Sort(Player.SortByALittleAlliterationScore);
                foreach (var player in players)
                {
                    Console.WriteLine($"{player.TwitterHandle}:{player.ALittleAlliterationScore}");
                    Assert.LessOrEqual(player.ALittleAlliterationScore, previousScore);
                    previousScore = player.ALittleAlliterationScore;
                }

            }
        }

    }
}