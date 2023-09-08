using System.Text.Json.Serialization;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;


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

class SpotifyApi {
    private const string ClientId = "6e91c3589c6040da9d24780524804018";
    private const string ClientSecret = "12e41af7e4be4e3e8d7907f2bffc1fa2";
    private const int maxLimit = 50; //given by spotify documentation
    private HttpClient client;
    public SpotifyApi(HttpClient client) {
        this.client = client;
    }


    public Token GetAccessToken() {
        string tokenRequest = "grant_type=client_credentials&client_id="+ClientId+"&client_secret="+ClientSecret;

        var content = new StringContent(tokenRequest, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

        var response = client.PostAsync("https://accounts.spotify.com/api/token", content).Result;

        var responseContent = response.Content.ReadAsStringAsync().Result;

        Token accessToken = JsonSerializer.Deserialize<Token>(responseContent);

        return accessToken;
    }

    private SpotifyPlaylistResponse sendHttpGet(string playlistId, int limit, int offset, Token accessToken) {
        string completeUri = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks?fields=items%28track%28name%2Cartists%28name%29%29%29%2Ctotal%2Climit&limit={limit}&offset={offset}";
        
        var message = new HttpRequestMessage(HttpMethod.Get, completeUri);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

        var response = client.SendAsync(message).Result;

        var responseContent = response.Content.ReadAsStringAsync().Result;

        SpotifyPlaylistResponse spotifyResponse = JsonSerializer.Deserialize<SpotifyPlaylistResponse>(responseContent);
        return spotifyResponse;
        
    }

    private int GetSongAmount(string playlistId, Token accessToken) {
            
            SpotifyPlaylistResponse spotifyResponse = sendHttpGet(playlistId, 1, 0, accessToken);
            
            return spotifyResponse.Total;
    }

    public List<Item> GetSongList(string playlistId, Token accessToken) {
        List<Item> songList= new();
        int remainingSongs = GetSongAmount(playlistId, accessToken);
        int offset = 0;
        SpotifyPlaylistResponse spotifyResponse;
        
        while(remainingSongs > 0) {
            Console.WriteLine($"REMAING SONGS = {remainingSongs}");
            Thread.Sleep(1000); //can cancel if infinite loop
            if(remainingSongs >= maxLimit) {
                spotifyResponse = sendHttpGet(playlistId, maxLimit, offset, accessToken);
                remainingSongs -= maxLimit;
                offset += maxLimit;
            }
            else {
                spotifyResponse = sendHttpGet(playlistId, remainingSongs, offset, accessToken);
                remainingSongs = 0;
            }
            foreach(Item i in spotifyResponse.Items) {
                    songList.Add(i);
                }
        }
        return songList; 
    }
}

