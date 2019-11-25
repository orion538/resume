using System.IO;
using System.Text.Json;

namespace Resume.Domain
{
    public static class JsonParser
    {
        public static T GetModelFromJson<T>(JsonSerializerOptions options, string pathToFile)
        {
            var jsonString = File.ReadAllText(pathToFile);
            return JsonSerializer.Deserialize<T>(jsonString, options);
        }
    }
}
