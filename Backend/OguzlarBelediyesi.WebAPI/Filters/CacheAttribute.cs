using System;

namespace OguzlarBelediyesi.WebAPI.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public sealed class CacheAttribute : Attribute
{
    public CacheAttribute(int durationSeconds, params string[] tags)
    {
        DurationSeconds = Math.Max(durationSeconds, 1);
        Tags = tags;
    }

    public int DurationSeconds { get; }
    public string[] Tags { get; }
}
