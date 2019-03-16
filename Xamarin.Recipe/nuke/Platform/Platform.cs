using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;

[PublicAPI]
[TypeConverter(typeof(TypeConverter<Nuke.Common.Configuration>))]
public class Platform : Enumeration
{
    /// <summary>
    /// The ios platform.
    /// </summary>
    public static Platform iOS = new Platform { Value = nameof(iOS) };

    /// <summary>
    /// The android platform.
    /// </summary>
    public static Platform Android = new Platform { Value = nameof(Android) };

    /// <summary>
    /// The watch os platform.
    /// </summary>
    public static Platform WatchOS = new Platform { Value = nameof(WatchOS) };

    /// <summary>
    /// Performs an implicit conversion from <see cref="Platform"/> to <see cref="System.String"/>.
    /// </summary>
    /// <param name="platform">The platform.</param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public static implicit operator string(Platform platform)
    {
        return platform.Value;
    }
}
