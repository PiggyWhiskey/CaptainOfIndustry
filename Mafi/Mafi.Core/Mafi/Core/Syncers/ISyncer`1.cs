// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Syncers.ISyncer`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

#nullable disable
namespace Mafi.Core.Syncers
{
  public interface ISyncer<T>
  {
    /// <summary>
    /// It is important that the this boolean is not reset by Syncer until it gets read by a Trigger.
    /// Otherwise next sync would clear this value and if no render (Trigger) was running at that time
    /// the change would be lost. This can happen when view gets hidden between sync and render phases.
    /// </summary>
    bool HasChanged { get; }

    T GetValueAndReset();
  }
}
