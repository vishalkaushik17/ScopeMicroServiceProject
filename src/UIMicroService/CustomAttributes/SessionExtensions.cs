//using GenericFunction;

//namespace ScopeCoreWebApp.CustomAttributes;

//public static class SessionExtensions
//{
//    public static void SetObjectAsJson(this ISession session, string key, object value)
//    {
//        session.SetString(key, value.ToJson());
//    }

//    public static T GetObjectFromJson<T>(this ISession session, string key)
//    {
//        var value = session.GetString(key);
//        return value == null ? default(T) : value.FromJsonToObject<T>();
//    }
//}