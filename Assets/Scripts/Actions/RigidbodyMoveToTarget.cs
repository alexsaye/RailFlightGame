using UnityEngine;

/// <summary>
/// Moves a <see cref="Rigidbody"/> to a target position using relative force.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class RigidbodyMoveToTarget : MonoBehaviour
{
  private Rigidbody rb;

  public Vector3 Target;

  public CorrectorTuning XTuning;
  private Corrector xCorrector = new Corrector();

  public CorrectorTuning YTuning;
  private Corrector yCorrector = new Corrector();

  public CorrectorTuning ZTuning;
  private Corrector zCorrector = new Corrector();

  private void Start()
  {
    rb = GetComponent<Rigidbody>();
  }

  private void FixedUpdate()
  {
    var relativeTarget = transform.InverseTransformPoint(Target);
    var x = xCorrector.Next(relativeTarget.x, XTuning);
    var y = yCorrector.Next(relativeTarget.y, YTuning);
    var z = zCorrector.Next(relativeTarget.z, ZTuning);
    rb.AddRelativeForce(x, y, z);
  }
}