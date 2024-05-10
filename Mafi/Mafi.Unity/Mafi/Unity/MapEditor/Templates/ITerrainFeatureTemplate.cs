// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.ITerrainFeatureTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using System;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  public interface ITerrainFeatureTemplate
  {
    string Name { get; }

    Proto.ID CategoryId { get; }

    Option<string> IconAssetPath { get; }

    float Order { get; }

    Type FeatureType { get; }

    /// <summary>Global features affect the entire terrain</summary>
    bool IsGlobal { get; }

    ITerrainFeatureBase CreateNewFeatureAt(Tile3f position, IRandom rng);
  }
}
