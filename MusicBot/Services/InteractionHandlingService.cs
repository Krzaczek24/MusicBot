using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MusicBot.Services
{
    public class InteractionHandlingService
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _handler;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        public InteractionHandlingService(DiscordSocketClient client, InteractionService handler, IServiceProvider services, IConfiguration config)
        {
            _client = client;
            _handler = handler;
            _services = services;
            _configuration = config;
        }

        public async Task InitializeAsync()
        {
            _client.Ready += ReadyAsync;
            _handler.Log += Program.Log;

            await _handler.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

            _client.InteractionCreated += HandleInteraction;
        }

        private async Task ReadyAsync()
        {
            await RemoveGlobalCommands();

            if (Program.IsDebug())
            {
                ulong guildId = _configuration.GetValue<ulong>("testGuild");

                await RemoveGuildCommands(guildId);
                await _handler.RegisterCommandsToGuildAsync(guildId, true);
            }
            else
            {
                await _handler.RegisterCommandsGloballyAsync(true);
            }
        }

        private async Task HandleInteraction(SocketInteraction interaction)
        {
            try
            {
                // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
                var context = new SocketInteractionContext(_client, interaction);

                // Execute the incoming command.
                var result = await _handler.ExecuteCommandAsync(context, _services);

                if (!result.IsSuccess)
                {
                    switch (result.Error)
                    {
                        case InteractionCommandError.UnmetPrecondition:
                            // implement
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {
                // If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
                // response, or at least let the user know that something went wrong during the command execution.
                if (interaction.Type is InteractionType.ApplicationCommand)
                    await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }

        private async Task RemoveGlobalCommands() => await RemoveCommands(await _client.GetGlobalApplicationCommandsAsync());
        private async Task RemoveGuildCommands(ulong guildId) => await RemoveCommands(await _client.GetGuild(guildId).GetApplicationCommandsAsync());

        private async Task RemoveCommands(IEnumerable<SocketApplicationCommand> commands)
        {
            foreach (var command in commands)
                await command.DeleteAsync();
        }
    }
}
