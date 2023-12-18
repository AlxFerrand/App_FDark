namespace App_FDark.Services.concretServices
{
    public class StatusDictionary
    {
        public static Dictionary<int, string> statusDictionary = new Dictionary<int, string>
        {
            { 0, "NonDéfinis"},
            { 1, "Proposé"},
            { 2, "Vérifié"},
            { 3, "Accepté"},
            { 4, "Archivé"}
        };
    }
}
