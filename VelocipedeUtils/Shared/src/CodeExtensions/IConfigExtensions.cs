namespace VelocipedeUtils.Extensions;

/// <summary>
/// Class for getting settings from config file.
/// </summary>
public interface IConfigExtensions
{
    /// <summary>
    /// Gets settings for server from config file.
    /// </summary>
    T GetConfigSettings<T>(string filename, string sectionName);
}