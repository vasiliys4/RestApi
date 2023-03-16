using RestApi.Models.Base;

namespace RestApi.Models
{
    public class Car: BaseModel
    {
        public string? Name { get; set; }
        public string? Number { get; set; }
    }
}
