using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Utility
{
    [TestFixture()]
    public class AnagramFinderTest
    {
        [TestFixture]
        public class FindAnagrams
        {
            [Test]
            public void Example_FindsExpectedWords()
            {
                AnagramFinder finder = new AnagramFinder();
                List<string> results = finder.FindAnagram("teen nine");
                Assert.AreEqual("nineteen", results[0]);
            }

            [Test]
            public void ExampleWithNoResults_ReturnsEmptyList()
            {
                AnagramFinder finder = new AnagramFinder();
                // ReSharper disable StringLiteralTypo
                List<string> results = finder.FindAnagram("hitittttt");
                // ReSharper restore StringLiteralTypo

                Assert.AreEqual(0, results.Count);
            }

            [Test]
            public void Example_ReturnsListSortedByLength()
            {
                AnagramFinder finder = new AnagramFinder {MinimumWordLength = 2};
                List<string> results = finder.FindAnagram("teen nine");

                Assert.AreEqual(12, results.Count);
                int previousWordLength = results[0].Length;
                foreach (string word in results)
                {
                    Assert.LessOrEqual(word.Length, previousWordLength, $"Expected {word} to have length less than {previousWordLength}, but it has length {word.Length}");
                    previousWordLength = word.Length;
                }
            }

        }

        [TestFixture]
        public class ParseResponse
        {
            [Test]
            [SuppressMessage("ReSharper", "StringLiteralTypo")]
            public void Example_ReturnsExpectedList()
            {
                #region const string EXAMPLE_HTML = @"....
                const string EXAMPLE_HTML = @"
<!DOCTYPE HTML>
<html>
<head>
   <TITLE>The Internet Anagram Server : 
Anagrams for: nineteen</TITLE>
	<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
   <META name=""description"" content=""Discover the magic of anagrams with the Internet Anagram Server"">
   <META name=""keywords"" content=""internet, anagram, server, pangram, word, fun, hall, fame, wordserver, wordplay, pun, crossword, transmogrification, transmogrify, rearrangement, servant, Inert Net Grave Near Mars, quotations, quotes"">
   <META name=""author"" content=""Anu Garg (garg AT wordsmith.org)"">
	<meta http-equiv=Content-Type content=""text/html; charset=windows-1252"">
	<link href=""/awad/style.css"" type=""text/css"" rel=""stylesheet"">
	<base href=//wordsmith.org/anagram/>
<script src=""/misc.js"" type=""text/javascript""></script>

<script type=""text/javascript"">
function validate()
{
   document.redirect_form.submit();
   return true;
}
</script>

</head>

<body bgcolor=""#FFFFFF"" topmargin=""0"" leftmargin=""0"" rightmargin=""0"" marginwidth=""0"" marginheight=""0"">

<script async src=""//pagead2.googlesyndication.com/pagead/js/adsbygoogle.js""></script>
<script>
     (adsbygoogle = window.adsbygoogle || []).push({
          google_ad_client: ""ca-pub-0068747404870456"",
          enable_page_level_ads: true
     });
</script>

<script type=""text/javascript"">
(function() {
  var ARTICLE_URL = window.location.href;
  var CONTENT_ID = 'everything';
  document.write(
    '<scr'+'ipt '+
    'src=""//survey.g.doubleclick.net/survey?site=_ecjxdrc2oz3xrvetrvilu74c7y'+
    '&amp;url='+encodeURIComponent(ARTICLE_URL)+
    (CONTENT_ID ? '&amp;cid='+encodeURIComponent(CONTENT_ID) : '')+
    '&amp;random='+(new Date).getTime()+
    '"" type=""text/javascript"">'+'\x3C/scr'+'ipt>');
})();
</script>

<!-- ==============================Top Bar================================= -->
<topbar>
<form name=redirect_form method=""post"" action=""/awad/go.cgi"">
<table width=""100%"" cellspacing=""0"" cellpadding=""2"" border=""0"" bgcolor=""#bbbbbb"">
<tr>
<td class=""topbar"" align=""left"" valign=""middle""><font face=""verdana,arial,sans-serif"" color=""white"">&nbsp; <a href=""/"" class=""homelink""><b>Wordsmith.org</b>: the magic of words</a></font></td>
   <td align=right valign=middle>
      <select name=""service"" onChange=""return validate(redirect_form)"">
         <option value="""">Other Services</option>
         <option value=""awad"">A.Word.A.Day</option>
			<option value=""anagram"">Internet Anagram Server</option>
			<option value=""anagramtimes"">The Anagram Times</option>
			<option value=""pangram"">Pangram Finder</option>
         <option value=""board"">Wordsmith Talk</option>
         <option value=""chat"">Wordsmith Chat</option>
         <option value=""wordserver"">Wordserver</option>
         <option value=""listat"">Listat</option>
      </select>
      <noscript>
      <input type=""submit"" value=""Go""> &nbsp;
      </noscript>
   </td>
</tr>
</table>
</form>
<p>
</topbar>

<toplinks>
<div class=""centered"">
<a href=""index.html""><img src=""https://lh6.ggpht.com/wordsmith.org/SDuzuqjcWAI/AAAAAAAAABg/L-KYUA1Yuhc/s800/masthead.gif"" height=""64"" width=""430"" alt=""I, Rearrangement Servant OR Internet Anagram Server"" /></a><br>
<A HREF=""about.html"" class=""anagram_menu"">About</A>
<A HREF=""advanced.html"" class=""anagram_menu"">Advanced</A>
<A HREF=""hof.html"" class=""anagram_menu"">Hall of Fame</A>
<A HREF=""anagram-check.html"" class=""anagram_menu"">Checker</a>
<A HREF=""animation.html"" class=""anagram_menu"">Animation</A>
<A HREF=""odd.html"" class=""anagram_menu"">Odds&nbsp;&amp;&nbsp;Ends</A>
<A HREF=""faq.html"" class=""anagram_menu"">FAQ</A>
<A HREF=""tips.html"" class=""anagram_menu"">Tips</A>
<A HREF=""practical.html"" class=""anagram_menu"">Uses</A>
<A HREF=""search.html"" class=""anagram_menu"">Search</A>
<a href=""contribute.html"" class=""anagram_menu"">Contribute</a>
<A HREF=""feedback.html"" class=""anagram_menu"">Contact</A><br>
<A HREF=""https://www.anagramtimes.com/"" class=""anagram_menu"">The Anagram Times</A>
</div>
<P>
</toplinks>
<blockquote>

<h3>
Anagrams for: nineteen</h3>

<div class=""p402_premium"">
  <!-- YOUR PREMIUM CONTENT HERE -->
<div class=""p402_hide"">
   <!-- YOUR OPTIONAL HIDDEN CONTENT HERE -->
   <p style=""width:220px;float:right;border:1px dotted;border-color:#99CCFF;padding:10px;font-size:10pt;color:#333333""><b>Thought of the Moment</b><br><br>HELP! MY TYPEWRITER IS BROKEN! -E.E. CUMMINGS<br><br><a href=""/awad/subscribe.html"">Receive quotations (and words) in our daily newsletter</a>. It's free.</p>
</div> 
Try <a href=advanced.html>advanced</a> options to fine-tune these anagram results.<p>
<script>document.body.style.cursor='wait';</script><b>8 found. Displaying all:
</b><br>Nineteen<br>
Teen Nine<br>
Nee En Tin<br>
Nee En Nit<br>
Nee Net In<br>
Nee Ten In<br>
Teen En In<br>
Tee En Inn<br>
<script>document.body.style.cursor='default';</script></div>
<script type=""text/javascript"">
  try { _402_Show(); } catch(e) {}
</script>

<bottomlinks>
<p><br>
<hr noshade align=""center"" size=""1"" width=""265"">
<div style=""text-align:center; font-size: 9pt;"">
<a href=""new.html"">What's New</a> |
<a href=""awards.html"">Awards &amp; Articles</a> |
<a href=""siteindex.html"">Site&nbsp;Map</A><br>
<a href=""anagrams.html"">Anagrams Found</a>
<p>
<span style=""color:gray;font-size:8pt;"">&copy; 1994-2018 Wordsmith</span>
</div>
</bottomlinks>
</blockquote>

<script>
(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
})(window,document,'script','//www.google-analytics.com/analytics.js','ga');
ga('create', 'UA-32209-1', 'wordsmith.org');
ga('require', 'displayfeatures');
ga('send', 'pageview');
</script>

</body>
</html>
";
#endregion
                AnagramFinder finder = new AnagramFinder();
                var results = finder.ParseResponse(EXAMPLE_HTML);
                Assert.AreEqual("nineteen", results[0]);
                Console.WriteLine(string.Join(",", results));
            }


        }
    }
}