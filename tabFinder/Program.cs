using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;

Console.WriteLine("Welcome to TAB Finder!");

HttpClient client = new();
SpotifyApi spotifyApi= new SpotifyApi(client);

Token accessToken = spotifyApi.GetAccessToken();

string playlistId = "5qUj0OW96Y0Vyc3SUlXY6B"; //Julian's Stoner metal +

List<Item> songList = spotifyApi.GetSongList(playlistId, accessToken);

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

string appName = "Tab Finder";
string googleAPIkey = "AIzaSyBPtgi0C3wGzBKlYd-sS3ERGSwVMK33niA";

var googleClient = new YouTubeService(new BaseClientService.Initializer {
    ApplicationName = appName,
    ApiKey = googleAPIkey,
});

Console.WriteLine($"google api keeeyy = {googleClient.ApiKey}");

