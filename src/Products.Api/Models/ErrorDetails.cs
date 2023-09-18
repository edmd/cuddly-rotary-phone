using Newtonsoft.Json;

namespace Products.Api.Models
{
    public class ErrorDetails : Resource
    {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public override string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
    }
}