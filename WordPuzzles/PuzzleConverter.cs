using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WordPuzzles
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
            throw new Exception("Unable to determine type.");
        }
    }
}