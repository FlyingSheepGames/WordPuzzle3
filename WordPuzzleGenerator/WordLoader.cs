using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WordPuzzles.Puzzle;
using WordPuzzles.Utility;

namespace WordPuzzleGenerator
{
    public class WordLoader
    {
        // ReSharper disable once InconsistentNaming
        static readonly WordRepository WordRepository = new WordRepository() { ExcludeAdvancedWords = true };

        private static void LoadTenLetterWords()
        {
            StringBuilder rowsToAdd = new StringBuilder();
            foreach (string word in File.ReadAllLines(@"C:\Users\cbeauvai\source\repos\WordPuzzle3\WordPuzzlesTest\data\10LetterWords.txt"))
            {
                if (word.Length == 10)
                {
                    var category = WordRepository.CategorizeWord(word);
                    string newWordRow = $"{word}\t{category}\t{word.Length}";
                    rowsToAdd.AppendLine(newWordRow);
                    Console.WriteLine(newWordRow);
                }
            }
            Clipboard.SetText(rowsToAdd.ToString());
            Console.WriteLine("Results copied to clipboard. Press a key to continue.");
            Console.ReadKey();
        }

    }
}