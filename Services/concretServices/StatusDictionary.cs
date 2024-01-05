namespace App_FDark.Services.concretServices
{
    public class StatusDictionary
    {
        public static Dictionary<int, string> statusDictionary = new Dictionary<int, string>
        {
            { 1, "Proposé"},
            { 2, "Signalé"},
            { 3, "Accepté"},
            { 4, "Archivé"}
        };
    }
}
