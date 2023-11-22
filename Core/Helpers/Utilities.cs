using System.Drawing;

namespace Helpers;

public class Utilities
{
    public static T GetRandomEnumValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(new Random().Next(0, values.Length));
    }
}