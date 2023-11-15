using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A Catmull-Rom spline made from precalculated segments.
/// </summary>
[Serializable]
public class CatmullRomPath : IPath
{
  private List<CatmullRomPathSegment> segments;

  public float Length { get; private set; }

  public bool Closed { get; private set; }

  public CatmullRomPath(IEnumerable<Vector3> points, float tension, float alpha, bool closed)
  {
    segments = new List<CatmullRomPathSegment>();

    var path = points.Distinct().ToList();
    if (path.Count < 2)
    {
      // The spline is just a single point.
      var point = Enumerable.FirstOrDefault(path);
      segments.Add(new CatmullRomPathSegment(point, point, point, point, tension, alpha));
      return;
    }

    if (closed)
    {
      // Prepend the last point as the control for the first point.
      path.Insert(0, path[(int)(path.Count - 1)]);

      // Append the first point as the new last point.
      path.Add(path[1]);

      // Append the second point as the control for the new last point.
      path.Add(path[2]);
    }
    else
    {
      // Prepend a control behind the first point.
      path.Insert(0, path[0] - path[1]);

      // Append a control ahead of the last point.
      path.Add(path[path.Count - 1] - path[path.Count - 2]);
    }

    for (var i = 1; i < path.Count - 2; i++)
    {
      segments.Add(new CatmullRomPathSegment(path[i - 1], path[i], path[i + 1], path[i + 2], tension, alpha));
    }

    Length = segments.Sum(segment => segment.Length);
    Closed = closed;
  }

  public Vector3 Sample(float progress)
  {
    // Scale the progress to the total length, then iteratively reduce it by each segment to determine the segment to sample and the distance along it.
    var segmentDistance = Mathf.Clamp01(progress) * Length;
    var segmentIndex = 0;
    while (segmentIndex < segments.Count - 1 && segmentDistance > segments[segmentIndex].Length)
    {
      segmentDistance -= segments[segmentIndex].Length;
      segmentIndex++;
    }

    // Divide the distance along the segment by the segment's length to get the progress within the segment.
    var segmentProgress = segmentDistance / segments[segmentIndex].Length;

    return segments[segmentIndex].Sample(segmentProgress);
  }
}
