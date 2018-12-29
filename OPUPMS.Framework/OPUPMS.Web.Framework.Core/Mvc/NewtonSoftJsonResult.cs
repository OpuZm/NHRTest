using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace OPUPMS.Web.Framework.Core.Mvc
{
    public class NewtonSoftJsonResult : ActionResult
    {
        public NewtonSoftJsonResult()
        {
            JsonRequestBehavior = JsonRequestBehavior.DenyGet;
        }

        public Encoding ContentEncoding { get; set; }

        public string ContentType { get; set; }

        public object Data { get; set; }

        public JsonRequestBehavior JsonRequestBehavior { get; set; }

        public Formatting Formatting { get; set; }

        public IList<JsonConverter> Converters { get; set; }

        public JsonSerializerSettings Settings { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (JsonRequestBehavior == JsonRequestBehavior.DenyGet &&
                String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This request has been blocked because sensitive information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (!String.IsNullOrEmpty(ContentType))
            {
                response.ContentType = ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (Data != null)
            {
                string json = null;
                if (Converters != null)
                {
                    if (Settings != null)
                    {
                        if (Settings.Converters == null)
                        {
                            Settings.Converters = Converters;
                        }
                        else
                        {
                            foreach (var item in Converters)
                            {
                                Settings.Converters.Add(item);
                            }
                        }

                        json = JsonConvert.SerializeObject(Data, Formatting, Settings);
                    }
                    else
                    {
                        json = JsonConvert.SerializeObject(Data, Formatting, Converters.ToArray());
                    }
                }
                else
                {
                    json = JsonConvert.SerializeObject(Data, Formatting, Settings);
                }

                response.Write(json);
            }
        }
    }
}
