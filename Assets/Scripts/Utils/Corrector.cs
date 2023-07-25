using System;
using UnityEngine;

/// <summary>
/// Provides continuous error correction through a PID controller.
/// </summary>
[Serializable]
public class Corrector
{
  private float accumulatedError = 0f;
  private float previousError = 0f;

  /// <summary>
  /// Calculate the next error correction.
  /// </summary>
  public float Next(float error, CorrectorTuning tuning)
  {
    // Correct for the present error.
    var proportional = error * tuning.Proportional;

    // Correct for the accumulated past errors.
    this.accumulatedError += error;
    var integral = this.accumulatedError * tuning.Integral;

    // Correct for the predicted future error.
    var derivative = (error - this.previousError) * tuning.Derivative;
    this.previousError = error;

    return proportional + integral + derivative;
  }
}

/// <summary>
/// PID tuning for a <see cref="Corrector"/>.
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