using System;
using System.Collections.Generic;
using System.Text;
using Nuke.Common;

partial class XamarinBuild
{
    /// <summary>
    /// Logs the specified information.
    /// </summary>
    /// <param name="info">The information.</param>
    static void Log(string info) => Logger.Normal(info);

    /// <summary>
    /// Logs the specified information.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="args">The arguments.</param>
    static void Log(string info, params object[] args) => Logger.Normal(info, args);

    /// <summary>
    /// Logs the specified debug message.
    /// </summary>
    /// <param name="info">The information.</param>
    static void Trace(string info) => Logger.Trace(info);

    /// <summary>
    /// Logs the specified debug message.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="args">The arguments.</param>
    static void Trace(string info, params object[] args) => Logger.Trace(info, args);

    /// <summary>
    /// Logs the specified information message.
    /// </summary>
    /// <param name="info">The information.</param>
    static void Information(string info) => Logger.Info(info);

    /// <summary>
    /// Logs the specified information message.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="args">The arguments.</param>
    static void Information(string info, params object[] args) => Logger.Info(info, args);

    /// <summary>
    /// Logs the specified warning message.
    /// </summary>
    /// <param name="info">The information.</param>
    static void Warning(string info) => Logger.Warn(info);

    /// <summary>
    /// Logs the specified warning message.
    /// </summary>
    /// <param name="info">The information.</param>
    /// <param name="args">The arguments.</param>
    static void Warning(string info, params object[] args) => Logger.Warn(info, args);

}