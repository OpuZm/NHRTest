using Newtonsoft.Json;

namespace OPUPMS.Web.Framework.Core.Json
{
    public static class JsonDefaultSerializerSettings
    {
        static readonly JsonSerializerSettings _ignoreNullValue;
        static readonly JsonSerializerSettings _ignoreNullValueAndFormatDateTime;

        static JsonDefaultSerializerSettings()
        {
            _ignoreNullValue = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            _ignoreNullValueAndFormatDateTime = new JsonSerializerSettings
            {
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public static JsonSerializerSettings IgnoreNullValue
        {
            get { return _ignoreNullValue; }
        }

        public static JsonSerializerSettings IgnoreNullValueAndFormatDateTime
        {
            get { return _ignoreNullValueAndFormatDateTime; }
        }
    }
}
