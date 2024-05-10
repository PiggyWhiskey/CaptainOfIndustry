// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.GameMenu.LoadSave.SaveModStatus
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.SaveGame;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.GameMenu.LoadSave
{
  internal class SaveModStatus
  {
    public readonly string Name;
    public readonly int Version;
    public readonly Option<Mafi.Core.Mods.ModData> ModData;
    public bool IsSelected;

    public bool IsAvailable
    {
      get
      {
        Mafi.Core.Mods.ModData valueOrNull = this.ModData.ValueOrNull;
        return valueOrNull != null && valueOrNull.IsFullyLoaded;
      }
    }

    public Option<Type> ModType
    {
      get
      {
        Type modType = this.ModData.ValueOrNull?.ModType;
        return (object) modType == null ? Option<Type>.None : (Option<Type>) modType;
      }
    }

    /// <summary>We failed to load mod's type.</summary>
    public SaveModStatus(ModInfoRaw info, bool isSelected)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Name = info.Name;
      this.Version = info.Version;
      this.IsSelected = isSelected;
    }

    public SaveModStatus(Mafi.Core.Mods.ModData data, bool isSelected)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.ModData = (Option<Mafi.Core.Mods.ModData>) data;
      this.Name = data.Name;
      this.Version = 0;
      this.IsSelected = isSelected;
    }
  }
}
