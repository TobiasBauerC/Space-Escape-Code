using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameData
{
    public static class Localization
    {
        //The dictionary
        static Dictionary<string, string> localizations;

        public static string FilePath
        {
            get
            {
                string persistentPath = Application.persistentDataPath;
                string savePath = string.Format("{0}/Localization.csv", persistentPath);
                return savePath;
            }
        }

        private static void Initialization()
        {
            localizations = new Dictionary<string, string>();
            //Load the CSV

            string[] rows;
            if (File.Exists(FilePath))
            {
                StreamReader sr = File.OpenText(FilePath);
                rows = sr.ReadToEnd().Split('\n');
            }
            else
            {
                Debug.LogFormat("{0} does not exist", FilePath);
                TextAsset data = Resources.Load<TextAsset>("Localization");
                rows = data.text.Split('\n');
            }

            //Get the system language
            string language = Application.systemLanguage.ToString();

            string[] languages = rows[0].Split(',');

            //Default language to english
            int languageIndex = 1;

            for (int i = 1; i < languages.Length; i++)
            {
                //Get the language index
                if (languages[i] == language)
                {
                    languageIndex = i;
                    break;
                }
            }

            for (int i = 1; i < rows.Length; i++)
            {
                //Parse the csv into the dictionary
                string[] cols = rows[i].Split(',');

                string key = cols[0];
                string value = cols[languageIndex];

                localizations.Add(key, value);
            }
        }

        //Get the localized string
        public static string Get(string pKey)
        {
            //while (!Downloader.isDownloaded) { }

            //Make sure the CSV has been loaded
            if (localizations == null)
                Initialization();

            return localizations[pKey];
        }

        //A fancy way to get the localization string, not necessary 
        public static string Localize(this string pKey)
        {
            return Get(pKey);
        }
    }
}