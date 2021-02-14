using Newtonsoft.Json;

namespace MewsicBot_Core
{
    // Config.json
    internal struct Config
    {
        [JsonProperty("StringPrefix")]
        internal string StringPrefix { get; private set; }

        [JsonProperty("AutoReconnect")]
        internal bool AutoReconnect { get; private set; }  // true

        [JsonProperty("EnableDms")]
        internal bool EnableDms { get; private set; }  // true

        [JsonProperty("EnableMentionPrefix")]
        internal bool EnableMentionPrefix { get; private set; } //true
    }

    // Config.json
    internal struct Token
    {
        [JsonProperty("Token")]
        internal string Bot_Token { get; private set; }
    }
}

/*
Config.json:

{
  "StringPrefix": ",",
  "AutoReconnect": true,
  "EnableDms": true,
  "EnableMentionPrefix": true
}

*/

/*
Token.json:

{
  "Token": "[REDACTED]"
}

*/