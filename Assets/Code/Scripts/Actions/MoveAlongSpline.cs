using UnityEngine;

[ExecuteAlways]
public class MoveAlongSpline : MonoBehaviour
{
  public Spline Spline;

  public float Progress = 0f;

  public float Speed = 1f;

  private void Update()
  {
    if (Application.isPlaying)
    {
      this.Progress += this.Speed * Time.deltaTime;
    }
    this.Progress = Mathf.Repeat(this.Progress, 1f);
    this.transform.position = this.Spline.Sample(this.Progress);
  }
}
