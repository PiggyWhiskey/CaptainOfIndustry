// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.Components.Charts.GraphChartPoints
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework.Components.Charts
{
  public class GraphChartPoints
  {
    private readonly DataSeries m_data;
    private readonly float m_size;
    private readonly ColorRgba m_color;
    private readonly RectTransform m_transform;
    private readonly BuildableCanvasMb m_buildableCanvasMb;
    private bool m_meshGenerated;

    public GraphChartPoints(
      RectTransform parent,
      DataSeries data,
      float size,
      Material material,
      ColorRgba color)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_data = data;
      this.m_size = size;
      this.m_color = color;
      GameObject objectToPosition = new GameObject("Data points");
      this.m_transform = objectToPosition.AddComponent<RectTransform>();
      this.m_transform.SetParent((Transform) parent, false);
      LayoutHelper.Fill(parent.gameObject, objectToPosition);
      CanvasRenderer canvasRenderer = objectToPosition.AddComponent<CanvasRenderer>();
      canvasRenderer.materialCount = 1;
      canvasRenderer.SetMaterial(material, 0);
      this.m_buildableCanvasMb = objectToPosition.AddComponent<BuildableCanvasMb>();
    }

    public void Hide() => this.m_transform.gameObject.SetActive(false);

    public void Show()
    {
      if (!this.m_meshGenerated)
        return;
      this.m_transform.gameObject.SetActive(true);
    }

    public void Generate(AxisMeasure horizontalMeasure, AxisMeasure verticalMeasure)
    {
      if (this.m_data.Values.IsEmpty<long>())
      {
        this.Hide();
        this.m_meshGenerated = false;
      }
      else
      {
        this.m_meshGenerated = true;
        MeshBuilder instance = MeshBuilder.Instance;
        float num = this.m_size / 2f;
        Color color = this.m_color.ToColor();
        for (int index = 0; index < this.m_data.Values.Count; ++index)
        {
          float position1 = horizontalMeasure.ValueToPosition((long) index);
          float position2 = verticalMeasure.ValueToPosition(this.m_data.Values[index]);
          instance.AddQuad(new Vector3(position1 - num, position2 - num, 0.0f), new Vector3(position1 - num, position2 + num, 0.0f), new Vector3(position1 + num, position2 + num, 0.0f), new Vector3(position1 + num, position2 - num, 0.0f), (Color32) color);
        }
        instance.UpdateMbAndClear((IBuildable) this.m_buildableCanvasMb);
      }
    }
  }
}
