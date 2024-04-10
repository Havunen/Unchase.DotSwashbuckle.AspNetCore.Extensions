﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.Extensions.DependencyInjection;
using DotSwashbuckle.AspNetCore.SwaggerGen;
using Unchase.DotSwashbuckle.AspNetCore.Extensions.Filters;
using Unchase.DotSwashbuckle.AspNetCore.Extensions.Options;
using DotSwashbuckle.AspNetCore.SwaggerGen.XmlComments;

namespace Unchase.DotSwashbuckle.AspNetCore.Extensions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="SwaggerGenOptions"/>.
    /// </summary>
    public static class SwaggerGenOptionsExtensions
    {
        #region Extension methods

        /// <summary>
        /// Change all responses by specific http status codes in OpenApi document.
        /// </summary>
        /// <typeparam name="T">Type of response example.</typeparam>
        /// <param name="swaggerGenOptions"><see cref="SwaggerGenOptions"/>.</param>
        /// <param name="httpStatusCode">HTTP status code.</param>
        /// <param name="responseDescription">Response description.</param>
        /// <param name="responseExampleOption"><see cref="ResponseExampleOptions"/>.</param>
        /// <param name="responseExample">New example for response.</param>
        /// <returns>
        /// Returns <see cref="SwaggerGenOptions"/>.
        /// </returns>
        public static SwaggerGenOptions ChangeAllResponsesByHttpStatusCode<T>(
            this SwaggerGenOptions swaggerGenOptions,
            int httpStatusCode,
            string responseDescription = null,
            ResponseExampleOptions responseExampleOption = ResponseExampleOptions.None,
            T responseExample = default) where T : class
        {
            swaggerGenOptions.DocumentFilter<ChangeResponseByHttpStatusCodeDocumentFilter<T>>(httpStatusCode, responseDescription, responseExampleOption, responseExample);
            return swaggerGenOptions;
        }

        /// <summary>
        /// Change all responses by specific http status codes in OpenApi document.
        /// </summary>
        /// <typeparam name="T">Type of response example.</typeparam>
        /// <param name="swaggerGenOptions"><see cref="SwaggerGenOptions"/>.</param>
        /// <param name="httpStatusCode">HTTP status code.</param>
        /// <param name="responseDescription">Response description.</param>
        /// <param name="responseExampleOption"><see cref="ResponseExampleOptions"/>.</param>
        /// <param name="responseExample">New example for response.</param>
        /// <returns>
        /// Returns <see cref="SwaggerGenOptions"/>.
        /// </returns>
        public static SwaggerGenOptions ChangeAllResponsesByHttpStatusCode<T>(
            this SwaggerGenOptions swaggerGenOptions,
            HttpStatusCode httpStatusCode,
            string responseDescription = null,
            ResponseExampleOptions responseExampleOption = ResponseExampleOptions.None,
            T responseExample = default) where T : class
        {
            return swaggerGenOptions.ChangeAllResponsesByHttpStatusCode((int)httpStatusCode, responseDescription, responseExampleOption, responseExample);
        }

        /// <summary>
        /// Add filters to fix enums in OpenApi document.
        /// </summary>
        /// <param name="swaggerGenOptions"><see cref="SwaggerGenOptions"/>.</param>
        /// <param name="configureOptions">An <see cref="Action{FixEnumsOptions}"/> to configure options for filters.</param>
        public static SwaggerGenOptions AddEnumsWithValuesFixFilters(
            this SwaggerGenOptions swaggerGenOptions,
            Action<FixEnumsOptions> configureOptions = null)
        {
            // local function
            void EmptyAction(FixEnumsOptions x) { }

            swaggerGenOptions.SchemaFilter<XEnumNamesSchemaFilter>(configureOptions ?? EmptyAction);
            swaggerGenOptions.ParameterFilter<XEnumNamesParameterFilter>(configureOptions ?? EmptyAction);
            swaggerGenOptions.DocumentFilter<DisplayEnumsWithValuesDocumentFilter>();
            return swaggerGenOptions;
        }

        /// <summary>
        /// Inject human-friendly descriptions for Operations, Parameters and Schemas based on XML Comment files (from summary and remarks).
        /// </summary>
        /// <param name="swaggerGenOptions"><see cref="SwaggerGenOptions"/>.</param>
        /// <param name="xmlDocFactory">A factory method that returns XML Comments as an XPathDocument.</param>
        /// <param name="includeControllerXmlComments">
        /// Flag to indicate if controller XML comments (i.e. summary) should be used to assign Tag descriptions.
        /// Don't set this flag if you're customizing the default tag for operations via TagActionsBy.
        /// </param>
        /// <param name="excludedTypes">Types for which remarks will be excluded.</param>
        public static SwaggerGenOptions IncludeXmlCommentsWithRemarks(
            this SwaggerGenOptions swaggerGenOptions,
            Func<IReadOnlyDictionary<string, XmlCommentDescriptor>> xmlDocFactory,
            bool includeControllerXmlComments = false,
            params Type[] excludedTypes)
        {
            swaggerGenOptions.IncludeXmlComments(xmlDocFactory, includeControllerXmlComments);

            var distinctExcludedTypes = excludedTypes?.Distinct().ToArray() ?? new Type[] { };

            var xmlDoc = xmlDocFactory();
            swaggerGenOptions.ParameterFilter<XmlCommentsWithRemarksParameterFilter>(xmlDoc, distinctExcludedTypes);
            swaggerGenOptions.RequestBodyFilter<XmlCommentsWithRemarksRequestBodyFilter>(xmlDoc, distinctExcludedTypes);
            swaggerGenOptions.SchemaFilter<XmlCommentsWithRemarksSchemaFilter>(xmlDoc, distinctExcludedTypes);

            if (includeControllerXmlComments)
            {
                swaggerGenOptions.DocumentFilter<XmlCommentsWithRemarksDocumentFilter>(xmlDoc, distinctExcludedTypes);
            }

            return swaggerGenOptions;
        }

        #endregion
    }
}