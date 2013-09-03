using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using T1.CoreUtils.Entities;

namespace T1.CoreUtils
{
    public static class JS
    {

        public static string Stringify(object input, bool doNotWrapExceptions = false)
        {
            JObject ex = doNotWrapExceptions ? null : HandleError(input);
            return JsonConvert.SerializeObject(
                ex ?? input,
                Formatting.None,
                GetJsonSerializerSettings()
            );

        }

        public static string StringifyFormatted(object input, bool doNotWrapExceptions = false)
        {
            JObject ex = doNotWrapExceptions ? null : HandleError(input);
            return JsonConvert.SerializeObject(
                ex ?? input,
                Formatting.Indented,
                GetJsonSerializerSettings()
            );
        }

        private static JsonSerializerSettings GetJsonSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //TypeNameHandling = TypeNameHandling.Objects,
                Converters = { 
                    new IsoDateTimeConverter(), new DataTableConverter(), new DataSetConverter(), new StringEnumConverter()
                }
            };
        }

        private static JsonSerializer GetJsonSerializer()
        {
            return new JsonSerializer()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //TypeNameHandling = TypeNameHandling.Objects,
                Converters = { 
                    new IsoDateTimeConverter(), new DataTableConverter(), new DataSetConverter(), new StringEnumConverter()
                }
            };
        }

        public static JObject Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return null;
            return JObject.Parse(input);
        }

        public static JObject ToJObject(object input)
        {
            try
            {
                //already a string
                if (input.GetType() == typeof(string))
                {
                    var s = input as string;
                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        if (s.Length == 0) return null;
                        if (s[0] == '{') return JObject.Parse(s);
                    }
                }

                return JObject.FromObject(input, GetJsonSerializer());
            }
            catch (Exception ex)
            {
                //if the original was an exception, convert to SerialError...
                if (input as Exception != null)
                {
                    var ret = JObject.FromObject(SerialError.FromObject(input), GetJsonSerializer());
                    ret["__CONVERTED"] = new JValue("Converted to a SerialError");
                    ret["__CONVERTED_REASON"] = new JValue(ex.Message);
                    return ret;
                }

                throw;
            }
        }

        public static void ShallowMerge(ref JObject result, params JObject[] objects)
        {
            if (result == null) result = new JObject();
            foreach (JObject o in objects)
            {
                foreach (var prop in o)
                {
                    result[prop.Key] = null;
                    result[prop.Key] = prop.Value;
                }
            }
        }

        private static JObject HandleError(object input)
        {
            var ex = SerialError.FromObject(input);
            if (ex == null) return null;

            var ret = ToJObject(ex);
            try
            {
                if (App.UseDebugMode) ret["FullExceptionDetails"] = JS.ToJObject(input);
            }
            catch (Exception)
            {
                ret["FullExceptionDetails"] = new JValue(ex.ToString());
            }
            return ret;
        }
    }

}
