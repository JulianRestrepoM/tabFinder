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

string playlistId = "2jIOsU05KQRhTiHVjxpMns"; //put the playlist id
string songListFilepath = Path.Combine(Directory.GetCurrentDirectory()+"/files", "songList.txt");

if(!File.Exists(songListFilepath)) {
    HttpClient client = new();
    SpotifyApi spotifyApi= new SpotifyApi(client);

    Token accessToken = spotifyApi.GetAccessToken();
                    
    List<Item> songListSpotify = spotifyApi.GetSongList(playlistId, accessToken);

    spotifyApi.SaveSongs(songListFilepath, songListSpotify);  
}



/***********************************************************/
/*Downloads every video from a youtube channel and then adds them to videoList.txt*/

string channelListFilepath = Path.Combine(Directory.GetCurrentDirectory()+"/files", "channels.txt");
string videoListFilepath = Path.Combine(Directory.GetCurrentDirectory()+"/files", "videoList.txt");

if(!File.Exists(videoListFilepath)) {
    YoutubeApi youtubeApi = new YoutubeApi();

    List<PlaylistItem> videoList = youtubeApi.GetVideosList(channelListFilepath);

    youtubeApi.SaveVideos(channelListFilepath, videoListFilepath, videoList);
}

/***********************************************************/
/*read the songs and videos from the existing .txt files and then match them! results are added to results.txt*/


string[] savedSongsList = File.ReadAllLines(songListFilepath);
string[] savedVideosList = File.ReadAllLines(videoListFilepath);

using (StreamWriter outputFile = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory()+"/files", "results.txt"))) {
    foreach(string currSong in savedSongsList) {
        outputFile.WriteLine(currSong);
        foreach(string currVid in savedVideosList) {
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


