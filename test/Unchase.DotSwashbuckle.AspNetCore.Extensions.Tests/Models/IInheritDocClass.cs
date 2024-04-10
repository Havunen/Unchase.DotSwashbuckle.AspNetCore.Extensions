namespace Unchase.DotSwashbuckle.AspNetCore.Extensions.Tests.Models
{
    /// <summary>
    /// InheritDocClass - inheritdoc
    /// </summary>
    /// <remarks>
    /// InheritDocClass remarks - inheritdoc
    /// </remarks>
    public interface IInheritDocClass : IInheritDocCommon
    {
        /// <summary>
        /// Name - inheritdoc
        /// </summary>
        /// <remarks>
        /// Name remarks - inheritdoc
        /// </remarks>
        string Name { get; set; }
    }
}
