using System;
using Contensive.BaseClasses;

namespace Contensive.Addons.Rss.Controllers {
    public sealed class GenericController {
        //
        //
        public static string GetGMTFromDate(DateTime DateValue) {
            string result = "";
            int WorkLong;
            //
            if (DateValue != default(DateTime)) {
                switch (DateValue.DayOfWeek) {
                    case DayOfWeek.Sunday:
                        result = "Sun, ";
                        break;
                    case DayOfWeek.Monday:
                        result = "Mon, ";
                        break;
                    case DayOfWeek.Tuesday:
                        result = "Tue, ";
                        break;
                    case DayOfWeek.Wednesday:
                        result = "Wed, ";
                        break;
                    case DayOfWeek.Thursday:
                        result = "Thu, ";
                        break;
                    case DayOfWeek.Friday:
                        result = "Fri, ";
                        break;
                    case DayOfWeek.Saturday:
                        result = "Sat, ";
                        break;
                }
                //
                WorkLong = DateValue.Day;
                if (WorkLong < 10) {
                    result = $"{result}0{WorkLong} ";
                } else {
                    result = $"{result}{WorkLong} ";
                }
                //
                switch (DateValue.Month) {
                    case 1:
                        result = $"{result}Jan ";
                        break;
                    case 2:
                        result = $"{result}Feb ";
                        break;
                    case 3:
                        result = $"{result}Mar ";
                        break;
                    case 4:
                        result = $"{result}Apr ";
                        break;
                    case 5:
                        result = $"{result}May ";
                        break;
                    case 6:
                        result = $"{result}Jun ";
                        break;
                    case 7:
                        result = $"{result}Jul ";
                        break;
                    case 8:
                        result = $"{result}Aug ";
                        break;
                    case 9:
                        result = $"{result}Sep ";
                        break;
                    case 10:
                        result = $"{result}Oct ";
                        break;
                    case 11:
                        result = $"{result}Nov ";
                        break;
                    case 12:
                        result = $"{result}Dec ";
                        break;
                }
                //
                result = $"{result}{DateValue.Year} ";
                //
                WorkLong = DateValue.Hour;
                if (WorkLong < 10) {
                    result = $"{result}0{WorkLong}:";
                } else {
                    result = $"{result}{WorkLong}:";
                }
                //
                WorkLong = DateValue.Minute;
                if (WorkLong < 10) {
                    result = $"{result}0{WorkLong}:";
                } else {
                    result = $"{result}{WorkLong}:";
                }
                //
                WorkLong = DateValue.Second;
                if (WorkLong < 10) {
                    result = $"{result}0{WorkLong}";
                } else {
                    result = $"{result}{WorkLong}";
                }
                //
                result = $"{result} GMT";
            }
            return result;
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
                    if (!cdnFilePathPrefixAbsolute.Substring(cdnFilePathPrefixAbsolute.Length, 1).Equals("/")) {
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
