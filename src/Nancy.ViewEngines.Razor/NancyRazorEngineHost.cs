﻿namespace Nancy.ViewEngines.Razor
{
    using System.Web.Razor;
    using System.Web.Razor.Generator;
    using System.Web.Razor.Parser;

    using Nancy.ViewEngines.Razor.CSharp;
    using Nancy.ViewEngines.Razor.VisualBasic;

    /// <summary>
    /// A custom razor engine host responsible for decorating the existing code generators with nancy versions.
    /// </summary>
    public class NancyRazorEngineHost : RazorEngineHost
    {
        private readonly IAssemblyCatalog assemblyCatalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="NancyRazorEngineHost"/> class.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="assemblyCatalog">An <see cref="IAssemblyCatalog"/> instance.</param>
        public NancyRazorEngineHost(RazorCodeLanguage language, IAssemblyCatalog assemblyCatalog)
            : base(language)
        {
            this.assemblyCatalog = assemblyCatalog;
            this.DefaultBaseClass = typeof (NancyRazorViewBase).FullName;
            this.DefaultNamespace = "RazorOutput";
            this.DefaultClassName = "RazorView";

            var context = new GeneratedClassContext("Execute", "Write", "WriteLiteral", "WriteTo", "WriteLiteralTo",
                typeof (HelperResult).FullName, "DefineSection");
            context.ResolveUrlMethodName = "ResolveUrl";

            this.GeneratedClassContext = context;
        }

        /// <summary>
        /// Decorates the code parser.
        /// </summary>
        /// <param name="incomingCodeParser">The incoming code parser.</param>
        /// <returns></returns>
        public override ParserBase DecorateCodeParser(ParserBase incomingCodeParser)
        {
            if (incomingCodeParser is CSharpCodeParser)
            {
                return new NancyCSharpRazorCodeParser(this.assemblyCatalog);
            }

            if (incomingCodeParser is VBCodeParser)
            {
                return new NancyVisualBasicRazorCodeParser(this.assemblyCatalog);
            }

            return base.DecorateCodeParser(incomingCodeParser);
        }
    }
}