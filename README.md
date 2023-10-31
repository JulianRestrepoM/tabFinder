# WELCOME TO TABFINDER

TabFinder is C# program that takes your Spotify playlist and, for each song, finds Youtube guitar tutorials for them.

## How to Use:

In order to run TabFinder yourself, you need to have your own Spotify and Youtube developer accounts and have your own API credentials, which you can then add to SpotifyAPI.cs and YoutubeAPI.cs respectively. You also need to select the playlist ID you want to use under Program.cs

Alternatively, you can also just send me a message and I'll happily run it for you. You can also message if you have any channel recommendations.

## tabfinder/files

This folder is where important files used by TabFinder are created and stored. channels.txt is the only file that program needs at the start of execution, in order to speed up subsequent executions, and reduce API costs, the Spotify component is only run if songList.txt doesn't exist and the Youtube component doesn't run if videoList.txt exists. results.txt is always generated on each execution

### channels.txt

This is the list of Youtube channels that will be used to find the tutorials. Because of Youtube's API costs, its impossible to use Youtube's search function to find tutorials  based on **all** videos, so it is limited to specified channels.

In order to add more channels you need to find the channel ID and add it to the file.

### songList.txt

When the Spotify component of TabFinder is run, the song name and artists for each item on the playlist are extracted and stored here.

### videoList.txt

This is where all the relevant metadata: title, channel name, link, of every single uploaded videos of the specified channels are stored

### results.txt

This is the output of TabFinder


