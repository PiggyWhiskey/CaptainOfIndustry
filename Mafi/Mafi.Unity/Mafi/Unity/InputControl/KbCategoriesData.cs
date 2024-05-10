// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.KbCategoriesData
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl
{
  public static class KbCategoriesData
  {
    public static readonly Set<KbCategory> MutuallyExclusiveCategories;
    public static readonly Dict<KbCategory, LocStrFormatted> Translations;

    static KbCategoriesData()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      KbCategoriesData.MutuallyExclusiveCategories = new Set<KbCategory>()
      {
        KbCategory.Build,
        KbCategory.Demolish,
        KbCategory.Designation,
        KbCategory.Transport
      };
      KbCategoriesData.Translations = new Dict<KbCategory, LocStrFormatted>()
      {
        {
          KbCategory.Camera,
          (LocStrFormatted) Tr.Camera
        },
        {
          KbCategory.Speed,
          (LocStrFormatted) Tr.GameSpeed
        },
        {
          KbCategory.Tools,
          (LocStrFormatted) Tr.ToolsTitle
        },
        {
          KbCategory.Designation,
          (LocStrFormatted) Tr.Designations
        },
        {
          KbCategory.Build,
          (LocStrFormatted) Tr.BuildMode
        },
        {
          KbCategory.Transport,
          (LocStrFormatted) Tr.TransportMode
        },
        {
          KbCategory.Demolish,
          (LocStrFormatted) TrCore.Demolish
        },
        {
          KbCategory.CopyTool,
          (LocStrFormatted) TrCore.CopyTool
        },
        {
          KbCategory.PauseTool,
          (LocStrFormatted) TrCore.PauseTool
        },
        {
          KbCategory.Windows,
          (LocStrFormatted) Tr.WindowsShortcuts
        },
        {
          KbCategory.General,
          (LocStrFormatted) Tr.GeneralShortcuts
        },
        {
          KbCategory.PhotoMode,
          (LocStrFormatted) Tr.PhotoMode
        },
        {
          KbCategory.MapEditor,
          (LocStrFormatted) Tr.Menu__MapEditor
        }
      };
    }
  }
}
