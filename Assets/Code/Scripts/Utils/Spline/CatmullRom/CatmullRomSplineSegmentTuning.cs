using System;
using UnityEngine;

/// <summary>
/// Tuning for a Catmull-Rom spline segment.
/// </summary>
[Serializable]
public struct CatmullRomSplineSegmentTuning
{
  /// <summary>
  /// The tightness of the curve.
  /// </summary>
  [Tooltip("The tightness of the curve.")]
  [Range(0f, 1f)]
  public float Tension;

  /// <summary>
  /// The style of the curve, where 0 is uniform, 0.5 is centripetal, and 1 is chordal.
  /// </summary>
  [Range(0f, 1f)]
  [Tooltip("The style of the curve, where 0 is uniform, 0.5 is centripetal, and 1 is chordal.")]
  public float Alpha;
}