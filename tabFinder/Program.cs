using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc.Formatters;

Console.WriteLine("Welcome to TAB Finder!");

HttpClient client = new();

string clientID = "6e91c3589c6040da9d24780524804018";
string clientSecret = "12e41af7e4be4e3e8d7907f2bffc1fa2";

string tokenRequest = "grant_type=client_credentials&client_id="+clientID+"&client_secret="+clientSecret;


client.BaseAddress = new Uri("https://accounts.spotify.com/api/");

var content = new StringContent(tokenRequest, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

var response = client.PostAsync("token", content).Result;

var responseContent = response.Content.ReadAsStringAsync().Result;
Console.WriteLine(responseContent);

Console.WriteLine("Hello World!");

