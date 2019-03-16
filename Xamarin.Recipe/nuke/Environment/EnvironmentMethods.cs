using static Nuke.Common.EnvironmentInfo;

partial class XamarinBuild
{

    /// <summary>
    /// Environments the variable.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    static string EnvironmentVariable(string key) => Variable(key);

    /// <summary>
    /// Determines whether [has environment variable] [the specified key].
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns>
    ///   <c>true</c> if [has environment variable] [the specified key]; otherwise, <c>false</c>.
    /// </returns>
    static bool HasEnvironmentVariable(string key) => Variable(key) != null;
}