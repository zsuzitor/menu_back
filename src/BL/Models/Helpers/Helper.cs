

namespace BL.Models.Helpers
{
    public static class Helper
    {

        public static bool Eql(this string a, string b)
        {
            return string.Compare(a, b, true) == 0;
        }

    }
}
