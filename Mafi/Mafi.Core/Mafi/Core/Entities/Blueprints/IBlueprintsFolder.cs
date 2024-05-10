// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Blueprints.IBlueprintsFolder
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections;
using Mafi.Collections.ReadonlyCollections;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Entities.Blueprints
{
  public interface IBlueprintsFolder : IBlueprintItem
  {
    bool IsEmpty { get; }

    Option<IBlueprintsFolder> ParentFolder { get; }

    IIndexable<IBlueprintsFolder> Folders { get; }

    IIndexable<IBlueprint> Blueprints { get; }

    Lyst<Proto> PreviewProtos { get; }
  }
}
