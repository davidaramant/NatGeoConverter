#pragma warning disable 1591
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.17020
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Website_Generator
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#line 2 "SiteLayout.cshtml"
using Website_Generator.Models;

#line default
#line hidden


[System.CodeDom.Compiler.GeneratedCodeAttribute("RazorTemplatePreprocessor", "2.6.0.0")]
public partial class SiteLayout : SiteLayoutBase
{

#line hidden

#line 1 "SiteLayout.cshtml"
public SiteLayoutModel Model { get; set; }

#line default
#line hidden


public override void Execute()
{
WriteLiteral("<!DOCTYPE html>\n<html");

WriteLiteral(" lang=\"en\"");

WriteLiteral(">\n  <head>\n      <meta");

WriteLiteral(" charset=\"utf-8\"");

WriteLiteral("/>\n      <meta");

WriteLiteral(" http-equiv=\"X-UA-Compatible\"");

WriteLiteral(" content=\"IE=edge\"");

WriteLiteral("/>\n      <meta");

WriteLiteral(" name=\"viewport\"");

WriteLiteral(" content=\"width=device-width, initial-scale=1\"");

WriteLiteral("/>\n      <title>");


#line 9 "SiteLayout.cshtml"
        Write(Model.PageTitle);


#line default
#line hidden
WriteLiteral("</title>\n");


#line 10 "SiteLayout.cshtml"
  

#line default
#line hidden

#line 10 "SiteLayout.cshtml"
   foreach( var cssUrl in Model.GetCssUrls() ) {


#line default
#line hidden
WriteLiteral("    <link");

WriteAttribute ("href", " href=\"", "\""

#line 11 "SiteLayout.cshtml"
, Tuple.Create<string,object,bool> ("", cssUrl

#line default
#line hidden
, false)
);
WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral("/>\n");


#line 12 "SiteLayout.cshtml"
  }


#line default
#line hidden
WriteLiteral("    <link");

WriteLiteral(" rel=\"shortcut icon\"");

WriteAttribute ("href", " href=\"", "\""

#line 13 "SiteLayout.cshtml"
, Tuple.Create<string,object,bool> ("", Model.IconUrl

#line default
#line hidden
, false)
);
WriteLiteral("/>\n  </head>\n  <body");

WriteAttribute ("class", " class=\"", "\""

#line 15 "SiteLayout.cshtml"
, Tuple.Create<string,object,bool> ("", Model.Body.BodyClass

#line default
#line hidden
, false)
);
WriteLiteral(">\n    <div");

WriteLiteral(" class=\"navbar navbar-default navbar-fixed-top\"");

WriteLiteral(" role=\"navigation\"");

WriteLiteral(">  \n    <div");

WriteLiteral(" class=\"container\"");

WriteLiteral(">\n      <div");

WriteLiteral(" class=\"navbar-collapse collapse\"");

WriteLiteral(">\n        <ul");

WriteLiteral(" class=\"nav navbar-nav\"");

WriteLiteral(">\n          <ul");

WriteLiteral(" class=\"breadcrumb list-inline\"");

WriteLiteral(">\n");


#line 21 "SiteLayout.cshtml"
        

#line default
#line hidden

#line 21 "SiteLayout.cshtml"
         foreach( var link in Model.Body.GetBreadcrumbParts() ) {
          if( link.HasUrl ) {


#line default
#line hidden
WriteLiteral("            <li><a");

WriteAttribute ("href", " href=\"", "\""

#line 23 "SiteLayout.cshtml"
, Tuple.Create<string,object,bool> ("", link.Url

#line default
#line hidden
, false)
);
WriteLiteral(">");


#line 23 "SiteLayout.cshtml"
                               Write(link.Name);


#line default
#line hidden
WriteLiteral("</a></li>\n");


#line 24 "SiteLayout.cshtml"
          } else {


#line default
#line hidden
WriteLiteral("            <li");

WriteLiteral(" class=\"active\"");

WriteLiteral(">");


#line 25 "SiteLayout.cshtml"
                          Write(link.Name);


#line default
#line hidden
WriteLiteral("</li>\n");


#line 26 "SiteLayout.cshtml"
          }
        }


#line default
#line hidden
WriteLiteral("          </ul>\n        </ul>\n        <div");

WriteLiteral(" class=\"nav navbar-right\"");

WriteLiteral(">\n          <div");

WriteLiteral(" class=\"btn-group\"");

WriteLiteral(">\n");


#line 32 "SiteLayout.cshtml"
          

#line default
#line hidden

#line 32 "SiteLayout.cshtml"
           if( Model.Body.Previous != null )
          {


#line default
#line hidden
WriteLiteral("            <a");

WriteLiteral(" class=\"btn navbar-btn btn-default btn-page\"");

WriteAttribute ("href", " href=\"", "\""

#line 34 "SiteLayout.cshtml"
                          , Tuple.Create<string,object,bool> ("", Model.Body.Previous.Url

#line default
#line hidden
, false)
);
WriteLiteral(">\n              <span");

WriteLiteral(" class=\"glyphicon glyphicon-chevron-left\"");

WriteLiteral("/>\n            </a>\n");


#line 37 "SiteLayout.cshtml"
          } else {


#line default
#line hidden
WriteLiteral("            <button");

WriteLiteral(" class=\"btn navbar-btn btn-default btn-page\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(">\n              <span");

WriteLiteral(" class=\"glyphicon glyphicon-chevron-left\"");

WriteLiteral("/>\n            </button>\n");


#line 41 "SiteLayout.cshtml"
          }


#line default
#line hidden
WriteLiteral("            <button");

WriteLiteral(" class=\"btn navbar-btn btn-default\"");

WriteAttribute ("disabled", " disabled=\"", "\""

#line 42 "SiteLayout.cshtml"
                          , Tuple.Create<string,object,bool> ("", Model.Body.AllowResizeText

#line default
#line hidden
, false)
);
WriteLiteral(" onClick=\"toggleHorizontal();\"");

WriteLiteral(">\n              <span");

WriteLiteral(" class=\"glyphicon glyphicon-resize-horizontal\"");

WriteLiteral("/>\n            </button>\n            <button");

WriteLiteral(" class=\"btn navbar-btn btn-default\"");

WriteAttribute ("disabled", " disabled=\"", "\""

#line 45 "SiteLayout.cshtml"
                          , Tuple.Create<string,object,bool> ("", Model.Body.AllowResizeText

#line default
#line hidden
, false)
);
WriteLiteral(" onClick=\"toggleVertical();\"");

WriteLiteral(">\n              <span");

WriteLiteral(" class=\"glyphicon glyphicon-resize-vertical\"");

WriteLiteral("/>\n            </button>                     \n");


#line 48 "SiteLayout.cshtml"
          

#line default
#line hidden

#line 48 "SiteLayout.cshtml"
           if( Model.Body.Next != null )
          {


#line default
#line hidden
WriteLiteral("            <a");

WriteLiteral(" class=\"btn navbar-btn btn-default btn-page\"");

WriteAttribute ("href", " href=\"", "\""

#line 50 "SiteLayout.cshtml"
                          , Tuple.Create<string,object,bool> ("", Model.Body.Next.Url

#line default
#line hidden
, false)
);
WriteLiteral(">\n              <span");

WriteLiteral(" class=\"glyphicon glyphicon-chevron-right\"");

WriteLiteral("/>\n            </a>\n");


#line 53 "SiteLayout.cshtml"
          } else {


#line default
#line hidden
WriteLiteral("            <button");

WriteLiteral(" class=\"btn navbar-btn btn-default btn-page\"");

WriteLiteral(" disabled=\"disabled\"");

WriteLiteral(">\n              <span");

WriteLiteral(" class=\"glyphicon glyphicon-chevron-right\"");

WriteLiteral("/>\n            </button>\n");


#line 57 "SiteLayout.cshtml"
          }


#line default
#line hidden
WriteLiteral("          </div>\n        </div>\n      </div>\n    </div>\n  </div>\n");

WriteLiteral("    ");


#line 63 "SiteLayout.cshtml"
Write(Model.RenderBody());


#line default
#line hidden
WriteLiteral("\n");


#line 64 "SiteLayout.cshtml"
 foreach( var jsUrl in Model.GetJSUrls() ) {


#line default
#line hidden
WriteLiteral("  <script");

WriteAttribute ("src", " src=\"", "\""

#line 65 "SiteLayout.cshtml"
, Tuple.Create<string,object,bool> ("", jsUrl

#line default
#line hidden
, false)
);
WriteLiteral("></script>\n");


#line 66 "SiteLayout.cshtml"
}


#line default
#line hidden
WriteLiteral("  </body>\n</html>");

}
}

// NOTE: this is the default generated helper class. You may choose to extract it to a separate file 
// in order to customize it or share it between multiple templates, and specify the template's base 
// class via the @inherits directive.
public abstract class SiteLayoutBase
{

		// This field is OPTIONAL, but used by the default implementation of Generate, Write, WriteAttribute and WriteLiteral
		//
		System.IO.TextWriter __razor_writer;

		// This method is OPTIONAL
		//
		/// <summary>Executes the template and returns the output as a string.</summary>
		/// <returns>The template output.</returns>
		public string GenerateString ()
		{
			using (var sw = new System.IO.StringWriter ()) {
				Generate (sw);
				return sw.ToString ();
			}
		}

		// This method is OPTIONAL, you may choose to implement Write and WriteLiteral without use of __razor_writer
		// and provide another means of invoking Execute.
		//
		/// <summary>Executes the template, writing to the provided text writer.</summary>
		/// <param name="writer">The TextWriter to which to write the template output.</param>
		public void Generate (System.IO.TextWriter writer)
		{
			__razor_writer = writer;
			Execute ();
			__razor_writer = null;
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the template output without HTML escaping it.</summary>
		/// <param name="value">The literal value.</param>
		protected void WriteLiteral (string value)
		{
			__razor_writer.Write (value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the TextWriter without HTML escaping it.</summary>
		/// <param name="writer">The TextWriter to which to write the literal.</param>
		/// <param name="value">The literal value.</param>
		protected static void WriteLiteralTo (System.IO.TextWriter writer, string value)
		{
			writer.Write (value);
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a value to the template output, HTML escaping it if necessary.</summary>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected void Write (object value)
		{
			WriteTo (__razor_writer, value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes an object value to the TextWriter, HTML escaping it if necessary.</summary>
		/// <param name="writer">The TextWriter to which to write the value.</param>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected static void WriteTo (System.IO.TextWriter writer, object value)
		{
			if (value == null)
				return;

			var write = value as Action<System.IO.TextWriter>;
			if (write != null) {
				write (writer);
				return;
			}

			//NOTE: a more sophisticated implementation would write safe and pre-escaped values directly to the
			//instead of double-escaping. See System.Web.IHtmlString in ASP.NET 4.0 for an example of this.
			System.Net.WebUtility.HtmlEncode (value.ToString (), writer);
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to the template output.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		protected void WriteAttribute (string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			WriteAttributeTo (__razor_writer, name, prefix, suffix, values);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to a TextWriter.
		/// </summary>
		/// <param name="writer">The TextWriter to which to write the attribute.</param>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		///<remarks>Used by Razor helpers to write attributes.</remarks>
		protected static void WriteAttributeTo (System.IO.TextWriter writer, string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			// this is based on System.Web.WebPages.WebPageExecutingBase
			// Copyright (c) Microsoft Open Technologies, Inc.
			// Licensed under the Apache License, Version 2.0
			if (values.Length == 0) {
				// Explicitly empty attribute, so write the prefix and suffix
				writer.Write (prefix);
				writer.Write (suffix);
				return;
			}

			bool first = true;
			bool wroteSomething = false;

			for (int i = 0; i < values.Length; i++) {
				Tuple<string,object,bool> attrVal = values [i];
				string attPrefix = attrVal.Item1;
				object value = attrVal.Item2;
				bool isLiteral = attrVal.Item3;

				if (value == null) {
					// Nothing to write
					continue;
				}

				// The special cases here are that the value we're writing might already be a string, or that the 
				// value might be a bool. If the value is the bool 'true' we want to write the attribute name instead
				// of the string 'true'. If the value is the bool 'false' we don't want to write anything.
				//
				// Otherwise the value is another object (perhaps an IHtmlString), and we'll ask it to format itself.
				string stringValue;
				bool? boolValue = value as bool?;
				if (boolValue == true) {
					stringValue = name;
				} else if (boolValue == false) {
					continue;
				} else {
					stringValue = value as string;
				}

				if (first) {
					writer.Write (prefix);
					first = false;
				} else {
					writer.Write (attPrefix);
				}

				if (isLiteral) {
					writer.Write (stringValue ?? value);
				} else {
					WriteTo (writer, stringValue ?? value);
				}
				wroteSomething = true;
			}
			if (wroteSomething) {
				writer.Write (suffix);
			}
		}
		// This method is REQUIRED. The generated Razor subclass will override it with the generated code.
		//
		///<summary>Executes the template, writing output to the Write and WriteLiteral methods.</summary>.
		///<remarks>Not intended to be called directly. Call the Generate method instead.</remarks>
		public abstract void Execute ();

}
}
#pragma warning restore 1591