using System.Text.Json.Serialization;

namespace HMS.Models.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender : byte
    {
        Male = 1,
        Female
    }
}
