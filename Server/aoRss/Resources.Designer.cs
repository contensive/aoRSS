﻿// ------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by a tool.
// Runtime Version:4.0.30319.42000
// 
// Changes to this file may cause incorrect behavior and will be lost if
// the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------




using System.Diagnostics;
using Microsoft.VisualBasic;

namespace Contensive.Addons.Rss.My.Resources {

    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    /// <summary>
    /// A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [DebuggerNonUserCode()]
    [System.Runtime.CompilerServices.CompilerGenerated()]
    [HideModuleName()]
    internal static class Resources {

        private static System.Resources.ResourceManager resourceMan;

        private static System.Globalization.CultureInfo resourceCulture;

        /// <summary>
        /// Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Resources.ResourceManager ResourceManager {
            get {
                if (ReferenceEquals(resourceMan, null)) {
                    var temp = new System.Resources.ResourceManager("Contensive.Addons.Rss.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }

        /// <summary>
        /// Looks up a localized string similar to 
        /// Option Explicit
        /// &apos;
        /// #Const DebugBuild = False
        /// &apos;
        /// &apos;=======================================================================
        /// &apos;   sitepropertyNames
        /// &apos;=======================================================================
        /// &apos;
        /// Public Const siteproperty_serverPageDefault_name = &quot;serverPageDefault&quot;
        /// Public Const siteproperty_serverPageDefault_defaultValue = &quot;index.php&quot;
        /// &apos;
        /// &apos;=======================================================================
        /// &apos;   content replacements
        /// &apos;====================================== [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string LegacyVb6_ccCommonModule {
            get {
                return ResourceManager.GetString("LegacyVb6_ccCommonModule", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to  
        /// Option Explicit
        /// &apos;
        /// &apos;========================================================================
        /// &apos;   kma defined errors
        /// &apos;       1000-1999 Contensive
        /// &apos;       2000-2999 Datatree
        /// &apos;
        /// &apos;   see kmaErrorDescription() for transations
        /// &apos;========================================================================
        /// &apos;
        /// Const Error_DataTree_RootNodeNext = 2000
        /// Const Error_DataTree_NoGoNext = 2001
        /// &apos;
        /// &apos;========================================================================
        /// &apos;
        /// &apos;========================================== [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string LegacyVb6_KmaCommonModule {
            get {
                return ResourceManager.GetString("LegacyVb6_KmaCommonModule", resourceCulture);
            }
        }

        /// <summary>
        /// Looks up a localized string similar to select top 1 m.name as personName, o.organizationName 
        /// from ccmembers m left join organizations o on o.id=m.organizationId
        /// where o.id={0}.
        /// </summary>
        internal static string sampleSql {
            get {
                return ResourceManager.GetString("sampleSql", resourceCulture);
            }
        }
    }
}