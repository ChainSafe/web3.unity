using System;

namespace Setup.Utils;

/// <summary>
/// Fir ignoring serialization of a property.
/// Deserialization still works.
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class JsonIgnoreSerializationAttribute : Attribute
{
    
}