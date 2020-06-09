using Discord.Commands;

namespace Sky_Bot.Extensions
{
    public class RequireRole : PreconditionAttribute
    {
        private readonly string _name;
        public RequireRole(string name) => _name = name;

        
    }
}