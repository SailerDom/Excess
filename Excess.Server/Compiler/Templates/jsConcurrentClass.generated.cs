﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ASP
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("RazorGenerator", "2.0.0.0")]
    internal partial class _Templates_jsConcurrentClass_template : RazorGenerator.Templating.RazorTemplateBase
    {
#line hidden
        #line 2 "..\..\Templates\jsConcurrentClass.template"
            
  public dynamic Model { get; set; }

        #line default
        #line hidden
        
        public override void Execute()
        {
WriteLiteral("\r\n");

WriteLiteral("\r\n");

            
            #line 6 "..\..\Templates\jsConcurrentClass.template"
Write(Model.Name);

            
            #line default
            #line hidden
WriteLiteral(" = function(__ID)\r\n{\r\n");

WriteLiteral("\t");

            
            #line 8 "..\..\Templates\jsConcurrentClass.template"
Write(Model.Body);

            
            #line default
            #line hidden
WriteLiteral("\r\n\r\n    this.__ID = __ID;\r\n}\");\r\n");

        }
    }
}
#pragma warning restore 1591