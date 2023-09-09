using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Microsoft.VisualBasic;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.SignalR;



class YoutubeApi {
    private const string appName = "Tab Finder";
    private const string googleAPIkey = "AIzaSyBPtgi0C3wGzBKlYd-sS3ERGSwVMK33niA";

    YouTubeService YouTubeService = new YouTubeService(new BaseClientService.Initializer { //initializes the youtube client with the API key
    ApplicationName = appName,
    ApiKey = googleAPIkey,
    });

    public List<PlaylistItem> GetVideosList (string channelsFilePath) {
        List<PlaylistItem> youtubeVidList = new();

        string[] channelList = File.ReadAllLines(channelsFilePath);

        int savedVidsAmount = 0;
        foreach(string currChannel in channelList) {

            string currPlayListId = currChannel.Substring(0, currChannel.IndexOf(","));

            var nextPageToken = "";
            //the get requests and saving the response to a list
            while(nextPageToken != null) {
                // Thread.Sleep(500); //so i have time to cancel
                var channelVidsRequest = YouTubeService.PlaylistItems.List("snippet,contentDetails");
                channelVidsRequest.PlaylistId = currPlayListId;
                channelVidsRequest.MaxResults = 50;
                channelVidsRequest.PageToken = nextPageToken;

                var channelVidsResponse = channelVidsRequest.ExecuteAsync().Result;

                foreach(var vid in channelVidsResponse.Items) {
                    youtubeVidList.Add(vid);
                    savedVidsAmount++;
                }
                Console.WriteLine("Saved "+savedVidsAmount+" vids"+ " from "+ currChannel);
    
                nextPageToken = channelVidsResponse.NextPageToken; //uses token to get the next set of videos
            }
        }
        return youtubeVidList;
    }

    public void SaveVideos(string channelsFilePath, string saveFilePath, List<PlaylistItem> videoList) {
        const string youtubePrefix = "https://www.youtube.com/watch?v=";

        //adds the videos from the list to a .txt
        using (StreamWriter outputFile = new StreamWriter(saveFilePath)) {
            foreach(PlaylistItem currVid in videoList) {
                outputFile.WriteLine(currVid.Snippet.Title+","+currVid.Snippet.ChannelTitle+","+youtubePrefix+currVid.ContentDetails.VideoId);
            }
        }
    }
}