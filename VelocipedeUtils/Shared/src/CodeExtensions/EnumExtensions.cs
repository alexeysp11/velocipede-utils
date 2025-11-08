using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace VelocipedeUtils.Shared.CodeExtensions;

/// <summary>
/// Enum extensions.
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Get Display name of an enum.
    /// </summary>
    public static string GetDisplayName(this Enum enumValue)
    {
        try
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }
        catch (Exception)
        {
            return enumValue.ToString();
        }
    }
}
