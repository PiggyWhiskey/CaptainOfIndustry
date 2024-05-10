// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.AddSurfaceDecalCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  public class AddSurfaceDecalCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly Proto.ID ProtoId;
    public readonly RectangleTerrainArea2i Area;
    public readonly Rotation90 Rotation;
    public readonly bool IsReflected;
    public readonly int ColorKey;

    public static void Serialize(AddSurfaceDecalCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<AddSurfaceDecalCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, AddSurfaceDecalCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      RectangleTerrainArea2i.Serialize(this.Area, writer);
      writer.WriteInt(this.ColorKey);
      writer.WriteBool(this.IsReflected);
      Proto.ID.Serialize(this.ProtoId, writer);
      Rotation90.Serialize(this.Rotation, writer);
    }

    public static AddSurfaceDecalCmd Deserialize(BlobReader reader)
    {
      AddSurfaceDecalCmd addSurfaceDecalCmd;
      if (reader.TryStartClassDeserialization<AddSurfaceDecalCmd>(out addSurfaceDecalCmd))
        reader.EnqueueDataDeserialization((object) addSurfaceDecalCmd, AddSurfaceDecalCmd.s_deserializeDataDelayedAction);
      return addSurfaceDecalCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<AddSurfaceDecalCmd>(this, "Area", (object) RectangleTerrainArea2i.Deserialize(reader));
      reader.SetField<AddSurfaceDecalCmd>(this, "ColorKey", (object) reader.ReadInt());
      reader.SetField<AddSurfaceDecalCmd>(this, "IsReflected", (object) reader.ReadBool());
      reader.SetField<AddSurfaceDecalCmd>(this, "ProtoId", (object) Proto.ID.Deserialize(reader));
      reader.SetField<AddSurfaceDecalCmd>(this, "Rotation", (object) Rotation90.Deserialize(reader));
    }

    public AddSurfaceDecalCmd(
      Proto.ID protoId,
      RectangleTerrainArea2i area,
      Rotation90 rotation = default (Rotation90),
      bool isReflected = false,
      int colorKey = 0)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ProtoId = protoId;
      this.Area = area;
      this.Rotation = rotation;
      this.IsReflected = isReflected;
      this.ColorKey = colorKey;
    }

    static AddSurfaceDecalCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      AddSurfaceDecalCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      AddSurfaceDecalCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
