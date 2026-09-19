using System.Text.Json.Serialization;

namespace SideApp.Model
{
    internal class Kanji
    {
        [JsonPropertyName("kanji")]
        public string kanji { get; set; }
        [JsonPropertyName("kun_readings")]
        public string[]? ReadingKun { get; set; }
        [JsonPropertyName("on_readings")]
        public string[]? ReadingOn { get; set; }
        [JsonPropertyName("meanings")]
        public string[] Meaning { get; set; }
        [JsonPropertyName("grade")]
        public int Grade { get; set; }
    }
}
