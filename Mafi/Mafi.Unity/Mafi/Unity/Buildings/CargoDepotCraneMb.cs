// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Buildings.CargoDepotCraneMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Buildings
{
  internal class CargoDepotCraneMb : MonoBehaviour
  {
    private CargoDepotMb m_cargoDepotMb;
    private int m_slotIndex;

    public void Initialize(CargoDepotMb cargoDepotMb, int slotIndex)
    {
      this.m_cargoDepotMb = cargoDepotMb;
      this.m_slotIndex = slotIndex;
    }

    public CargoDepotCraneMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
