﻿using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace WordPuzzles.Puzzle
{
    public class AnacrosticParameterSet
    {
        public string Phrase;
        public List<string> WordsToUse = new List<string>();
        public List<string> WordsToIgnore = new List<string>();
        public string TwitterUrl;
        public long TweetId;
        // ReSharper disable once InconsistentNaming
        private static readonly string BASE_DIRECTORY = ConfigurationManager.AppSettings["BaseDirectory"]; //@"E:\utilities\WordSquare\data\";

        public void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AnacrosticParameterSet));
            if (!Directory.Exists(BASE_DIRECTORY + @"anacrostics"))
            {
                Directory.CreateDirectory(BASE_DIRECTORY + @"anacrostics");
            }
            using (FileStream stream = File.OpenWrite(BASE_DIRECTORY + $@"anacrostics\parameter_set_{TweetId}.xml"))
            {
                serializer.Serialize(stream, this);
            }
        }

        public void Deserialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AnacrosticParameterSet));
            string filePath = BASE_DIRECTORY + $@"anacrostics\parameter_set_{TweetId}.xml";
            if (!File.Exists(filePath)) return;
            using (FileStream stream = File.OpenRead(filePath))
            {
                if (serializer.Deserialize(stream) is AnacrosticParameterSet setReadFromDisk)
                {
                    WordsToIgnore = setReadFromDisk.WordsToIgnore;
                    WordsToUse = setReadFromDisk.WordsToUse;
                }
            }
        }
    }
}