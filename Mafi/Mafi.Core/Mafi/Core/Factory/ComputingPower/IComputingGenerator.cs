// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.ComputingPower.IComputingGenerator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Factory.ComputingPower
{
  public interface IComputingGenerator : 
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    Computing MaxComputingGenerationCapacity { get; }

    /// <summary>Called by computing manager every tick.</summary>
    Computing GenerateComputing();
  }
}
