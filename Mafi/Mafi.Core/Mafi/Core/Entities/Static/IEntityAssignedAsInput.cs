﻿// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.IEntityAssignedAsInput
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities.Static
{
  public interface IEntityAssignedAsInput : 
    ILayoutEntity,
    IStaticEntity,
    IEntityWithPosition,
    IRenderedEntity,
    IEntity,
    IIsSafeAsHashKey,
    IAreaSelectableEntity
  {
    IReadOnlySet<IEntityAssignedAsOutput> AssignedOutputs { get; }

    bool CanBeAssignedWithOutput(IEntityAssignedAsOutput entity);

    void AssignStaticOutputEntity(IEntityAssignedAsOutput entity);

    void UnassignStaticOutputEntity(IEntityAssignedAsOutput entity);

    bool AllowNonAssignedOutput { get; set; }
  }
}
