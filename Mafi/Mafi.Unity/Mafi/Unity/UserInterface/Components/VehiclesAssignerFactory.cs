// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.VehiclesAssignerFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Input;
using Mafi.Core.Prototypes;
using Mafi.Core.Vehicles;
using Mafi.Unity.Camera;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class VehiclesAssignerFactory
  {
    private readonly ProtosDb m_protosDb;
    private readonly UnlockedProtosDb m_unlockedProtosDb;
    private readonly CameraController m_cameraController;
    private readonly IInputScheduler m_inputScheduler;
    private readonly IVehiclesManager m_vehiclesManager;

    public VehiclesAssignerFactory(
      ProtosDb protosDb,
      UnlockedProtosDb unlockedProtosDb,
      CameraController cameraController,
      IInputScheduler inputScheduler,
      IVehiclesManager vehiclesManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb;
      this.m_unlockedProtosDb = unlockedProtosDb;
      this.m_cameraController = cameraController;
      this.m_inputScheduler = inputScheduler.CheckNotNull<IInputScheduler>();
      this.m_vehiclesManager = vehiclesManager.CheckNotNull<IVehiclesManager>();
    }

    public VehiclesAssignerView<T> CreateNewView<T>(Func<IEntityAssignedWithVehicles> entityProvider) where T : DrivingEntityProto
    {
      return new VehiclesAssignerView<T>(this.m_protosDb.All<T>().ToImmutableArray<T>(), this.m_unlockedProtosDb, this.m_cameraController, this.m_inputScheduler, this.m_vehiclesManager, entityProvider);
    }
  }
}
