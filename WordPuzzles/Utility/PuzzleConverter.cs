using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WordPuzzles.Puzzle;

namespace WordPuzzles.Utility
{
    public class PuzzleConverter : JsonCreationConverter<IPuzzle>
    {
        private bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        protected override IPuzzle Create(Type objectType, JObject jObject)
        {
            if (FieldExists("IsWordSquare", jObject))
            {
                return new WordSquare();
            }
            if (FieldExists("IsMultipleCluesPuzzle", jObject))
            {
                return new MultipleCluesPuzzle();
            }

            if (FieldExists("IsWordSearchMoreOrLess", jObject))
            {
                return new WordSearchMoreOrLess();
            }

            if (FieldExists("IsAnacrostic", jObject))
            {
                return new Anacrostic(jObject["OriginalPhrase"].Value<string>());
            }

            if (FieldExists("IsReadDownColumnPuzzle", jObject))
            {
                return new ReadDownColumnPuzzle();
            }

            if (FieldExists("IsPhraseSegmentPuzzle", jObject))
            {
                return new PhraseSegmentPuzzle();
            }

            if (FieldExists("IsHiddenRelatedWordsPuzzle", jObject))
            {
                return new HiddenRelatedWordsPuzzle();
            }

            if (FieldExists("IsLettersAndArrowsPuzzle", jObject))
            {
                return new LettersAndArrowsPuzzle(jObject["Size"].Value<int>());
            }

            throw new Exception("Unable to determine type.");
        }
    }
}