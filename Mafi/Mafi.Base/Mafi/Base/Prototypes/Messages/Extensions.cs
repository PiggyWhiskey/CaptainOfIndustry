// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Messages.Extensions
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Localization;

#nullable disable
namespace Mafi.Base.Prototypes.Messages
{
  internal static class Extensions
  {
    internal static StaticEntityProto GetProto(this StaticEntityProto.ID id, ProtosDb db)
    {
      return db.GetOrThrow<StaticEntityProto>((Mafi.Core.Prototypes.Proto.ID) id);
    }

    internal static StaticEntityProto GetProto(this MachineProto.ID id, ProtosDb db)
    {
      return db.GetOrThrow<StaticEntityProto>((Mafi.Core.Prototypes.Proto.ID) id);
    }

    internal static ResearchNodeProto GetProto(this ResearchNodeProto.ID id, ProtosDb db)
    {
      return db.GetOrThrow<ResearchNodeProto>((Mafi.Core.Prototypes.Proto.ID) id);
    }

    internal static ProductProto GetProto(this ProductProto.ID id, ProtosDb db)
    {
      return db.GetOrThrow<ProductProto>((Mafi.Core.Prototypes.Proto.ID) id);
    }

    internal static DynamicGroundEntityProto GetProto(this DynamicEntityProto.ID id, ProtosDb db)
    {
      return db.GetOrThrow<DynamicGroundEntityProto>((Mafi.Core.Prototypes.Proto.ID) id);
    }

    internal static EntityProto GetProto(this EntityProto.ID id, ProtosDb db)
    {
      return db.GetOrThrow<EntityProto>((Mafi.Core.Prototypes.Proto.ID) id);
    }

    internal static string bc(this LocStr str) => string.Format("<bc>{0}</bc>", (object) str);

    internal static string NameBc(this Mafi.Core.Prototypes.Proto proto)
    {
      return string.Format("<bc>{0}</bc>", (object) proto.Strings.Name);
    }

    internal static ImmutableArray<GoalProto> CreateGoals(
      this ProtosDb db,
      params GoalProto[] protos)
    {
      foreach (GoalProto proto in protos)
        db.Add<Mafi.Core.Prototypes.Proto>((Mafi.Core.Prototypes.Proto) proto);
      return ImmutableArray.Create<GoalProto>(protos);
    }
  }
}
