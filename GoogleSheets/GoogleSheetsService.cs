using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;
using InputOption = Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum;

namespace GoogleSheets
{
    public class GoogleSheetsService : SheetsService
    {
        public GoogleSheetsService(Initializer initializer) : base(initializer) { }

        public IEnumerable<T> Get<T>(string spreadsheetId, string range, string[] columnNames = null) where T : class
        {
            IList<IList<object>> rows = Spreadsheets.Values.Get(spreadsheetId, range).Execute().Values;

            int index = 0;

            if (columnNames == null)
            {
                columnNames = rows[0].Select(x => x.ToString()).ToArray();
                index = 1;
            }

            for (; index < rows.Count; index++)
                yield return RowConverter.FromRow<T>(rows[index], columnNames);
        }

        public void Update<T>(List<T> data, string spreadsheetId, string range, InputOption inputOption = InputOption.USERENTERED, bool headersIncluedInRange = false) where T : class
        {
            var rows = RowConverter.ToRows(data, headersIncluedInRange) as IList<IList<object>>;

            var requestBody = new ValueRange { Values = rows };

            UpdateRequest request = Spreadsheets.Values.Update(new ValueRange { Values = rows }, spreadsheetId, range);
            request.ValueInputOption = inputOption;

            request.Execute();
        }

        public async Task UpdateAsync<T>(List<T> data, string spreadsheetId, string range, InputOption inputOption = InputOption.USERENTERED, bool headersIncluedInRange = false) where T : class
        {
            var rows = RowConverter.ToRows(data, headersIncluedInRange) as IList<IList<object>>;

            var requestBody = new ValueRange { Values = rows };

            UpdateRequest request = Spreadsheets.Values.Update(new ValueRange { Values = rows }, spreadsheetId, range);
            request.ValueInputOption = inputOption;

            await request.ExecuteAsync();
        }

        public static GoogleSheetsService New(string credentialsURL, string appName, List<string> scopes = null, string credDataStoreLocation = "token.json")
        {
            if (scopes == null)
                scopes = new List<string> { Scope.Spreadsheets };

            UserCredential userCredential;

            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                var dataStore = new FileDataStore(credDataStoreLocation, true);
                ClientSecrets clientSecrets = GoogleClientSecrets.Load(stream).Secrets;

                userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, scopes, "user", CancellationToken.None, dataStore).Result;
            }

            var baseClientInitializer = new Initializer()
            {
                HttpClientInitializer = userCredential,
                ApplicationName = appName
            };

            return new GoogleSheetsService(baseClientInitializer);
        }
    }
}
