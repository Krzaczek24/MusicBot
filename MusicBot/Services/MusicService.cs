using Discord;
using Discord.WebSocket;
using MusicBot.Extensions;
using Microsoft.Extensions.Configuration;
using SharpLink;
using MusicBot.Configuration;

namespace MusicBot.Services
{
    public class MusicService
    {
        private const string _host = "localhost";
        private const int _port = 2333;
        public LavalinkManager Lavalink { get; private set; }

        public MusicService(IConfiguration config, DiscordSocketClient discord)
        {
            var lavalinkSettings = config.GetLavalinkSettings();

            Lavalink = new LavalinkManager(discord, new LavalinkManagerConfig()
            {
                RESTHost = lavalinkSettings.Host,
                RESTPort = lavalinkSettings.Port,
                WebSocketHost = lavalinkSettings.Host,
                WebSocketPort = lavalinkSettings.Port,
                Authorization = config.GetCredentials().LavalinkPassword,
                TotalShards = 1
            });

            discord.Ready += async () => await Lavalink.StartAsync();
        }

        public async Task<LavalinkPlayer> GetPlayer(SocketGuildUser user)
        {
            if (!user.IsBot)
                throw new InvalidOperationException("Only bots can play music");
            return Lavalink.GetPlayer(user.Guild.Id) ?? await Lavalink.JoinAsync(user.VoiceChannel);
        }

        public void Search(LavalinkPlayer player)
        {

        }

        public void Play()
        {

        }

        public void VolumeUp()
        {

        }

        public void VolumeDown()
        {

        }

        public void SwitchMute()
        {

        }
    }
}
