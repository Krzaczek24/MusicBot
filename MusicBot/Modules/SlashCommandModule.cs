using Discord;
using Discord.Commands;
using Discord.Interactions;
using DiscordCommon.Constants;
using MusicBot.Constants;
using MusicBot.Enums;
using MusicBot.Services;

namespace MusicBot.Modules
{
    public class SlashCommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService? Commands { get; set; }

        private readonly InteractionHandlingService _handler;
        private readonly MusicService _musicService;

        public SlashCommandModule(InteractionHandlingService handler, MusicService musicService)
        {
            _handler = handler;
            _musicService = musicService;
        }

        [SlashCommand("join", "Let bot join to your channel")]
        public async Task InitMusic([ChannelTypes(ChannelType.Voice)] IVoiceChannel channel)
        {
            var audioClient = await channel.ConnectAsync(true, false, true);
            if (audioClient == null)
            {
                await ReplyAsync($"{MentionUtils.MentionUser(Context.Guild.CurrentUser.Id)} has no permissions to join to channel '{channel.Name}'");
                return;
            }
            var message = await ReplyAsync("Here is your component");

            await message.AddReactionsAsync(new[]
            {
                Emojis.TrackPrevious,
                Emojis.PlayPause,
                Emojis.TrackNext,
                Emojis.Sound,
                Emojis.LoudSound,
                Emojis.Mute,
                Emojis.ThumbsUp,
                Emojis.ThumbsDown
            });
        }

        [SlashCommand("play", "Let bot play music of your choice")]
        public async Task PlayMusic(string searchPhrase, MusicProvider musicService = MusicProvider.Default)
        {
            var bot = Context.Guild.CurrentUser;
            if (bot.VoiceChannel == null)
            {
                await ReplyAsync($"{MentionUtils.MentionUser(bot.Id)} was not initiated!");
                return;
            }

            if (musicService == MusicProvider.Default)
                musicService = MusicProvider.YouTube;

            var player = await _musicService.GetPlayer(Context.Guild.CurrentUser);
            var response = await _musicService.Lavalink.GetTracksAsync($"{Dictionaries.MusicProviderSearchKeyword[musicService]}:{searchPhrase}");
            var track = response.Tracks.First();
            await player.PlayAsync(track);
            //            LoadTracksResponse response = await LavaLink.GetTracksAsync($"ytsearch:{command.Data.Options.First().Value}");

            //            LavalinkTrack track = response.Tracks.First();

            //            await player.PlayAsync(track);
        }
    }
}
