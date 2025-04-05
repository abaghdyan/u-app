using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VistaLOS.Application.Common.Helpers;

public static class JsonHelper
{
    public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new() {
        ContractResolver = new CamelCasePropertyNamesContractResolver()
    };

    public static T? DeserializeObject<T>(string? value)
        where T : class
    {
        if (value == null) {
            return null;
        }

        try {
            return JsonConvert.DeserializeObject<T>(value, DefaultJsonSerializerSettings);
        }
        catch {
            return null;
        }
    }

    public static string SerializeObject(object? value)
    {
        return JsonConvert.SerializeObject(value, DefaultJsonSerializerSettings);
    }
}
