// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Terrain.ITreeRenderer
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Terrain.Trees;

#nullable disable
namespace Mafi.Unity.Terrain
{
  public interface ITreeRenderer
  {
    void RemoveTreeHighlight(TreeMb treeMbValue, ColorRgba highlightColor);

    Option<TreeMb> GetMbOrNoneFor(TreeId tree);

    Option<TreeMb> GetPlannedMbOrNoneFor(TreeId tree);

    void HighlightTree(TreeMb treeMbValue, ColorRgba highlightColor);
  }
}
