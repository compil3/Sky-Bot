using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text;
using Discord;
using LGFA.Properties;
using Microsoft.Extensions.Primitives;

namespace LGFA.Modules
{
    public class EmbedHelpers
    {
        public static string Splitter(string lgfa)
        {
            if (lgfa.Contains("LGFA - Season"))
            {
                return lgfa.Replace("LGFA - Season", "");
            }
            else if (lgfa.Contains("LGFA PSN - Season"))
            {
                return lgfa.Replace("LGFA PSN - Season", "");
            }
            return lgfa;
        }

    }
}

