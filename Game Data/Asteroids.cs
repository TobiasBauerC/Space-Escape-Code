using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameData
{
    public static class Asteroids
    {
        private static Dictionary<string, AsteroidInfo> entries;

        public static string FilePath
        {
            get
            {
                string dataPath = Application.persistentDataPath;
                string filePath = string.Format("{0}/Asteroids.csv", dataPath);
                return filePath;
            }
        }

        private static void Initialization()
        {
            entries = new Dictionary<string, AsteroidInfo>();

            string asteroidsCSV;
            if (File.Exists(FilePath))
            {
                asteroidsCSV = File.ReadAllText(FilePath);
            }
            else
            {
                asteroidsCSV = Resources.Load<TextAsset>("Asteroids").ToString();
            }

            string[] rows = asteroidsCSV.Split('\n');

            for (int i = 1; i < rows.Length; ++i)
            {
                string[] values = rows[i].Split(',');

                string name = values[0];
                float speed = float.Parse(values[1]);
                int damage = int.Parse(values[2]);
                Vector3 scale = new Vector3(float.Parse(values[3]), float.Parse(values[3]), float.Parse(values[3]));

                AsteroidInfo a = new AsteroidInfo(name, speed, damage, scale);

                entries.Add(name, a);
            }
        }

        public static AsteroidInfo Get(string pKey)
        {
            if (entries == null)
                Initialization();

            return entries[pKey];
        }
    }

    public class AsteroidInfo
    {
        public string name
        {
            get;
            private set;
        }

        public float speed
        {
            get;
            private set;
        }

        public int damage
        {
            get;
            private set;
        }

        public Vector3 scale
        {
            get;
            private set;
        }

        public AsteroidInfo(string pName, float pSpeed, int pDamage, Vector3 pScale)
        {
            name = pName;
            speed = pSpeed;
            damage = pDamage;
            scale = pScale;
        }
    }
}
