// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.ILayoutEntityProto
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  public interface ILayoutEntityProto : IStaticEntityProto, IEntityProto, IProto
  {
    /// <summary>Entity layout in relative coordinates.</summary>
    EntityLayout Layout { get; }

    /// <summary>Available binding ports of this entity.</summary>
    ImmutableArray<IoPortTemplate> Ports { get; }

    /// <summary>Flip is disabled if true.</summary>
    bool CannotBeReflected { get; }

    /// <summary>Only one instance of this proto's entity is allowed.</summary>
    bool IsUnique { get; }

    /// <summary>
    /// If set, will automatically build miniZippers at ports.
    /// </summary>
    bool AutoBuildMiniZippers { get; }

    /// <summary>3D model of this entity.</summary>
    LayoutEntityProto.Gfx Graphics { get; }
  }
}
