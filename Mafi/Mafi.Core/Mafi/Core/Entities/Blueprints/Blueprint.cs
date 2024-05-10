// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Blueprints.Blueprint
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain.Surfaces;
using Mafi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable disable
namespace Mafi.Core.Entities.Blueprints
{
  internal class Blueprint : IBlueprint, IBlueprintItem, IBlueprintItemFriend
  {
    private readonly Blueprint.DistinctDecalProtosInfo m_distinctDecalProtosInfo;

    public string GameVersion { get; }

    public int SaveVersion { get; }

    public string Name { get; private set; }

    public string Desc { get; private set; }

    public ImmutableArray<EntityConfigData> Items { get; private set; }

    public ImmutableArray<TileSurfaceCopyPasteData> Surfaces { get; private set; }

    public Option<string> ProtosThatFailedToLoad { get; private set; }

    public ImmutableArray<KeyValuePair<Proto, int>> MostFrequentProtos { get; private set; }

    public Set<Proto> AllDistinctProtos { get; }

    public Blueprint(
      string name,
      ImmutableArray<EntityConfigData> items,
      ImmutableArray<TileSurfaceCopyPasteData> surfaces,
      ConfigSerializationContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMostFrequentProtos\u003Ek__BackingField = (ImmutableArray<KeyValuePair<Proto, int>>) ImmutableArray.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_distinctDecalProtosInfo = new Blueprint.DistinctDecalProtosInfo(ImmutableArray<string>.Empty, ImmutableArray<string>.Empty, context.TileSurfaceDecalsSlimIdManager, context.ProtosDb);
      this.GameVersion = "0.6.3a";
      this.SaveVersion = 168;
      this.Name = name;
      this.Desc = "";
      this.Items = items;
      this.Surfaces = surfaces;
      this.MostFrequentProtos = this.getMostFrequentProtos(this.Items);
      this.AllDistinctProtos = this.getDistinctProtos();
    }

    private Blueprint(
      string gameVersion,
      int saveVersion,
      string name,
      string desc,
      ImmutableArray<EntityConfigData> items,
      ImmutableArray<TileSurfaceCopyPasteData> surfaces,
      Blueprint.DistinctDecalProtosInfo distinctDecalProtosInfo)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CMostFrequentProtos\u003Ek__BackingField = (ImmutableArray<KeyValuePair<Proto, int>>) ImmutableArray.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_distinctDecalProtosInfo = distinctDecalProtosInfo;
      this.GameVersion = gameVersion;
      this.SaveVersion = saveVersion;
      this.Name = name;
      this.Desc = desc;
      this.Items = items;
      this.Surfaces = surfaces;
      this.MostFrequentProtos = this.getMostFrequentProtos(this.Items);
      this.AllDistinctProtos = this.getDistinctProtos();
      StringBuilder stringBuilder = (StringBuilder) null;
      Set<string> set = (Set<string>) null;
      foreach (EntityConfigData entityConfigData in this.Items)
      {
        if (entityConfigData.Prototype.IsNone)
        {
          if (set == null)
            set = new Set<string>();
          if (stringBuilder == null)
            stringBuilder = new StringBuilder();
          Option<string> option = entityConfigData.ProtoModName;
          string str1 = option.ValueOrNull ?? "Core";
          option = entityConfigData.ProtoId;
          string str2 = option.ValueOrNull ?? "";
          if (!set.Contains(str2))
          {
            set.Add(str2);
            stringBuilder.AppendLine(str2 + " (" + str1 + ")");
          }
        }
      }
      for (int index = 0; index < distinctDecalProtosInfo.ProtoIds.Length; ++index)
      {
        string protoId = distinctDecalProtosInfo.ProtoIds[index];
        if (!distinctDecalProtosInfo.ProtosDb.TryGetProto<TerrainTileSurfaceDecalProto>(new Proto.ID(protoId), out TerrainTileSurfaceDecalProto _))
        {
          if (set == null)
            set = new Set<string>();
          if (stringBuilder == null)
            stringBuilder = new StringBuilder();
          string modName = distinctDecalProtosInfo.ModNames[index];
          if (!set.Contains(protoId))
          {
            set.Add(protoId);
            stringBuilder.AppendLine(protoId + " (" + modName + ")");
          }
        }
      }
      if (stringBuilder == null)
        return;
      this.ProtosThatFailedToLoad = (Option<string>) stringBuilder.ToString();
    }

    private Set<Proto> getDistinctProtos()
    {
      return this.Items.Where((Func<EntityConfigData, bool>) (x => x.Prototype.HasValue)).Select<EntityConfigData, Proto>((Func<EntityConfigData, Proto>) (x => x.Prototype.Value)).ToSet<Proto>();
    }

    private Set<Proto> getDistinctDecalProtos()
    {
      return this.Surfaces.Where((Func<TileSurfaceCopyPasteData, bool>) (x => x.SurfaceData.DecalSlimId.IsNotPhantom)).Select<TileSurfaceCopyPasteData, Proto>((Func<TileSurfaceCopyPasteData, Proto>) (x => (Proto) x.SurfaceData.DecalSlimId.AsProtoOrPhantom(this.m_distinctDecalProtosInfo.TileSurfaceDecalsSlimIdManager))).ToSet<Proto>();
    }

    [MustUseReturnValue]
    private ImmutableArray<KeyValuePair<Proto, int>> getMostFrequentProtos(
      ImmutableArray<EntityConfigData> items)
    {
      Dict<Proto, int> dict = new Dict<Proto, int>();
      bool flag1 = false;
      bool flag2 = false;
      foreach (EntityConfigData entityConfigData in items)
      {
        Proto valueOrNull = entityConfigData.Prototype.ValueOrNull;
        if (!(valueOrNull == (Proto) null) && !(valueOrNull is MiniZipperProto))
        {
          dict.IncOrInsert1<Proto>(entityConfigData.Prototype.Value);
          bool flag3 = valueOrNull is TransportProto || valueOrNull is ZipperProto;
          flag1 |= !flag3;
          flag2 = ((flag2 ? 1 : 0) | (flag3 ? 0 : (valueOrNull.Id.Value.Contains("SmokeStack") ? 1 : 0))) != 0;
        }
      }
      if (flag1)
        dict.RemoveKeys((Predicate<Proto>) (x =>
        {
          bool mostFrequentProtos;
          switch (x)
          {
            case TransportProto _:
            case ZipperProto _:
              mostFrequentProtos = true;
              break;
            default:
              mostFrequentProtos = false;
              break;
          }
          return mostFrequentProtos;
        }));
      if (flag2)
        dict.RemoveKeys((Predicate<Proto>) (x => x.Id.Value.Contains("SmokeStack")));
      return dict.OrderByDescending<KeyValuePair<Proto, int>, int>((Func<KeyValuePair<Proto, int>, int>) (x => x.Value)).ToImmutableArray<KeyValuePair<Proto, int>>();
    }

    public void SetName(string name) => this.Name = name;

    public void SetDescription(string desc) => this.Desc = desc;

    public IBlueprint CreateCopyForSave()
    {
      return (IBlueprint) new Blueprint(this.GameVersion, this.SaveVersion, this.Name, this.Desc, this.Items, this.Surfaces, this.m_distinctDecalProtosInfo);
    }

    internal void SerializeForBlueprints(BlobWriter writer)
    {
      writer.WriteString(this.GameVersion);
      writer.WriteIntNotNegative(this.SaveVersion);
      writer.WriteString(this.Name);
      writer.WriteString(this.Desc);
      writer.WriteIntNotNegative(this.Items.Length);
      foreach (EntityConfigData entityConfigData in this.Items)
        entityConfigData.SerializeForBlueprints(writer);
      TileSurfaceCopyPasteData.SerializeAllForBlueprints(this.Surfaces, writer);
      Set<Proto> distinctDecalProtos = this.getDistinctDecalProtos();
      writer.WriteIntNotNegative(distinctDecalProtos.Count);
      foreach (Proto proto in distinctDecalProtos)
      {
        writer.WriteString(proto.Mod.Name);
        writer.WriteString(proto.Id.Value);
      }
    }

    internal static Blueprint DeserializeForBlueprints(
      BlobReader reader,
      ConfigSerializationContext context,
      int libraryVersion)
    {
      string gameVersion = reader.ReadString();
      int saveVersion = reader.ReadIntNotNegative();
      string name = reader.ReadString();
      string desc = reader.ReadString();
      int length1 = reader.ReadIntNotNegative();
      ImmutableArrayBuilder<EntityConfigData> immutableArrayBuilder1 = new ImmutableArrayBuilder<EntityConfigData>(length1);
      for (int i = 0; i < length1; ++i)
        immutableArrayBuilder1[i] = EntityConfigData.DeserializeForBlueprints(reader, context);
      ImmutableArray<EntityConfigData> immutableArrayAndClear = immutableArrayBuilder1.GetImmutableArrayAndClear();
      ImmutableArray<TileSurfaceCopyPasteData> surfaces = libraryVersion < 2 ? ImmutableArray<TileSurfaceCopyPasteData>.Empty : TileSurfaceCopyPasteData.DeserializeAllForBlueprints(reader);
      int length2 = libraryVersion >= 2 ? reader.ReadIntNotNegative() : 0;
      ImmutableArrayBuilder<string> immutableArrayBuilder2 = new ImmutableArrayBuilder<string>(length2);
      ImmutableArrayBuilder<string> immutableArrayBuilder3 = new ImmutableArrayBuilder<string>(length2);
      for (int i = 0; i < length2; ++i)
      {
        immutableArrayBuilder2[i] = reader.ReadString();
        immutableArrayBuilder3[i] = reader.ReadString();
      }
      Blueprint.DistinctDecalProtosInfo distinctDecalProtosInfo = new Blueprint.DistinctDecalProtosInfo(immutableArrayBuilder2.GetImmutableArrayAndClear(), immutableArrayBuilder3.GetImmutableArrayAndClear(), context.TileSurfaceDecalsSlimIdManager, context.ProtosDb);
      return new Blueprint(gameVersion, saveVersion, name, desc, immutableArrayAndClear, surfaces, distinctDecalProtosInfo);
    }

    public struct DistinctDecalProtosInfo
    {
      public readonly ImmutableArray<string> ModNames;
      public readonly ImmutableArray<string> ProtoIds;
      public readonly TileSurfaceDecalsSlimIdManager TileSurfaceDecalsSlimIdManager;
      public readonly ProtosDb ProtosDb;

      public DistinctDecalProtosInfo(
        ImmutableArray<string> modNames,
        ImmutableArray<string> protoIds,
        TileSurfaceDecalsSlimIdManager tileSurfaceDecalsSlimIdManager,
        ProtosDb protosDb)
      {
        MBiHIp97M4MqqbtZOh.rMWAw2OR8();
        this.ModNames = modNames;
        this.ProtoIds = protoIds;
        this.TileSurfaceDecalsSlimIdManager = tileSurfaceDecalsSlimIdManager;
        this.ProtosDb = protosDb;
      }
    }
  }
}
