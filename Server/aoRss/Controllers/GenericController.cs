using System;
using Contensive.BaseClasses;

namespace Contensive.Addons.Rss.Controllers {
    public sealed class GenericController {
        //
        //
        public static string GetGMTFromDate(DateTime DateValue) {
            if (DateValue == default) {
                return "";
            }
            return DateValue.ToUniversalTime().ToString("R");
        }
        //
        // ====================================================================================================
        /// <summary>
        /// return an anchor tag for the provided feed
        /// </summary>
        /// <param name="cp"></param>
        /// <param name="rssfeed"></param>
        /// <returns></returns>
        public static string getFeedLink(CPBaseClass cp, Models.Db.RSSFeedModel rssfeed) {
            if (string.IsNullOrEmpty(rssfeed.RSSFilename)) {
                return $"<img src=\"/rssFeeds/IconXML-25x13.gif\" width=25 height=13 border=0 class=\"RSSFeedImage\">&nbsp;{rssfeed.name}&nbsp;(Coming Soon)";
            } else {
                return $"<img src=\"/rssFeeds/IconXML-25x13.gif\" width=25 height=13 border=0 class=\"RSSFeedImage\">&nbsp;<a class=\"RSSFeedLink\" href=\"{cp.Http.CdnFilePathPrefixAbsolute}{rssfeed.RSSFilename}\">{rssfeed.name}</a>";
            }
        }
        //
        // ====================================================================================================
        /// <summary>
        /// tmp Placeholder for CP.Http.CdnFilePathPrefixAbsolute
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        public static string getCdnFilePathPrefixAbsolute(CPBaseClass cp) {

            if (!cp.ServerConfig.isLocalFileSystem) {
                //
                // -- remote file system, return cdnfileurl
                return cp.GetAppConfig().cdnFileUrl;
            } else {
                //
                // -- local file system
                string cdnFilePathPrefixAbsolute = cp.Site.GetText("CdnFilePathPrefixAbsolute")?.Replace(@"\", "/");
                if (!string.IsNullOrWhiteSpace(cdnFilePathPrefixAbsolute)) {
                    if (cdnFilePathPrefixAbsolute[cdnFilePathPrefixAbsolute.Length - 1] != '/') {
                        cdnFilePathPrefixAbsolute += "/";
                    }
                } else {
                    cdnFilePathPrefixAbsolute = $"https://{cp.Site.DomainPrimary}/{cp.GetAppConfig().cdnFileUrl}";
                }
                return cdnFilePathPrefixAbsolute;
            }
        }

    }
}
