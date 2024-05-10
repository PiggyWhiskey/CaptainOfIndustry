// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.ZipperConfigExtensions
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Zippers
{
  public static class ZipperConfigExtensions
  {
    public static bool? GetForceEvenInputs(this EntityConfigData data)
    {
      return data.GetBool("ForceEvenInputs");
    }

    public static void SetForceEvenInputs(this EntityConfigData data, bool value)
    {
      data.SetBool("ForceEvenInputs", new bool?(value));
    }

    public static bool? GetForceEvenOutputs(this EntityConfigData data)
    {
      return data.GetBool("ForceEvenOutputs");
    }

    public static void SetForceEvenOutputs(this EntityConfigData data, bool value)
    {
      data.SetBool("ForceEvenOutputs", new bool?(value));
    }

    public static ImmutableArray<bool>? GetPrioritizedPorts(this EntityConfigData data)
    {
      return data.GetArray<bool>("PrioritizedPorts", (Func<BlobReader, bool>) (r => r.ReadBool()));
    }

    public static void SetPrioritizedPorts(this EntityConfigData data, ImmutableArray<bool> value)
    {
      data.SetArray<bool>("PrioritizedPorts", new ImmutableArray<bool>?(value), (Action<bool, BlobWriter>) ((b, writer) => writer.WriteBool(b)));
    }
  }
}
