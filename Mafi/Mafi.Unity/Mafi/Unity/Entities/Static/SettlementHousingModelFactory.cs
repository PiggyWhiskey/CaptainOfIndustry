// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.SettlementHousingModelFactory
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Base;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Settlements;
using Mafi.Core.Entities.Static.Layout;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class SettlementHousingModelFactory : 
    IProtoModelFactory<SettlementHousingModuleProto>,
    IFactory<SettlementHousingModuleProto, GameObject>
  {
    private readonly LayoutEntityModelFactory m_layoutEntityModelFactory;
    private readonly AssetsDb m_assetsDb;
    private readonly IRandom m_random;
    private readonly Dict<Material, Lyst<GameObject>> m_housePartsTmp;

    public SettlementHousingModelFactory(
      LayoutEntityModelFactory layoutEntityModelFactory,
      AssetsDb assetsDb,
      RandomProvider randomProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_housePartsTmp = new Dict<Material, Lyst<GameObject>>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_layoutEntityModelFactory = layoutEntityModelFactory;
      this.m_assetsDb = assetsDb;
      this.m_random = randomProvider.GetNonSimRandomFor((object) this);
    }

    public GameObject Create(SettlementHousingModuleProto proto)
    {
      GameObject gameObject = this.m_layoutEntityModelFactory.Create((ILayoutEntityProto) proto);
      if (proto.Id == Ids.Buildings.Housing)
        this.colorT1Houses(gameObject, proto);
      else if (proto.Id == Ids.Buildings.HousingT2)
        this.colorT2Houses(gameObject, proto);
      else if (proto.Id == Ids.Buildings.HousingT3)
        this.colorT3Houses(gameObject, proto);
      gameObject.CombineMeshesWithSameMaterialRecursively(out int _, out int _, out int _);
      return gameObject;
    }

    private void colorT1Houses(GameObject go, SettlementHousingModuleProto proto)
    {
      ImmutableArray<Material> immutableArray = proto.Graphics.MaterialPaths.Map<Material>(new Func<string, Material>(this.m_assetsDb.GetSharedMaterial));
      Transform transform = go.transform;
      int index1 = 0;
      for (int childCount1 = transform.childCount; index1 < childCount1; ++index1)
      {
        Transform child1 = transform.GetChild(index1);
        int index2 = 0;
        for (int childCount2 = child1.childCount; index2 < childCount2; ++index2)
        {
          Transform child2 = child1.GetChild(index2);
          if (child2.gameObject.name.StartsWith("Container", StringComparison.Ordinal))
          {
            Material material = immutableArray[this.m_random];
            int index3 = 0;
            for (int childCount3 = child2.childCount; index3 < childCount3; ++index3)
            {
              GameObject gameObject = child2.GetChild(index3).gameObject;
              if (gameObject.name.StartsWith("contcolor", StringComparison.Ordinal))
                gameObject.SetSharedMaterialRecursively(material);
            }
          }
        }
      }
    }

    private void colorT2Houses(GameObject go, SettlementHousingModuleProto proto)
    {
      ImmutableArray<Material> immutableArray = proto.Graphics.MaterialPaths.Map<Material>(new Func<string, Material>(this.m_assetsDb.GetSharedMaterial));
      Transform transform = go.transform;
      int index1 = 0;
      for (int childCount1 = transform.childCount; index1 < childCount1; ++index1)
      {
        Transform child1 = transform.GetChild(index1);
        if (child1.childCount != 0 && !child1.gameObject.name.EndsWith("NoRand"))
        {
          this.m_housePartsTmp.Clear();
          int index2 = 0;
          for (int childCount2 = child1.childCount; index2 < childCount2; ++index2)
          {
            Transform child2 = child1.GetChild(index2);
            if (!child2.gameObject.name.EndsWith("NoRand"))
            {
              MeshRenderer component = child2.gameObject.GetComponent<MeshRenderer>();
              if ((bool) (UnityEngine.Object) component && !component.sharedMaterial.name.EndsWith("NoRand"))
              {
                Lyst<GameObject> lyst;
                if (this.m_housePartsTmp.TryGetValue(component.sharedMaterial, out lyst))
                {
                  lyst.Add(child2.gameObject);
                }
                else
                {
                  lyst = new Lyst<GameObject>()
                  {
                    child2.gameObject
                  };
                  this.m_housePartsTmp.Add(component.sharedMaterial, lyst);
                }
              }
            }
          }
          foreach (KeyValuePair<Material, Lyst<GameObject>> keyValuePair in this.m_housePartsTmp)
          {
            Material material = immutableArray[this.m_random];
            foreach (GameObject go1 in keyValuePair.Value)
              go1.SetSharedMaterialRecursively(material);
          }
        }
      }
      this.m_housePartsTmp.Clear();
    }

    private void colorT3Houses(GameObject go, SettlementHousingModuleProto proto)
    {
      if (proto.Graphics.MaterialPaths.Length <= 2 || proto.Graphics.MaterialPaths.Length % 2 != 0)
      {
        Log.Error(string.Format("Invalid materials count for settlement '{0}', it must be multiple of 2 and >= 4.", (object) proto.Id));
      }
      else
      {
        ImmutableArray<Material> immutableArray = proto.Graphics.MaterialPaths.Map<Material>(new Func<string, Material>(this.m_assetsDb.GetSharedMaterial));
        int maxValueExcl = immutableArray.Length / 2 - 1;
        Material first = immutableArray.First;
        Material second = immutableArray.Second;
        Lyst<MeshRenderer> lyst1 = new Lyst<MeshRenderer>();
        Lyst<MeshRenderer> lyst2 = new Lyst<MeshRenderer>();
        Transform transform = go.transform;
        int index1 = 0;
        for (int childCount1 = transform.childCount; index1 < childCount1; ++index1)
        {
          Transform child1 = transform.GetChild(index1);
          if (child1.childCount != 0 && !child1.gameObject.name.EndsWith("NoRand"))
          {
            int index2 = 0;
            for (int childCount2 = child1.childCount; index2 < childCount2; ++index2)
            {
              Transform child2 = child1.GetChild(index2);
              if (!child2.gameObject.name.EndsWith("NoRand"))
              {
                MeshRenderer component = child2.gameObject.GetComponent<MeshRenderer>();
                if ((bool) (UnityEngine.Object) component)
                {
                  Material sharedMaterial = component.sharedMaterial;
                  if ((UnityEngine.Object) sharedMaterial == (UnityEngine.Object) first)
                    lyst1.Add(component);
                  else if ((UnityEngine.Object) sharedMaterial == (UnityEngine.Object) second)
                    lyst2.Add(component);
                }
              }
            }
            int num = this.m_random.NextInt(maxValueExcl);
            Material a = immutableArray[num * 2 + 2];
            Material b = immutableArray[num * 2 + 3];
            if (this.m_random.TestProbability(Percent.Fifty))
              Swap.Them<Material>(ref a, ref b);
            foreach (Renderer renderer in lyst1)
              renderer.sharedMaterial = a;
            lyst1.Clear();
            foreach (Renderer renderer in lyst2)
              renderer.sharedMaterial = b;
            lyst2.Clear();
          }
        }
      }
    }
  }
}
