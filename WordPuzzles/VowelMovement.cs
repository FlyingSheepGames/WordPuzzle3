using System.Collections.Generic;
using System.Text;

namespace WordPuzzles
{
    public class VowelMovement
    {
        public string Clue;

        public VowelMovement() : this("")
        {
            
        }
        public VowelMovement(string solution)
        {
            Solution= solution;
            Clue = ReplaceAllCapsWordsWithAsterisks(solution);
        }

        private VowelMovement(Dictionary<int, string> dictionary)
        {
            if (dictionary.ContainsKey(1))
            {
                Clue = dictionary[1];
            }

            if (dictionary.ContainsKey(3))
            {
                Solution = dictionary[3];
            }
            if (dictionary.ContainsKey(24))
            {
                this.TwitterUrl = dictionary[24];
            }

            if (dictionary.ContainsKey(5))
            {
                this.DatePosted = dictionary[5];
            }
        }

        public string DatePosted { get; set; }

        public string TwitterUrl { get; set; }

        internal string ReplaceAllCapsWordsWithAsterisks(string solution)
        {
            StringBuilder replacedText = new StringBuilder(solution.Length);
            char lastCharacter = ' ';
            int countOfLettersSkipped = 0;
            foreach (char character in solution)
            {
                if (char.IsUpper(character))
                {
                    lastCharacter = character;
                    countOfLettersSkipped++;
                }
                else
                {
                    if (!char.IsLetter(character))
                    {
                        if (lastCharacter != ' ')
                        {
                            if (countOfLettersSkipped == 1)
                            {
                                replacedText.Append(lastCharacter);
                            }
                            else
                            {
                                replacedText.Append("*");
                            }
                        }
                        lastCharacter = ' ';
                    }
                    else
                    {
                        if (lastCharacter != ' ')
                        {
                            replacedText.Append(lastCharacter);
                        }
                        lastCharacter = ' ';
                    }
                    replacedText.Append(character);
                    countOfLettersSkipped = 0;
                }
            }
            return replacedText.ToString();
        }

        public string Solution { get; set; }

        public string GoogleSheetRow => string.Join("\t", "", Clue, "", Solution, Theme, "", "", "", InitialConsonant, FinalConsonant, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "scheduled");

        public string FinalConsonant { get; set; }

        public string InitialConsonant { get; set; }

        public string Theme { get; set; }

        public List<VowelMovement> FindPuzzle(string startConsonant, string endConsonant)
        {
            GoogleSheet sheet = new GoogleSheet() {GoogleSheetKey = "1iYI-nE-5hYN7J3ckXmJjmzbxLmqs5UAGpGJh5yNUKgM" };

            string startConsonant1;
            if (string.IsNullOrWhiteSpace(startConsonant))
            {
                startConsonant1 = null;
            }
            else
            {
                startConsonant1 = startConsonant.ToLower();
                switch (startConsonant1)
                {
                    case "c" :
                    case "k" :
                        startConsonant1 = "c, k";
                        break;
                    case "g":
                        startConsonant1 = "g (hard)";
                        break;
                    case "n":
                    case "kn":
                        startConsonant1 = "n, kn";
                        break;
                    case "q":
                        startConsonant1 = "qu";
                        break;
                    case "j":
                        startConsonant1 = "j/soft g";
                        break;

                }
            }


            if (!string.IsNullOrWhiteSpace(endConsonant))
            {
                endConsonant = endConsonant.ToLower();
                switch (endConsonant)
                {
                    case "c":
                    case "k":
                        endConsonant = "c, k";
                        break;
                    case "h":
                    case "w":
                    case "y":
                        endConsonant = "(h) (w) (y)";
                        break;
                }
            }






            List<Dictionary<int, string>> queryResults = sheet.ExecuteQuery(string.Format($@"SELECT * WHERE I = '{startConsonant1}' AND J = '{endConsonant}'"));

            List<VowelMovement> vowelMovements = new List<VowelMovement>();
            foreach (var dictionary in queryResults)
            {
                var vowelMovement = new VowelMovement(dictionary);
                if (string.IsNullOrWhiteSpace(vowelMovement.TwitterUrl) &&
                    string.IsNullOrWhiteSpace(vowelMovement.DatePosted))
                {
                    vowelMovements.Add(vowelMovement);
                }
            }
            return vowelMovements;
        }

        public string GetTweet()
        {
            StringBuilder tweetBuilder = new StringBuilder();
            if (Theme != null && Theme.StartsWith("#"))
            {
                tweetBuilder.AppendLine(Theme);
            }
            tweetBuilder.AppendLine(Clue);
            tweetBuilder.AppendLine();
            tweetBuilder.AppendLine(@"#HowToPlay: https://t.co/rSa0rUCvRC");

            return tweetBuilder.ToString();
        }
    }
}