// Decompiled with JetBrains decompiler
// Type: Mafi.Core.SaveGame.ModInfoRaw
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.SaveGame
{
  [OnlyForSaveCompatibility(null)]
  [GenerateSerializer(false, null, 0)]
  public readonly struct ModInfoRaw
  {
    public readonly string Name;
    public readonly int Version;
    public readonly string TypeStr;

    public static void Serialize(ModInfoRaw value, BlobWriter writer)
    {
      writer.WriteString(value.Name);
      writer.WriteInt(value.Version);
      writer.WriteString(value.TypeStr);
    }

    public static ModInfoRaw Deserialize(BlobReader reader)
    {
      return new ModInfoRaw(reader.ReadString(), reader.ReadInt(), reader.ReadString());
    }

    public ModInfoRaw(string name, int version, string typeStr)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Name = name;
      this.Version = version;
      this.TypeStr = typeStr;
    }
  }
}
