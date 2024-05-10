// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.ModData
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;

#nullable disable
namespace Mafi.Core.Mods
{
  public class ModData
  {
    public readonly ModGroup Group;
    public readonly string Name;
    public readonly Type ModType;
    public readonly Option<string> AssetsPath;
    public int? Version;
    public Option<System.Exception> Exception;

    /// <summary>
    /// Danger: This is only set after GameBuilder.TryInstantiateModsAndProtos was called.
    /// </summary>
    public bool IsFullyLoaded
    {
      get
      {
        if (!this.Version.HasValue)
        {
          int num = this.Exception.IsNone ? 1 : 0;
        }
        return this.Version.HasValue;
      }
    }

    public bool FailedToLoad => this.Exception.HasValue;

    public ModData(ModGroup group, string name, Type modType, Option<string> assetsPath)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      if (modType == (Type) null)
        Log.Error("Mod type cannot be null! Mod name: " + name);
      this.Group = group;
      this.ModType = modType;
      this.AssetsPath = assetsPath;
      this.Name = name;
    }

    public void SetLoadSuccess(int version)
    {
      if (this.Exception.HasValue)
        Log.Error("Cannot mark mod '" + this.Name + "' that had exception as loaded!");
      else
        this.Version = new int?(version);
    }

    public void SetLoadFail(System.Exception exception)
    {
      Assert.That<int?>(this.Version).IsNull<int>();
      this.Exception = (Option<System.Exception>) exception;
    }
  }
}
