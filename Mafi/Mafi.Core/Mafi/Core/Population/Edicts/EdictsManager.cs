// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.EdictsManager
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.GameLoop;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  [GlobalDependency(RegistrationMode.AsEverything, false, false)]
  [GenerateSerializer(false, null, 0)]
  public class EdictsManager : 
    ICommandProcessor<ToggleEdictEnabledCmd>,
    IAction<ToggleEdictEnabledCmd>
  {
    public ImmutableArray<Edict> AllEdicts;
    private static readonly Action<object, BlobWriter> s_serializeDataDelayedAction;
    private static readonly Action<object, BlobReader> s_deserializeDataDelayedAction;

    public EdictsManager(
      ProtosDb protosDb,
      DependencyResolver resolver,
      IGameLoopEvents gameLoopEvents)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      this.AllEdicts = ImmutableArray<Edict>.Empty;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      EdictsManager edictsManager = this;
      gameLoopEvents.RegisterNewGameCreated((object) this, (Action) (() =>
      {
        Lyst<Edict> lyst = new Lyst<Edict>();
        foreach (EdictProto edictProto in protosDb.All<EdictProto>())
        {
          if (!(resolver.Instantiate(edictProto.Implementation, (object) edictProto) is Edict edict2))
            Log.Error(string.Format("Failed create instance of {0} for {1}", (object) edictProto.Implementation, (object) edictProto));
          else
            lyst.Add(edict2);
        }
        edictsManager.AllEdicts = lyst.ToImmutableArray();
      }));
    }

    [InitAfterLoad(InitPriority.Normal)]
    private void initSelf(DependencyResolver resolver)
    {
      Lyst<Edict> lyst = new Lyst<Edict>();
      lyst.AddRange(this.AllEdicts);
      foreach (EdictProto edictProto1 in resolver.Resolve<ProtosDb>().All<EdictProto>())
      {
        EdictProto edictProto = edictProto1;
        if (!this.AllEdicts.Any((Func<Edict, bool>) (x => (Proto) x.Prototype == (Proto) edictProto)))
        {
          if (!(resolver.Instantiate(edictProto.Implementation, (object) edictProto) is Edict edict))
            Log.Error(string.Format("Failed create instance of {0} for {1}", (object) edictProto.Implementation, (object) edictProto));
          else
            lyst.Add(edict);
        }
      }
      this.AllEdicts = lyst.ToImmutableArray();
    }

    public void Invoke(ToggleEdictEnabledCmd cmd)
    {
      Edict toToggle1 = this.AllEdicts.FirstOrDefault((Func<Edict, bool>) (x => x.Prototype.Id == cmd.EdictProtoId));
      if (toToggle1 == null)
      {
        cmd.SetResultError(string.Format("Failed to find edict with id {0}", (object) cmd.EdictProtoId));
      }
      else
      {
        Edict toToggle;
        if (toToggle1.IsEnabled)
        {
          for (Option<Edict> option = (Option<Edict>) toToggle1; option.HasValue && option.Value.IsEnabled; option = (Option<Edict>) this.AllEdicts.FirstOrDefault((Func<Edict, bool>) (x => x.Prototype == toToggle.Prototype.NextTier)))
          {
            toToggle = option.Value;
            toToggle.ToggleEnabled();
            if (toToggle.Prototype.NextTier.IsNone)
              break;
          }
        }
        else
          enableIfCanEnableParents(toToggle1);
        cmd.SetResultSuccess();
      }

      bool enableIfCanEnableParents(Edict toToggle)
      {
        if (toToggle.IsEnabled)
          return true;
        if (toToggle.Prototype.PreviousTier.IsNone)
        {
          toToggle.ToggleEnabled();
          return toToggle.IsEnabled;
        }
        Edict toToggle1 = this.AllEdicts.FirstOrDefault((Func<Edict, bool>) (x => x.Prototype == toToggle.Prototype.PreviousTier));
        if (toToggle1 == null)
        {
          Log.Error("Edict implementation not found!");
          toToggle.ToggleEnabled();
          return toToggle.IsEnabled;
        }
        if (enableIfCanEnableParents(toToggle1))
          toToggle.ToggleEnabled();
        return toToggle.IsEnabled;
      }
    }

    public static void Serialize(EdictsManager value, BlobWriter writer)
    {
      if (!writer.TryStartClassSerialization<EdictsManager>(value))
        return;
      writer.EnqueueDataSerialization((object) value, EdictsManager.s_serializeDataDelayedAction);
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      ImmutableArray<Edict>.Serialize(this.AllEdicts, writer);
    }

    public static EdictsManager Deserialize(BlobReader reader)
    {
      EdictsManager edictsManager;
      if (reader.TryStartClassDeserialization<EdictsManager>(out edictsManager))
        reader.EnqueueDataDeserialization((object) edictsManager, EdictsManager.s_deserializeDataDelayedAction);
      return edictsManager;
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.AllEdicts = ImmutableArray<Edict>.Deserialize(reader);
      reader.RegisterInitAfterLoad<EdictsManager>(this, "initSelf", InitPriority.Normal);
    }

    static EdictsManager()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      EdictsManager.s_serializeDataDelayedAction = (Action<object, BlobWriter>) ((obj, writer) => ((EdictsManager) obj).SerializeData(writer));
      EdictsManager.s_deserializeDataDelayedAction = (Action<object, BlobReader>) ((obj, reader) => ((EdictsManager) obj).DeserializeData(reader));
    }
  }
}
