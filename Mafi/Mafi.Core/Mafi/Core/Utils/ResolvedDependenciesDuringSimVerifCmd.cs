// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.ResolvedDependenciesDuringSimVerifCmd
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Input;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Utils
{
  [GenerateSerializer(false, null, 0)]
  public class ResolvedDependenciesDuringSimVerifCmd : InputCommand
  {
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;
    public readonly ImmutableArray<string> ResolvedDepTypes;

    public static void Serialize(ResolvedDependenciesDuringSimVerifCmd value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<ResolvedDependenciesDuringSimVerifCmd>(value))
        return;
      writer.EnqueueDataSerialization((object) value, ResolvedDependenciesDuringSimVerifCmd.s_serializeDataDelayedAction);
    }

    protected override void SerializeData(BlobWriter writer)
    {
      base.SerializeData(writer);
      ImmutableArray<string>.Serialize(this.ResolvedDepTypes, writer);
    }

    public static ResolvedDependenciesDuringSimVerifCmd Deserialize(BlobReader reader)
    {
      ResolvedDependenciesDuringSimVerifCmd duringSimVerifCmd;
      if (reader.TryStartClassDeserialization<ResolvedDependenciesDuringSimVerifCmd>(out duringSimVerifCmd))
        reader.EnqueueDataDeserialization((object) duringSimVerifCmd, ResolvedDependenciesDuringSimVerifCmd.s_deserializeDataDelayedAction);
      return duringSimVerifCmd;
    }

    protected override void DeserializeData(BlobReader reader)
    {
      base.DeserializeData(reader);
      reader.SetField<ResolvedDependenciesDuringSimVerifCmd>(this, "ResolvedDepTypes", (object) ImmutableArray<string>.Deserialize(reader));
    }

    public override bool IsVerificationCmd => true;

    public ResolvedDependenciesDuringSimVerifCmd(ImmutableArray<string> resolvedDepTypes)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ResolvedDepTypes = resolvedDepTypes;
    }

    static ResolvedDependenciesDuringSimVerifCmd()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      ResolvedDependenciesDuringSimVerifCmd.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((InputCommand<bool>) obj).SerializeData(writer));
      ResolvedDependenciesDuringSimVerifCmd.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((InputCommand<bool>) obj).DeserializeData(reader));
    }
  }
}
