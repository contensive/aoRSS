using System;
using System.Diagnostics;
using Contensive.Addons.Rss.Controllers;
using Contensive.BaseClasses;
using Microsoft.VisualBasic;

namespace Contensive.Addons.Rss.Views {
    // 
    public class AddonClass : AddonBaseClass {
        // 
        // - if nuget references are not there, open nuget command line and click the 'reload' message at the top, or run "Update-Package -reinstall" - close/open
        // - Verify project root name space is empty
        // - Change the namespace (AddonCollectionVb) to the collection name
        // - Change this class name to the addon name
        // - Create a Contensive Addon record with the namespace apCollectionName.ad
        // 
        // =====================================================================================
        /// <summary>
        /// AddonDescription
        /// </summary>
        /// <param name="CP"></param>
        /// <returns></returns>
        public override object Execute(CPBaseClass CP) {
            string result = "";
            var sw = new Stopwatch();
            sw.Start();
            try {
                // 
                // -- initialize application. If authentication needed and not login page, pass true
                using (var ae = new applicationController(CP, false)) {
                    // 
                    // -- your code
                    result = "Hello World";
                    if (ae.packageErrorList.Count > 0) {
                        result = "Hey user, this happened - " + Strings.Join(ae.packageErrorList.ToArray(), "<br>");
                    }
                }
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
            }
            return result;
        }
    }
}