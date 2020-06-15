using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Addons.CommandsExtension;
using Discord.Commands;
using Discord.WebSocket;
using LGFA.Extensions;

namespace LGFA.Modules
{
    [RequireUserPermission(ChannelPermission.SendMessages)]
    public class Help : ModuleBase
    {

        private readonly CommandService _commands;
        private readonly IServiceProvider _map;

        public Help(IServiceProvider map, CommandService commands)
        {
            _commands = commands;
            _map = map;
        }

        [Command("help")]
        [Alias("h")]
        [Summary("Lists the bot's commands.")]
        public async Task HelpCommand([Optional] string path)
        {
            EmbedBuilder output = new EmbedBuilder();
            var user = Context.User as SocketGuildUser;

            var options = new RequestOptions { Timeout = 4 };
            await Context.Message.DeleteAsync();

            if (path == null)
            {
                output.Title = "Help Menu";
                foreach (var md in _commands.Modules.Where(m => m.Parent == null))
                {
                    if (!user.GuildPermissions.BanMembers && md.Name == "Update") continue;
                    if (!user.Roles.Any(r=>r.Name.Contains("Manager") || r.Name.Contains("Owner")) && md.Name == "Manager") continue;
                    HelpBuilder(md, ref output);
                }

                output.Footer = new EmbedFooterBuilder
                {
                    Text = "Use .help <command> to get help with a command. Eg: .help table or .help manager"
                };
            }
            else
            {
                var result = _commands.Search(Context, path);
                if (!result.IsSuccess)
                {
                    await ReplyAsync($"Sorry {Context.User.Mention}, `{path}` is not a valid command.");
                    return;
                }
                else
                {
                    foreach (var match in result.Commands)
                    {
                        if (!user.GuildPermissions.BanMembers && match.Command.Module.Name == "Update")
                        {
                            if (path == "update")
                            {
                                await Context.User.SendMessageAsync(
                                    $"`{Context.User.Username}` you do not have permission to use this command.");
                                return;
                            }
                            continue;
                        }

                        if (!user.Roles.Any(r => r.Name.Contains("Manager") || r.Name.Contains("Owner")) &&
                            match.Command.Module.Name == "Manager")
                        {
                            if (path == "ab" || path == "rb") 
                            {
                                await Context.User.SendMessageAsync($"`{Context.User.Username}`you do not have permission to use this command.");
                                return;
                            }
                            continue;
                        }
                        //var mod = match.Command.Module;
                        FindCommand(match.Command, ref output);
                    }
                }
            }

            await Context.User.SendMessageAsync(embed: output.Build());
            await ReplyAsync($"Check your DMs {Context.User.Mention}").AutoRemove(5);

        }

        private void FindCommand(CommandInfo command, ref EmbedBuilder output)
        {
            output.Title = $"Command: *{command.Name}*";
            output.Description = $"**Module:** {command.Module.Name}\n" +
                                 $"**Command Summary:** {command.Summary}\n" +
                                 (!string.IsNullOrEmpty(command.Remarks) ? $"**Remarks:** *{command.Remarks}*\n" : "") +
                                 (command.Aliases.Any()
                                     ? $"**Tag(s)**: `{string.Join(", ", command.Aliases.Select(x => $"{x}"))}`\n"
                                     : "") +
                                 $"**Usage:**`{GetPrefix(command)} {GetAliases(command)}`";
        }

        private void AddCommands(ModuleInfo match, ref EmbedBuilder output)
        {
            foreach (var command in match.Commands)
            {

                command.CheckPreconditionsAsync(Context, _map).GetAwaiter().GetResult();
                AddCommand(command, ref output);
            }

        }

        private void AddCommand(CommandInfo command, ref EmbedBuilder output)
        {
            output.Title = $"Command: *{command.Name}*";
            output.Description = $"**Module:** {command.Module.Name}\n" +
                $"**Command Summary:** {command.Summary}\n" +
                                 (!string.IsNullOrEmpty(command.Remarks) ? $"**Remarks:** *{command.Remarks}*\n" : "") +
                                 (command.Aliases.Any()
                                     ? $"**Tag(s)**: `{string.Join(", ", command.Aliases.Select(x => $"{x}"))}`\n"
                                     : "") +
                                       $"**Usage:**`{GetPrefix(command)} {GetAliases(command)}`";
        }

        private static void HelpBuilder(ModuleInfo module, ref EmbedBuilder builder)
        {
            foreach (var sub in module.Submodules) HelpBuilder(sub, ref builder);
            builder.AddField(f =>
            {
                f.Name = $"**{module.Name}**";
                f.Value = $"Command: {string.Join(" ", module.Commands.Select(x => $"`.{x.Name}`"))}";
            });
        }



        public string GetAliases(CommandInfo command)
        {
            StringBuilder output = new StringBuilder();
            if (!command.Parameters.Any()) return output.ToString();
            foreach (var param in command.Parameters)
            {
                if (param.IsOptional)
                    output.Append($"<Optional {param.Name}>");
                else if (param.IsMultiple)
                    output.Append($"|{param.Name}| ");
                else if (param.IsRemainder)
                    output.Append($"<{param.Name}> ");
                else output.Append($"<{param.Name}> ");
            }

            return output.ToString();
        }

        public string GetPrefix(CommandInfo command)
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


    }
}