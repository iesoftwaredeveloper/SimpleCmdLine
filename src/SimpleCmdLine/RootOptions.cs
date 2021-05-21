namespace SimpleCmdLine
{
    public class RootOptions
    {
        public RootOptions(string name, int optInt, /*decimal optDecimal, */bool optBool, string[] optString)
        {
            Name = name;
            OptInt = optInt;
            OptBool = optBool;
            // OptDecimal = optDecimal; // Avoid a bug https://github.com/dotnet/command-line-api/issues/1284
            OptString = optString;
        }

        public void SetOptDecimal(decimal optDecimal)
        {
            OptDecimal = optDecimal;
        }
        public string Name { get; }
        public int OptInt { get; }
        public decimal OptDecimal { get; private set; }
        public bool OptBool { get; }
        public string[] OptString { get; }
    }
}