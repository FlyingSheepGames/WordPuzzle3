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
            if (FieldExists("Theme", jObject))
            {
                return new WordSquare();
            }

            if (FieldExists("OriginalPhrase", jObject))
            {
                return new Anacrostic(jObject["OriginalPhrase"].Value<string>());
            }

            if (FieldExists("SpecialCharacter", jObject))
            {
                return new ReadDownColumnPuzzle();
            }

            if (FieldExists("IsPhraseSegmentPuzzle", jObject))
            {
                return new PhraseSegmentPuzzle();
            }

            if (FieldExists("isHiddenRelatedWordsPuzzle", jObject))
            {
                return new HiddenRelatedWordsPuzzle();
            }
            throw new Exception("Unable to determine type.");
        }
    }
}