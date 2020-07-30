using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace WordPuzzles.Utility
{
    public class WordRepository
    {
        // ReSharper disable once InconsistentNaming
        private static readonly string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";

        private ThemeRepository themeRepository = new ThemeRepository();

        [XmlIgnore]
        public List<WordCategory> CategoriesToInclude
        {
            get;
            set;
        }

        [XmlIgnore]
        public bool ExludeAdvancedWords { get; set; }

        readonly Random _randomNumberGenerator = new Random();

        private readonly List<string> _oneLetterWords = new List<string>() {"a", "i"};
        private readonly List<string> _twoLetterWords = new List<string>()
        {
            "ad",
            "am",
            "an",
            "as",
            "at",
            "ax",
            "be",
            "by",
            "do",
            "go",
            "he",
            "hi",
            "if",
            "in",
            "is",
            "it",
            "me",
            "my",
            "no",
            "of",
            "OK",
            "on",
            "or",
            "ox",
            "so",
            "to",
            "up",
            "us",
            "we",

        };

        private readonly List<string> _threeLetterWords = new List<string>();
        private readonly List<string> _fourLetterWords = new List<string>();
        private readonly List<string> _fiveLetterWords = new List<string>();
        private readonly List<string> _sixLetterWords = new List<string>();
        private readonly List<string> _sevenLetterWords = new List<string>();
        private readonly List<string> _eightLetterWords = new List<string>();
        private readonly List<string> _tenLetterWords = new List<string>();
        internal bool _alreadyLoaded;
        private static readonly Dictionary<string, string> DictionaryOfClues = new Dictionary<string, string>();

        public bool IgnoreCache = true;
        public void LoadAllWords()
        {
            CategoriesToInclude = new List<WordCategory>() {WordCategory.BasicWord};
            if (!ExludeAdvancedWords)
            {
                CategoriesToInclude.Add(WordCategory.AdvancedWord);
            }
            LoadAllWordsWithCategories();
        }


        public void LoadAllWordsWithCategories()
        {
            const int WORD_INDEX = 0;
            const int CATEGORY_INDEX = 1;
            const int HINT_INDEX = 3;

            GoogleSheet sheet = new GoogleSheet() { GoogleSheetKey = "1XHFx8xwOJFWUMAB9wrVmG10MFw4EHazeUnrrKBvpzY4", IgnoreCache = IgnoreCache };
            var results = sheet.ExecuteQuery("SELECT *");
            foreach (var result in results)
            {
                if (!result.ContainsKey(WORD_INDEX))
                {
                    continue;
                }
                string currentWord = result[WORD_INDEX];

                if (result.ContainsKey(CATEGORY_INDEX))
                {
                    WordCategory category;
                    string categoryAsString = result[CATEGORY_INDEX];
                    if (Enum.TryParse(categoryAsString, out category))
                    {
                        if (!CategoriesToInclude.Contains(category))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        Debug.WriteLine($"Unable to parse category {categoryAsString}");
                    }
                }

                switch (currentWord.Length)
                {
                    case 3:
                        _threeLetterWords.Add(currentWord);
                        break;
                    case 4:
                        _fourLetterWords.Add(currentWord);
                        break;
                    case 5:
                        _fiveLetterWords.Add(currentWord);
                        break;
                    case 6:
                        _sixLetterWords.Add(currentWord);
                        break;
                    case 7:
                        _sevenLetterWords.Add(currentWord);
                        break;
                    case 8:
                        _eightLetterWords.Add(currentWord);
                        break;
                    case 10:
                        _tenLetterWords.Add(currentWord);
                        break;
                }

                if (result.ContainsKey(HINT_INDEX))
                {
                    string hintForCurrentWord = result[HINT_INDEX];
                    if (!string.IsNullOrWhiteSpace(hintForCurrentWord))
                    {
                        if (!DictionaryOfClues.ContainsKey(currentWord))
                        {
                            DictionaryOfClues.Add(currentWord, hintForCurrentWord);
                        }
                    }
                }
            }
            _alreadyLoaded = true;
        }


        public void RemoveWordFromFile(string wordToRemove, string fileName)
        {
            StringBuilder newFileContents = new StringBuilder();
            foreach (string line in File.ReadAllLines(fileName))
            {
                if (line != wordToRemove)
                {
                    newFileContents.AppendLine(line);
                }
            }

            File.WriteAllText(fileName, newFileContents.ToString());
        }

        public List<string> WordsStartingWith(string startingCharacters, int wordLength = 5)
        {
            if (startingCharacters.Length == 1)
            {
                return WordsWithCharacterAtIndex(startingCharacters[0], 0, wordLength);

            }
            if (!_alreadyLoaded)
            {
                LoadAllWords();
            }
            List<string> matchingWords = new List<string>();
            List<string> wordsToSearch = null;


            wordsToSearch = GetWordsToSearchBaseOnLength(wordLength);


            if (wordsToSearch == null)
                throw new ArgumentException($"We don't support {wordLength} letter words yet.", nameof(wordLength));

            foreach (string word in wordsToSearch)
            {
                if (word.StartsWith(startingCharacters))
                {
                    matchingWords.Add(word);
                }
            }

            return matchingWords;
        }

        public List<string> WordsWithCharacterAtIndex(char expectedCharacter, int expectedIndex, int wordLength)
        {
            if (!_alreadyLoaded)
            {
                LoadAllWords();
            }

            List<string> matchingWords = new List<string>();
            List<string> wordsToSearch = null;

            wordsToSearch = GetWordsToSearchBaseOnLength(wordLength);


            if (wordsToSearch == null)
                throw new ArgumentException($"We don't support {wordLength} letter words yet.", nameof(wordLength));

            foreach (string word in wordsToSearch)
            {
                if (word[expectedIndex] == expectedCharacter)
                {
                    matchingWords.Add(word);
                }
            }

            return matchingWords;
        }

        public bool IsAWord(string wordCandidate)
        {

            if (!_alreadyLoaded)
            {
                LoadAllWords();
            }
            int wordLength = wordCandidate.Length;

            var wordsToSearch = GetWordsToSearchBaseOnLength(wordLength);
            return wordsToSearch.Contains(wordCandidate);
        }

        public string GetRandomWord(int wordLength = 5)
        {
            if (!_alreadyLoaded)
            {
                LoadAllWords();
            }

            List<string> listOfWords = _fiveLetterWords;
            if (wordLength == 6)
            {
                listOfWords = _sixLetterWords;
            }

            if (wordLength == 4)
            {
                listOfWords = _fourLetterWords;
            }
            int wordCount = listOfWords.Count;
            int randomIndex = _randomNumberGenerator.Next(wordCount);
            return listOfWords[randomIndex];
        }

        public string FindClueFor(string clueAsString)
        {
            if (!_alreadyLoaded)
            {
                LoadAllWords();
            }

            if (DictionaryOfClues.ContainsKey(clueAsString))
            {
                return DictionaryOfClues[clueAsString];
            }

            return null;
        }

        public List<string> GetRelatedWordsForTheme(string theme)
        {
            return themeRepository.FindWordsForTheme(theme);

            GoogleSheet sheet = new GoogleSheet() {GoogleSheetKey = "1ZJmh_woTIRDW1lspRX728GdkUc81J1K_iYeOoPYNfcA" };
            List<string> wordsForTheme = new List<string>();

            List<Dictionary<int, string>> findRelatedWords = sheet.ExecuteQuery(string.Format($@"SELECT * WHERE A = '{theme}'"));
            foreach (var dictionary in findRelatedWords)
            {
                if (dictionary.ContainsKey(1))
                {
                    string wordToAdd = dictionary[1];
                    if (!wordsForTheme.Contains(wordToAdd)) //skip any duplicates
                    {
                        wordsForTheme.Add(wordToAdd);
                    }
                }
            }

            return wordsForTheme;
        }

        public bool IsSingleSyllable(string word)
        {
            return IsSingleSyllable(word, out _, out _);
        }

        public bool IsSingleSyllable(string word, out string actualStartConsonant, out string actualEndConsonant)
        {
            word = word.ToLower();
            actualStartConsonant = null;
            actualEndConsonant = null;
            char firstCharacter = word[0];

            string[] groupsOfConsonants = word.Split(new[]
            {
                'a', 'e', 'i', 'o', 'u'
            }, StringSplitOptions.RemoveEmptyEntries);

            if (2 < groupsOfConsonants.Length) return false;
            char lastCharacter = word[word.Length - 1];
            if (1 == groupsOfConsonants.Length)
            {
                if (new List<char>() {'a', 'i', 'e', 'o', 'u', }.Contains(firstCharacter))
                {
                    actualEndConsonant = groupsOfConsonants[0];
                }
                else
                {
                    actualStartConsonant = groupsOfConsonants[0];
                }

                if (lastCharacter == 'y')//envy
                {
                    return false;
                }
                return true;
            }
            //remaining case: exactly two groups of consonants.
            //if it ends with another vowel (except e), probably 2 syllables. 
            if (new List<char>() {'a', 'i', 'o', 'u', 'y'}.Contains(lastCharacter))
            {
                return false;
            }

            if (word.EndsWith("ie")) //cookie
            {
                return false;
            }
            if (new List<char>() { 'a', 'i', 'e', 'o', 'u', 'y' }.Contains(firstCharacter))
            {
                return false;
            }

            actualStartConsonant = groupsOfConsonants[0];
            actualEndConsonant = groupsOfConsonants[1];
            return true;
        }

        public string GenerateRowsForGoogleSheet()
        {
            StringBuilder googleRowsBuilder = new StringBuilder();

            if (!_alreadyLoaded)
            {
                LoadAllWords();
            }

            foreach (string key in DictionaryOfClues.Keys)
            {
                googleRowsBuilder.AppendLine($"{key}\t{key.Length}\t{DictionaryOfClues[key]}");
            }

            foreach (string word in _oneLetterWords)
            {
                googleRowsBuilder.AppendLine($"{word}\t{word.Length}");
            }

            foreach (string word in _threeLetterWords)
            {
                googleRowsBuilder.AppendLine($"{word}\t{word.Length}");
            }
            foreach (string word in _fourLetterWords)
            {
                googleRowsBuilder.AppendLine($"{word}\t{word.Length}");
            }
            foreach (string word in _fiveLetterWords)
            {
                googleRowsBuilder.AppendLine($"{word}\t{word.Length}");
            }
            foreach (string word in _sixLetterWords)
            {
                googleRowsBuilder.AppendLine($"{word}\t{word.Length}");
            }

            return googleRowsBuilder.ToString();
        }

        public List<string> WordsMatchingPattern(string pattern)
        {
            if (!_alreadyLoaded)
            {
                LoadAllWords();
            }
            List<string> matchingWords = new List<string>();
            List<string> wordsToSearch = null;
            int wordLength = pattern.Length;

            wordsToSearch = GetWordsToSearchBaseOnLength(wordLength);


            if (wordsToSearch == null)
                throw new ArgumentException($"We don't support {wordLength} letter words yet.", nameof(wordLength));

            foreach (string word in wordsToSearch)
            {
                bool isAMatch = true;
                for (var index = 0; index < pattern.Length; index++)
                {
                    char letter = pattern[index];
                    if (letter == '_') continue;
                    if (word[index] != letter)
                    {
                        isAMatch = false;
                        break;
                    }
                }
                if (isAMatch)
                {
                    matchingWords.Add(word);
                }
            }

            return matchingWords;
        }

        private List<string> GetWordsToSearchBaseOnLength(int wordLength)
        {
            List<string> wordsToSearch = new List<string>();
            switch (wordLength)
            {
                case 1:
                    wordsToSearch = _oneLetterWords;
                    break;
                case 2:
                    wordsToSearch = _twoLetterWords;
                    break;
                case 3:
                    wordsToSearch = _threeLetterWords;
                    break;
                case 4:
                    wordsToSearch = _fourLetterWords;
                    break;
                case 5:
                    wordsToSearch = _fiveLetterWords;
                    break;
                case 6:
                    wordsToSearch = _sixLetterWords;
                    break;
                case 7:
                    wordsToSearch = _sevenLetterWords;
                    break;
                case 8:
                    wordsToSearch = _eightLetterWords;
                    break;
                case 10:
                    wordsToSearch = _tenLetterWords;
                    break;
                default:
                    throw new Exception($"Words of length {wordLength} are not supported yet.");
            }

            return wordsToSearch;
        }

        public WordCategory CategorizeWord(string word)
        {
            string html = WebRequestUtility.ReadHtmlPageFromUrl($"https://kids.wordsmyth.net/we/?ent={word}");
            if (html.Contains("<span class=\"dictionary\">Advanced Dictionary</span>"))
            {
                return WordCategory.AdvancedWord;
            }

            if (html.Contains("Did you mean this word?"))
            {
                return WordCategory.NotAWord;
            }

            if (html.Contains("<tr class=\"definition\">"))
            {
                return WordCategory.BasicWord;
            }

            return WordCategory.AdvancedWord;
        }

        public List<string> FindThemesForWord(string word)
        {
            return themeRepository.FindThemesForWord(word);
        }
    }
}