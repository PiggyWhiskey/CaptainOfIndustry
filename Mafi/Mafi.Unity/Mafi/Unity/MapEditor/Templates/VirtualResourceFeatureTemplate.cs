// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MapEditor.Templates.VirtualResourceFeatureTemplate
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base.Terrain.FeatureGenerators;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MapEditor.Templates
{
  public class VirtualResourceFeatureTemplate : 
    TerrainFeatureTemplateBase<VirtualResourceFeatureGenerator>
  {
    private readonly Proto.ID m_resourceProto;
    private readonly Quantity m_defaultQuantity;
    private readonly RelTile1i m_defaultRadius;
    private readonly ProtosDb m_protosDb;

    public override string Name { get; }

    public override Proto.ID CategoryId => IdsUnity.TerrainFeatureTemplates.VirtualResources;

    public override Option<string> IconAssetPath { get; }

    public VirtualResourceFeatureTemplate(
      string name,
      Proto.ID resourceProto,
      Quantity defaultQuantity,
      RelTile1i defaultRadius,
      Option<string> iconAssetPath,
      ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = name;
      this.IconAssetPath = iconAssetPath;
      this.m_resourceProto = resourceProto;
      this.m_defaultQuantity = defaultQuantity;
      this.m_defaultRadius = defaultRadius;
      this.m_protosDb = protosDb;
    }

    public override VirtualResourceFeatureGenerator CreateNewFeatureAt(Tile3f position, IRandom rng)
    {
      return new VirtualResourceFeatureGenerator(new VirtualResourceFeatureGenerator.Configuration()
      {
        ConfiguredCapacity = this.m_defaultQuantity,
        Position = position.Xy,
        MaxRadius = this.m_defaultRadius,
        VirtualResource = this.m_protosDb.GetOrThrow<VirtualResourceProductProto>(this.m_resourceProto)
      });
    }
  }
}
