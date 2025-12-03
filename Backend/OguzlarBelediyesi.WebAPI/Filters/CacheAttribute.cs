using System;

namespace OguzlarBelediyesi.WebAPI.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class CacheAttribute : Attribute
{
    public CacheAttribute(int durationSeconds)
    {
        DurationSeconds = Math.Max(durationSeconds, 1);
    }

    public int DurationSeconds { get; }
}
