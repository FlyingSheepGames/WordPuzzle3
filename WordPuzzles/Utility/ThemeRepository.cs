using System.Collections.Generic;

namespace WordPuzzles.Utility
{
    public class ThemeRepository
    {
        public Dictionary<string, List<string>> Themes { get; set; }

        public ThemeRepository()
        {
            Themes = LoadThemes();
        }

        private Dictionary<string, List<string>> LoadThemes()
        {
            var dictionaryOfThemes = new Dictionary<string, List<string>>();
            // ReSharper disable StringLiteralTypo
            GoogleSheet sheet = new GoogleSheet() { GoogleSheetKey = "1ZJmh_woTIRDW1lspRX728GdkUc81J1K_iYeOoPYNfcA" };
            // ReSharper restore StringLiteralTypo

            List<Dictionary<int, string>> findRelatedWords = sheet.ExecuteQuery(@"SELECT *");
            foreach (var dictionary in findRelatedWords)
            {
                if (!dictionary.ContainsKey(0))
                {
                    continue;
                }

                string theme = dictionary[0];
                if (!dictionaryOfThemes.ContainsKey(theme))
                {
                    dictionaryOfThemes.Add(theme, new List<string>());
                }

                List<string> wordsForTheme = dictionaryOfThemes[theme];

                if (string.IsNullOrWhiteSpace(theme))
                {
                    continue;
                }
                if (!dictionary.ContainsKey(1))
                {
                    continue;
                }

                string wordToAdd = dictionary[1];
                if (string.IsNullOrWhiteSpace(wordToAdd))
                {
                    continue;
                }

                wordToAdd = wordToAdd.ToLower();
                if (!wordsForTheme.Contains(wordToAdd)) //skip any duplicates
                {
                    wordsForTheme.Add(wordToAdd);
                }
            }

            return dictionaryOfThemes;
        }

        public List<string> FindThemesForWord(string word)
        {
            var themesForWord = new List<string>();
            foreach (var themeList in Themes)
            {
                if (themeList.Value.Contains(word.ToLower()))
                {
                    themesForWord.Add(themeList.Key);
                }
            }

            return themesForWord;
        }

        public List<string> FindWordsForTheme(string theme)
        {
            if (Themes.ContainsKey(theme))
            {
                return Themes[theme];
            }
            return new List<string>();
        }
    }

}