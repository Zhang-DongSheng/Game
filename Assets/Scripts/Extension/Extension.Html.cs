using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Scripting;
using System;
using System.IO;

namespace Game
{
    public static partial class Extension
    {
        public static IConfiguration WithScripts<T>(this IConfiguration config, T scripting)
            where T : IScriptingService
        {
            return config.With(scripting);
        }

        public static IDocument ToHtmlDocument(this String sourceCode, IConfiguration configuration = null, DomEventHandler onError = null)
        {
            var context = BrowsingContext.New(configuration ?? Configuration.Default);
            var htmlParser = context.GetService<IHtmlParser>();

            if (onError is not null)
            {
                htmlParser.Error += onError;
            }
            return htmlParser.ParseDocument(sourceCode);
        }

        public static INodeList ToHtmlFragment(this String sourceCode, IElement contextElement = null, IConfiguration configuration = null)
        {
            var context = BrowsingContext.New(configuration);
            var htmlParser = context.GetService<IHtmlParser>();
            return htmlParser.ParseFragment(sourceCode, contextElement);
        }

        public static INodeList ToHtmlFragment(this String sourceCode, String contextElement, IConfiguration configuration = null)
        {
            var doc = String.Empty.ToHtmlDocument();
            var element = doc.CreateElement(contextElement);
            return sourceCode.ToHtmlFragment(element, configuration);
        }

        public static IDocument ToHtmlDocument(this Stream content, IConfiguration configuration = null)
        {
            var context = BrowsingContext.New(configuration);
            var htmlParser = context.GetService<IHtmlParser>();
            return htmlParser.ParseDocument(content);
        }
    }
}