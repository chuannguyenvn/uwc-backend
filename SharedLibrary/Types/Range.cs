namespace Commons.Types;

public struct Range<T> where T : IComparable<T>
{
    public T Min { get; private set; }
    public T Max { get; private set; }
    public bool MinInclusive { get; private set; }
    public bool MaxInclusive { get; private set; }
    
    public Range(T min, T max)
    {
        Min = min;
        Max = max;
        MinInclusive = true;
        MaxInclusive = true;
    }

    public Range(T min, T max, bool minInclusive, bool maxInclusive)
    {
        Min = min;
        Max = max;
        MinInclusive = minInclusive;
        MaxInclusive = maxInclusive;
    }

    public bool Contains(T value)
    {
        var min = MinInclusive ? Min.CompareTo(value) <= 0 : Min.CompareTo(value) < 0;
        var max = MaxInclusive ? Max.CompareTo(value) >= 0 : Max.CompareTo(value) > 0;
        return min && max;
    }
}