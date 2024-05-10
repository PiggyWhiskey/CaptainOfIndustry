// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.StaticEntityMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Unity.InstancedRendering;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  public class StaticEntityMb : EntityMb
  {
    protected static readonly int TINT_COLOR_SHADER_ID;
    private Option<KeyValuePair<Renderer, Material>[]> m_originalMaterials;

    public void Initialize(ILayoutEntity layoutEntity)
    {
      this.Initialize((IStaticEntity) layoutEntity, StaticEntityMb.GetTransformData(layoutEntity));
    }

    protected void Initialize(IStaticEntity staticEntity, StaticEntityTransform entityTransform)
    {
      if (this.transform.position != Vector3.zero)
      {
        Assert.Fail(string.Format("MB of {0} is not at zero.", (object) staticEntity.Prototype.Id));
        this.transform.position = Vector3.zero;
      }
      entityTransform.Apply(this.gameObject.transform);
      this.Initialize((IEntity) staticEntity);
    }

    public static StaticEntityTransform GetTransformData(ILayoutEntity entity)
    {
      return StaticEntityMb.GetTransformData(entity.Transform, entity.Prototype.Graphics.PrefabOrigin, entity.Prototype.Layout);
    }

    public static StaticEntityTransform GetTransformData(
      TileTransform entityTransform,
      RelTile3f prefabOrigin,
      EntityLayout layout)
    {
      return new StaticEntityTransform((layout.GetModelOrigin(entityTransform) - new RelTile3f(entityTransform.TransformMatrix.Transform(prefabOrigin.Xy.Vector2f).ExtendZ(prefabOrigin.Z))).ToVector3(), entityTransform.Rotation.Quaternion.ToUnityQuaternion(), entityTransform.IsReflected ? new Vector3(-1f, 1f, 1f) : Vector3.one);
    }

    public virtual void EnsureBlueprintMaterial(Material material, ColorRgba color)
    {
      if (!(bool) (UnityEngine.Object) material)
      {
        Log.Error("Trying to set invalid material.");
      }
      else
      {
        if (this.m_originalMaterials.IsNone)
          this.m_originalMaterials = (Option<KeyValuePair<Renderer, Material>[]>) ((IEnumerable<Renderer>) this.gameObject.GetComponentsInChildren<Renderer>()).Where<Renderer>((Func<Renderer, bool>) (x => !x.gameObject.HasTag(UnityTag.NoHi))).Where<Renderer>((Func<Renderer, bool>) (x => x.gameObject.layer == Layer.Unity00Default.ToId())).Select<Renderer, KeyValuePair<Renderer, Material>>((Func<Renderer, KeyValuePair<Renderer, Material>>) (r => Make.Kvp<Renderer, Material>(r, r.sharedMaterial))).ToArray<KeyValuePair<Renderer, Material>>();
        Lyst<KeyValuePair<Material, Material>> list = new Lyst<KeyValuePair<Material, Material>>();
        foreach (KeyValuePair<Renderer, Material> keyValuePair in this.m_originalMaterials.Value)
        {
          if ((bool) (UnityEngine.Object) keyValuePair.Key)
          {
            Material material1;
            if (!list.TryGetValue<Material, Material>(keyValuePair.Key.sharedMaterial, out material1))
            {
              material1 = InstancingUtils.InstantiateMaterialAndCopyTextures(material, keyValuePair.Key.sharedMaterial, true);
              material1.SetColor(StaticEntityMb.TINT_COLOR_SHADER_ID, color.ToColor());
              list.Add<Material, Material>(keyValuePair.Key.sharedMaterial, material1);
            }
            keyValuePair.Key.sharedMaterial = material1;
          }
        }
      }
    }

    public virtual void EnsureDefaultMaterial()
    {
      if (this.m_originalMaterials.IsNone)
        return;
      foreach (KeyValuePair<Renderer, Material> keyValuePair in this.m_originalMaterials.Value)
      {
        if ((bool) (UnityEngine.Object) keyValuePair.Key)
          keyValuePair.Key.sharedMaterial = keyValuePair.Value;
      }
      this.m_originalMaterials = Option<KeyValuePair<Renderer, Material>[]>.None;
    }

    public StaticEntityMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static StaticEntityMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      StaticEntityMb.TINT_COLOR_SHADER_ID = Shader.PropertyToID("_TintColor");
    }
  }
}
