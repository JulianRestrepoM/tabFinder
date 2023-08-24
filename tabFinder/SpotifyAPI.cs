using System.Text.Json.Serialization;


public class Artist {
    [JsonPropertyName("name")]
    public required string Name {get; set;}
}

public class Track {
    [JsonPropertyName("artists")]
    public required Artist[] Artists {get; set;}
    [JsonPropertyName("name")]
    public required string Name {get; set;}
}

public class Item {
    [JsonPropertyName("track")]
    public required Track Track {get; set;}
}

public class SpotifyPlaylistResponse {
    [JsonPropertyName("items")]
    public required List<Item> Items {get; set;}
    [JsonPropertyName("limit")]
    public required int Limit {get; set;}
    [JsonPropertyName("total")]
    public required int Total {get; set;}
}

class Token {
    [JsonPropertyName("access_token")]
    public required string AccessToken {get; set;}
    [JsonPropertyName("token_type")]
    public required string TokenType {get; set;}
    [JsonPropertyName("expires_in")]
    public int ExpiresIn{get; set;}
}

