using System;
using System.Drawing;

namespace eventCountDown
{
    class Utils
    {
        // whether a specific font exists
        public static bool FontExists(string fontName)
        {
            // if font exists, testFont will be that font. Otherwise it will fallback to system default
            using (var testFont = new Font(fontName, 8F))
            {
                return 0 == string.Compare(
                  fontName,
                  testFont.Name,
                  StringComparison.InvariantCultureIgnoreCase);
            }
        }
    }
}
