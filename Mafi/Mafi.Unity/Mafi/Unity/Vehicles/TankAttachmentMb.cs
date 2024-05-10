// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.TankAttachmentMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Products;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.Entities.Dynamic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  internal class TankAttachmentMb : AttachmentMb
  {
    protected static readonly int ICON_SCALE_SHADER_ID;
    protected static readonly int ICON_TEX_SHADER_ID;
    protected static readonly int COLOR_SHADER_ID;
    protected static readonly int ACCENT_COLOR_SHADER_ID;
    protected static readonly float ICON_SCALE;
    private Truck m_truck;
    private Option<ProductProto> m_product;
    private AssetsDb m_assetsDb;
    private TankAttachmentProto m_proto;
    private Option<MeshRenderer> m_iconMeshRenderer;
    private Option<MeshRenderer> m_bodyMeshRenderer;

    public void Initialize(TankAttachmentProto proto, AssetsDb assetsDb)
    {
      this.m_proto = proto;
      this.m_assetsDb = assetsDb;
      this.Initialize((AttachmentProto) proto);
    }

    private void initializeIcon()
    {
      Transform resultTransform;
      if (!this.transform.TryFindChild(this.m_proto.Graphics.IconChildName, out resultTransform))
      {
        Log.WarningOnce(string.Format("Failed to get icon child for attachment {0}.", (object) this.m_truck.Prototype));
      }
      else
      {
        MeshRenderer component;
        if (!resultTransform.gameObject.TryGetComponent<MeshRenderer>(out component))
          Log.WarningOnce(string.Format("Game object icon for '{0}' is missing MeshRenderer.", (object) this.m_truck.Prototype));
        else if (!(bool) (Object) component.sharedMaterial)
        {
          Log.WarningOnce(string.Format("Material of icon for '{0}' is missing.", (object) this.m_truck.Prototype));
        }
        else
        {
          component.sharedMaterial = Object.Instantiate<Material>(component.sharedMaterial);
          component.sharedMaterial.SetFloat(TankAttachmentMb.ICON_SCALE_SHADER_ID, TankAttachmentMb.ICON_SCALE);
          this.m_iconMeshRenderer = (Option<MeshRenderer>) component;
          this.updateIcon();
        }
      }
    }

    private void initializeBody()
    {
      Transform resultTransform;
      if (!this.transform.TryFindChild(this.m_proto.Graphics.BodyChildName, out resultTransform))
      {
        Log.WarningOnce(string.Format("Failed to get body child for attachment {0}.", (object) this.m_truck.Prototype));
      }
      else
      {
        MeshRenderer component;
        if (!resultTransform.gameObject.TryGetComponent<MeshRenderer>(out component))
          Log.WarningOnce(string.Format("Child '{0}' '{1}' is missing MeshRenderer.", (object) this.m_proto.Graphics.BodyChildName, (object) this.m_truck.Prototype));
        else if (!(bool) (Object) component.sharedMaterial)
        {
          Log.WarningOnce(string.Format("Material of body for '{0}' is missing.", (object) this.m_truck.Prototype));
        }
        else
        {
          component.sharedMaterial = Object.Instantiate<Material>(component.sharedMaterial);
          this.m_bodyMeshRenderer = (Option<MeshRenderer>) component;
          this.updateColors();
        }
      }
    }

    private void updateIcon()
    {
      if (this.m_iconMeshRenderer.IsNone)
        return;
      if (this.m_product.HasValue && this.m_product.Value.IsNotPhantom)
        this.m_iconMeshRenderer.Value.sharedMaterial.SetTexture(TankAttachmentMb.ICON_TEX_SHADER_ID, (Texture) this.m_assetsDb.GetSharedTexture(this.m_product.Value.IconPath));
      else
        this.m_iconMeshRenderer.Value.sharedMaterial.SetTexture(TankAttachmentMb.ICON_TEX_SHADER_ID, (Texture) null);
    }

    private void updateColors()
    {
      if (this.m_bodyMeshRenderer.IsNone || this.m_product.IsNone)
        return;
      this.m_bodyMeshRenderer.Value.sharedMaterial.SetColor(TankAttachmentMb.COLOR_SHADER_ID, this.m_product.Value.Graphics.TransportColor.ToColor());
      this.m_bodyMeshRenderer.Value.sharedMaterial.SetColor(TankAttachmentMb.ACCENT_COLOR_SHADER_ID, this.m_product.Value.Graphics.TransportAccentColor.ToColor());
    }

    public override void SyncUpdate(DynamicGroundEntityMb parent)
    {
      if (this.m_truck.Cargo.TotalQuantity.IsZero)
      {
        if (this.m_iconMeshRenderer.HasValue)
          this.m_iconMeshRenderer.Value.sharedMaterial.SetTexture(TankAttachmentMb.ICON_TEX_SHADER_ID, (Texture) null);
        if (this.m_bodyMeshRenderer.HasValue)
        {
          this.m_bodyMeshRenderer.Value.sharedMaterial.SetColor(TankAttachmentMb.COLOR_SHADER_ID, this.m_proto.Graphics.DefaultColor.ToColor());
          this.m_bodyMeshRenderer.Value.sharedMaterial.SetColor(TankAttachmentMb.ACCENT_COLOR_SHADER_ID, this.m_proto.Graphics.DefaultAccentColor.ToColor());
        }
        this.m_product = (Option<ProductProto>) this.m_truck.Cargo.FirstOrPhantom.Product;
      }
      else
      {
        ProductProto product = this.m_truck.Cargo.FirstOrPhantom.Product;
        if (product == this.m_product)
          return;
        if (this.m_iconMeshRenderer.IsNone)
          this.initializeIcon();
        if (this.m_bodyMeshRenderer.IsNone)
          this.initializeBody();
        this.m_product = (Option<ProductProto>) product;
        this.updateIcon();
        this.updateColors();
      }
    }

    public override void SetTruck(Truck truck) => this.m_truck = truck.CheckNotNull<Truck>();

    public override void Reset()
    {
      base.Reset();
      this.m_truck = (Truck) null;
    }

    public TankAttachmentMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static TankAttachmentMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      TankAttachmentMb.ICON_SCALE_SHADER_ID = Shader.PropertyToID("_IconScale");
      TankAttachmentMb.ICON_TEX_SHADER_ID = Shader.PropertyToID("_IconTex");
      TankAttachmentMb.COLOR_SHADER_ID = Shader.PropertyToID("_Color");
      TankAttachmentMb.ACCENT_COLOR_SHADER_ID = Shader.PropertyToID("_AccentColor");
      TankAttachmentMb.ICON_SCALE = 0.6f;
    }
  }
}
