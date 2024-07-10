using System;
using System.Collections;
using UnityEngine.Networking;

namespace Urd.Utils
{
	public class GoogleSheetLoader
	{
		public const int RETRIES = 3;

		private const string EXCEL_FORMAT = "https://docs.google.com/spreadsheets/d/{0}/export?format=tsv&gid={1}";

		private int _retry;

		public IEnumerator LoadTextDataCo(string sheetUrlKey, string sheetPageKey, Action<bool, string[]> callback)
		{
			// create a new "WWW" object that will fetch the web data
			UnityWebRequest webRequest = UnityWebRequest.Get(String.Format(EXCEL_FORMAT, sheetUrlKey, sheetPageKey));

			// wait until the web data has finished downloading
			yield return webRequest.SendWebRequest();
			// you could also see whether (WWW.isDone == true) to see if it has finished yet

			if (webRequest.result != UnityWebRequest.Result.Success)
			{
				callback?.Invoke(false, new string[0]);
				yield break;
			}

			// treat the web data like alphanumeric text (a string)
			var csvData = webRequest.downloadHandler.text;

			// convert text into rows by splitting along line breaks
			string[] rows = csvData.Split("\n"[0]);

			callback?.Invoke(true, rows);
		}
	}
}
