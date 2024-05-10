// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.LineOverlayRendererHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  public readonly struct LineOverlayRendererHelper
  {
    private readonly LineMb m_lineMb;

    public bool IsShown => this.m_lineMb.gameObject.activeSelf;

    public LineOverlayRendererHelper(LinesFactory linesFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lineMb = linesFactory.CreateLinePooled(Vector3.zero, Vector3.one, 1f, Color.white);
      this.m_lineMb.Hide();
    }

    public void SetWidth(float width) => this.m_lineMb.SetWidth(width);

    public void SetColor(Color color) => this.m_lineMb.SetColor(color);

    public void ShowLine(Tile3f from, Tile3f to)
    {
      this.m_lineMb.SetStartPoint(from.ToVector3());
      this.m_lineMb.SetEndPoint(to.ToVector3());
      this.m_lineMb.Show();
    }

    public void UpdateLineStart(Vector3 position) => this.m_lineMb.SetStartPoint(position);

    public void UpdateLineStart(Tile3f position)
    {
      this.m_lineMb.SetStartPoint(position.ToVector3());
    }

    public void HideLine() => this.m_lineMb.Hide();
  }
}
