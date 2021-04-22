using NUnit.Framework;
using WordPuzzles.Utility;

namespace WordPuzzlesTest.Utility
{
    [TestFixture]
    public class FtpHelperTest
    {
        [TestFixture]
        public class UploadFile
        {
            [Test]
            public void PutsFileOnServer()
            {
                FtpHelper helper = new FtpHelper();
                helper.ReadCredentialsFromFile();
                helper.UploadFile(@"data\area.txt", "area.txt");

                Assert.IsTrue(helper.CheckFileExists("area.txt"));
            }
        }

        [TestFixture]
        public class CheckFileExists
        {
            [Test]
            public void ExistingFile_ReturnsTrue()
            {
                FtpHelper helper = new FtpHelper();
                helper.ReadCredentialsFromFile();
                Assert.IsTrue(helper.CheckFileExists("index.htm"));
            }

            [Test]
            public void ExistingFile_InSubdirectory_ReturnsTrue()
            {
                FtpHelper helper = new FtpHelper();
                helper.ReadCredentialsFromFile();
                Assert.IsTrue(helper.CheckFileExists(@"anacrostics\index.html"));
            }

            [Test]
            public void FakeFile_ReturnsFalse()
            {
                FtpHelper helper = new FtpHelper();
                helper.ReadCredentialsFromFile();
                Assert.IsFalse(helper.CheckFileExists("this_file_does_not_exist.htm"));
            }
        }
    }
}