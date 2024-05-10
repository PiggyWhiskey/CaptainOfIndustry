// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.LineMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity
{
  [RequireComponent(typeof (LineRenderer))]
  public class LineMb : MonoBehaviour
  {
    private Option<MbPool<LineMb>> m_owningPool;
    private LineRenderer m_lineRenderer;

    private void Awake() => this.m_lineRenderer = this.GetComponent<LineRenderer>();

    internal void Initialize(Material sharedMaterial)
    {
      this.m_lineRenderer.sharedMaterial = sharedMaterial;
    }

    internal void Initialize(MbPool<LineMb> pool, Material sharedMaterial)
    {
      this.m_owningPool = (Option<MbPool<LineMb>>) pool.CheckNotNull<MbPool<LineMb>>();
      this.m_lineRenderer.sharedMaterial = sharedMaterial;
    }

    public void SetPoints(Vector3 p1, Vector3 p2)
    {
      Vector3[] andInit = ArrayPool<Vector3>.GetAndInit(p1, p2);
      this.m_lineRenderer.positionCount = 2;
      this.m_lineRenderer.SetPositions(andInit);
      andInit.ReturnToPool<Vector3>();
    }

    public void SetPoints(Vector3[] positions)
    {
      this.m_lineRenderer.positionCount = positions.Length;
      this.m_lineRenderer.SetPositions(positions);
    }

    public void SetPoint(int index, Vector3 position)
    {
      this.m_lineRenderer.SetPosition(index, position);
    }

    public void SetStartPoint(Vector3 position) => this.m_lineRenderer.SetPosition(0, position);

    public void SetEndPoint(Vector3 position)
    {
      this.m_lineRenderer.SetPosition(this.m_lineRenderer.positionCount - 1, position);
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
    }

    public void SetTextureMode(LineTextureMode mode)
    {
      Assert.That<Option<MbPool<LineMb>>>(this.m_owningPool).IsNone<MbPool<LineMb>>();
      this.m_lineRenderer.textureMode = mode;
    }

    public void ReturnToPool()
    {
      if (this.m_owningPool.HasValue)
      {
        this.m_owningPool.ValueOrNull.ReturnInstanceKeepReference(this);
      }
      else
      {
        Log.Warning("Attempting to return non-pooled line to pool");
        this.gameObject.Destroy();
      }
    }

    public void Show() => this.gameObject.SetActive(true);

    public void Hide() => this.gameObject.SetActive(false);

    public LineMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
