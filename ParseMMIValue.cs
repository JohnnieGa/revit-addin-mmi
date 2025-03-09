

namespace RevitPinElementsMMI
{
    public static class ParseMMIValue
    {
        public static int GetParsedMMIValue(string mmi)
        {
            int mmiValue;

            // Puts a standardvalue on the mmi-value if the parameter MMI cant get to an int
            if (string.IsNullOrWhiteSpace(mmi) || !int.TryParse(mmi, out mmiValue))
            {
                mmiValue = 0;
            }
            return mmiValue;
        }
    }
}
