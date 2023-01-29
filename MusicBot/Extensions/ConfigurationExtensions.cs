using MusicBot.Configuration;
using Microsoft.Extensions.Configuration;
using MusicBot.Configuration.UserSecretsSections;

namespace MusicBot.Extensions
{
    internal static class ConfigurationExtensions
    {
        public static T? GetSection<T>(this IConfiguration configuration)
        {
            return configuration.GetSection(typeof(T).Name).Get<T>();
        }

        public static Credentials GetCredentials(this IConfiguration configuration)
        {
            return configuration.Get<UserSecrets>()!.Credentials!;
        }

        public static LavalinkSettings GetLavalinkSettings(this IConfiguration configuration)
        {
            return configuration.GetSection<LavalinkSettings>()!;
        }
    }
}
