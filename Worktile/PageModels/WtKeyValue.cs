namespace Worktile.PageModels
{
    class WtKeyValue
    {
        public WtKeyValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }
}
