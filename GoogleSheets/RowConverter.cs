using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleSheets
{
    public class RowConverter
    {
        public static T FromRow<T>(IList<object> row, string[] columnNames) where T : class
        {
            if (row.Count() != columnNames.Length)
                throw new InvalidOperationException($"The row size of {row.Count()} does not match the number of column names provided ({columnNames.Length}).");

            dynamic dynamic = new ExpandoObject();

            string propertyName;
            object propertyValue;
            IDictionary<string, object> dictionary;

            for (int index = 0; index < row.Count(); index++)
            {
                propertyName = columnNames[index];
                propertyValue = row[index];
                dictionary = dynamic as IDictionary<string, object>;

                if (dictionary.ContainsKey(propertyName))
                    dictionary[propertyName] = propertyValue;
                else
                    dictionary.Add(propertyName, propertyValue);
            }

            string json = JsonConvert.SerializeObject(dynamic);

            return JsonConvert.DeserializeObject<T>(json);
        }

        public static IList<object> ToRow<T>(T data) where T : class
        {
            var dict = ((dynamic)data) as IDictionary<string, object>;

            var row = new List<object>();

            foreach (KeyValuePair<string, object> pair in dict)
                row.Add(pair.Value);

            return row as IList<object>;
        }

        public static IEnumerable<IList<object>> ToRows<T>(IEnumerable<T> objects, bool prependHeaders) where T : class
        {
            if (prependHeaders)
                yield return GetHeaderRow(objects.FirstOrDefault()) as IList<object>;

            foreach (T data in objects)
                yield return ToRow(data);
        }

        private static IEnumerable<object> GetHeaderRow<T>(T data)
        {
            string json = JsonConvert.SerializeObject(data);

            dynamic dynamicData = JObject.Parse(json);

            var dict = dynamicData as IDictionary<string, object>;

            foreach (KeyValuePair<string, object> pair in dict)
                yield return pair.Key;
        }

        private static void AddProperty(ExpandoObject expandoObject, string propertyName, object propertyValue)
        {
            var dict = expandoObject as IDictionary<string, object>;

            if (dict.ContainsKey(propertyName))
                dict[propertyName] = propertyValue;
            else
                dict.Add(propertyName, propertyValue);
        }
    }
}
