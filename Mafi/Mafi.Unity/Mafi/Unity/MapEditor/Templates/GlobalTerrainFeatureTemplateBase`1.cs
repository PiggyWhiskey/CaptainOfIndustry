// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.GlobalTerrainFeatureTemplateBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  public abstract class GlobalTerrainFeatureTemplateBase<T> : ITerrainFeatureTemplate where T : ITerrainFeatureBase
  {
    public abstract string Name { get; }

    public abstract Proto.ID CategoryId { get; }

    public virtual Option<string> IconAssetPath => Option<string>.None;

    public virtual float Order => 0.0f;

    public Type FeatureType => typeof (T);

    public bool IsGlobal => true;

    ITerrainFeatureBase ITerrainFeatureTemplate.CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      return (ITerrainFeatureBase) this.CreateNewFeature(rng);
    }

    public abstract T CreateNewFeature(IRandom rng);

    protected GlobalTerrainFeatureTemplateBase()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
