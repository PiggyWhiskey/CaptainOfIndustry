// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.StartingLocationTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Generation;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  [MapEditorTemplateFactory]
  internal class StartingLocationTemplate : TerrainFeatureTemplateFactoryBase<StartingLocationV2>
  {
    public override string Name => "Starting location";

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.Special;

    public override Option<string> IconAssetPath
    {
      get => (Option<string>) "Assets/Unity/UserInterface/Toolbar/WorldMap.svg";
    }

    public override StartingLocationV2 CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      return new StartingLocationV2(new StartingLocationV2.Configuration()
      {
        Position = position.Tile3iRounded,
        Difficulty = StartingLocationDifficulty.Medium
      });
    }

    public StartingLocationTemplate()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
