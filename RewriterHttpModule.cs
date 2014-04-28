// UrlRewriter - A .NET URL Rewriter module
// Version 2.0
//
// Copyright 2011 Intelligencia
// Copyright 2011 Seth Yates
// 

using System;
using System.Web;
using Intelligencia.UrlRewriter.Configuration;
using Intelligencia.UrlRewriter.Utilities;

namespace Intelligencia.UrlRewriter
{
    /// <summary>
    /// Main HTTP Module for the URL Rewriter.
    /// Rewrites URL's based on patterns and conditions specified in the configuration file.
    /// This class cannot be inherited.
    /// </summary>
    public sealed class RewriterHttpModule : IHttpModule
    {
        /// <summary>
        /// Initialises the module.
        /// </summary>
        /// <param name="context">The application context.</param>
        void IHttpModule.Init(HttpApplication context)
        {
            context.BeginRequest += BeginRequest;
        }

        /// <summary>
        /// Disposes of the module.
        /// </summary>
        void IHttpModule.Dispose()
        {
        }

        /// <summary>
        /// Current Configuration for the URL rewriter.
        /// </summary>
        public static RewriterConfiguration Configuration
        {
            get
            {
                return RewriterConfiguration.Current;
            }
        }

        /// <summary>
        /// The original query string.
        /// </summary>
        public static string OriginalQueryString
        {
            get
            {
                return _rewriter.OriginalQueryString;
            }
            set
            {
                _rewriter.OriginalQueryString = value;
            }
        }

        /// <summary>
        /// The final querystring, after rewriting.
        /// </summary>
        public static string QueryString
        {
            get
            {
                return _rewriter.QueryString;
            }
            set
            {
                _rewriter.QueryString = value;
            }
        }

        /// <summary>
        /// The raw URL for the current request, before any rewriting.
        /// </summary>
        public static string RawUrl
        {
            get { return _rewriter.RawUrl; }
        }

        /// <summary>
        /// Event handler for the "BeginRequest" event.
        /// </summary>
        /// <param name="sender">The sender object</param>
        /// <param name="args">Event args</param>
        private void BeginRequest(object sender, EventArgs args)
        {
            // Add our PoweredBy header
            // HttpContext.Current.Response.AddHeader(Constants.HeaderXPoweredBy, Configuration.XPoweredBy);

            _rewriter.Rewrite();
        }

        /// <summary>
        /// Resolves an Application-path relative location
        /// </summary>
        /// <param name="location">The location</param>
        /// <returns>The absolute location.</returns>
        public static string ResolveLocation(string location)
        {
            return _rewriter.ResolveLocation(location);
        }

        private static RewriterEngine _rewriter = new RewriterEngine(
            new HttpContextFacade(),
            new ConfigurationManagerFacade(),
            RewriterConfiguration.Current);
    }
}
