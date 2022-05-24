using System.Text.Json.Serialization;

namespace Praksa.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RpgClass
    {
        Knight,
        Mage,
        Claric

    }
}
