// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Vehicles.Jobs.IJobProvider`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities.Dynamic;

#nullable disable
namespace Mafi.Core.Vehicles.Jobs
{
  /// <summary>
  /// Represents a job provider for a vehicle.
  /// When a vehicle is without job it asks its job provider to get a next job.
  /// </summary>
  public interface IJobProvider<in TVehicle> where TVehicle : Vehicle
  {
    bool TryGetJobFor(TVehicle vehicle);
  }
}
