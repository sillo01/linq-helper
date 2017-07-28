using Newtonsoft.Json;

namespace LinqHelper
{
    public interface IDataEntity
    {
        [JsonIgnore]
        string Id { get; }
    }
}
