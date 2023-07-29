using UnityEngine;

/// <summary>
/// Add force towards a target position.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ForceTowardsPosition : MonoBehaviour
{
  private Rigidbody rb;

  public Vector3 Target;

  [Header("Force")]

  public CorrectorTuning XTuning;
  private Corrector xCorrector = new Corrector();

  public CorrectorTuning YTuning;
  private Corrector yCorrector = new Corrector();

  public CorrectorTuning ZTuning;
  private Corrector zCorrector = new Corrector();

  [Header("Relative Force")]

  public CorrectorTuning RelativeXTuning;
  private Corrector relativeXCorrector = new Corrector();

  public CorrectorTuning RelativeYTuning;
  private Corrector relativeYCorrector = new Corrector();

  public CorrectorTuning RelativeZTuning;
  private Corrector relativeZCorrector = new Corrector();

  private void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  private void FixedUpdate()
  {
    var x = xCorrector.Next(Target.x, XTuning);
    var y = yCorrector.Next(Target.y, YTuning);
    var z = zCorrector.Next(Target.z, ZTuning);
    rb.AddForce(x, y, z);

    var relativeTarget = transform.InverseTransformPoint(Target);
    var relativeX = relativeXCorrector.Next(relativeTarget.x, RelativeXTuning);
    var relativeY = relativeYCorrector.Next(relativeTarget.y, RelativeYTuning);
    var relativeZ = relativeZCorrector.Next(relativeTarget.z, RelativeZTuning);
    rb.AddRelativeForce(relativeX, relativeY, relativeZ);
  }
}