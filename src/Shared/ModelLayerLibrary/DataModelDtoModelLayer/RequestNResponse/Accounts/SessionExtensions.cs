using GenericFunction;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ModelTemplates.RequestNResponse.Accounts;

/// <summary>
/// Session management extension class to get / set value on session.
/// </summary>
public static class SessionExtensions
{
    public static void SetObjectAsJsonInSession(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value, Formatting.Indented,
    new JsonSerializerSettings()
    {
        ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    }));
    }

    public static T GetObjectFromJsonFromSession<T>(this ISession session, string key)
    {
        var value = session.GetString(key);

        return value == null ? default(T) : value.FromJsonToObject<T>();
    }
}
