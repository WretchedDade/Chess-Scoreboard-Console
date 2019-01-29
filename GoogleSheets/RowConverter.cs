using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
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

        public static IEnumerable<T> FromRows<T>(IList<IList<object>> rows, string[] columnNames = null) where T : class
        {
            if (typeof(T).IsClass)
            {
                int startingIndex = 0;

                if (columnNames == null)
                {
                    columnNames = rows[0].Select(x => x.ToString()).ToArray();
                    startingIndex = 1;
                }

                for (int index = startingIndex; index < rows.Count; index++)
                    yield return FromRow<T>(rows[index], columnNames);
            }
        }

        public static IEnumerable<T> FromRows<T>(IList<IList<object>> rows) where T : struct
        {
            for (int index = 0; index < rows.Count; index++)
                yield return (T)rows[index][0];
        }

        public static IList<object> ToRow<T>(T obj)
        {
            var row = new List<object>();

            if(typeof(T).IsValueType)
                row.Add(obj);

            if (typeof(T).IsClass)
            {
                // Reflection code taken from https://stackoverflow.com/questions/4943817/mapping-object-to-dictionary-and-vice-versa/4944547#4944547
                IDictionary<string, object> dict = GetAsDictionary<T>(obj);

                foreach (KeyValuePair<string, object> pair in dict)
                    row.Add(pair.Value);
            }

            return row as IList<object>;
        }

        public static IEnumerable<IList<object>> ToRows<T>(IEnumerable<T> objects, string header) where T : struct
        {
            if (!string.IsNullOrWhiteSpace(header))
                yield return new List<object> { header } as IList<object>;

            foreach (T obj in objects)
                yield return ToRow(obj);
        }

        public static IEnumerable<IList<object>> ToRows<T>(IEnumerable<T> objects, bool prependHeaders = false)
        {
            if (prependHeaders && typeof(T).IsClass)
                yield return GetHeaderRow(objects.FirstOrDefault()).ToList() as IList<object>;

            foreach (T obj in objects)
                yield return ToRow(obj);
        }

        public static IEnumerable<object> GetHeaderRow<T>(T data)
        {
            var jObject = JObject.Parse(JsonConvert.SerializeObject(data));

            foreach (dynamic jToken in jObject.Children())
                yield return jToken.Name;
        }

        private static IDictionary<string, object> GetAsDictionary<T>(T obj)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(propertyInfo => propertyInfo.Name, propertyInfo => propertyInfo.GetValue(obj, null));
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
