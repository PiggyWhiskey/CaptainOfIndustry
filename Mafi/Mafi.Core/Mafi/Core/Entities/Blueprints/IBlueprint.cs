// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Blueprints.IBlueprint
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

#nullable disable
namespace Mafi.Core.Entities.Blueprints
{
  public interface IBlueprint : IBlueprintItem
  {
    string GameVersion { get; }

    int SaveVersion { get; }

    ImmutableArray<EntityConfigData> Items { get; }

    ImmutableArray<TileSurfaceCopyPasteData> Surfaces { get; }

    Option<string> ProtosThatFailedToLoad { get; }

    ImmutableArray<KeyValuePair<Proto, int>> MostFrequentProtos { get; }

    Set<Proto> AllDistinctProtos { get; }
  }
}
