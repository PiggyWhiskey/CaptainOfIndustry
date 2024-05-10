// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Generation.IEditableTerrainFeature
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;

#nullable disable
namespace Mafi.Core.Terrain.Generation
{
  public interface IEditableTerrainFeature : ITerrainFeatureBase
  {
    bool Is2D { get; }

    bool CanRotate { get; }

    ITerrainFeatureConfig Config { get; }

    /// <summary>
    /// Validates the config and returns whether it is valid. Errors to the uer may be added to the list of errors.
    /// </summary>
    bool ValidateConfig(IResolver resolver, Lyst<string> errors);

    /// <summary>
    /// Returns handle data, null if the feature is global and has no handle.
    /// </summary>
    HandleData? GetHandleData();
  }
}
