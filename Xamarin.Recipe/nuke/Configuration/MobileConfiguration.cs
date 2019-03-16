using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;

namespace Configuration
{
    [PublicAPI]
    [TypeConverter(typeof(TypeConverter<Nuke.Common.Configuration>))]
    public class MobileConfiguration : Enumeration
    {
        /// <summary>
        /// The debug mock
        /// </summary>
        public static MobileConfiguration DebugMock =
            new MobileConfiguration { Value = nameof(DebugMock) };

        /// <summary>
        /// The debug dev
        /// </summary>
        public static MobileConfiguration DebugDev =
            new MobileConfiguration { Value = nameof(DebugDev) };

        /// <summary>
        /// The debug test
        /// </summary>
        public static MobileConfiguration DebugTest =
            new MobileConfiguration { Value = nameof(DebugTest) };

        /// <summary>
        /// The mock
        /// </summary>
        public static MobileConfiguration Mock =
            new MobileConfiguration { Value = nameof(Mock) };

        /// <summary>
        /// The dev
        /// </summary>
        public static MobileConfiguration Dev =
            new MobileConfiguration { Value = nameof(Dev) };

        /// <summary>
        /// The test
        /// </summary>
        public static MobileConfiguration Test =
            new MobileConfiguration { Value = nameof(Test) };

        /// <summary>
        /// The store
        /// </summary>
        public static MobileConfiguration Store =
            new MobileConfiguration { Value = nameof(Store) };

        /// <summary>
        /// Performs an implicit conversion from <see cref="MobileConfiguration"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(MobileConfiguration configuration)
        {
            return configuration.Value;
        }
    }
}
