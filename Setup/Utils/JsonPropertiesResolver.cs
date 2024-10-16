using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace Setup.Utils;

/// <summary>
/// Resolver for custom serialization.
/// </summary>
public class JsonPropertiesResolver : DefaultContractResolver
{
    protected override List<MemberInfo> GetSerializableMembers(Type objectType)
    {
        //Return properties that do NOT have the JsonIgnoreSerializationAttribute
        return objectType.GetProperties()
            .Where(pi => !Attribute.IsDefined(pi, typeof(JsonIgnoreSerializationAttribute)))
            .ToList<MemberInfo>();
    }
}