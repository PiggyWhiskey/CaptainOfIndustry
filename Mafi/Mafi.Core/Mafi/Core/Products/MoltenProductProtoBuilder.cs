// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Products.MoltenProductProtoBuilder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Products
{
  public class MoltenProductProtoBuilder : IProtoBuilder
  {
    public ProtosDb ProtosDb => this.Registrator.PrototypesDb;

    public ProtoRegistrator Registrator { get; }

    public MoltenProductProtoBuilder(ProtoRegistrator registrator)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Registrator = registrator;
    }

    /// <summary>
    /// Starts building of a new molten product by resetting the builder.
    /// </summary>
    public MoltenProductProtoBuilder.State Start(
      string name,
      ProductProto.ID id,
      string translationComment = null)
    {
      return new MoltenProductProtoBuilder.State(this, id, name, translationComment);
    }

    public sealed class State : ProductBuilderState<MoltenProductProtoBuilder.State>
    {
      private readonly ProductProto.ID m_protoId;
      private string m_materialPath;

      internal State(
        MoltenProductProtoBuilder builder,
        ProductProto.ID protoId,
        string name,
        string translationComment = null)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        // ISSUE: explicit constructor call
        base.\u002Ector((IProtoBuilder) builder, protoId, name, translationComment ?? "molten product");
        this.m_protoId = protoId;
      }

      [MustUseReturnValue]
      public MoltenProductProtoBuilder.State SetMaterial(string materialPath)
      {
        this.m_materialPath = materialPath.CheckNotNullOrEmpty();
        return this;
      }

      public MoltenProductProto BuildAndAdd()
      {
        return this.AddToDb<MoltenProductProto>(new MoltenProductProto(this.m_protoId, this.Strings, this.m_materialPath == null ? MoltenProductProto.Gfx.Empty : new MoltenProductProto.Gfx(this.PrefabPath ?? "Assets/Base/Transports/MoltenMetal/MoltenMetal.prefab", this.CustomIconPath, this.ValueOrThrow(this.m_materialPath, "MaterialPath"))));
      }
    }
  }
}
