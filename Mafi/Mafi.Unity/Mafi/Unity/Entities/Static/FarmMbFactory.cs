﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.FarmMbFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.GameLoop;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class FarmMbFactory : IEntityMbFactory<Farm>, IFactory<Farm, EntityMb>
  {
    private readonly ProtoModelFactory m_modelFactory;
    private readonly AssetsDb m_assetsDb;
    private readonly RandomProvider m_randomProvider;
    private readonly IGameLoopEvents m_gameLoopEvents;

    public FarmMbFactory(
      ProtoModelFactory modelFactory,
      AssetsDb assetsDb,
      RandomProvider randomProvider,
      IGameLoopEvents gameLoopEvents)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_modelFactory = modelFactory;
      this.m_assetsDb = assetsDb;
      this.m_randomProvider = randomProvider;
      this.m_gameLoopEvents = gameLoopEvents;
    }

    public EntityMb Create(Farm farm)
    {
      FarmMb farmMb = this.m_modelFactory.CreateModelFor<FarmProto>(farm.Prototype).AddComponent<FarmMb>();
      farmMb.Initialize(farm, this.m_assetsDb, this.m_randomProvider, this.m_gameLoopEvents);
      return (EntityMb) farmMb;
    }
  }
}
