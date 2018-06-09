using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameData
{
    public static class Constants
    {
        private static Dictionary<string, object> values;

        public static string FilePath
        {
            get
            {
                string persistentPath = Application.persistentDataPath;
                string savePath = string.Format("{0}/Constants.csv", persistentPath);
                return savePath;
            }
        }

        private static void Initialization()
        {
            values = new Dictionary<string, object>();

            //get the data from the csv

            string[] rows;
            if (File.Exists(FilePath))
            {
                StreamReader sr = File.OpenText(FilePath);
                rows = sr.ReadToEnd().Split('\n');
            }
            else
            {
                Debug.LogFormat("{0} does not exist", FilePath);
                TextAsset data = Resources.Load<TextAsset>("Constants");
                rows = data.text.Split('\n');
            }

            for (int i = 1; i < rows.Length; i++)
            {
                string[] cols = rows[i].Split(',');
                string key = cols[0];
                string type = cols[1];
                string value = cols[2];

                switch (type)
                {
                    case "Int":
                        int iValue;

                        if (int.TryParse(value, out iValue))
                        {
                            values.Add(key, iValue);
                        }
                        break;

                    case "Float":
                        float fValue;

                        if (float.TryParse(value, out fValue))
                        {
                            values.Add(key, fValue);
                        }
                        break;

                    case "String":
                        values.Add(key, value);
                        break;
                }
            }
        }

        public static T Get<T>(string pKey)
        {
            //while (!Downloader.isDownloaded) { }
            if (values == null)
                Initialization();

            return (T)values[pKey];
        }
    }
}