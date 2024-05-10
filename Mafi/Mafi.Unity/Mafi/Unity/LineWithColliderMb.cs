// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.LineWithColliderMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  [RequireComponent(typeof (CapsuleCollider))]
  [RequireComponent(typeof (LineRenderer))]
  public class LineWithColliderMb : MonoBehaviour
  {
    private LineRenderer m_lineRenderer;
    private CapsuleCollider m_collider;
    private float m_colliderWidthBiasMult;

    private void Awake()
    {
      this.m_lineRenderer = this.GetComponent<LineRenderer>();
      this.m_collider = this.GetComponent<CapsuleCollider>();
      this.m_collider.direction = 2;
    }

    internal void Initialize(Material sharedMaterial, float colliderWidthBiasMult)
    {
      this.m_lineRenderer.sharedMaterial = sharedMaterial;
      this.m_colliderWidthBiasMult = colliderWidthBiasMult;
    }

    public void SetPoints(Vector3 p1, Vector3 p2)
    {
      Vector3[] andInit = ArrayPool<Vector3>.GetAndInit(p1, p2);
      this.m_lineRenderer.positionCount = 2;
      this.m_lineRenderer.SetPositions(andInit);
      andInit.ReturnToPool<Vector3>();
      this.updateCollider();
    }

    public void SetStartPoint(Vector3 position)
    {
      this.m_lineRenderer.SetPosition(0, position);
      this.updateCollider();
    }

    public void SetEndPoint(Vector3 position)
    {
      this.m_lineRenderer.SetPosition(this.m_lineRenderer.positionCount - 1, position);
      this.updateCollider();
    }

    public void SetColor(Color color)
    {
      this.m_lineRenderer.startColor = color;
      this.m_lineRenderer.endColor = color;
    }

    public void SetWidth(float width)
    {
      this.m_lineRenderer.startWidth = width;
      this.m_lineRenderer.endWidth = width;
      this.updateCollider();
    }

    public void Show() => this.gameObject.SetActive(true);

    public void Hide() => this.gameObject.SetActive(false);

    private void updateCollider()
    {
      if (this.m_lineRenderer.positionCount != 2)
      {
        Log.Warning("AddOrUpdateLineCollider only works for simple lines.");
      }
      else
      {
        Vector3 position1 = this.m_lineRenderer.GetPosition(0);
        Vector3 position2 = this.m_lineRenderer.GetPosition(1);
        this.gameObject.transform.localPosition = (position1 + position2) / 2f;
        this.m_collider.radius = this.m_lineRenderer.startWidth * 0.5f * this.m_colliderWidthBiasMult;
        Vector3 forward = position2 - position1;
        this.m_collider.height = (forward.magnitude - 6f * this.m_collider.radius).Max(0.0f);
        this.gameObject.transform.localRotation = Quaternion.LookRotation(forward, Vector3.up);
      }
    }

    public LineWithColliderMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
