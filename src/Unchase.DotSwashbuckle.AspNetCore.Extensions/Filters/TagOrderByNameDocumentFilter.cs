using System.Linq;

using Microsoft.OpenApi.Models;
using DotSwashbuckle.AspNetCore.SwaggerGen;

namespace Unchase.DotSwashbuckle.AspNetCore.Extensions.Filters
{
    /// <summary>
    /// Document filter for ordering tags by name in OpenApi document.
    /// </summary>
    public class TagOrderByNameDocumentFilter :
        IDocumentFilter
    {
        #region Methods

        /// <summary>
        /// Apply filter.
        /// </summary>
        /// <param name="swaggerDoc"><see cref="OpenApiDocument"/>.</param>
        /// <param name="context"><see cref="DocumentFilterContext"/>.</param>
        public void Apply(
            OpenApiDocument swaggerDoc,
            DocumentFilterContext context)
        {
            swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(t => t.Name).ToList();
        }

        #endregion
    }
}