using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Resume.Domain
{
    public static class JsonParser
    {
        private const string PathToSchema = "Schemas/resume.json";

        public static T GetModelFromJson<T>(JsonSerializerOptions options, string pathToFile)
        {
            var jsonString = File.ReadAllText(pathToFile);

            var schema = JSchema.Parse(File.ReadAllText(PathToSchema));
            var json = JObject.Parse(jsonString);

            if (json.IsValid(schema, out IList<string> errorMessages))
            {
                return JsonSerializer.Deserialize<T>(jsonString, options);
            }

            throw new ArgumentException(string.Join(", \r\n", errorMessages.ToArray()));
        }
    }
}
