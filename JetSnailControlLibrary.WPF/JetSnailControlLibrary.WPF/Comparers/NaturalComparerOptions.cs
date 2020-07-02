namespace JetSnailControlLibrary.WPF
{
    [System.Flags()]
    public enum NaturalComparerOptions
    {
        None,
        RomanNumbers,
        //DecimalValues <- we could put this as an option 
        //IgnoreSpaces <- we could put this as an option 
        //IgnorePunctuation <- we could put this as an option 
        Default = None
    }
}