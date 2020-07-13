using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;

namespace JetSnailControlLibrary.WPF
{
    /// <summary>
    ///     A custom sort support multi column comparison.
    /// </summary>
    public class DataGridColumnNaturalComparer : IComparer
    {
        #region Constructor

        public DataGridColumnNaturalComparer(List<DataGridColumn> columns)
        {
            _comparer = new NaturalComparer();
            _propertyNames = columns.Select(c => c.SortMemberPath).ToList();
        }

        #endregion

        /// <summary>
        ///     Return value by compare specified columns.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int IComparer.Compare(object x, object y)
        {
            _parser1 = new ObjectParser(x, _propertyNames);
            _parser2 = new ObjectParser(y, _propertyNames);

            do
            {
                var result = _comparer.Compare(_parser1.CurrentToken?.ToString(), _parser2.CurrentToken?.ToString());

                if (result != 0) return result;

                _parser1.NextToken();
                _parser2.NextToken();
            } while (!((_parser1.TokenType == TokenType.Nothing) & (_parser2.TokenType == TokenType.Nothing)));

            //identical 
            return 0;
        }

        private enum TokenType
        {
            Normal,
            Nothing
        }

        /// <summary>
        ///     A parser to populate an object by specified property names.
        /// </summary>
        private class ObjectParser
        {
            #region Constructor

            public ObjectParser(object source, List<string> propertyNames)
            {
                _source = source;
                _propertyNames = propertyNames;
                _length = propertyNames.Count;

                NextToken();
            }

            #endregion

            /// <summary>
            ///     Move to next token and set token type to nothing if reaches the end.
            /// </summary>
            public void NextToken()
            {
                _index += 1;

                if (_index < 0)
                    Debugger.Break();

                if (_index < _length)
                {
                    CurrentToken = _source.GetType().GetProperty(_propertyNames[_index])?.GetValue(_source);
                    TokenType = TokenType.Normal;
                }
                else
                {
                    CurrentToken = null;
                    TokenType = TokenType.Nothing;
                }
            }

            #region Private Properties

            private readonly int _length;
            private readonly List<string> _propertyNames;
            private readonly object _source;
            private int _index = -1;

            #endregion

            #region Public Properties

            public TokenType TokenType { get; private set; }
            public object CurrentToken;

            #endregion
        }

        #region Private Properties

        private readonly IComparer _comparer;
        private readonly List<string> _propertyNames;

        private ObjectParser _parser1;
        private ObjectParser _parser2;

        #endregion
    }
}