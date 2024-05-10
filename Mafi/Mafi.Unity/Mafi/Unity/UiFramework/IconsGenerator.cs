// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UiFramework.IconsGenerator
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Transports;
using Mafi.Core.Prototypes;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.Factory.Transports;
using Mafi.Unity.Ports.Io;
using Mafi.Unity.Vehicles;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UiFramework
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  internal class IconsGenerator
  {
    private static readonly int OFFSET_PERC_SHADER_ID;
    private static readonly Color LIGHT_COLOR;
    private static readonly AngleDegrees1f LIGHT_PITCH;
    private static readonly AngleDegrees1f LIGHT_ROLL;
    private readonly ProtosDb m_protosDb;
    private readonly AssetsDb m_assetsDb;
    private readonly ProtoModelFactory m_modelFactory;
    private readonly TransportModelFactory m_transportFactory;
    private readonly IoPortModelFactory m_portModelFactory;
    private readonly TruckAttachmentFactory m_truckAttachmentFactory;
    private readonly CameraController m_cameraController;
    private readonly ColorizableMaterialsCache m_colorizableMaterialsCache;
    private readonly Material m_outlineBgMaterial;
    private GameObjectRenderer m_goRenderer;

    public IconsGenerator(
      ProtosDb protosDb,
      AssetsDb assetsDb,
      ProtoModelFactory modelFactory,
      TransportModelFactory transportFactory,
      IoPortModelFactory portModelFactory,
      TruckAttachmentFactory truckAttachmentFactory,
      CameraController cameraController,
      ColorizableMaterialsCache colorizableMaterialsCache)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_protosDb = protosDb.CheckNotNull<ProtosDb>();
      this.m_assetsDb = assetsDb.CheckNotNull<AssetsDb>();
      this.m_modelFactory = modelFactory.CheckNotNull<ProtoModelFactory>();
      this.m_transportFactory = transportFactory.CheckNotNull<TransportModelFactory>();
      this.m_portModelFactory = portModelFactory.CheckNotNull<IoPortModelFactory>();
      this.m_truckAttachmentFactory = truckAttachmentFactory;
      this.m_cameraController = cameraController;
      this.m_colorizableMaterialsCache = colorizableMaterialsCache;
      this.m_outlineBgMaterial = assetsDb.GetClonedMaterial("Assets/Core/IconOverlay/IconOutline.mat");
    }

    public bool GenerateAll { get; set; }

    public Option<string> NameSubstr { get; set; }

    public bool DisableOutline { get; set; }

    public void Initialize(GameObject rootGo)
    {
      this.m_goRenderer = new GameObjectRenderer(rootGo, 20f, this.m_cameraController.Camera);
    }

    private bool iconExists(string path)
    {
      return !this.GenerateAll && this.m_assetsDb.ContainsAsset(path);
    }

    private bool shouldSkip(string id)
    {
      return this.NameSubstr.HasValue && !id.ToLowerInvariant().Contains(this.NameSubstr.Value);
    }

    public IEnumerable<string> GenerateLayoutEntities(
      string outDirPath,
      AngleDegrees1f cameraPitch,
      AngleDegrees1f cameraYaw,
      AngleDegrees1f cameraFov,
      int size = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<string>) new IconsGenerator.\u003CGenerateLayoutEntities\u003Ed__35(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__outDirPath = outDirPath,
        \u003C\u003E3__cameraPitch = cameraPitch,
        \u003C\u003E3__cameraYaw = cameraYaw,
        \u003C\u003E3__cameraFov = cameraFov,
        \u003C\u003E3__size = size
      };
    }

    public IEnumerable<string> GenerateTransports(
      string outDirPath,
      AngleDegrees1f cameraPitch,
      AngleDegrees1f cameraYaw,
      AngleDegrees1f cameraFov)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<string>) new IconsGenerator.\u003CGenerateTransports\u003Ed__36(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__outDirPath = outDirPath,
        \u003C\u003E3__cameraPitch = cameraPitch,
        \u003C\u003E3__cameraYaw = cameraYaw,
        \u003C\u003E3__cameraFov = cameraFov
      };
    }

    private GameObject createTransportPreview(TransportProto proto)
    {
      GameObject transportPreview = new GameObject("Transport preview");
      ImmutableArray<Tile3i> pivots = ImmutableArray.Create<Tile3i>(new Tile3i(-2, -1, 0), new Tile3i(-2, 0, 0), new Tile3i(0, 0, 0));
      string error;
      this.m_transportFactory.CreateModel(proto, pivots, out error, noFlowIndicators: true).ValueOrThrow(error).transform.SetParent(transportPreview.transform, false);
      GameObject modelFor1 = this.m_portModelFactory.CreateModelFor(proto.PortsShape, true);
      modelFor1.transform.SetParent(transportPreview.transform, false);
      IoPortMb.SetTransform(modelFor1, pivots.Last, Direction90.PlusX);
      GameObject modelFor2 = this.m_portModelFactory.CreateModelFor(proto.PortsShape, true);
      modelFor2.transform.SetParent(transportPreview.transform, false);
      IoPortMb.SetTransform(modelFor2, pivots.First, Direction90.MinusY);
      return transportPreview;
    }

    public IEnumerable<string> GenerateProducts(
      string outDirPath,
      AngleDegrees1f cameraPitch,
      AngleDegrees1f cameraYaw,
      AngleDegrees1f cameraFov)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<string>) new IconsGenerator.\u003CGenerateProducts\u003Ed__38(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__outDirPath = outDirPath,
        \u003C\u003E3__cameraPitch = cameraPitch,
        \u003C\u003E3__cameraYaw = cameraYaw,
        \u003C\u003E3__cameraFov = cameraFov
      };
    }

    public IEnumerable<string> GenerateVehicles(
      string outDirPath,
      AngleDegrees1f cameraPitch,
      AngleDegrees1f cameraYaw,
      AngleDegrees1f cameraFov)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<string>) new IconsGenerator.\u003CGenerateVehicles\u003Ed__39(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__outDirPath = outDirPath,
        \u003C\u003E3__cameraPitch = cameraPitch,
        \u003C\u003E3__cameraYaw = cameraYaw,
        \u003C\u003E3__cameraFov = cameraFov
      };
    }

    public IEnumerable<string> GenerateTrees(
      string outDirPath,
      AngleDegrees1f cameraPitch,
      AngleDegrees1f cameraYaw,
      AngleDegrees1f cameraFov,
      int size = 0)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<string>) new IconsGenerator.\u003CGenerateTrees\u003Ed__40(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__outDirPath = outDirPath,
        \u003C\u003E3__cameraPitch = cameraPitch,
        \u003C\u003E3__cameraYaw = cameraYaw,
        \u003C\u003E3__cameraFov = cameraFov,
        \u003C\u003E3__size = size
      };
    }

    private IEnumerable<int> renderOutlinedModel(GameObject go, string path)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerable<int>) new IconsGenerator.\u003CrenderOutlinedModel\u003Ed__41(-2)
      {
        \u003C\u003E4__this = this,
        \u003C\u003E3__go = go,
        \u003C\u003E3__path = path
      };
    }

    static IconsGenerator()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      IconsGenerator.OFFSET_PERC_SHADER_ID = Shader.PropertyToID("_OffsetPercent");
      IconsGenerator.LIGHT_COLOR = Color.white;
      IconsGenerator.LIGHT_PITCH = 40.Degrees();
      IconsGenerator.LIGHT_ROLL = 110.Degrees();
    }
  }
}
