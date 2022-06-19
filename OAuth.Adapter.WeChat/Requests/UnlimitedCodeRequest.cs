using System.Text.Json.Serialization;

namespace OAuth.Adapter.WeChat.Requests;

public class UnlimitedCodeRequest
{
    [JsonPropertyName("scene")]
    public string Scene { get; set; }

    [JsonPropertyName("env_version")]
    public string EnvironmentVersion { get; set; }
}