using System.Reflection;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Defines a contract for <see cref="BaseFilterEx" /> class.
    /// </summary>
    public interface IFilterEx
    {
        /// <summary>
        ///     An object representing the public field property that the filter expression to apply on.
        /// </summary>
        PropertyInfo FieldInfo { get; }

        /// <summary>
        ///     Indicates whether the filter expression finds a match to the input parameter.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool IsMatch(object input);
    }
}