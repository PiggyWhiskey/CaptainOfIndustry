// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.UiLinesRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using UnityEngine;
using UnityEngine.UI;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  [RequireComponent(typeof (CanvasRenderer))]
  public class UiLinesRenderer : MaskableGraphic
  {
    private readonly Set<UiLine> m_lines;

    public UiLine AddLine(Vector2 from, Vector2 to, float thickness, Color color)
    {
      UiLine uiLine = new UiLine(this, from, to, thickness, color);
      this.m_lines.Add(uiLine);
      this.SetVerticesDirty();
      return uiLine;
    }

    public void RemoveLine(UiLine line)
    {
      if (!this.m_lines.Remove(line))
        return;
      this.SetVerticesDirty();
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
      vh.Clear();
      foreach (UiLine line in this.m_lines)
      {
        int currentVertCount = vh.currentVertCount;
        vh.AddVert((Vector3) (line.From + line.RightVector), (Color32) line.Color, (Vector4) new Vector2(1f, 0.0f));
        vh.AddVert((Vector3) (line.From - line.RightVector), (Color32) line.Color, (Vector4) new Vector2(0.0f, 0.0f));
        vh.AddVert((Vector3) (line.To - line.RightVector), (Color32) line.Color, (Vector4) new Vector2(0.0f, 1f));
        vh.AddVert((Vector3) (line.To + line.RightVector), (Color32) line.Color, (Vector4) new Vector2(1f, 1f));
        vh.AddTriangle(currentVertCount, currentVertCount + 1, currentVertCount + 2);
        vh.AddTriangle(currentVertCount + 2, currentVertCount + 3, currentVertCount);
      }
    }

    public UiLinesRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lines = new Set<UiLine>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
