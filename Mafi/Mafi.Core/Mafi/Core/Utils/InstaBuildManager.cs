// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.InstaBuildManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  internal class InstaBuildManager : 
    IInstaBuildManager,
    ICommandProcessor<SetInstaBuildCmd>,
    IAction<SetInstaBuildCmd>
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public static void Serialize(InstaBuildManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<InstaBuildManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, InstaBuildManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsInstaBuildEnabled);
    }

    public static InstaBuildManager Deserialize(BlobReader reader)
    {
      InstaBuildManager instaBuildManager;
      if (reader.TryStartClassDeserialization<InstaBuildManager>(out instaBuildManager))
        reader.EnqueueDataDeserialization((object) instaBuildManager, InstaBuildManager.s_deserializeDataDelayedAction);
      return instaBuildManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsInstaBuildEnabled = reader.ReadBool();
    }

    public bool IsInstaBuildEnabled { get; private set; }

    public InstaBuildManager(IInstaBuildConfig config)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.SetInstaBuild(config.IsInstaBuildEnabled);
    }

    public void SetInstaBuild(bool isEnabled)
    {
    }

    void IAction<SetInstaBuildCmd>.Invoke(SetInstaBuildCmd cmd)
    {
      this.SetInstaBuild(cmd.SetEnabled);
      cmd.SetResultSuccess();
    }

    static InstaBuildManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      InstaBuildManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InstaBuildManager) obj).SerializeData(writer));
      InstaBuildManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InstaBuildManager) obj).DeserializeData(reader));
    }
  }
}
