using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A Catmull-Rom spline made from precalculated segments.
/// </summary>
[ExecuteAlways]
[Serializable]
public class CatmullRomSpline : Spline
{
  /// <summary>
  /// The tuning parameters for the spline.
  /// </summary>
  [Tooltip("The tuning parameters for the spline.")]
  public CatmullRomSplineSegmentTuning Tuning = new CatmullRomSplineSegmentTuning() { Tension = 0, Alpha = 0.5f };

  /// <summary>
  /// Whether the spline is a closed loop.
  /// </summary>
  [Tooltip("Whether the spline is a closed loop.")]
  public bool Closed = false;

  /// <summary>
  /// Whether to approximate uniformity when sampling the spline to counter disproportionate segment lengths.
  /// </summary>
  [Tooltip("Whether to approximate uniformity when sampling the spline to counter disproportionate segment lengths.")]
  public bool ApproximateUniformity = true;

  /// <summary>
  /// Whether to guide the spline through the attached game object's children.
  /// </summary>
  [Tooltip("Whether to guide the spline through the attached game object's children.")]
  public bool UseChildrenAsPoints = true;

  /// <summary>
  /// The points to guide the spline through.
  /// </summary>
  [Tooltip("The points to guide the spline through.")]
  public List<Vector3> Points;

  private List<CatmullRomSplineSegment> segments;
  private float length;

  public override Vector3 Sample(float progress)
  {
    if (this.segments.Count > 0)
    {
      if (ApproximateUniformity)
      {
        return this.SampleUniformly(progress);
      }
      return this.SampleNonUniformly(progress);
    }
    return Vector3.zero;
  }

  private Vector3 SampleUniformly(float progress)
  {
    // Scale the progress to the total length, then iteratively reduce it by each segment to determine the segment to sample and the distance along it.
    var segmentDistance = Mathf.Clamp01(progress) * this.length;
    var segmentIndex = 0;
    while (segmentIndex < this.segments.Count - 1 && segmentDistance > this.segments[segmentIndex].Length)
    {
      segmentDistance -= this.segments[segmentIndex].Length;
      segmentIndex++;
    }

    // Divide the distance along the segment by the segment's length to get the progress within the segment.
    var segmentProgress = segmentDistance / this.segments[segmentIndex].Length;

    return this.segments[segmentIndex].Sample(segmentProgress);
  }

  private Vector3 SampleNonUniformly(float progress)
  {
    // Scale the progress to the segment count to determine the segment to sample.
    var scaledProgress = Mathf.Clamp01(progress) * this.segments.Count;
    var segmentIndex = Mathf.Min(Mathf.FloorToInt(scaledProgress), this.segments.Count - 1);

    // Subtract the segment index to get the progress within the segment.
    var segmentProgress = scaledProgress - segmentIndex;

    return this.segments[segmentIndex].Sample(segmentProgress);
  }

  public void Recalculate()
  {
    if (this.UseChildrenAsPoints)
    {
      this.Points ??= new List<Vector3>();
      this.Points.Clear();
      this.Points.AddRange(this.transform.Cast<Transform>().Select(child => child.position));
    }

    this.segments ??= new List<CatmullRomSplineSegment>();
    this.segments.Clear();

    var path = this.Points.Distinct().ToList();
    if (path.Count < 2)
    {
      // The spline is just a single point.
      var point = Enumerable.FirstOrDefault<Vector3>(path);
      this.segments.Add(new CatmullRomSplineSegment(point, point, point, point, this.Tuning));
      return;
    }

    if (this.Closed)
    {
      // Prepend the last point as the control for the first point.
      path.Insert(0, (Vector3)path[(int)(path.Count - 1)]);

      // Append the first point as the new last point.
      path.Add((Vector3)path[1]);

      // Append the second point as the control for the new last point.
      path.Add((Vector3)path[2]);
    }
    else
    {
      // Prepend a control behind the first point.
      path.Insert(0, (Vector3)(path[0] - path[1]));

      // Append a control ahead of the last point.
      path.Add((Vector3)(path[(int)(path.Count - 1)] - path[(int)(path.Count - 2)]));
    }

    for (var i = 1; i < path.Count - 2; i++)
    {
      this.segments.Add(new CatmullRomSplineSegment(path[i - 1], path[i], path[i + 1], path[i + 2], this.Tuning));
    }

    this.length = this.segments.Sum(segment => segment.Length);
  }

  private void Start()
  {
    this.Recalculate();
  }

  private void Update()
  {
    // Only recalculate on update in the editor.
    if (Application.isEditor && !Application.isPlaying)
    {
      this.Recalculate();
    }
  }

  private void OnValidate()
  {
    this.Recalculate();
  }
}