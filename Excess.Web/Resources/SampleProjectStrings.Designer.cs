﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Excess.Web.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class SampleProjectStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SampleProjectStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Excess.Web.Resources.SampleProjectStrings", typeof(SampleProjectStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to public class Impure 
        ///{
        ///    int _i = 0;
        ///    public void modify(int ii)
        ///    {
        ///        _i += ii;
        ///    }
        ///}
        ///
        ///public pure class Pure
        ///{
        ///    public void modify(Impure ip, int ii)
        ///    {
        ///        //this call modifies state
        ///        ip.modify(ii);
        ///    }
        ///}.
        /// </summary>
        internal static string PureTest1 {
            get {
                return ResourceManager.GetString("PureTest1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to public static class StaticPure 
        ///{
        ///    public int doSomething(int ii)
        ///    {
        ///    }
        ///}
        ///
        ///public pure class Pure
        ///{
        ///    public void modify(StaticPure sp, int ii)
        ///    {
        ///        //this call does not modify state
        ///        sp.doSomething(ii);
        ///    }
        ///}.
        /// </summary>
        internal static string PureTest2 {
            get {
                return ResourceManager.GetString("PureTest2", resourceCulture);
            }
        }
    }
}
