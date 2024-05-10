// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Mods.ModsLoader
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.SaveGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

#nullable disable
namespace Mafi.Core.Mods
{
  public sealed class ModsLoader
  {
    public const string ASSET_BUNDLES_DIR_NAME = "AssetBundles";
    public const string DLCS_DIR_NAME = "DLCs";

    public static ImmutableArray<ModData> LoadAllModsFrom(
      ModGroup group,
      string modsRootPath,
      bool loadAssemblyDependencies,
      out ImmutableArray<ModInfoRaw> failedMods,
      out string error)
    {
      error = "";
      failedMods = ImmutableArray<ModInfoRaw>.Empty;
      if (!Directory.Exists(modsRootPath))
      {
        Log.Warning("Directory for loading mods '" + modsRootPath + "' not found.");
        return ImmutableArray<ModData>.Empty;
      }
      Log.Info("Loading mods from '" + modsRootPath + "'.");
      Lyst<ModData> loadedMods = new Lyst<ModData>();
      Lyst<ModInfoRaw> lyst = new Lyst<ModInfoRaw>();
      AppDomain currentDomain = AppDomain.CurrentDomain;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      currentDomain.AssemblyLoad -= ModsLoader.\u003C\u003EO.\u003C0\u003E__onAssemblyLoad ?? (ModsLoader.\u003C\u003EO.\u003C0\u003E__onAssemblyLoad = new AssemblyLoadEventHandler(ModsLoader.onAssemblyLoad));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      currentDomain.AssemblyLoad += ModsLoader.\u003C\u003EO.\u003C0\u003E__onAssemblyLoad ?? (ModsLoader.\u003C\u003EO.\u003C0\u003E__onAssemblyLoad = new AssemblyLoadEventHandler(ModsLoader.onAssemblyLoad));
      foreach (string enumerateDirectory in Directory.EnumerateDirectories(modsRootPath))
      {
        string fileName = Path.GetFileName(enumerateDirectory);
        string str1 = Path.Combine(enumerateDirectory, fileName + ".dll");
        if (!File.Exists(str1))
        {
          Log.Warning("No mod found at '" + str1 + "'.");
        }
        else
        {
          string str2 = Path.Combine(enumerateDirectory, "AssetBundles");
          if (!Directory.Exists(str2) || Directory.EnumerateFiles(str2).IsEnumerableEmpty<string>())
          {
            Log.Info("No asset bundles found at '" + str2 + "'");
            str2 = (string) null;
          }
          try
          {
            ModsLoader.loadModFromFileOrThrow(group, fileName, str1, loadAssemblyDependencies, (Option<string>) str2, loadedMods);
          }
          catch (Exception ex)
          {
            Log.Exception(ex, "Failed to load mods from '" + fileName + "'.");
            lyst.AddAssertNew(new ModInfoRaw(fileName, 0, fileName));
            error += string.Format("Failed to load mods from '{0}': {1}.\n", (object) fileName, (object) ex);
          }
        }
      }
      failedMods = lyst.ToImmutableArray();
      return loadedMods.ToImmutableArray();
    }

    private static void onAssemblyLoad(object sender, AssemblyLoadEventArgs args)
    {
      Log.Info("Assembly loaded: " + args.LoadedAssembly.FullName);
    }

    private static void loadModFromFileOrThrow(
      ModGroup group,
      string name,
      string filePath,
      bool loadAssemblyDependencies,
      Option<string> assetBundlesPath,
      Lyst<ModData> loadedMods)
    {
      Log.Info("Loading mods from '" + filePath + "'.");
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      AssemblyName assemblyName = AssemblyName.GetAssemblyName(filePath);
      Assembly assembly = ((IEnumerable<Assembly>) assemblies).FirstOrDefault<Assembly>((Func<Assembly, bool>) (a => AssemblyName.ReferenceMatchesDefinition(a.GetName(), assemblyName)));
      if (loadAssemblyDependencies)
      {
        if ((object) assembly == null)
          assembly = Assembly.LoadFrom(filePath);
      }
      else if ((object) assembly == null)
        assembly = Assembly.Load(File.ReadAllBytes(filePath));
      foreach (Type modType in ((IEnumerable<Type>) assembly.GetExportedTypes()).Where<Type>((Func<Type, bool>) (t => t.IsAssignableTo<IMod>())))
      {
        loadedMods.Add(new ModData(group, name, modType, assetBundlesPath));
        Log.Info(string.Format("Mods type '{0}' registered with ", (object) modType) + (assetBundlesPath.HasValue ? "asset bundles at '" + assetBundlesPath.Value + "'." : "no asset bundles."));
      }
    }

    public ModsLoader()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
