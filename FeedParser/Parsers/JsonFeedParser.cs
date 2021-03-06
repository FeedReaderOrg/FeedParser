using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace FeedParser
{
    // https://jsonfeed.org/version/1.1
    class JsonFeedAuthor
    {
        public string name { get; set; }

        public string url { get; set; }

        public string avatar { get; set; }
    }

    class JsonFeedHub
    {
        public string type { get; set; }
        public string url { get; set; }
    }

    class JsonFeed
    {
        public string version { get; set; }

        public string title { get; set; }

        public string home_page_url { get; set; }

        public string feed_url { get; set; }

        public string description { get; set; }

        public string user_comment { get; set; }

        public string next_url { get; set; }

        public string icon { get; set; }

        public string favicon { get; set; }

        public List<JsonFeedAuthor> authors { get; set; }

        public string language { get; set; }

        public bool expired { get; set; }

        public List<JsonFeedHub> hubs { get; set; }

        public List<JsonFeedItem> items { get; set; }
    }

    class JsonFeedItem
    {
        public string id { get; set; }

        public string url { get; set; }

        public string external_url { get; set; }

        public string title { get; set; }

        public string content_html { get; set; }

        public string content_text { get; set; }

        public string summary { get; set; }

        public string image { get; set; }

        public string banner_image { get; set; }

        public string date_published { get; set; }

        public string date_modified { get; set; }

        public List<JsonFeedAuthor> authors { get; set; }

        public List<string> tags { get; set; }

        public string language { get; set; }
    }

    class JsonFeedParser : FeedParser
    {
        private readonly string _content;
        private FeedInfo _feedInfo;
        private List<FeedItem> _feddItems;

        public JsonFeedParser(string content)
        {
            _content = content;
        }

        public override FeedInfo ParseFeedInfo()
        {
            if (_feedInfo == null)
            {
                ParseJsonFeed();
            }
            return _feedInfo;
        }

        public override List<FeedItem> ParseFeedItems()
        {
            if (_feddItems == null)
            {
                ParseJsonFeed();
            }
            return _feddItems;
        }

        private void ParseJsonFeed()
        {
            var jsonFeed = JsonSerializer.Deserialize<JsonFeed>(_content);
            _feedInfo = new FeedInfo()
            {
                Description = jsonFeed.description,
                IconUri = jsonFeed.icon,
                Name = jsonFeed.title,
                WebsiteLink = jsonFeed.home_page_url
            };

            if (string.IsNullOrEmpty(_feedInfo.Description))
            {
                _feedInfo.Description = jsonFeed.user_comment;
            }

            if (string.IsNullOrEmpty(_feedInfo.IconUri))
            {
                _feedInfo.IconUri = jsonFeed.favicon;
            }

            if (jsonFeed.items == null)
            {
                _feddItems = new List<FeedItem>();
            }
            else
            {
                _feddItems = jsonFeed.items.Select(f => ParseJsonFeedItem(f)).ToList();
            }
        }

        private FeedItem ParseJsonFeedItem(JsonFeedItem jsonFeedItem)
        {
            var feedItem = new FeedItem()
            {
                Content = jsonFeedItem.content_text,
                Summary = jsonFeedItem.summary,
                Guid = jsonFeedItem.id,
                PermentLink = jsonFeedItem.url,
                Title = jsonFeedItem.title,
                TopicPictureUri = jsonFeedItem.image
            };

            if (string.IsNullOrEmpty(feedItem.Content))
            {
                feedItem.Content = jsonFeedItem.content_html;
            }

            if (string.IsNullOrEmpty(feedItem.Summary))
            {
                feedItem.Summary = GetSummary(feedItem.Content);
            }

            if (string.IsNullOrEmpty(feedItem.PermentLink))
            {
                feedItem.PermentLink = jsonFeedItem.id;
            }

            if (string.IsNullOrEmpty(feedItem.TopicPictureUri))
            {
                feedItem.TopicPictureUri = jsonFeedItem.banner_image;

                if (string.IsNullOrEmpty(feedItem.TopicPictureUri))
                {
                    feedItem.TopicPictureUri = TryGetImageUrl(feedItem.Content);
                }

                if (string.IsNullOrEmpty(feedItem.TopicPictureUri))
                {
                    feedItem.TopicPictureUri = TryGetImageUrl(feedItem.Summary);
                }
            }

            if (!string.IsNullOrEmpty(jsonFeedItem.date_published))
            {
                feedItem.PubDate = DateTime.Parse(jsonFeedItem.date_published).ToUniversalTime();
            }
            else if (!string.IsNullOrEmpty(jsonFeedItem.date_modified))
            {
                feedItem.PubDate = DateTime.Parse(jsonFeedItem.date_modified).ToUniversalTime();
            }

            return feedItem;
        }
    }
}
