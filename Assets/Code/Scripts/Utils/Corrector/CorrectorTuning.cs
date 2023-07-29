using System;
using UnityEngine;

/// <summary>
/// Tuning for correction through a PID controller.
/// </summary>
[Serializable]
public struct CorrectorTuning
{  /// <summary>
   /// The influence that the present error has on feedback.
   /// </summary>
  [Tooltip("The influence that the present error has on feedback.")]
  public float Proportional;

  /// <summary>
  /// The influence that the accumulated past errors have on feedback.
  /// </summary>
  [Tooltip("The influence that the accumulated past errors have on feedback.")]
  public float Integral;

  /// <summary>
  /// The influence that the predicted future error has on feedback.
  /// </summary>
  [Tooltip("The influence that the predicted future error has on feedback.")]
  public float Derivative;
}