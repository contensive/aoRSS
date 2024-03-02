



using System;
using System.Collections.Generic;

namespace Contensive.Addons.Rss.Models.Domain {
    public class FeedModel {
        public int Id;
        public string Name;
        public string Description;
        public string Link;
        public string LogoFilename;
        public string RSSFilename;
        public List<FeedEntryModel> entryList;
        // Public EntryCnt As Integer
        // Public EntrySize As Integer
        // Public EntryDateIndex As New Dictionary(Of String, Integer)
    }

    public class FeedEntryModel {
        public string Title;
        public string Description;
        public DateTime DatePublish;
        public DateTime DateExpires;
        public string Link;
        public int storyContentID;
        // Public storyTableID As Integer
        public int storyRecordID;
        public string PodcastMediaLink;
    }
}