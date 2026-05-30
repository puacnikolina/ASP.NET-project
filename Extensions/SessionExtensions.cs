using System.Text.Json;

namespace Project.Extensions
{   
    public static class SessionExtensions
    {

        //da mozemo cart da cuvamo u sessionu, jer session podrzava samo stringove, a mi zelimo da cuvamo listu cart itema, pa serijalizujemo i deserijalizujemo tu listu u json format
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            session.SetString(key, json);
        }

        public static T? GetObject<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(json);
        }


    }
}
