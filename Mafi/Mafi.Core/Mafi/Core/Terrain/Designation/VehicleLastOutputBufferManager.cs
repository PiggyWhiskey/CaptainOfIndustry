// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Terrain.Designation.VehicleLastOutputBufferManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Vehicles;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Terrain.Designation
{
  [GenerateSerializer(false, null, 0)]
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class VehicleLastOutputBufferManager
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    private readonly Dict<Vehicle, RegisteredOutputBuffer> m_buffers;

    public static void Serialize(VehicleLastOutputBufferManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<VehicleLastOutputBufferManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, VehicleLastOutputBufferManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      Dict<Vehicle, RegisteredOutputBuffer>.Serialize(this.m_buffers, writer);
    }

    public static VehicleLastOutputBufferManager Deserialize(BlobReader reader)
    {
      VehicleLastOutputBufferManager outputBufferManager;
      if (reader.TryStartClassDeserialization<VehicleLastOutputBufferManager>(out outputBufferManager))
        reader.EnqueueDataDeserialization((object) outputBufferManager, VehicleLastOutputBufferManager.s_deserializeDataDelayedAction);
      return outputBufferManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      reader.SetField<VehicleLastOutputBufferManager>(this, "m_buffers", (object) Dict<Vehicle, RegisteredOutputBuffer>.Deserialize(reader));
    }

    public void ReportOutputBufferFor(Vehicle v, RegisteredOutputBuffer buffer)
    {
      this.m_buffers[v] = buffer;
    }

    public bool TryGetLastOutputBufferFor(Vehicle v, out RegisteredOutputBuffer buffer)
    {
      if (!this.m_buffers.TryGetValue(v, out buffer))
        return false;
      if (!buffer.Entity.IsDestroyed)
        return true;
      this.m_buffers.Remove(v);
      return false;
    }

    public void ClearOutputBufferFor(Vehicle v) => this.m_buffers.Remove(v);

    public VehicleLastOutputBufferManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.m_buffers = new Dict<Vehicle, RegisteredOutputBuffer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static VehicleLastOutputBufferManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      VehicleLastOutputBufferManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((VehicleLastOutputBufferManager) obj).SerializeData(writer));
      VehicleLastOutputBufferManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((VehicleLastOutputBufferManager) obj).DeserializeData(reader));
    }
  }
}
