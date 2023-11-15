using System;
using UnityEngine;

/// <summary>
/// Provides continuous error correction through a PID controller.
/// </summary>
[Serializable]
public class PIDCorrector : ICorrector
{
  /// <summary>
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

  private float accumulatedError = 0f;

  private float previousError = 0f;

  public PIDCorrector(float proportional, float integral, float derivative)
  {
    Proportional = proportional;
    Integral = integral;
    Derivative = derivative;
  }

  public float Correct(float error)
  {
    // Correct for the present error.
    var proportional = error * Proportional;

    // Correct for the accumulated past errors.
    accumulatedError += error;
    var integral = accumulatedError * Integral;

    // Correct for the predicted future error.
    var derivative = (error - previousError) * Derivative;
    previousError = error;

    return proportional + integral + derivative;
  }
}
