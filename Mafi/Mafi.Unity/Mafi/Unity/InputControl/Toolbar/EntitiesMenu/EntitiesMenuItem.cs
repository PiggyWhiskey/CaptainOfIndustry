// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.EntitiesMenu.EntitiesMenuItem
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.EntitiesMenu
{
  public class EntitiesMenuItem
  {
    public readonly Option<Mafi.Core.Prototypes.Proto> Proto;
    public readonly Option<Mafi.Core.Prototypes.Proto> ExtraLockingProto;
    public readonly LocStrFormatted Name;
    public readonly string IconPath;
    public readonly ImmutableArray<ToolbarCategoryProto> Categories;

    public EntitiesMenuItem(
      Mafi.Core.Prototypes.Proto prototype,
      LocStrFormatted name,
      string iconPath,
      ImmutableArray<ToolbarCategoryProto> categories)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Proto = (Option<Mafi.Core.Prototypes.Proto>) prototype;
      this.Name = name;
      this.IconPath = iconPath.CheckNotNull<string>();
      this.Categories = categories;
    }

    public EntitiesMenuItem(
      Option<Mafi.Core.Prototypes.Proto> prototype,
      Option<Mafi.Core.Prototypes.Proto> extraLockingProto,
      LocStrFormatted name,
      string iconPath,
      ImmutableArray<ToolbarCategoryProto> categories)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Proto = prototype;
      this.ExtraLockingProto = extraLockingProto;
      this.Name = name;
      this.IconPath = iconPath.CheckNotNull<string>();
      this.Categories = categories;
    }

    public bool IsUnlocked(UnlockedProtosDbForUi unlockedProtos)
    {
      if (!this.Proto.IsNone && !unlockedProtos.IsUnlocked((IProto) this.Proto.Value))
        return false;
      return this.ExtraLockingProto.IsNone || unlockedProtos.IsUnlocked((IProto) this.ExtraLockingProto.Value);
    }
  }
}
