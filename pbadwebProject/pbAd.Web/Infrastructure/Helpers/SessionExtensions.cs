using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pbAd.Web.Infrastructure.Helpers
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value.ToString());
        }

        public static void SetBoolean(this ISession session, string key, bool value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static bool? GetBoolean(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return null;
            }
            return BitConverter.ToBoolean(data, 0);
        }

        public static void SetDouble(this ISession session, string key, double value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static double GetDouble(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return 0;
            }
            return BitConverter.ToDouble(data, 0);
        }
        public static void SetInt(this ISession session, string key, int value)
        {
            session.Set(key, BitConverter.GetBytes(value));
        }

        public static int GetInt(this ISession session, string key)
        {
            var data = session.Get(key);
            if (data == null)
            {
                return 0;
            }
            return BitConverter.ToInt32(data, 0);
        }
    }
}
