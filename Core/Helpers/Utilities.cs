namespace Helpers;

public class Utilities
{
    public static T GetRandomEnumValue<T>() where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(new Random().Next(0, values.Length));
    }

    public static T GetRandomEnumValueExcept<T>(T except) where T : Enum
    {
        var values = Enum.GetValues(typeof(T));
        var random = new Random();
        T value;
        do
        {
            value = (T)values.GetValue(random.Next(0, values.Length));
        } while (value.Equals(except));

        return value;
    }
}