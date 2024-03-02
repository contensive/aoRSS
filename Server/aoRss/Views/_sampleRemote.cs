using System;
using System.Collections.Generic;
using System.Diagnostics;
using Contensive.Addons.Rss.Controllers;
using Contensive.BaseClasses;
// 
namespace Contensive.Addons.Rss.Views {
    // 
    public class GetProjectListNoDetailsClass : AddonBaseClass {
        // 
        // =====================================================================================
        // 
        public override object Execute(CPBaseClass cp) {
            string result = "";
            var sw = new Stopwatch();
            sw.Start();
            try {
                // 
                // -- initialize application. If authentication needed and not login page, pass true
                using (var ae = new applicationController(cp, true)) {
                    // 
                    // -- optionally add a timer to report how long this section took
                    ae.packageProfileList.Add(new applicationController.packageProfileClass() { name = "applicationControllerConstructor", time = sw.ElapsedMilliseconds });
                    if (ae.packageErrorList.Count == 0) {
                        // 
                        // -- get a request variable from either a querystring or a post
                        int integerValueFromUI = cp.Doc.GetInteger("integerValueFromUI");
                        // 
                        // -- get an object from the UI (javascript object stringified)
                        // -- first inject the fake data to simpulate UI input, then read it
                        cp.Doc.SetProperty("objectValueFromUI", fakeData);
                        var json_serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        var objectValueFromUI = json_serializer.Deserialize<sampleStringifiedObject>(cp.Doc.GetText("objectValueFromUI"));
                        string test = objectValueFromUI.firstname;
                        // 
                        // -- create sample data
                        var personList = Models.Db.BaseModel.createList<Models.Db.PeopleModel>(cp, "", "");
                        // 
                        // -- add sample data to a node
                        ae.packageNodeList.Add(new applicationController.packageNodeClass() {
                            dataFor = "nameOfThisDataForRemoteToRecognize",
                            data = personList
                        });
                    }
                    // 
                    // -- optionally add a timer to report how long this section took
                    ae.packageProfileList.Add(new applicationController.packageProfileClass() { name = "getProjectListNoDetailsClass", time = sw.ElapsedMilliseconds });
                    result = ae.getSerializedPackage();
                }
            } catch (Exception ex) {
                cp.Site.ErrorReport(ex);
            }
            return result;
        }
        // 
        private class sampleStringifiedObject {
            public string firstname;
            public int id;
            public List<string> friendList;
        }
        // 
        private string fakeData = "{\"firstname\":\"nameData\", \"id\":\"9\", \"friendList\":[\"Tom\",\"Dick\",\"Harry\"]}";
        // 
    }
    // 
    // 
}