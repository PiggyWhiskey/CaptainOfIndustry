// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Buildings.NuclearWasteStorageInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Buildings.Storages.NuclearWaste;
using Mafi.Core.Prototypes;
using Mafi.Unity.UserInterface.Components;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Buildings
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class NuclearWasteStorageInspector : StorageInspectorGeneric<NuclearWasteStorage>
  {
    public NuclearWasteStorageInspector(
      InspectorContext inspectorContext,
      BuildingsAssigner buildingsAssigner,
      UnlockedProtosDb unlockedProtosDb,
      VehiclesAssignerFactory vehiclesAssignerFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext, buildingsAssigner, unlockedProtosDb, vehiclesAssignerFactory);
    }
  }
}
