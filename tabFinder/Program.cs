using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Threading;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Microsoft.VisualBasic;
using Google.Apis.YouTube.v3.Data;
/***********************************************************/

Console.WriteLine("Welcome to TAB Finder!");

/***********************************************************/
/* Save songs from given spotify playlist to songList.txt*/

HttpClient client = new();
SpotifyApi spotifyApi= new SpotifyApi(client);

Token accessToken = spotifyApi.GetAccessToken();

string playlistId = "5qUj0OW96Y0Vyc3SUlXY6B"; //Julian's Stoner metal +

List<Item> songListSpotify = spotifyApi.GetSongList(playlistId, accessToken);

//copies songs from list to file
using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory()+"/files", "songList.txt"))) {
    foreach(Item currSong in songListSpotify) {
        if(currSong.Track.Name.Contains("-")) { //some titles have extra info like Iron man - 2012 remastered, we want to remove everything after the -
            int index = currSong.Track.Name.IndexOf("-");
            currSong.Track.Name = currSong.Track.Name.Substring(0, index-1); //-1 to also remove the space before -
        }
        string artists = currSong.Track.Artists[0].Name;
        string toWrite = currSong.Track.Name;
        foreach(Artist currArtist in currSong.Track.Artists.Skip(1)) {
            artists += $", {currArtist.Name}";
        }
        outputFile.WriteLine($"{currSong.Track.Name.ToUpper()} - {artists}"); //seperate song title and artist by " - "

    }
}

/***********************************************************/
/*Downloads every video from a youtube channel and then adds them to videoList.txt*/

string appName = "Tab Finder";
string googleAPIkey = "AIzaSyBPtgi0C3wGzBKlYd-sS3ERGSwVMK33niA";
string martyMusicID = "UCmnlTWVJysjWPFiZhQ5uudg";
string martyMusicUploads = "UUmnlTWVJysjWPFiZhQ5uudg";

var youTubeService = new YouTubeService(new BaseClientService.Initializer { //initializes the youtube client with the API key
    ApplicationName = appName,
    ApiKey = googleAPIkey,
});


List<PlaylistItem> youtubeVidList = new();

var nextPageToken = "";
int savedVidsAmount = 0;

//the get requests and saving the response to a list
while(nextPageToken != null) {
    // Thread.Sleep(500); //so i have time to cancel
    var channelVidsRequest = youTubeService.PlaylistItems.List("snippet,contentDetails");
    channelVidsRequest.PlaylistId = martyMusicUploads;
    channelVidsRequest.MaxResults = 50;
    channelVidsRequest.PageToken = nextPageToken;

    var channelVidsResponse = await channelVidsRequest.ExecuteAsync();

    foreach(var vid in channelVidsResponse.Items) {
        youtubeVidList.Add(vid);
        savedVidsAmount++;
    }
    Console.WriteLine($"SAVED {savedVidsAmount} Vids");
    
    nextPageToken = channelVidsResponse.NextPageToken; //uses token to get the next set of videos
}

string youtubePrefix = "https://www.youtube.com/watch?v=";

//adds the videos from the list to a .txt
using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory()+"/files", "videoList.txt"))) {
    foreach(PlaylistItem currVid in youtubeVidList) {
        outputFile.WriteLine(currVid.Snippet.Title+","+currVid.Snippet.ChannelTitle+","+youtubePrefix+currVid.ContentDetails.VideoId);
    }
}

/***********************************************************/
/*read the songs and videos from the existing .txt files and then match them! results are added to results.txt*/


string[] songList = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory()+"/files", "songList.txt"));
string[] vidList = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory()+"/files", "videoList.txt"));

using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory()+"/files", "results.txt"))) {
    foreach(string currSong in songList) {
        outputFile.WriteLine(currSong);
        foreach(string currVid in vidList) {
            if(currVid.ToUpper().Contains(currSong.Substring(0, currSong.IndexOf("-")-1))) { //only doing exact matches to song title, all in uppercase for ease of matching
                //add the channel, title. and link to the video in a nice format
                int firstIndex = currVid.IndexOf(",");
                int secondIndex = currVid.IndexOf(",", firstIndex+1);
                int length = currVid.Length;
                string channel = currVid.Substring(firstIndex+1, secondIndex-firstIndex);
                string url = currVid.Substring(secondIndex+1);
                string title = currVid.Substring(0, firstIndex);
                outputFile.WriteLine("     "+channel);
                outputFile.WriteLine("          "+title);
                outputFile.WriteLine("          "+url);
            }
        }
        outputFile.WriteLine(""); //empty space between songs
    }
}

/***********************************************************/

Console.WriteLine("tabFinder Finished!!!");


