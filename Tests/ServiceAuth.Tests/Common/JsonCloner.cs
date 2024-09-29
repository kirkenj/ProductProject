using System.Text.Json;

namespace ServiceAuth.Tests.Common
{
    public static class JsonCloner
    {
        public static T Clone<T>(T obj)
        {
            return JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(obj)) ?? throw new Exception();
        }
    }
}
