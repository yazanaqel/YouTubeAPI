using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Mvc;
using YouTubeAPI.Models;

namespace YouTubeAPI.Controllers;
[ApiController]
[Route("[controller]")]
public class YouTubeController : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetVideos(string? pageToken = null, int maxResult = 50)
	{
		var youtubeSetvice = new YouTubeService(new BaseClientService.Initializer
		{
			ApiKey = "AIzaSyBoCsXlPSjmQfT7a6TEwZBopUZoprfPnRs",
			ApplicationName = "MyFirstYouTube"
		});

		var searchRequest = youtubeSetvice.Search.List("snippet");

		searchRequest.ChannelId = "UCqCnjtxdlG9qEgFJIUeLJNg";

		searchRequest.Order = SearchResource.ListRequest.OrderEnum.Date;
		searchRequest.MaxResults = maxResult;
		searchRequest.PageToken = pageToken;

		var searchResponse = await searchRequest.ExecuteAsync();

		var videoList = searchResponse.Items.Select(item => new VideoDetails
		{
			Title = item.Snippet.Title,
			Link = $"https://www.youtube.com/match?v={item.Id.VideoId}",
			Thumbnail = item.Snippet.Thumbnails.Medium.Url,
			PublishDate = item.Snippet.PublishedAtDateTimeOffset
		})
			.OrderByDescending(x => x.PublishDate)
			.ToList();

		return Ok(videoList);
	}
}
