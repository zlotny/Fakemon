public static class StringKeysReplacer
{
    public static string Replace(string originString)
    {
        // FIXME: This should be a map with key value pairs.
        originString = originString.Replace("{PLAYERNAME}", PlayerInformation.Instance.m_name);
        return originString;
    }
}