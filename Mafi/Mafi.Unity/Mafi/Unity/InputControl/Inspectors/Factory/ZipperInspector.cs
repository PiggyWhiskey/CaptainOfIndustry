// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.Factory.ZipperInspector
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Factory.Zippers;
using Mafi.Core.Ports.Io;
using Mafi.Core.Prototypes;
using Mafi.Unity.Camera;
using System;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors.Factory
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  internal class ZipperInspector : EntityInspector<Zipper, ZipperWindowView>, IDisposable
  {
    public const int RENDER_TEXTURE_SIZE = 448;
    private static readonly Vector2 MAX_POS;
    public readonly int MaxPortsCount;
    private readonly CameraController m_cameraController;

    public UnityEngine.Camera ZipperViewCamera { get; }

    public RenderTexture ZipperViewRenderTexture { get; }

    public ZipperInspector(
      InspectorContext inspectorContext,
      CameraController cameraController,
      ProtosDb protosDb)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector(inspectorContext);
      this.m_cameraController = cameraController;
      this.MaxPortsCount = protosDb.All<ZipperProto>().Max<ZipperProto>((Func<ZipperProto, int>) (x => x.Ports.Length));
      this.ZipperViewRenderTexture = new RenderTexture(448, 448, 32);
      this.ZipperViewCamera = new GameObject("ZipperCamera").AddComponent<UnityEngine.Camera>();
      this.ZipperViewCamera.enabled = false;
      this.ZipperViewCamera.clearFlags = CameraClearFlags.Color;
      this.ZipperViewCamera.backgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
      this.ZipperViewCamera.fieldOfView = 10f;
      this.ZipperViewCamera.nearClipPlane = 45f;
      this.ZipperViewCamera.farClipPlane = 65f;
      this.ZipperViewCamera.cullingMask = Layer.Unity00Default.ToMask();
      this.ZipperViewCamera.targetTexture = this.ZipperViewRenderTexture;
      this.ZipperViewCamera.useOcclusionCulling = false;
    }

    public void Dispose() => this.ZipperViewRenderTexture.DestroyIfNotNull();

    protected override ZipperWindowView GetView() => new ZipperWindowView(this);

    protected override void OnActivated()
    {
      Assert.That<int>(this.WindowView.PortNumberPanels.Count).IsEqualTo(this.MaxPortsCount);
      base.OnActivated();
      int length = this.SelectedEntity.Ports.Length;
      for (int index = 0; index < this.MaxPortsCount; ++index)
        this.WindowView.PortNumberPanels[index].GameObject.SetActive(index < length);
      this.ZipperViewCamera.enabled = true;
    }

    protected override void OnDeactivated()
    {
      base.OnDeactivated();
      this.ZipperViewCamera.enabled = false;
    }

    public override void RenderUpdate(GameTime gameTime)
    {
      base.RenderUpdate(gameTime);
      Zipper selectedEntity = this.SelectedEntity;
      if (!this.ZipperViewCamera.enabled || selectedEntity == null)
        return;
      Vector3 vector3 = selectedEntity.Position3f.ToVector3();
      this.ZipperViewCamera.transform.localRotation = this.m_cameraController.CameraModel.Rotation;
      this.ZipperViewCamera.transform.localPosition = vector3 - this.ZipperViewCamera.transform.forward * 55f;
      int index = 0;
      ImmutableArray<IoPort> ports = this.SelectedEntity.Ports;
      for (int length = ports.Length; index < length; ++index)
      {
        ports = this.SelectedEntity.Ports;
        Vector3 screenPoint = this.ZipperViewCamera.WorldToScreenPoint(ports[index].ExpectedConnectedPortCoord.ToCenterVector3());
        this.WindowView.PortNumberPanels[index].RectTransform.anchoredPosition = Vector2.Max(Vector2.zero, Vector2.Min((Vector2) screenPoint, ZipperInspector.MAX_POS));
      }
    }

    static ZipperInspector()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      ZipperInspector.MAX_POS = new Vector2(448f, 448f);
    }
  }
}
