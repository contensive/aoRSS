using System;
using System.Collections.Generic;
using Contensive.BaseClasses;

namespace Contensive.Addons.Rss.Controllers {
    // 
    // ====================================================================================================
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class applicationController : IDisposable {
        // 
        // privates passed in, do not dispose
        // 
        private CPBaseClass cp;
        // 
        // ====================================================================================================
        /// <summary>
        /// Errors accumulated during rendering.
        /// </summary>
        /// <returns></returns>
        public List<packageErrorClass> packageErrorList { get; set; } = new List<packageErrorClass>();
        // 
        // ====================================================================================================
        /// <summary>
        /// data accumulated during rendering
        /// </summary>
        /// <returns></returns>
        public List<packageNodeClass> packageNodeList { get; set; } = new List<packageNodeClass>();
        // 
        // ====================================================================================================
        /// <summary>
        /// list of name/time used to performance analysis
        /// </summary>
        /// <returns></returns>
        public List<packageProfileClass> packageProfileList { get; set; } = new List<packageProfileClass>();
        // 
        // ====================================================================================================
        /// <summary>
        /// get the serialized results
        /// </summary>
        /// <returns></returns>
        public string getSerializedPackage() {
            string result = "";
            try {
                result = serializeObject(cp, new packageClass() {
                    success = packageErrorList.Count.Equals(0),
                    nodeList = packageNodeList,
                    errorList = packageErrorList,
                    profileList = packageProfileList
                });
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            // 
            return result;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks></remarks>
        public applicationController(CPBaseClass cp, bool requiresAuthentication = true) {
            this.cp = cp;
            string sql = "";
            var cs = cp.CSNew();
            string localSystemStatus = "";
            if (requiresAuthentication & !cp.User.IsAuthenticated) {
                packageErrorList.Add(new packageErrorClass() { number = (int)constants.resultErrorEnum.errAuthentication, description = "Authorization is required." });
                cp.Response.SetStatus(((int)constants.httpErrorEnum.forbidden).ToString() + " Forbidden");
            }
        }
        // 
        public static string serializeObject(CPBaseClass CP, object dataObject) {
            string result = "";
            try {
                var json_serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                result = json_serializer.Serialize(dataObject);
            } catch (Exception ex) {
                CP.Site.ErrorReport(ex);
            }
            return result;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// list of events and their stopwatch times
        /// </summary>
        public class packageProfileClass {
            public string name;
            public long time;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// remote method top level data structure
        /// </summary>
        [Serializable()]
        public class packageClass {
            public bool success = false;
            public List<packageErrorClass> errorList = new List<packageErrorClass>();
            public List<packageNodeClass> nodeList = new List<packageNodeClass>();
            public List<packageProfileClass> profileList;
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// data store for jsonPackage
        /// </summary>
        [Serializable()]
        public class packageNodeClass {
            public string dataFor = "";
            public object data; // IEnumerable(Of Object)
        }
        // 
        // ====================================================================================================
        /// <summary>
        /// error list for jsonPackage
        /// </summary>
        [Serializable()]
        public class packageErrorClass {
            public int number = 0;
            public string description = "";
        }
        // 
        #region  IDisposable Support 
        protected bool disposed = false;
        // 
        // ==========================================================================================
        /// <summary>
        /// dispose
        /// </summary>
        /// <param name="disposing"></param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing) {
            if (!disposed) {
                if (disposing) {
                    // 
                    // ----- call .dispose for managed objects
                    // 
                }
                // 
                // Add code here to release the unmanaged resource.
                // 
            }
            disposed = true;
        }
        // Do not change or add Overridable to these methods.
        // Put cleanup code in Dispose(ByVal disposing As Boolean).
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~applicationController() {
            Dispose(false);
        }
        #endregion
    }
}