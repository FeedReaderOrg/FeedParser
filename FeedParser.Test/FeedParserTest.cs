using System.Collections.Generic;
using Xunit;

namespace FeedParser.Test
{
    public class FeedParserTest
    {
        [Fact]
        public void ParseTechCrunchFeed20200727()
        {
            var (feedInfo, feedItems) = Parse("TechCrunchFeed.2020.07.27.xml");

            Assert.Equal("TechCrunch", feedInfo.Name);
            Assert.Equal("https://techcrunch.com/wp-content/uploads/2015/02/cropped-cropped-favicon-gradient.png?w=32", feedInfo.IconUri);

            Assert.Equal(20, feedItems.Count);
            Assert.Equal("https://techcrunch.com/wp-content/uploads/2020/07/Episode101_Still_IBRAM_X_KENDI.jpg?w=680", feedItems[0].TopicPictureUri);
        }

        [Fact]
        public void ParseNetflixTechblog20200728()
        {
            var (feedInfo, feedItems) = Parse("NetflixTechblog.2020.07.28.xml");
            
            Assert.Equal("Netflix TechBlog - Medium", feedInfo.Name);

            Assert.Equal(10, feedItems.Count);
            Assert.StartsWith("Stanislav Kirdey, William HighImagine having to go through 2.5GB", feedItems[0].Summary);
            Assert.StartsWith("By Torio Risianto, Bhargavi Reddy, Tanvi Sahni, Andrew ParkBackground on data efficiencyAt Netflix", feedItems[2].Summary);
        }

        [Fact]
        public void ParseIsoCpp20200730()
        {
            var (feedInfo, feedItems) = Parse("IsoCpp.2020.07.30.xml");
            
            Assert.NotNull(feedInfo);

            Assert.Equal(50, feedItems.Count);
            foreach (var feedItem in feedItems)
            {
                Assert.Equal(feedItem.PermentLink.Trim(), feedItem.PermentLink);
            }
        }

        [Fact]
        public void ParseFoxNews20200802()
        {
            var (feedInfo, feedItems) = Parse("FoxNews.2020.08.02.xml");

            Assert.NotNull(feedInfo);

            Assert.Equal(10, feedItems.Count);
        }

        [Fact]
        public void ParseCNET20200804()
        {
            var (feedInfo, feedItems) = Parse("CNET.2020.08.04.xml");

            Assert.NotNull(feedInfo);

            Assert.Equal(25, feedItems.Count);
            Assert.Equal("6c6f6eab-f151-42be-adc4-2974166f2f47", feedItems[0].Guid);
        }

        [Fact]
        public void ParseESPN20200805()
        {
            var (feedInfo, feedItems) = Parse("ESPN.2020.08.05.xml");

            Assert.NotNull(feedInfo);

            Assert.Equal(30, feedItems.Count);
            Assert.Equal("https://a.espncdn.com/photo/2020/0805/r727993_1296x518_5-2.jpg", feedItems[0].TopicPictureUri);
        }

        [Fact]
        public void ParseOMGUbuntu20200806()
        {
            var (feedInfo, feedItems) = Parse("OMG.Ubuntu.2020.08.06.xml");

            Assert.NotNull(feedInfo);

            Assert.Equal(27, feedItems.Count);
            Assert.Equal("http://feedproxy.google.com/~r/d0od/~3/9IiijXuE4a0/pinta-image-editor-back-from-the-dead", feedItems[0].PermentLink);
        }

        [Fact]
        public void ParseDigitalTrends20200809()
        {
            var (feedInfo, feedItems) = Parse("DigitalTrends.2020.08.09.xml");

            Assert.NotNull(feedInfo);

            Assert.Equal(30, feedItems.Count);
            Assert.Equal("https://icdn2.digitaltrends.com/image/digitaltrends/i7f2t_aq-440x292-c.jpg", feedItems[0].TopicPictureUri);
        }

        [Fact]
        public void ParseGizModo20200821_ExtractVideoPoster()
        {
            var (feedInfo, feedItems) = Parse("GizModo.2020.08.21.xml");

            Assert.NotNull(feedInfo);

            Assert.Equal(25, feedItems.Count);

            var item = feedItems.Find(i => i.Title == "Patty Jenkins Loves That Wonder Woman 1984's Golden Armor Was Kept Secret While Filming");
            Assert.NotNull(item);
            Assert.Equal("https://i.kinja-img.com/gawker-media/image/upload/s--X69Ewj_q--/c_fit,fl_progressive,q_80,w_636/cazeqgkt5e7iruca8j7h.jpg", item.TopicPictureUri);
        }

        [Fact]
        public void ParseBirdShome20200901()
        {
            var (feedInfo, feedItems) = Parse("BirdShome.2020.09.01.xml");

            Assert.NotNull(feedInfo);
            Assert.Equal("博客园_鸟食轩", feedInfo.Name);
            Assert.Equal(" Microsoft .NET[C#] MVP 2003", feedInfo.Description);
            Assert.Equal("http://www.cnblogs.com/birdshome/", feedInfo.WebsiteLink);

            Assert.NotNull(feedItems);
            Assert.Equal(10, feedItems.Count);
            Assert.Equal("低功耗Atom下载机兼Home Server咯 - birdshome", feedItems[3].Title);
            Assert.StartsWith("天天忽悠3G的今天", feedItems[3].Summary);
            Assert.StartsWith("【摘要】天天忽悠3G的今天", feedItems[3].Content);
        }

        [Fact]
        public void ParseYouTubeChannelChineseChessMasterClass20201120()
        {
            var (feedInfo, feedItems) = Parse("YouTube.Channel.ChineseChessMasterClass.2020.11.20.xml");

            Assert.NotNull(feedInfo);
            Assert.Equal("象棋MasterClass", feedInfo.Name);
            Assert.Equal("https://www.youtube.com/channel/UChvX1XzDIPLrs4mNWMRj0vw", feedInfo.WebsiteLink);

            Assert.NotNull(feedItems);
            Assert.Equal(15, feedItems.Count);
            Assert.Equal("许银川【大战】柳大华：一步就杀居然【不会走】！|| 2020年象棋甲级联赛 || 第14轮《惊天大漏》||", feedItems[0].Title);
            Assert.Equal("https://www.youtube.com/watch?v=17MiiwPmXqE", feedItems[0].PermentLink);
            Assert.Equal("https://i2.ytimg.com/vi/17MiiwPmXqE/hqdefault.jpg", feedItems[0].TopicPictureUri);
        }

        [Fact]
        public void ParseGabNews20200306()
        {
            var (feedInfo, feedItems) = Parse("GabNews.2021.03.06.json");

            Assert.NotNull(feedInfo);
            Assert.Equal("Gab News", feedInfo.Name);
            Assert.Equal("Updates From The Free Speech Social Network ", feedInfo.Description);
            Assert.Equal("https://news.gab.com/wp-content/uploads/2019/08/cropped-paste-1.png", feedInfo.IconUri);
            Assert.Equal(10, feedItems.Count);
            Assert.Equal("Another Update On The Gab Breach", feedItems[0].Title);
            Assert.Equal("I\u2019m writing to update the Gab community with what we know about the breach that occurred this week. As I have already mentioned we are conducting our own internal investigation. We are also working with federal law enforcement to assist with their investigation of the breach, criminal ransom demands and the numerous threats of violence that were directed at our team. Finally, we have hired one of the best rapid response security teams in the world to investigate the breach.&nbsp;\n\n\n\n\n\n\n\nAt this time we still have not received a copy of the data that was released by the hackers and as such we cannot independently verify its authenticity until we get a copy of it ourselves to do so.&nbsp;\n\n\n\nThat being said, several third-parties have received a copy of the data including researchers and journalists. We will share what they have published about the data, but once again we have not yet independently verified it and will continue our own internal and external investigations.&nbsp;\n\n\n\nAccording to Troy Hunt, a researcher who received the data, 43,015 unique email addresses were included along with another 24,017 emails that were shared in public posts on Gab. 7,097 hashed passwords were also included along with 62.4GB of Gab posts that can be accessed on the site right now by nature of Gab being a public forum.&nbsp;While the passwords were hashed and appear to be limited in scope based on Troy\u2019s analysis, we recommend that all Gab users change their passwords for good measure.&nbsp;\n\n\n\nTroy also reports that the data includes a small 9.53MB file of DMs (a feature that Gab only had public for a few weeks before removing it.) Troy finishes his analysis by concluding that this is \u201ca minor breach in terms of personal information exposure.\u201d&nbsp;\n\n\n\nI want to reiterate that this incident was isolated to our Gab Social product, which is built on the open source Mastodon backend. None of Gab\u2019s other products or services,&nbsp;including the Gab Shop and the GabPRO upgrade system, show any signs of being compromised.&nbsp;These services are built on separate code and infrastructure.&nbsp;\n\n\n\nFrom what we know now: the vast majority of the breached data appears to be Gab posts that are already public on the site anyway.&nbsp;As we learn more we will continue to update you on the progress of our internal and external investigations.&nbsp;\n\n\n\nThank you for your continued support and prayers.&nbsp;\n\n\n\nAndrew TorbaCEO, Gab.comMarch 6th, 2021Jesus is King", feedItems[0].Content);
            Assert.Equal("https://news.gab.com/wp-content/uploads/2020/06/at-5-2.png", feedItems[0].TopicPictureUri);
            Assert.Equal(15, feedItems[0].PubDate.Hour);
            Assert.Equal(20, feedItems[0].PubDate.Minute);
        }

        private (FeedInfo, List<FeedItem>) Parse(string filename)
        {
            var parser = FeedParser.Create(TestUtils.LoadTestData(filename));
            return (parser.ParseFeedInfo(), parser.ParseFeedItems());
        }
    }
}
