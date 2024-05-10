// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Ports.Io.PortSpec
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Ports.Io
{
  [GenerateSerializer(false, null, 0)]
  public readonly struct PortSpec
  {
    public readonly char Name;
    public readonly IoPortType Type;
    public readonly IoPortShapeProto Shape;
    public readonly bool CanOnlyConnectToTransports;

    public PortSpec(
      char name,
      IoPortType type,
      IoPortShapeProto shape,
      bool canOnlyConnectToTransports)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.Name = name;
      this.Type = type;
      this.Shape = shape;
      this.CanOnlyConnectToTransports = canOnlyConnectToTransports;
    }

    public static void Serialize(PortSpec value, BlobWriter writer)
    {
      writer.WriteChar(value.Name);
      writer.WriteInt((int) value.Type);
      writer.WriteGeneric<IoPortShapeProto>(value.Shape);
      writer.WriteBool(value.CanOnlyConnectToTransports);
    }

    public static PortSpec Deserialize(BlobReader reader)
    {
      return new PortSpec(reader.ReadChar(), (IoPortType) reader.ReadInt(), reader.ReadGenericAs<IoPortShapeProto>(), reader.ReadBool());
    }
  }
}
