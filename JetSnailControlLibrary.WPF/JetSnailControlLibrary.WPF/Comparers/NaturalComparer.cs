using System.Collections;
using System.Collections.Generic;
using System.Globalization;

// this was translated automatically from VB
// it looks to work 
namespace JetSnailControlLibrary.WPF
{
    public class NaturalComparer : IComparer<string>, IComparer
    {
        int IComparer.Compare(object x, object y)
        {
            return ((IComparer<string>) this).Compare((string) x, (string) y);
        }

        int IComparer<string>.Compare(string string1, string string2)
        {
            _parser1.Init(string1);
            _parser2.Init(string2);
            do
            {
                int result;
                if ((_parser1.TokenType == TokenType.Numerical) & (_parser2.TokenType == TokenType.Numerical))
                    // both string1 and string2 are numerical 
                    result = decimal.Compare(_parser1.NumericalValue, _parser2.NumericalValue);
                else
                    result = string.CompareOrdinal(_parser1.StringValue, _parser2.StringValue);
                if (result != 0) return result;

                _parser1.NextToken();
                _parser2.NextToken();
            } while (!((_parser1.TokenType == TokenType.Nothing) & (_parser2.TokenType == TokenType.Nothing)));

            //identical 
            return 0;
        }

        private enum TokenType
        {
            Nothing,
            Numerical,
            String
        }

        #region StringParser Class

        private class StringParser
        {
            private readonly NaturalComparer _naturalComparer;
            private char _curChar;
            private int _idx;
            private int _len;
            private decimal _numericalValue;
            private string _source;

            public StringParser(NaturalComparer naturalComparer)
            {
                _naturalComparer = naturalComparer;
            }

            public TokenType TokenType { get; private set; }

            public decimal NumericalValue
            {
                get
                {
                    if (TokenType == TokenType.Numerical)
                        return _numericalValue;
                    throw new NaturalComparerException(
                        "Internal Error: NumericalValue called on a non numerical value.");
                }
            }

            public string StringValue { get; private set; }

            public void Init(string source)
            {
                if (source == null)
                    source = string.Empty;
                _source = source;
                _len = source.Length;
                _idx = -1;
                _numericalValue = 0;
                NextChar();
                NextToken();
            }

            public void NextToken()
            {
                do
                {
                    //CharUnicodeInfo.GetUnicodeCategory 
                    if (_curChar == '\0')
                    {
                        TokenType = TokenType.Nothing;
                        StringValue = null;
                        return;
                    }

                    if (char.IsDigit(_curChar))
                    {
                        ParseNumericalValue();
                        return;
                    }

                    if (char.IsLetter(_curChar))
                    {
                        ParseString();
                        return;
                    }

                    //ignore this character and loop some more 
                    NextChar();
                } while (true);
            }

            private void NextChar()
            {
                _idx += 1;
                _curChar = _idx >= _len ? '\0' : _source[_idx];
            }

            private void ParseNumericalValue()
            {
                var start = _idx;
                var numberDecimalSeparator = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator[0];
                var numberGroupSeparator = NumberFormatInfo.CurrentInfo.NumberGroupSeparator[0];
                do
                {
                    NextChar();
                    if (_curChar == numberDecimalSeparator)
                    {
                        // parse digits after the Decimal Separator 
                        do
                        {
                            NextChar();
                            if (!char.IsDigit(_curChar) && _curChar != numberGroupSeparator)
                                break;
                        } while (true);

                        break;
                    }

                    if (!char.IsDigit(_curChar) && _curChar != numberGroupSeparator)
                        break;
                } while (true);

                StringValue = _source.Substring(start, _idx - start);
                TokenType = decimal.TryParse(StringValue, out _numericalValue) ? TokenType.Numerical : TokenType.String;
            }

            private void ParseString()
            {
                var start = _idx;
                var roman = (_naturalComparer._naturalComparerOptions & NaturalComparerOptions.RomanNumbers) != 0;
                var romanValue = 0;
                var lastRoman = int.MaxValue;
                var cptLastRoman = 0;
                do
                {
                    if (roman)
                    {
                        var thisRomanValue = RomanLetterValue(_curChar);
                        if (thisRomanValue > 0)
                        {
                            var handled = false;

                            if (thisRomanValue == 1 || thisRomanValue == 10 || thisRomanValue == 100)
                            {
                                NextChar();
                                var nextRomanValue = RomanLetterValue(_curChar);
                                if ((nextRomanValue == thisRomanValue * 10) | (nextRomanValue == thisRomanValue * 5))
                                {
                                    handled = true;
                                    if (nextRomanValue <= lastRoman)
                                    {
                                        romanValue += nextRomanValue - thisRomanValue;
                                        NextChar();
                                        lastRoman = thisRomanValue / 10;
                                        cptLastRoman = 0;
                                    }
                                    else
                                    {
                                        roman = false;
                                    }
                                }
                            }
                            else
                            {
                                NextChar();
                            }

                            if (!handled)
                            {
                                if (thisRomanValue <= lastRoman)
                                {
                                    romanValue += thisRomanValue;
                                    if (lastRoman == thisRomanValue)
                                    {
                                        cptLastRoman += 1;
                                        switch (thisRomanValue)
                                        {
                                            case 1:
                                            case 10:
                                            case 100:
                                                if (cptLastRoman > 4)
                                                    roman = false;

                                                break;
                                            case 5:
                                            case 50:
                                            case 500:
                                                if (cptLastRoman > 1)
                                                    roman = false;

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        lastRoman = thisRomanValue;
                                        cptLastRoman = 1;
                                    }
                                }
                                else
                                {
                                    roman = false;
                                }
                            }
                        }
                        else
                        {
                            roman = false;
                        }
                    }
                    else
                    {
                        NextChar();
                    }

                    if (!char.IsLetter(_curChar)) break;
                } while (true);

                StringValue = _source.Substring(start, _idx - start);
                if (roman)
                {
                    _numericalValue = romanValue;
                    TokenType = TokenType.Numerical;
                }
                else
                {
                    TokenType = TokenType.String;
                }
            }
        }

        #endregion

        #region Methods

        private static int RomanLetterValue(char c)
        {
            switch (c)
            {
                case 'I':
                    return 1;
                case 'V':
                    return 5;
                case 'X':
                    return 10;
                case 'L':
                    return 50;
                case 'C':
                    return 100;
                case 'D':
                    return 500;
                case 'M':
                    return 1000;
                default:
                    return 0;
            }
        }

        public int RomanValue(string string1)
        {
            _parser1.Init(string1);

            if (_parser1.TokenType == TokenType.Numerical)
                return (int) _parser1.NumericalValue;
            return 0;
        }

        #endregion

        #region Private Properties

        private readonly NaturalComparerOptions _naturalComparerOptions;

        private readonly StringParser _parser1;
        private readonly StringParser _parser2;

        #endregion

        #region Constructor

        public NaturalComparer(NaturalComparerOptions naturalComparerOptions)
        {
            _naturalComparerOptions = naturalComparerOptions;
            _parser1 = new StringParser(this);
            _parser2 = new StringParser(this);
        }

        public NaturalComparer()
            : this(NaturalComparerOptions.Default)
        {
        }

        #endregion
    }
}