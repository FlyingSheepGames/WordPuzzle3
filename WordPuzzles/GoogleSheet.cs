using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json.Linq;

namespace WordPuzzles
{
    public class GoogleSheet
    {
        public string GoogleSheetKey;
        public bool IgnoreCache = true;
        
        internal static string EncodeQuery(string query)
        {
            return HttpUtility.UrlEncode(query);
        }

        public List<Dictionary<int, string>> ExecuteQuery(string query, string sheetName = null)
        {
            string encodedQuery = EncodeQuery(query);
            string url =
                @"https://spreadsheets.google.com/tq?tqx=out:json&tq=" + encodedQuery +
                @"&key=" + GoogleSheetKey;

            if (!string.IsNullOrWhiteSpace(sheetName))
            {
                url += $"&sheet={EncodeQuery(sheetName)}";
            }

            string response = WebRequestUtility.ReadHTMLPageFromUrl(url, IgnoreCache);

            var results = ReadGenericDictionaryFromResponse(response);

            return results;
        }

        private static List<Dictionary<int, string>> ReadGenericDictionaryFromResponse(string response)
        {
            List<Dictionary<int, string>> entries = new List<Dictionary<int, string>>();

            response = response.Substring(@"/*O_o*/google.visualization.Query.setResponse(".Length + 1);
            response = response.Substring(0, response.Length - 2); //trim last two ");"
            //Debug.WriteLine(response);

            JObject responseAsObject = JObject.Parse(response);
            //foreach (KeyValuePair<string, JToken> node in responseAsObject)

            JToken table = responseAsObject["table"];
            if (table == null)
            {
                return entries;
            }
            foreach (var row in table["rows"])
            {
                Dictionary<int, string> currentDictionary = new Dictionary<int, string>();
                //Console.WriteLine(row);
                //Console.WriteLine(row["c"]);
                //Console.WriteLine(row["c"][3]);
                //Console.WriteLine(row["c"][3]["v"]);
               
                /*
                if (row["c"][0]["v"].ToString() == "Type")
                {
                    Console.WriteLine(row);
                }
                */
                int index = 0;
                foreach (var item in row["c"])
                {
                    if (item.HasValues)
                    {
                        currentDictionary.Add(index, (string) item["v"]);
                    }
                    index++;
                }
                entries.Add(currentDictionary);
            }

            return entries;
        }
    }
}
