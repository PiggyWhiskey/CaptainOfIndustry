// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Vehicles.MultiLineOverlayRendererHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Vehicles
{
  public class MultiLineOverlayRendererHelper
  {
    private readonly LinesFactory m_linesFactory;
    private readonly Lyst<LineMb> m_lineMbs;

    public MultiLineOverlayRendererHelper(LinesFactory linesFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_lineMbs = new Lyst<LineMb>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_linesFactory = linesFactory;
    }

    public void AddLine(Tile3f from, Tile3f to, float width, Color color)
    {
      this.m_lineMbs.Add(this.m_linesFactory.CreateLinePooled(from.ToVector3(), to.ToVector3(), width, color));
    }

    public void ClearLines()
    {
      this.m_lineMbs.ForEachAndClear((Action<LineMb>) (x => x.ReturnToPool()));
    }
  }
}
