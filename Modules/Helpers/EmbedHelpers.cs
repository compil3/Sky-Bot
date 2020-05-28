namespace LGFA.Modules
{
    public class EmbedHelpers
    {
        public static string Splitter(string lgfa)
        {
            if (lgfa.Contains("LGFA - Season"))
                return lgfa.Replace("LGFA - Season", "");
            if (lgfa.Contains("LGFA PSN - Season")) return lgfa.Replace("LGFA PSN - Season", "");
            return lgfa;
        }
    }
}