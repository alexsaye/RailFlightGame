using UnityEngine;

/// <summary>
/// A precalculated segment of a Catmull-Rom spline.
/// </summary>
public struct CatmullRomPathSegment : IPath
{
  private Vector3 a;
  private Vector3 b;
  private Vector3 c;
  private Vector3 d;

  public float Length { get; private set; }

  public bool Closed => Length == 0f;

  public CatmullRomPathSegment(Vector3 startControl, Vector3 startPoint, Vector3 endPoint, Vector3 endControl, float tension, float alpha)
  {
    var startKnot = Mathf.Pow(Vector3.Distance(startControl, startPoint), alpha);
    var middleKnot = Mathf.Pow(Vector3.Distance(startPoint, endPoint), alpha);
    var endKnot = Mathf.Pow(Vector3.Distance(endPoint, endControl), alpha);

    var startTangent = (1f - tension) * (endPoint - startPoint + middleKnot * ((startPoint - startControl) / startKnot - (endPoint - startControl) / (startKnot + middleKnot)));
    var endTangent = (1f - tension) * (endPoint - startPoint + middleKnot * ((endControl - endPoint) / endKnot - (endControl - startPoint) / (middleKnot + endKnot)));

    a = 2f * (startPoint - endPoint) + startTangent + endTangent;
    b = -3f * (startPoint - endPoint) - 2f * startTangent - endTangent;
    c = startTangent;
    d = startPoint;

    Length = Vector3.Distance(startPoint, endPoint);
  }

  public Vector3 Sample(float progress)
  {
    return a * Mathf.Pow(progress, 3) + b * Mathf.Pow(progress, 2) + c * progress + d;
  }
}
