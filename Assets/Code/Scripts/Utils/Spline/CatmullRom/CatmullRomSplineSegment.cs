using UnityEngine;

/// <summary>
/// A precalculated segment of a Catmull-Rom spline.
/// </summary>
public struct CatmullRomSplineSegment
{
  private Vector3 a;
  private Vector3 b;
  private Vector3 c;
  private Vector3 d;

  public CatmullRomSplineSegment(Vector3 startControl, Vector3 startPoint, Vector3 endPoint, Vector3 endControl, CatmullRomSplineSegmentTuning tuning)
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