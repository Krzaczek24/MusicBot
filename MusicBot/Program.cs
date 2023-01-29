using Discord;
using Discord.Commands;
using Discord.Interactions;
using Discord.WebSocket;
using MusicBot.Configuration;
using MusicBot.Extensions;
using MusicBot.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MusicBot
{
    internal class Program
    {
        public static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        public async Task RunAsync()
        {
            using (var services = ConfigureServices())
            {
                var client = services.GetRequiredService<DiscordSocketClient>();
                client.Log += Log;
                services.GetRequiredService<CommandService>().Log += Log;

                var config = services.GetRequiredService<IConfiguration>();
                await client.LoginAsync(TokenType.Bot, config.GetCredentials().BotToken);
                await client.StartAsync();
                await services.GetRequiredService<InteractionHandlingService>().InitializeAsync();

                // =====================================================================

                //var lava = services.GetRequiredService<LavalinkService>();

                //var channel = (IVoiceChannel)await client.GetChannelAsync(1066139087091814400);
                ////await channel.ConnectAsync(true, false, true);

                //var lavaLink = services.GetRequiredService<LavalinkManager>();

                //await lavaLink.StartAsync();

                //LavalinkPlayer player = await lavaLink.JoinAsync(channel);

                //LoadTracksResponse response = await lavaLink.GetTracksAsync($"ytsearch:despacito");

                //LavalinkTrack track = response.Tracks.First();

                //await player.PlayAsync(track);

                // =====================================================================

                await Task.Delay(Timeout.Infinite);
            }        
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(new ConfigurationBuilder()
                    .AddUserSecrets<UserSecrets>()
                    .AddJsonFile("settings.json", optional: false)
                    .Build() as IConfiguration)
                .AddSingleton(new DiscordSocketConfig { GatewayIntents = GatewayIntents.All })
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<MusicService>()
                .AddSingleton<CommandService>()
                .AddSingleton<InteractionService>()
                .AddSingleton<InteractionHandlingService>()
                .BuildServiceProvider();
        }

        public static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

        //private async Task ClientReady()
        //{
        //    try
        //    {
        //        await Client.BulkOverwriteGlobalApplicationCommandsAsync(CommandsRepository.GetAllCommands().ToArray());
        //    }
        //    catch (HttpException exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //    }
        //}

        //private async Task SlashCommandHandler(SocketSlashCommand command)
        //{
        //    Console.WriteLine($"Slash [{command.Data.Name}] command received!");
        //    switch (command.Data.Name)
        //    {
        //        case "join":
        //            var channel = command.Data.Options.First().Value as IVoiceChannel;
        //            if (channel == null)
        //                return;
        //            await channel.ConnectAsync(true, false, true);
        //            return;
        //        case "play":
        //            var user = Client.GetGuild(340291326455185408).Users.Single(x => x.Id == Client.CurrentUser.Id);
        //            if (user.VoiceChannel == null && command.Channel is IVoiceChannel)
        //                await ((IVoiceChannel)command.Channel).ConnectAsync(true, false, true);

        //            await LavaLink.StartAsync();

        //            LavalinkPlayer player = LavaLink.GetPlayer(command.GuildId!.Value) ?? await LavaLink.JoinAsync(user.VoiceChannel ?? command.Channel as IVoiceChannel);

        //            LoadTracksResponse response = await LavaLink.GetTracksAsync($"ytsearch:{command.Data.Options.First().Value}");

        //            LavalinkTrack track = response.Tracks.First();

        //            await player.PlayAsync(track);

        //            //var sound = await user.VoiceChannel!.ConnectAsync(true, false, true);


        //            //==================================================================================
        //            //var endpoint = new ConnectionEndpoint("127.0.0.1", 2333);
        //            //var config = new LavalinkConfiguration()
        //            //{
        //            //    Password = Configuration.GetLavalinkPassword(),
        //            //    RestEndpoint = endpoint,
        //            //    SocketEndpoint = endpoint
        //            //};

        //            //var discord = new DiscordClient(new DiscordConfiguration()
        //            //{
        //            //    Token = Configuration.GetBotToken(),
        //            //    TokenType = LavaTokenType.Bot,
        //            //    MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug
        //            //});

        //            //var lavalink = discord.UseLavalink();

        //            //await discord.ConnectAsync();
        //            //await lavalink.ConnectAsync(config);

        //            //discord.UseVoiceNext();
        //            //==================================================================================
        //            //var socket = new WebSocketClient(new WebSocketConfiguration() { });
        //            //var voiceChannel = _client.GetChannel(1066139087091814400) as IVoiceChannel;
        //            //var textChannel = _client.GetChannel(1066138630717972511) as ITextChannel;
        //            //var player = new LavaPlayer(socket, voiceChannel, textChannel);
        //            //Lava
        //            //var track = new LavaTrack()
        //            //player.PlayAsync()
        //            return;
        //        case "leave":
        //            var voiceChannel = Client.GetGuild(340291326455185408).Users.Single(x => x.Id == Client.CurrentUser.Id).VoiceChannel;
        //            if (voiceChannel != null)
        //                await voiceChannel.DisconnectAsync();
        //            return;
        //        case "decode-emoji":
        //            string emotes = (string)command.Data.Options.First().Value;
        //            var codes = emotes.ToCharArray().TakeWhile(x => x >= 128).ToArray();
        //            string decoded = string.Join(string.Empty, codes.Select(c => $"\\u{(ushort)c:X4}"));
        //            await command.RespondAsync($"Emoji\n{emotes}\ncodes are\n\\{decoded}");
        //            return;
        //        case "clear-channel":
        //            if (command.Channel.Name != "testowy")
        //            {
        //                await command.RespondAsync($"{Emojis.X} ! Zakaz czyszczenia kanałów ! {Emojis.X}");
        //                return;
        //            }

        //            var messages = await command.Channel.GetMessagesAsync().Flatten().ToListAsync();
        //            if (messages == null || messages.Count == 0)
        //                return;

        //            foreach (var message in messages)
        //            {
        //                try { await command.Channel.DeleteMessageAsync(message); }
        //                catch { }
        //                await Task.Delay(500);
        //            }

        //            return;
        //    }
        //}

        //private async Task MessageCommandHandler(SocketMessageCommand command)
        //{
        //    Console.WriteLine($"Message [{command.Data.Name}] command received!");
        //    await command.RespondAsync($"You executed {command.Data.Name}");   
        //}

        //private async Task UserCommandHandler(SocketUserCommand command)
        //{
        //    Console.WriteLine($"User [{command.Data.Name}] command received!");
        //    await command.RespondAsync($"You executed {command.Data.Name}");
        //}

        //private async Task MessageHandler(SocketMessage msg)
        //{
        //    var message = msg as SocketUserMessage;
        //    if (message == null)
        //        return;

        //    if (message.Source != MessageSource.User)
        //        return;

        //    int argPos = 0;
        //    bool hasExclamationMark = message.HasCharPrefix('!', ref argPos);            
        //    bool botMentioned = message.HasMention(Client.CurrentUser);
        //    if ((!hasExclamationMark && !botMentioned) || message.Author.IsBot)
        //        return;

        //    Console.WriteLine($"Received message from {message.Author.Username}!");

        //    var context = new SocketCommandContext(Client, message);

        //    if (botMentioned && message.Content.Contains("fuck you", StringComparison.InvariantCultureIgnoreCase))
        //    {
        //        await message.AddReactionAsync(Emojis.MiddleFinger);
        //        await context.Channel.SendMessageAsync($"<@{message.Author.Id}> 🖕 FUCK YOU TOO 🖕");
        //    }
        //}

        //private async Task UserJoinedHandler(SocketGuildUser user)
        //{
        //    var channel = await Client.GetChannelAsync(1066138630717972511) as IMessageChannel;
        //    if (channel != null)
        //    {
        //        await channel.SendMessageAsync($"<@{user.Id}> {Emojis.Punch} Witamy w kolonii {Emojis.Punch}");
        //    }
        //}

        //private async Task UserLeftHandler(SocketGuild guild, SocketUser user)
        //{
        //    var channel = await Client.GetChannelAsync(1066138630717972511) as IMessageChannel;
        //    if (channel != null)
        //    {
        //        await channel.SendMessageAsync($"<@{user.Id}> {Emojis.Cry} Żegnaj {Emojis.Cry}");
        //    }
        //}

        ////private async Task UserUpdatedHandler(Cacheable<SocketGuildUser, ulong> something, SocketGuildUser user)
        ////{
        ////    Console.WriteLine("User updated");
        ////}

        //private async Task PresenceHandler(SocketUser user, SocketPresence oldPresence, SocketPresence newPresence)
        //{
        //    Func<IActivity, string> getName = (x) => x.Name;
        //    string message = user.Username;

        //    var newActivity = newPresence.Activities.ExceptBy(oldPresence.Activities.Select(getName), getName).SingleOrDefault();
        //    if (newActivity != null)
        //    {
        //        message += $" started";

        //        if (newActivity is RichGame)
        //        {
        //            message += $" playing {newActivity.Name}";
        //        }
        //        else if (newActivity is SpotifyGame)
        //        {
        //            message += $" listening {newActivity.Name} - '{((SpotifyGame)newActivity).TrackTitle}'";
        //        }
        //        else
        //        {
        //            message += $" {newActivity.Name}";
        //        }

        //        Console.WriteLine(message);
        //        return;
        //    }

        //    var oldActivity = oldPresence.Activities.ExceptBy(newPresence.Activities.Select(getName), getName).SingleOrDefault();
        //    if (oldActivity != null)
        //    {
        //        message += $" stopped";

        //        if (oldActivity is RichGame)
        //        {
        //            message += $" playing {oldActivity.Name}";

        //            var game = (RichGame)oldActivity;
        //            if (game?.Timestamps?.Start.HasValue == true)
        //            {
        //                var duration = DateTime.Now - game.Timestamps.Start.Value;
        //                message += $" after {duration.FormatTime()}";
        //            }
        //        }
        //        else if (oldActivity is SpotifyGame)
        //        {
        //            message += $" listening {oldActivity.Name}";
        //        }
        //        else
        //        {
        //            message += $" {oldActivity.Name}";
        //        }

        //        Console.WriteLine(message);
        //        return;
        //    }

        //    var commonActivity = newPresence.Activities.IntersectBy(oldPresence.Activities.Select(getName), getName).SingleOrDefault(x => x is SpotifyGame);
        //    if (commonActivity is SpotifyGame)
        //    {
        //        var spotify = (SpotifyGame)commonActivity;
        //        message += $" is listening now '{spotify.TrackTitle}'";

        //        Console.WriteLine(message);
        //        return;
        //    }

        //    var channel = await Client.GetChannelAsync(1066138630717972511) as IMessageChannel;
        //    if (channel != null)
        //    {
        //        if (oldPresence.Status == UserStatus.Offline)
        //        {
        //            await channel.SendMessageAsync($"Witaj {user.Username}!");
        //            return;
        //        }

        //        if (newPresence.Status == UserStatus.Offline)
        //        {
        //            await channel.SendMessageAsync($"Bywaj {user.Username}!");
        //            return;
        //        }
        //    }
        //}

        public static bool IsDebug()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}