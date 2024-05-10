// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.World.UiLine
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.World
{
  public class UiLine
  {
    private readonly UiLinesRenderer m_parentRenderer;
    public readonly float Thickness;

    public Vector2 From { get; private set; }

    public Vector2 To { get; private set; }

    public Vector2 RightVector { get; private set; }

    public Color Color { get; private set; }

    public UiLine(
      UiLinesRenderer parentRenderer,
      Vector2 from,
      Vector2 to,
      float thickness,
      Color color)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_parentRenderer = parentRenderer;
      this.Thickness = thickness;
      this.Color = color;
      this.SetPositions(from, to);
    }

    public void ChangeColor(Color newColor)
    {
      this.Color = newColor;
      this.m_parentRenderer.SetVerticesDirty();
    }

    public void Remove() => this.m_parentRenderer.RemoveLine(this);

    public void SetPositions(Vector2 from, Vector2 to)
    {
      this.From = from;
      this.To = to;
      Vector2 normalized = (this.To - this.From).normalized;
      this.RightVector = new Vector2(normalized.y, -normalized.x) * this.Thickness / 2f;
      this.m_parentRenderer.SetVerticesDirty();
    }
  }
}
