




using System;
using Contensive.BaseClasses;

namespace Contensive.Addons.Rss.Models.View {
    public class RequestModel {
        // 
        // ====================================================================================================
        // 
        private CPBaseClass cp;
        /// <summary>
        /// string that represents the guid of the blog record to be displayed
        /// </summary>
        /// <returns></returns>
        public string instanceId {
            get {
                if (_instanceId is null) {
                    _instanceId = cp.Doc.GetText("instanceId");
                    if (string.IsNullOrWhiteSpace(_instanceId))
                        _instanceId = "RssClientWithoutinstanceId-PageId-" + cp.Doc.PageId;
                }
                return _instanceId;
            }
        }
        private string _instanceId = null;
        /// <summary>
        /// Sample request property
        /// </summary>
        /// <returns></returns>
        public int sampleProperty {
            get {
                if (_sampleProperty is null) {
                    _sampleProperty = cp.Doc.GetInteger("sample1");
                }
                return Convert.ToInt32(_sampleProperty);
            }
        }
        private int? _sampleProperty = default;
        // 
        // todo - convert these to ondemand properties
        // 
        public string sampleQuickProperty {
            get {
                return cp.Doc.GetText("sample2");
            }
        }
        // 
        // 
        // 
        public RequestModel(CPBaseClass cp) {
            this.cp = cp;
        }
    }
}