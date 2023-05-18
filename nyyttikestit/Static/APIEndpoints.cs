using static System.Net.WebRequestMethods;

namespace nyyttikestit.Static
{
    internal static class APIEndpoints
    {
#if DEBUG
        // DO DEBUG THING
        internal const string ServerBaseUrl = "https://localhost:7237";

#else
        //DO PRODUCTION THING IN AZURE
        internal const string ServerBaseUrl = "https://nyytti-server.azurewebsites.net";
#endif
        internal const string ClientBaseUrl = "https://happy-coast-0bf629903.2.azurestaticapps.net";

        internal readonly static string s_Potlucks = $"{ServerBaseUrl}/api/Potlucks";
        internal readonly static string s_PostPot = $"{ServerBaseUrl}/api/Pots";
        internal readonly static string s_DeletePot = $"{ServerBaseUrl}/api/Pots";
        internal readonly static string s_Tags = $"{ServerBaseUrl}/api/Tags";
        internal readonly static string s_Courses = $"{ServerBaseUrl}/api/Courses";

        internal readonly static string s_Pots = $"{ServerBaseUrl}/api/Pots/getpots";

       

    }
   
}
