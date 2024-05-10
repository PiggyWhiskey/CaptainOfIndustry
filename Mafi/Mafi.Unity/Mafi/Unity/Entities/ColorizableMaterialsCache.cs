// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.ColorizableMaterialsCache
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities
{
  /// <summary>
  /// Utility for setting colors to entities with colorizable shaders. Colored materials are cached.
  /// </summary>
  /// <remarks>
  /// Should only be used to color GameObjects which were not previously colored and their materials have not been
  /// changed in any other way.
  /// </remarks>
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class ColorizableMaterialsCache
  {
    private readonly Dict<Pair<Material, Color>, Material> m_coloredMaterialsCache;
    private readonly List<Renderer> m_renderersBuffer;

    public void SetColorOfAllColorizableMaterials(GameObject entityGo, Color c)
    {
      Assert.That<List<Renderer>>(this.m_renderersBuffer).IsEmpty<Renderer>();
      entityGo.GetComponentsInChildren<Renderer>(this.m_renderersBuffer);
      foreach (Renderer renderer in this.m_renderersBuffer)
      {
        Material sharedMaterial = renderer.sharedMaterial;
        if ((Object) sharedMaterial == (Object) null || (Object) sharedMaterial.shader == (Object) null)
          Log.Error("Renderer on subobject " + renderer.gameObject.name + " of " + entityGo.name + " does not have a material.");
        else if (sharedMaterial.shader.name == "Mafi/Colorizable")
          renderer.sharedMaterial = this.getSharedColoredMaterial(sharedMaterial, c);
      }
      this.m_renderersBuffer.Clear();
    }

    private Material getSharedColoredMaterial(Material m, Color c)
    {
      Pair<Material, Color> key = Pair.Create<Material, Color>(m, c);
      Material sharedColoredMaterial1;
      if (this.m_coloredMaterialsCache.TryGetValue(key, out sharedColoredMaterial1))
        return sharedColoredMaterial1;
      Material sharedColoredMaterial2 = new Material(m);
      sharedColoredMaterial2.color = c;
      this.m_coloredMaterialsCache[key] = sharedColoredMaterial2;
      return sharedColoredMaterial2;
    }

    public ColorizableMaterialsCache()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_coloredMaterialsCache = new Dict<Pair<Material, Color>, Material>();
      this.m_renderersBuffer = new List<Renderer>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
