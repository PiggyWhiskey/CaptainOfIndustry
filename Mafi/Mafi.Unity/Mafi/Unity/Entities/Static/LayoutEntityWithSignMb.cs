// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.LayoutEntityWithSignMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Products;
using Mafi.Unity.InstancedRendering;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class LayoutEntityWithSignMb : StaticEntityMb
  {
    protected static readonly int ICON_SCALE_SHADER_ID;
    protected static readonly int ICON_TEX_SHADER_ID;
    protected Mafi.Core.Entities.Static.Layout.LayoutEntity m_entity;
    protected Option<MeshRenderer> m_signMeshRenderer;
    protected Option<ProductProto> m_productToRender;
    protected AssetsDb m_assetsDb;
    protected string m_signChildName;
    private Option<Material> m_originalSignMaterial;
    private static Option<Material> s_signBlueprintMaterial;

    protected void Initialize(AssetsDb assetsDb, Mafi.Core.Entities.Static.Layout.LayoutEntity entity)
    {
      this.Initialize((ILayoutEntity) entity);
      this.m_productToRender = Option<ProductProto>.None;
      this.m_signMeshRenderer = Option<MeshRenderer>.None;
      this.m_assetsDb = assetsDb;
      this.m_entity = entity;
      if (!LayoutEntityWithSignMb.s_signBlueprintMaterial.IsNone)
        return;
      LayoutEntityWithSignMb.s_signBlueprintMaterial = (Option<Material>) assetsDb.GetSharedMaterial("Assets/Core/Materials/BuildingSignBlueprint.mat");
    }

    protected void initializeSign(float iconScale)
    {
      Transform resultTransform;
      if (!this.transform.TryFindChild(this.m_signChildName, out resultTransform))
      {
        Log.WarningOnce(string.Format("Failed to get sign child for entity {0}.", (object) this.m_entity.Prototype));
      }
      else
      {
        MeshRenderer component;
        if (!resultTransform.gameObject.TryGetComponent<MeshRenderer>(out component))
          Log.WarningOnce(string.Format("Game object sign for '{0}' is missing MeshRenderer.", (object) this.m_entity.Prototype));
        else if (!(bool) (Object) component.sharedMaterial)
        {
          Log.WarningOnce(string.Format("Material of sign for '{0}' is missing.", (object) this.m_entity.Prototype));
        }
        else
        {
          if (this.m_entity.Transform.IsReflected)
            iconScale = -iconScale;
          component.sharedMaterial = Object.Instantiate<Material>(component.sharedMaterial);
          component.sharedMaterial.SetFloat(LayoutEntityWithSignMb.ICON_SCALE_SHADER_ID, iconScale);
          this.m_signMeshRenderer = (Option<MeshRenderer>) component;
          this.updateSign();
        }
      }
    }

    protected void updateSign()
    {
      if (this.m_signMeshRenderer.IsNone)
        return;
      if (this.m_productToRender.HasValue)
        this.m_signMeshRenderer.Value.sharedMaterial.SetTexture(LayoutEntityWithSignMb.ICON_TEX_SHADER_ID, (Texture) this.m_assetsDb.GetSharedTexture(this.m_productToRender.Value.IconPath));
      else
        this.m_signMeshRenderer.Value.sharedMaterial.SetTexture(LayoutEntityWithSignMb.ICON_TEX_SHADER_ID, (Texture) null);
    }

    public override void EnsureBlueprintMaterial(Material material, ColorRgba color)
    {
      if (this.m_signMeshRenderer.HasValue && this.m_originalSignMaterial.IsNone)
        this.m_originalSignMaterial = (Option<Material>) Object.Instantiate<Material>(this.m_signMeshRenderer.Value.sharedMaterial);
      base.EnsureBlueprintMaterial(material, color);
      this.ensureSignBlueprintMaterial(color);
    }

    private void ensureSignBlueprintMaterial(ColorRgba color)
    {
      if (this.m_signMeshRenderer.HasValue && this.m_originalSignMaterial.IsNone)
        this.m_originalSignMaterial = (Option<Material>) Object.Instantiate<Material>(this.m_signMeshRenderer.Value.sharedMaterial);
      if (this.m_originalSignMaterial.IsNone)
        return;
      this.m_signMeshRenderer.Value.sharedMaterial = InstancingUtils.InstantiateMaterialAndCopyTextures(LayoutEntityWithSignMb.s_signBlueprintMaterial.Value, this.m_signMeshRenderer.Value.sharedMaterial, true);
      this.m_signMeshRenderer.Value.sharedMaterial.SetColor(StaticEntityMb.TINT_COLOR_SHADER_ID, color.ToColor());
      this.m_signMeshRenderer.Value.sharedMaterial.SetFloat(LayoutEntityWithSignMb.ICON_SCALE_SHADER_ID, this.m_originalSignMaterial.Value.GetFloat(LayoutEntityWithSignMb.ICON_SCALE_SHADER_ID));
      this.updateSign();
    }

    public override void EnsureDefaultMaterial()
    {
      base.EnsureDefaultMaterial();
      if (this.m_originalSignMaterial.HasValue)
        this.m_signMeshRenderer.Value.sharedMaterial = this.m_originalSignMaterial.Value;
      this.updateSign();
    }

    public LayoutEntityWithSignMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static LayoutEntityWithSignMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      LayoutEntityWithSignMb.ICON_SCALE_SHADER_ID = Shader.PropertyToID("_IconScale");
      LayoutEntityWithSignMb.ICON_TEX_SHADER_ID = Shader.PropertyToID("_IconTex");
    }
  }
}
