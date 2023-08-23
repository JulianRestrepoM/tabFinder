using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc.Formatters;

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

Console.WriteLine("Access token = "+ accessToken.access_token);

/*************************************************/

string playlistId = "5qUj0OW96Y0Vyc3SUlXY6B"; //Julian's Stoner metal +
string limit = "1";
string getTotalUri = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks?fields=items%28track%28name%2Cartists%28name%29%29%29%2Ctotal%2Climit&limit={limit}";

var message = new HttpRequestMessage(HttpMethod.Get, getTotalUri);
message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.access_token);

response = client.SendAsync(message).Result;

responseContent = response.Content.ReadAsStringAsync().Result;

SpotifyResponse totalResponse = JsonSerializer.Deserialize<SpotifyResponse>(responseContent);

Console.WriteLine(responseContent);

Console.WriteLine($"response total = {totalResponse.Total}");
Console.WriteLine($"response limit = {totalResponse.Limit}");
Console.WriteLine($"items length = {totalResponse.Items.Count}");
Console.WriteLine($"response track = {totalResponse.Items[0].Track.Name}");
Console.WriteLine($"response artist = {totalResponse.Items[0].Track.Artists[0].Name}");


