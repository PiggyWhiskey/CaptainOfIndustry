// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Messages.Goals.Goal
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Messages.Goals
{
  public abstract class Goal
  {
    public readonly GoalProto Prototype;

    [DoNotSave(0, null)]
    public string Title { get; protected set; }

    public bool IsLocked { get; set; }

    public bool IsCompleted { get; private set; }

    protected Goal(GoalProto prototype)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Prototype = prototype;
    }

    public void Update()
    {
      if (this.IsCompleted)
        return;
      this.IsCompleted = this.UpdateInternal();
    }

    protected abstract bool UpdateInternal();

    public void ForceComplete() => this.IsCompleted = true;

    [InitAfterLoad(InitPriority.Lowest)]
    protected abstract void UpdateTitleOnLoad();

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsCompleted);
      writer.WriteBool(this.IsLocked);
      writer.WriteGeneric<GoalProto>(this.Prototype);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsCompleted = reader.ReadBool();
      this.IsLocked = reader.ReadBool();
      reader.SetField<Goal>(this, "Prototype", (object) reader.ReadGenericAs<GoalProto>());
      reader.RegisterInitAfterLoad<Goal>(this, "UpdateTitleOnLoad", InitPriority.Lowest);
    }
  }
}
