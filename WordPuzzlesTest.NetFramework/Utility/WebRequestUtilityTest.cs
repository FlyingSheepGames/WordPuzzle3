using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Utility
{
    [TestFixture]
    class WebRequestUtilityTest
    {
        [TestFixture]
        public class ReadHtmlPageFromUrl
        {
            [Test]
            public void BGG_RequestCountExceeded_ReturnsNull()
            {
                WebRequestUtilityInstance utility = new WebRequestUtilityInstance() { BggTooManyRequests = true};
                Assert.IsNull(utility.ReadHtmlPageFromUrl("http://boardgamegeek.com/"));
            }

            [Test]
            public void FailedRequest_ReturnsNull()
            {
                WebRequestUtilityInstance utility = new WebRequestUtilityInstance();
                const string EXAMPLE_URL = "http://www.example.com";
                utility.FailedRequests.Add(EXAMPLE_URL);
                Assert.IsNull(utility.ReadHtmlPageFromUrl(EXAMPLE_URL));
            }
        }
    }
}
