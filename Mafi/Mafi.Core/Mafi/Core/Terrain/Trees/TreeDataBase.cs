// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Trees.TreeDataBase
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Terrain.Trees
{
  [GenerateSerializer(false, null, 0)]
  public struct TreeDataBase
  {
    public TreeProto Proto;
    public Tile2f Position;
    public AngleSlim Rotation;
    public Percent Scale;

    public static void Serialize(TreeDataBase value, BlobWriter writer)
    {
      writer.WriteGeneric<TreeProto>(value.Proto);
      Tile2f.Serialize(value.Position, writer);
      AngleSlim.Serialize(value.Rotation, writer);
      Percent.Serialize(value.Scale, writer);
    }

    public static TreeDataBase Deserialize(BlobReader reader)
    {
      return new TreeDataBase(reader.ReadGenericAs<TreeProto>(), Tile2f.Deserialize(reader), AngleSlim.Deserialize(reader), Percent.Deserialize(reader));
    }

    public TreeDataBase(TreeProto proto, Tile2f position, AngleSlim rotation, Percent scale)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Proto = proto;
      this.Position = position;
      this.Rotation = rotation;
      this.Scale = scale;
    }

    public override readonly string ToString()
    {
      return string.Format("{0} at {1}", (object) this.Proto, (object) this.Position);
    }
  }
}
