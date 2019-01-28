using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using Newtonsoft.Json;

namespace ChessScoreboard.Core
{
    public class GoogleSheetsAPI : SheetsService
    {
        public GoogleSheetsAPI(Initializer initializer) : base(initializer) { }

        public IEnumerable<T> Get<T>(string spreadsheetId, string range, string[] columnNames = null) where T : class
        {
            bool pullColumnNamesFromRange = columnNames == null;

            ValueRange response = Spreadsheets.Values.Get(spreadsheetId, range).Execute();

            if (pullColumnNamesFromRange)
                columnNames = response.Values[0].Select(x => x.ToString()).ToArray();

            int startingIndex = pullColumnNamesFromRange ? 1 : 0;

            string json;
            IList<object> row;
            dynamic expandableObject;

            for (int i = startingIndex; i < response.Values.Count; i++)
            {
                row = response.Values[i];
                expandableObject = new ExpandoObject();

                for (int j = 0; j < row.Count(); j++)
                    AddProperty(expandableObject, columnNames[j], row[j]);

                json = JsonConvert.SerializeObject(expandableObject);

                yield return JsonConvert.DeserializeObject<T>(json);
            }
        }

        private void AddProperty(ExpandoObject expandoObject, string propertyName, object propertyValue)
        {
            var expandoDictionary = expandoObject as IDictionary<string, object>;

            if (expandoDictionary.ContainsKey(propertyName))
                expandoDictionary[propertyName] = propertyValue;
            else
                expandoDictionary.Add(propertyName, propertyValue);
        }

        public static GoogleSheetsAPI ForChessScoreboard()
        {
            UserCredential userCredential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                var dataStore = new FileDataStore(Constants.CredDataStoreLocation, true);
                var scopes = new List<string>() { Scope.Spreadsheets };
                ClientSecrets clientSecrets = GoogleClientSecrets.Load(stream).Secrets;

                userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, "user", CancellationToken.None, dataStore).Result;
            }

            var baseClientInitializer = new Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = Constants.AppName
            };

            return new GoogleSheetsAPI(baseClientInitializer);
        }

    }
}
