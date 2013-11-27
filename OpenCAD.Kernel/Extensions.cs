namespace OpenCAD.Kernel
{
    public static class Extensions
    {
        public static string Format(this string format, params object[] args)
        {
            return string.Format(format, args);
        }
    }
}
