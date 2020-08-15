using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace WordPuzzles.Utility
{
    //From https://stackoverflow.com/questions/41768215/english-json-dictionary-with-word-word-type-and-definition
    public class DataFromStackOverflowParser
    {
        public Dictionary<string, List<Clue>> ReadCluesFromFile(string fileToParse)
        {
            string words = File.ReadAllText(fileToParse);
            JObject results = JObject.Parse(words);
            var clues = new Dictionary<string, List<Clue>>();

            foreach (var result in results)
            {
                string wordToAdd = result.Key;

                if (!clues.ContainsKey(wordToAdd))
                {
                    clues[wordToAdd] = new List<Clue>();
                }
                var wordDetails = result.Value;
                var definitions = wordDetails["MEANINGS"];
                foreach (var definition in definitions)
                {
                    var definitionText = definition.First[1];
                    string clueText = definitionText.ToString();
                    clueText = clueText[0].ToString().ToUpperInvariant() + clueText.Substring(1);
                    //Console.WriteLine($"{wordToAdd}: {clueText}");
                    clues[wordToAdd].Add( 
                        new Clue()
                        {
                            ClueText = clueText, 
                            ClueSource = ClueSource.ClueSourceStackoverflowMeaning
                        });
                }
                var antonyms = wordDetails["ANTONYMS"];
                foreach (var antonymObject in antonyms)
                {
                    //Console.WriteLine($"{antonymObject}");
                    clues[wordToAdd].Add(
                        new Clue()
                        {
                            ClueText = "Opposite of " + antonymObject,
                            ClueSource = ClueSource.ClueSourceStackoverflowAntonym
                        });
                }
                var synonyms = wordDetails["SYNONYMS"];
                List<string> listOfSynonyms = new List<string>();
                foreach (var synonymObject in synonyms)
                {
                    //Console.WriteLine($"{synonymObject}");
                    string synonymCandidate = synonymObject.ToString();
                    if (!string.Equals(wordToAdd, synonymCandidate, StringComparison.InvariantCultureIgnoreCase))
                    {
                        listOfSynonyms.Add(synonymCandidate);
                    }
                }

                if (2 < listOfSynonyms.Count)
                {
                    clues[wordToAdd].Add(
                        new Clue()
                        {
                            ClueText = $"Synonyms include {listOfSynonyms[0]}, {listOfSynonyms[1]}, and {listOfSynonyms[2]}",
                            ClueSource = ClueSource.ClueSourceStackoverflowAntonym
                        });

                }

            }


            return clues;
        }
    }
}