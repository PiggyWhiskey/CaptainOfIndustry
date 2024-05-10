// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Buildings.Forestry.ForestryTowerConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Terrain.Trees;
using Mafi.Serialization;
using System;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Buildings.Forestry
{
  public static class ForestryTowerConfigExtensions
  {
    public static Percent? GetTargetHarvestPercent(this EntityConfigData data)
    {
      return data.GetPercent("TargetHarvestPercent");
    }

    public static void SetTargetHarvestPercent(this EntityConfigData data, Percent value)
    {
      data.SetPercent("TargetHarvestPercent", new Percent?(value));
    }

    public static bool TryGetTreeTypes(
      this EntityConfigData data,
      ref Lyst<KeyValuePair<TreePlantingGroupProto, int>> treeTypes)
    {
      ImmutableArray<TreePlantingGroupProto>? protoArray = data.GetProtoArray<TreePlantingGroupProto>("TreeTypesProtos", true);
      if (!protoArray.HasValue)
        return false;
      ImmutableArray<int>? array = data.GetArray<int>("TreeTypesValues", (Func<BlobReader, int>) (r => r.ReadInt()));
      if (!array.HasValue)
      {
        Log.Error("Tree type protos have value but values is null.");
        return false;
      }
      if (protoArray.Value.Length != array.Value.Length)
      {
        Log.Error(string.Format("Proto length {0} differs from values length {1}", (object) protoArray.Value.Length, (object) array.Value.Length));
        return false;
      }
      treeTypes.Clear();
      int index = 0;
      while (true)
      {
        int num = index;
        ImmutableArray<TreePlantingGroupProto> immutableArray = protoArray.Value;
        int length = immutableArray.Length;
        if (num < length)
        {
          Lyst<KeyValuePair<TreePlantingGroupProto, int>> lyst = treeTypes;
          immutableArray = protoArray.Value;
          KeyValuePair<TreePlantingGroupProto, int> keyValuePair = new KeyValuePair<TreePlantingGroupProto, int>(immutableArray[index], array.Value[index]);
          lyst.Add(keyValuePair);
          ++index;
        }
        else
          break;
      }
      return true;
    }

    public static void SetTreeTypes(
      this EntityConfigData data,
      Lyst<KeyValuePair<TreePlantingGroupProto, int>> treeTypes)
    {
      ImmutableArrayBuilder<TreePlantingGroupProto> immutableArrayBuilder1 = new ImmutableArrayBuilder<TreePlantingGroupProto>(treeTypes.Count);
      ImmutableArrayBuilder<int> immutableArrayBuilder2 = new ImmutableArrayBuilder<int>(treeTypes.Count);
      int i1 = 0;
      foreach (KeyValuePair<TreePlantingGroupProto, int> treeType in treeTypes)
      {
        immutableArrayBuilder1[i1] = treeType.Key;
        immutableArrayBuilder2[i1] = treeType.Value;
        ++i1;
      }
      data.SetProtoArray<TreePlantingGroupProto>("TreePlantingGroupProtos", new ImmutableArray<TreePlantingGroupProto>?(immutableArrayBuilder1.GetImmutableArrayAndClear()));
      data.SetArray<int>("TreeTypesValues", new ImmutableArray<int>?(immutableArrayBuilder2.GetImmutableArrayAndClear()), (Action<int, BlobWriter>) ((i, writer) => writer.WriteInt(i)));
    }
  }
}
