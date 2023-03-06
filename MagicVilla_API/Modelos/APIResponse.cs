using System.Net;
using System.Text.Json.Serialization;

namespace MagicVilla_API.Modelos
{
    public class APIResponse
    {
        public HttpStatusCode statusCode { get; set; }

        public bool IsExitoso { get; set; }


        public List<string> ErrorMessages { get; set;}
        
        public object Resultado { get; set; }

        [JsonIgnore]
        public IServiceProvider serviceProvider { get; set; }

    }
}
