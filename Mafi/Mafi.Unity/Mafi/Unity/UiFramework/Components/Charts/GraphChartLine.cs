// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.GraphChartLine
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public class GraphChartLine
  {
    private readonly DataSeries m_data;
    private readonly float m_width;
    private readonly ColorRgba m_color;
    private readonly RectTransform m_transform;
    private readonly BuildableCanvasMb m_buildableCanvasMb;
    private bool m_lineGenerated;

    public GraphChartLine(
      RectTransform parent,
      DataSeries data,
      float width,
      Material material,
      ColorRgba color)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_data = data;
      this.m_width = width;
      this.m_color = color;
      GameObject objectToPosition = new GameObject("Data line");
      this.m_transform = objectToPosition.AddComponent<RectTransform>();
      this.m_transform.SetParent((Transform) parent, false);
      LayoutHelper.Fill(parent.gameObject, objectToPosition);
      CanvasRenderer canvasRenderer = objectToPosition.AddComponent<CanvasRenderer>();
      canvasRenderer.materialCount = 1;
      canvasRenderer.SetMaterial(material, 0);
      this.m_buildableCanvasMb = objectToPosition.AddComponent<BuildableCanvasMb>();
      this.m_transform.gameObject.SetActive(false);
    }

    public void Hide() => this.m_transform.gameObject.SetActive(false);

    public void Show()
    {
      if (!this.m_lineGenerated)
        return;
      this.m_transform.gameObject.SetActive(true);
    }

    public void Generate(AxisMeasure horizontalMeasure, AxisMeasure verticalMeasure)
    {
      if (this.m_data.Values.Count <= 1)
      {
        this.Hide();
        this.m_lineGenerated = false;
      }
      else
      {
        this.m_lineGenerated = true;
        this.Show();
        MeshBuilder instance = MeshBuilder.Instance;
        instance.StartLine2D(this.m_width, this.m_color.ToColor());
        for (int index = 0; index < this.m_data.Values.Count; ++index)
        {
          Vector2 point = new Vector2(horizontalMeasure.ValueToPosition((long) index), verticalMeasure.ValueToPosition(this.m_data.Values[index]));
          instance.ContinueLine2D(point);
        }
        instance.UpdateMbAndClear((IBuildable) this.m_buildableCanvasMb, omitNormals: true);
      }
    }
  }
}
