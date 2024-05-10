// Decompiled with JetBrains decompiler
// Type: Mafi.Base.Prototypes.Machines.PowerGenerators.MechPowerGeneratorFromProductConfigExtensions
// Assembly: Mafi.Base, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 189C759C-DBD6-48EA-AEF6-5D1D80F045F4
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Base.xml

using Mafi.Core.Entities;

#nullable disable
namespace Mafi.Base.Prototypes.Machines.PowerGenerators
{
  public static class MechPowerGeneratorFromProductConfigExtensions
  {
    public static bool? GetAutoBalance(this EntityConfigData data) => data.GetBool("AutoBalance");

    public static void SetAutoBalance(this EntityConfigData data, bool value)
    {
      data.SetBool("AutoBalance", new bool?(value));
    }
  }
}
