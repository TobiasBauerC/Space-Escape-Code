using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace GameData
{
    public static class Downloader
    {
        //Get these from your own URL
        const string SPREADSHEET_ID = "1GvHw-AHf6WG9b4BUynYco02D9wds4FdgGOpmSlLnSEE"; //after the d/
        const string CONSTANTS_TAB_ID = "0";
        const string LOCALIZATION_TAB_ID = "620389613"; //gid
        const string ASTEROIDS_TAB_ID = "1860923926";
        const string URL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&id={0}&gid={1}";

        //Get url by using File->Download As->CSV
        //Then open download history in chrome/firefox and copy url
        //Make sure accessiblity is set to Anyone with link can view

        public static void Init()
        {
            DownloadCSV(LOCALIZATION_TAB_ID, Localization.FilePath);
            DownloadCSV(CONSTANTS_TAB_ID, Constants.FilePath);
            DownloadCSV(ASTEROIDS_TAB_ID, Asteroids.FilePath);
        }

        private static void DownloadCSV(string tabID, string filePath)
        {

            string currentDownload = filePath.Replace(string.Format("{0}/", Application.persistentDataPath), "");

            //Get the formatted URL
            string downloadUrl = string.Format(URL_FORMAT, SPREADSHEET_ID, tabID);

            //Download the data
            WWW website = new WWW(downloadUrl);

            //Wait for data to download
            while (!website.isDone) { }

            if (string.IsNullOrEmpty(website.text))
            {
                Debug.LogError("NO DATA WAS RECEIVED");
                //Load the last cached values
            }
            else
            {
                //Successfully got the data, process it
                //Save to disk
                File.WriteAllText(filePath, website.text);
            }
        }
    }
}
