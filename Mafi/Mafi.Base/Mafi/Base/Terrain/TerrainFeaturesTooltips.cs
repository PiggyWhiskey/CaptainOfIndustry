// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Terrain.TerrainFeaturesTooltips
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

#nullable disable
namespace Mafi.Base.Terrain
{
  public static class TerrainFeaturesTooltips
  {
    public const string SORTING_PRIORITY_ADJUSTMENT = "Adjustment applied to the sorting priority. Positive values cause the feature to be generated later.";
    public const string ORDER_MATTERS_NOTE = "Note that generation order of features matters and can be adjusted using 'Sorting priority adjustment'.";
    public const string WHERE_TO_EDIT_PRIORITY = "To change sorting priority, edit the 'Sorting priority adjustment' in the feature editor.";
    public const string MAX_INFLUENCE_DISTANCE_FOR_POLYGON = "Distance beyond the polygon perimeter that this feature can modify terrain. Increase if the generate feature is getting cut off, decrease for better performance.";
  }
}
