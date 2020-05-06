using System.Reflection;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     Represents a filter expression using contains checks.
    /// </summary>
    public class StringBaseFilterEx : BaseFilterEx
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="StringBaseFilterEx" /> class.
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="substring"></param>
        public StringBaseFilterEx(PropertyInfo fieldInfo, string substring = null) : base(fieldInfo)
        {
            Substring = substring;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     The substring to check for contain
        /// </summary>
        public string Substring { set; get; }

        #endregion

        #region Method

        /// <summary>
        ///     Indicates whether the filter expression finds a match to the input parameter.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>
        ///     <c>true</c> if the specified input is a match; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsMatch(object input)
        {
            var str = (string) FieldInfo.GetValue(input, null);
            return str.Contains(Substring);
        }

        #endregion
    }
}