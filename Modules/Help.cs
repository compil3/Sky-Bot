using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.CommandsExtension;
using Discord.Commands;
using LGFA.Extensions;

namespace LGFA.Modules
{
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public class HelpCommand : ModuleBase
    {

        private readonly CommandService _commands;
        private readonly IServiceProvider _map;

        public HelpCommand(IServiceProvider map,CommandService commands)
        {
            _commands = commands;
            _map = map;
        }

        [Command("help")]
        [Alias("h")]
        [Summary("Lists the bot's commands.")]
        public async Task Help(string path = "")
        {
            EmbedBuilder output = new EmbedBuilder();
            if (path == "")
            {
                output.Title = "Help";
                foreach (var mod in _commands.Modules.Where(m=>m.Parent==null))
                {
                    AddHelp(mod, ref output);
                }

                output.Footer = new EmbedFooterBuilder
                {
                    Text = "Use 'help <command>' to get help with a command."
                };
            }
            else
            {
                var mod = _commands.Modules.FirstOrDefault(
                    m => m.Name.Replace("Module", "").ToLower() == path.ToLower());
                if (mod == null)
                {
                    await ReplyAsync("No command could be found with that name.");
                    return;
                }

                output.Title = mod.Name;
                output.Description = $"{mod.Summary}\n" +
                                     (!string.IsNullOrEmpty(mod.Remarks) ? $"({mod.Remarks})\n" : "") +
                                     (mod.Aliases.Any() ? $"Prefix(es): {string.Join(",", mod.Aliases)}\n" : "") +
                                     (mod.Submodules.Any() ? $"Submodules: {mod.Submodules.Select(m => m.Name)}\n" : "") + " ";
                AddCommands(mod, ref output);
            }
            await ReplyAsync("", embed:output.Build());
        }

        private void AddCommands(ModuleInfo module, ref EmbedBuilder builder)
        {
            foreach (var command in module.Commands)
            {
                command.CheckPreconditionsAsync(Context, _map).GetAwaiter().GetResult();
                AddCommand(command, ref builder);
            }
        }

        private void AddCommand(CommandInfo command, ref EmbedBuilder builder)
        {

            builder.AddField(f =>
            {
                f.Name = $"**{command.Name}**";
                f.Value = $"{command.Summary}\n" +
                          (!string.IsNullOrEmpty(command.Remarks) ? $"({command.Remarks})\n" : "") +
                          (command.Aliases.Any()
                              ? $"**Aliases:** {string.Join(", ", command.Aliases.Select(x => $"`{x}`"))}\n"
                              : "") +
                          $"**Usage:** `{GetPrefix(command)} {GetAliases(command)}`";
            });
        }

        private string GetAliases(CommandInfo command)
        {
            StringBuilder output = new StringBuilder();
            if (!command.Parameters.Any()) return output.ToString();
            foreach (var param in command.Parameters)
            {
                if (param.IsOptional)
                    output.Append($"[{param.Name} = {param.DefaultValue}] ");
                else if (param.IsMultiple)
                    output.Append($"|{param.Name}| ");
                else if (param.IsRemainder)
                    output.Append($"...{param.Name} ");
                else output.Append($"<{param.Name}> ");
            }

            return output.ToString();
        }

        private string GetPrefix(CommandInfo command)
        {
            var output = GetPrefix(command.Module);
            output += $"{command.Aliases.First()} ";
            return output;
        }

        public string GetPrefix(ModuleInfo module)
        {
            string output = "";
            if (module.Parent != null) output = $"{GetPrefix(module.Parent)}{output}";
            if (module.Aliases.Any()) output += string.Concat(module.Aliases.FirstOrDefault(), " ");
            return output;
        }

        private void AddHelp(ModuleInfo module, ref EmbedBuilder builder)
        {
            foreach (var sub in module.Submodules) AddHelp(sub, ref builder);
            builder.AddField(f =>
            {
                f.Name = $"**{module.Name}**";
                f.Value = //$"Submodules: {string.Join(", ", module.Submodules.Select(m => m.Name))}" +
                         // $"\n" +
                          $"Commands: {string.Join(", ", module.Commands.Select(x => $"`{x.Name}`"))}" + "\n";
            });
        }

        //[Command("help")]
        //[Summary("Displays this help message.")]
        //public async Task HelpDisplay([Remainder] string command = null)
        //{
        //    var options = new RequestOptions{Timeout = 2};
        //    await Context.Message.DeleteAsync(options);

        //    const string botPrefix = ".";
        //    var helpEmbed = _commands.GetDefaultHelpEmbed(command, botPrefix);
        //    await Context.User.SendMessageAsync(embed: helpEmbed);
        //    await ReplyAsync($"Check your DMs {Context.User.Mention}").AutoRemove(10);
        //}
    }
}