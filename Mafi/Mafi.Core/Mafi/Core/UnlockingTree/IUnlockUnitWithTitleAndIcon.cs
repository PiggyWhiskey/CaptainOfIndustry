// Decompiled with JetBrains decompiler
// Type: Mafi.Core.UnlockingTree.IUnlockUnitWithTitleAndIcon
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using Mafi.Localization;

#nullable disable
namespace Mafi.Core.UnlockingTree
{
  /// <summary>
  /// Unit that can be displayed in the UI using an icon and title.
  /// </summary>
  public interface IUnlockUnitWithTitleAndIcon : IUnlockNodeUnit
  {
    LocStrFormatted Title { get; }

    LocStrFormatted Description { get; }

    Option<string> IconPath { get; }
  }
}
