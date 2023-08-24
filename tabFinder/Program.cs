using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading;

Console.WriteLine("Welcome to TAB Finder!");

HttpClient client = new();

//get Spotify Access token
string clientId = "6e91c3589c6040da9d24780524804018";
string clientSecret = "12e41af7e4be4e3e8d7907f2bffc1fa2";

string tokenRequest = "grant_type=client_credentials&client_id="+clientId+"&client_secret="+clientSecret;


// client.BaseAddress = new Uri("https://accounts.spotify.com/api/");

var content = new StringContent(tokenRequest, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

var response = client.PostAsync("https://accounts.spotify.com/api/token", content).Result;

var responseContent = response.Content.ReadAsStringAsync().Result;

Token accessToken = JsonSerializer.Deserialize<Token>(responseContent);

Console.WriteLine("Access token = "+ accessToken.AccessToken);

/*************************************************/
//make first call to playlist API, and save them to objects.
//record the total amount of songs in the playlist.

string playlistId = "5qUj0OW96Y0Vyc3SUlXY6B"; //Julian's Stoner metal +
int limit = 1;
int offset = 0;
string getTotalUri = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks?fields=items%28track%28name%2Cartists%28name%29%29%29%2Ctotal%2Climit&limit={limit}&offset={offset}";

var message = new HttpRequestMessage(HttpMethod.Get, getTotalUri);
message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

response = client.SendAsync(message).Result;

responseContent = response.Content.ReadAsStringAsync().Result;

SpotifyPlaylistResponse spotifyResponse = JsonSerializer.Deserialize<SpotifyPlaylistResponse>(responseContent);

Console.WriteLine(responseContent);

Console.WriteLine($"response total = {spotifyResponse.Total}");

/********************************************************/
//get all songs in a playlist and then store them in a list

List<Item> songList= new();
int remainingSongs = spotifyResponse.Total;
const int maxLimit = 50;

while(remainingSongs > 0) {
    Console.WriteLine($"REMAING SONGS = {remainingSongs}");
    Thread.Sleep(1000); //can cancell if infinite loop
    if(remainingSongs >= maxLimit) {
        limit = maxLimit;
        getTotalUri = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks?fields=items%28track%28name%2Cartists%28name%29%29%29%2Ctotal%2Climit&limit={limit}&offset={offset}";

        message = new HttpRequestMessage(HttpMethod.Get, getTotalUri);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

        response = client.SendAsync(message).Result;

        responseContent = response.Content.ReadAsStringAsync().Result;

        spotifyResponse = JsonSerializer.Deserialize<SpotifyPlaylistResponse>(responseContent);

        foreach(Item i in spotifyResponse.Items) {
            songList.Add(i);
        }
        remainingSongs -= limit;
        offset += limit;
    }
    else {
        limit = remainingSongs;
        getTotalUri = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks?fields=items%28track%28name%2Cartists%28name%29%29%29%2Ctotal%2Climit&limit={limit}&offset={offset}";

        message = new HttpRequestMessage(HttpMethod.Get, getTotalUri);
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

        response = client.SendAsync(message).Result;

        responseContent = response.Content.ReadAsStringAsync().Result;

        spotifyResponse = JsonSerializer.Deserialize<SpotifyPlaylistResponse>(responseContent);

        foreach(Item i in spotifyResponse.Items) {
            songList.Add(i);
        }
        remainingSongs -= limit;
        Console.WriteLine($"Remaining songs = {remainingSongs}");
    }
}



foreach(Item i in songList) {
    string artists = $"{i.Track.Artists[0].Name}";
    if(i.Track.Artists.Length > 1) {
        Console.WriteLine("THIS TRACK HAS MORE THAN ONE ARTIST");
        foreach(Artist j in i.Track.Artists.Skip(1)) {
            artists += $", {j.Name}";
        }
    }
    Console.WriteLine(i.Track.Name + " - " + artists);
}

Console.WriteLine($"SAVED SONGS AMOUNT = {songList.Count}");

/***********************************************************///

