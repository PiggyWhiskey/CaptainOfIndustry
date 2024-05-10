// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.ModsListHelper
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Mods;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.SaveGame
{
  internal static class ModsListHelper
  {
    public static void SerializeCustom(ImmutableArray<IMod> mods, BlobWriter writer)
    {
      writer.WriteIntNotNegative(mods.Length);
      foreach (IMod mod in mods)
      {
        writer.WriteString(mod.Name);
        writer.WriteInt(mod.Version);
        writer.WriteStringNoRef(mod.GetType().AssemblyQualifiedName);
      }
    }

    public static ImmutableArray<ModInfoRaw> DeserializeCustom(BlobReader reader)
    {
      int length = reader.ReadIntNotNegative();
      ImmutableArrayBuilder<ModInfoRaw> immutableArrayBuilder = new ImmutableArrayBuilder<ModInfoRaw>(length);
      for (int i = 0; i < length; ++i)
      {
        string name = reader.ReadString();
        int version = reader.ReadInt();
        string typeStr = reader.ReadStringNoRef();
        immutableArrayBuilder[i] = new ModInfoRaw(name, version, typeStr);
      }
      return immutableArrayBuilder.GetImmutableArrayAndClear();
    }
  }
}
