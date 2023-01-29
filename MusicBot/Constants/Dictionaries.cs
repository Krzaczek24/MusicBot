using MusicBot.Enums;

namespace MusicBot.Constants
{
    internal static class Dictionaries
    {
        public static IReadOnlyDictionary<MusicProvider, string> MusicProviderSearchKeyword { get; } = new Dictionary<MusicProvider, string>()
        {
            [MusicProvider.YouTube] = "ytsearch",
            [MusicProvider.Spotify] = "",
            [MusicProvider.SoundCloud] = "scsearch"
        };
    }
}
