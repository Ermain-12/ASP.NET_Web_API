using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using Newtonsoft.Json.Serialization;
using WebApiContrib.Formatting.Jsonp;

namespace EmployeeService
{
    public class CustomJsonFormatter : JsonMediaTypeFormatter
    {
        public CustomJsonFormatter()
        {
            this.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("text/html"));
        }

        public override void SetDefaultContentHeaders(Type type, HttpContentHeaders headers, MediaTypeHeaderValue mediaType)
        {
            base.SetDefaultContentHeaders(type, headers, mediaType);
            headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
    }

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            /// Enable Cross-Origin Resource Sharing
            EnableCorsAttribute cors = new EnableCorsAttribute("http://localhost:53803/api/employee", "*", "*");
            config.EnableCors(cors);


            config.Filters.Add(new RequireHttpsAttribute());

            // --------------- Allows padded JSON from an external resource -------------------
            // var jsonpFormatters = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            // config.Formatters.Insert(0, jsonpFormatters);


            // Removes the XML(JSON) formatter 
            //config.Formatters.Remove(config.Formatters.JsonFormatter);

            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add
            //              (
            //                new System.Net.Http.Headers.MediaTypeHeaderValue("text/html")
            //          );

            ////////////////////////config.Formatters.Add(new CustomJsonFormatter());


            //// Here, we specify the return content type
            // Here, for indented json
            ///config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            // Specify the camel-case properties
            ///config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}
