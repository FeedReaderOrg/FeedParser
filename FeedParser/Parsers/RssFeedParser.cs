using System.Linq;
using System.Collections.Generic;
using System.Xml;

namespace FeedParser
{
    public class RssFeedParser : XmlFeedParser
    {
        public RssFeedParser(XmlDocument xml)
            : base(xml)
        {
            FeedXmlNS.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");
        }

        public override FeedInfo ParseFeedInfo()
        {
            var feed = new FeedInfo();
            FeedXmlNS.AddNamespace("content", "http://purl.org/rss/1.0/modules/content/");

            // Parse channel. As spec, every feed has only one channel.
            var channelNode = FeedXml.SelectSingleNode("/rss/channel");
            feed.Name = channelNode["title"].InnerText;
            feed.WebsiteLink = channelNode["link"].InnerText;
            feed.Description = channelNode["description"].InnerText;
            feed.IconUri = channelNode.SelectSingleNode("/rss/channel/image")?["url"]?.InnerText;
            return feed;
        }

        public override List<FeedItem> ParseFeedItems()
        {
            var feedItems = new List<FeedItem>();
            foreach (XmlNode itemNode in FeedXml.SelectNodes("/rss/channel/item"))
            {
                feedItems.Add(ParseItem(itemNode));
            }
            return feedItems.OrderByDescending(i => i.PubDate).GroupBy(i => i.PermentLink).Select(i => i.First()).ToList();
        }

        private FeedItem ParseItem(XmlNode itemNode)
        {
            var item = new FeedItem();
            item.Title = itemNode["title"].InnerText;
            item.PermentLink = itemNode["link"].InnerText.Trim();
            item.PubDate = itemNode["pubDate"].InnerText.ToDateTime().ToUniversalTime();
            item.Guid = itemNode["guid"]?.InnerText;

            // Get content
            item.Content = itemNode["description"]?.InnerText;
            if (string.IsNullOrWhiteSpace(item.Content))
            {
                item.Content = itemNode.SelectSingleNode("content:encoded", FeedXmlNS)?.InnerText ?? string.Empty;
            }

            // Try to find topic picture.
            string imgUrl = TryGetImageUrl(itemNode);

            // In some feeds, description doesn't contain picture, try to find content if it has.
            // Standard, rdf 1.0: http://purl.org/rss/1.0/modules/content/
            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                imgUrl = TryGetImageUrl(itemNode.SelectSingleNode("content:encoded", FeedXmlNS)?.InnerText);
            }

            // have image node?
            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                imgUrl = itemNode.SelectSingleNode("image")?.InnerText;
            }

            // have enclosure node?
            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                var enclosureNode = itemNode.SelectSingleNode("enclosure");
                if (enclosureNode != null)
                {
                    string imgType = enclosureNode.Attributes["type"]?.InnerText;
                    if (imgType != null && imgType.StartsWith("image/"))
                    {
                        imgUrl = enclosureNode.Attributes["url"]?.InnerText;
                    }
                }
            }

            // Can we find the picture in the content?
            if (string.IsNullOrWhiteSpace(imgUrl))
            {
                imgUrl = TryGetImageUrl(item.Content);
            }

            // Save topic image.
            item.TopicPictureUri = imgUrl;

            // Get summary.
            item.Summary = GetSummary(item.Content);
            return item;
        }
    }
}
