// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Utils.IconProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Products;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Utils
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class IconProvider
  {
    public const string ICON_GO_NAME = "PIcon";
    private static readonly int COUNT_SHADER_ID;
    private static readonly ImmutableArray<string> TEXTURE_NAMES;
    private static readonly ImmutableArray<int> TEXTURE_IDS;
    private readonly AssetsDb m_assetsDb;
    private readonly GameObjectPool m_portIconsGoPool;
    private readonly Dict<MultiIconSpec, Material> m_iconMaterials;
    private readonly Material m_baseMaterialTemplate;

    public IconProvider(AssetsDb assetsDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_iconMaterials = new Dict<MultiIconSpec, Material>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_assetsDb = assetsDb;
      GameObject prefab;
      if (assetsDb.TryGetSharedPrefab("Assets/Core/IconOverlay/ProductMultiIcon.prefab", out prefab))
      {
        this.m_baseMaterialTemplate = prefab.GetComponent<MeshRenderer>().sharedMaterial;
      }
      else
      {
        Log.Error("Missing icon prefab.");
        this.m_baseMaterialTemplate = assetsDb.DefaultMaterial;
      }
      this.m_portIconsGoPool = new GameObjectPool("Port icons", 1024, (Func<GameObject>) (() =>
      {
        GameObject clonedPrefabOrEmptyGo = assetsDb.GetClonedPrefabOrEmptyGo("Assets/Core/IconOverlay/ProductMultiIcon.prefab");
        clonedPrefabOrEmptyGo.name = "PIcon";
        clonedPrefabOrEmptyGo.layer = Layer.Custom13Icons.ToId();
        return clonedPrefabOrEmptyGo;
      }), (Action<GameObject>) (x => { }));
    }

    public GameObject GetIconPooled(IconSpec iconSpec, Transform parent = null, Vector3 relPosFromParent = default (Vector3))
    {
      return this.GetMultiIconPooled(new MultiIconSpec(ImmutableArray.Create<string>(iconSpec.PrefabPath), iconSpec.Color), parent, relPosFromParent);
    }

    public GameObject GetProductIconPooled(
      ProductProto product,
      Transform parent = null,
      Vector3 relPosFromParent = default (Vector3))
    {
      return this.GetIconPooled(new IconSpec(product.Graphics.IconPath), parent, relPosFromParent);
    }

    public GameObject GetMultiIconPooled(
      MultiIconSpec iconSpec,
      Transform parent = null,
      Vector3 relPosFromParent = default (Vector3))
    {
      GameObject instance = this.m_portIconsGoPool.GetInstance();
      Material material;
      if (!this.m_iconMaterials.TryGetValue(iconSpec, out material))
      {
        material = new Material(this.m_baseMaterialTemplate);
        material.color = iconSpec.Color.ToColor();
        for (int index = 0; index < iconSpec.PrefabPaths.Length; ++index)
          material.SetTexture(IconProvider.TEXTURE_IDS[index], (Texture) this.m_assetsDb.GetSharedTexture(iconSpec.PrefabPaths[index]));
        material.SetFloat(IconProvider.COUNT_SHADER_ID, (float) iconSpec.PrefabPaths.Length);
        this.m_iconMaterials.Add(iconSpec, material);
      }
      instance.GetComponent<MeshRenderer>().sharedMaterial = material;
      instance.transform.SetParent(parent, false);
      instance.transform.localPosition = relPosFromParent;
      return instance;
    }

    public GameObject GetProductMultiIconPooled(
      ImmutableArray<ProductProto> protos,
      Transform parent = null,
      Vector3 relPosFromParent = default (Vector3))
    {
      return this.GetMultiIconPooled(new MultiIconSpec(protos.Map<string>((Func<ProductProto, string>) (x => x.Graphics.IconPath))), parent, relPosFromParent);
    }

    public void ReturnIconToPool(ref GameObject iconGo)
    {
      Assert.That<string>(iconGo.name).IsEqualTo<string>("PIcon");
      this.m_portIconsGoPool.ReturnInstance(ref iconGo);
    }

    static IconProvider()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      IconProvider.COUNT_SHADER_ID = Shader.PropertyToID("_Count");
      IconProvider.TEXTURE_NAMES = ImmutableArray.Create<string>("_MainTex", "_MainTex2", "_MainTex3", "_MainTex4");
      IconProvider.TEXTURE_IDS = IconProvider.TEXTURE_NAMES.Map<int>(new Func<string, int>(Shader.PropertyToID));
    }
  }
}
