using Newtonsoft.Json;

namespace Guayaba.LinqHelper
{
    public interface IDataEntity
    {
        [JsonIgnore]
        string Id { get; }
    }
}
