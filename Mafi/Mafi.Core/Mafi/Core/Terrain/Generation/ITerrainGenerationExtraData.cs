// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.ITerrainGenerationExtraData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  /// <summary>
  /// Additional terrain generation data such as trees or props.
  /// 
  /// Note: Implement this interface explicitly, so that callers of the derived classes do not see and accidentally call
  /// these methods.
  /// </summary>
  public interface ITerrainGenerationExtraData
  {
    void Initialize(Chunk64Area area);

    void ApplyInArea(Chunk64Area area, bool isInMapEditor);
  }
}
