using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WordPuzzles.Utility
{
    class FtpHelper
    {
        public string Username;
        public string Password;
        public void ReadCredentialsFromFile()
        {
            string EXPECTED_CREDENTIALS_FILE = $@"{Environment.GetEnvironmentVariable("HOMEPATH") }\ftp.txt";
            if (!File.Exists(EXPECTED_CREDENTIALS_FILE))
            {
                throw new Exception($@"Please create a text file at {EXPECTED_CREDENTIALS_FILE}, where the first line is the FTP username, and the second is the password.");
            }
            var lines = File.ReadAllLines(EXPECTED_CREDENTIALS_FILE);
            Username = lines[0];
            Password = lines[1];
        }

        public bool CheckFileExists(string expectedFilename)
        {
            WebRequest request = WebRequest.Create("ftp://flyingsheep.com/" + expectedFilename);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            request.Credentials = new NetworkCredential(Username, Password);

            try
            {
                using (var response = (FtpWebResponse) request.GetResponse())
                {
                    Console.WriteLine($"Status code: {response.StatusCode}");
                    Console.WriteLine($"Response: {response}");
                    return 0 < response.ContentLength;
                }
            }
            catch (WebException exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        public void UploadFile(string sourceFile, string targetFile)
        {
            byte[] fileContents;
            using (StreamReader sourceStream = new StreamReader(sourceFile))
            {
                fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
            }
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://flyingsheep.com/" + targetFile);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            request.Credentials = new NetworkCredential(Username, Password);

            request.ContentLength = fileContents.Length;

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(fileContents, 0, fileContents.Length);
            }

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                Console.WriteLine($"Upload File Complete, status {response.StatusDescription}");
            }
        }
    }
}
