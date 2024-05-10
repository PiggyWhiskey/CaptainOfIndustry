// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.DummyTreeRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain.Trees;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public class DummyTreeRenderer : ITreeRenderer
  {
    public void RemoveTreeHighlight(TreeMb treeMbValue, ColorRgba highlightColor)
    {
    }

    public Option<TreeMb> GetMbOrNoneFor(TreeId tree) => Option<TreeMb>.None;

    public Option<TreeMb> GetPlannedMbOrNoneFor(TreeId tree) => Option<TreeMb>.None;

    public void HighlightTree(TreeMb treeMbValue, ColorRgba highlightColor)
    {
    }

    public DummyTreeRenderer()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
