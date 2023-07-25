using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A precalculated Catmull-Rom spline.
/// </summary>
[Serializable]
public class Spline : ScriptableObject, ISpline
{
  private readonly List<SplineSegment> segments = new List<SplineSegment>();

  public void Fit(IEnumerable<Vector3> path, SplineTuning tuning, bool closed = false)
  {
    this.segments.Clear();

    var points = path.Distinct().ToList();
    if (points.Count < 2)
    {
      // The spline is just a single point.
      var point = points.FirstOrDefault();
      this.segments.Add(new SplineSegment(point, point, point, point, tuning));
      return;
    }

    if (closed)
    {
      // Prepend the last point as the control for the first point.
      points.Insert(0, points[points.Count - 1]);

      // Append the first point as the new last point.
      points.Add(points[1]);

      // Append the second point as the control for the new last point.
      points.Add(points[2]);
    }
    else
    {
      // Prepend a control behind the first point.
      points.Insert(0, (points[0] - points[1]));

      // Append a control ahead of the last point.
      points.Add(points[points.Count - 1] - points[points.Count - 2]);
    }

    for (var i = 1; i < points.Count - 2; i++)
    {
      this.segments.Add(new SplineSegment(points[i - 1], points[i], points[i + 1], points[i + 2], tuning));
    }
  }

  public Vector3 Sample(float progress)
  {
    // Find the progress relative to the segment.
    // TODO: This doesn't account for different segment lengths, which would be more useful.
    var segmentIndex = Mathf.Min(Mathf.FloorToInt(progress * this.segments.Count), this.segments.Count - 1);
    var segmentProgress = progress * this.segments.Count - segmentIndex;
    return this.segments[segmentIndex].Sample(segmentProgress);
  }
}

/// <summary>
/// A precalculated segment of a Catmull-Rom spline.
/// </summary>
public struct SplineSegment : ISpline
{
  private Vector3 a;
  private Vector3 b;
  private Vector3 c;
  private Vector3 d;

  public SplineSegment(Vector3 startControl, Vector3 startPoint, Vector3 endPoint, Vector3 endControl, SplineTuning tuning)
  {
    var startKnot = Mathf.Pow(Vector3.Distance(startControl, startPoint), tuning.Alpha);
    var middleKnot = Mathf.Pow(Vector3.Distance(startPoint, endPoint), tuning.Alpha);
    var endKnot = Mathf.Pow(Vector3.Distance(endPoint, endControl), tuning.Alpha);

    var startTangent = (1f - tuning.Tension) * (endPoint - startPoint + middleKnot * ((startPoint - startControl) / startKnot - (endPoint - startControl) / (startKnot + middleKnot)));
    var endTangent = (1f - tuning.Tension) * (endPoint - startPoint + middleKnot * ((endControl - endPoint) / endKnot - (endControl - startPoint) / (middleKnot + endKnot)));

    this.a = 2f * (startPoint - endPoint) + startTangent + endTangent;
    this.b = -3f * (startPoint - endPoint) - 2f * startTangent - endTangent;
    this.c = startTangent;
    this.d = startPoint;
  }

  public Vector3 Sample(float progress)
  {
    return this.a * Mathf.Pow(progress, 3) + this.b * Mathf.Pow(progress, 2) + this.c * progress + this.d;
  }
}

[Serializable]
public struct SplineTuning
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

public interface ISpline
{
  Vector3 Sample(float progress);
}