
namespace DekCalc.Functions
{
    internal class Assert
    {
        internal static void IsTrue(bool v, string errorMessage = "True was expected, but was false")
        {
            if (!v)
            {
                throw new Exception(errorMessage);
            }
        }
    }
}