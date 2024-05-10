// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Prototypes.TerrainEditorMenuCategoryProto
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Prototypes
{
  public class TerrainEditorMenuCategoryProto : Proto
  {
    /// <summary>
    /// Icon asset. When not set, this category won't be shown in the toolbar but features will still appear in
    /// dropdowns on their respective tabs.
    /// </summary>
    public readonly Option<string> IconAssetPath;
    public readonly string Tooltip;
    public readonly float Order;

    public TerrainEditorMenuCategoryProto(
      Proto.ID id,
      Option<string> iconAssetPath,
      string tooltip,
      float order)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(id, Proto.Str.Empty);
      this.IconAssetPath = iconAssetPath;
      this.Tooltip = tooltip;
      this.Order = order;
    }
  }
}
