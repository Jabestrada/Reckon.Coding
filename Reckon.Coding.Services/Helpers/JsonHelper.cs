using System.Text.Json;

namespace Reckon.Coding.Services.Helpers {
    public class JsonHelper {
        public static T ToObject<T>(string jsonString) {
            return JsonSerializer.Deserialize<T>(jsonString, 
                new JsonSerializerOptions { 
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
                });
        }
    }
}
