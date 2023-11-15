using UnityEngine;

/// <summary>
/// A sampleable path.
/// </summary>
public interface IPath
{
    /// <summary>
    /// Samples the path at a given progress, where 0 is the start and 1 is the end.
    /// </summary>
    Vector3 Sample(float progress);

    /// <summary>
    /// The length of the path.
    /// </summary>
    float Length { get; }

    /// <summary>
    /// Whether the path is a closed loop.
    /// </summary>
    bool Closed { get; }
}
