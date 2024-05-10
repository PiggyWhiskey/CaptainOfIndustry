// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.FarmMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Buildings.Farms;
using Mafi.Core.Entities.Static;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.GameLoop;
using Mafi.Core.Utils;
using Mafi.Utils;
using System;
using UnityEngine;
using UnityEngine.Rendering;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  public class FarmMb : 
    StaticEntityMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb,
    IEntityMbWithSyncUpdate
  {
    private static readonly Fix32 POSITION_RANDOMNESS_TILES;
    private static readonly int GROWTH_ID;
    private static readonly int HEIGHT_SAG_NORMAL_ID;
    private static readonly int PROPERTIES_ID;
    private static readonly int WIND_PARAMS_ID;
    private static readonly float SPRINKLER_START_DELAY_DURATION_SEC;
    private static readonly float SPRINKLER_STOP_DELAY_DURATION;
    private readonly XorRsr128PlusGenerator m_randomForCrops;
    private Farm m_farm;
    private AssetsDb m_assetsDb;
    private IRandom m_randomForSprinklers;
    public Mesh CropsMeshShared;
    public Material CropsMaterial;
    public int CropsDrawn;
    public Bounds WorldBounds;
    private readonly LoopAnimationHandler m_animHandler;
    private readonly uint[] m_drawArgs;
    private ComputeBuffer m_drawArgsBuffer;
    private FarmMb.DetailInstanceData[] m_detailsData;
    private ComputeBuffer m_detailsDataBuffer;
    private Option<Crop> m_currentCrop;
    private bool m_isNewCrop;
    private float m_growthFrom;
    private float m_growthTo;
    private float m_drynessFrom;
    private float m_drynessTo;
    private ParticleSystem[] m_sprinklerParticleSystems;
    private float[] m_sprinklerAngles;
    private bool m_isSprinkling;
    private bool m_shouldAnimate;
    private int m_nextSprinklerToStartIndex;
    private float m_nextSprinklerDelaySec;
    private int m_newGameSpeedMult;
    private int m_oldGameSpeedMult;

    public void Initialize(
      Farm farm,
      AssetsDb assetsDb,
      RandomProvider randomProvider,
      IGameLoopEvents gameLoopEvents)
    {
      this.Initialize((ILayoutEntity) farm);
      this.m_farm = farm;
      this.m_assetsDb = assetsDb;
      this.m_detailsData = new FarmMb.DetailInstanceData[farm.Prototype.Graphics.CropPositions.Length];
      this.m_detailsDataBuffer = new ComputeBuffer(this.m_detailsData.Length, 24);
      this.m_drawArgsBuffer = new ComputeBuffer(this.m_drawArgs.Length, 4, ComputeBufferType.DrawIndirect);
      this.m_drawArgs[1] = (uint) this.m_detailsData.Length;
      this.WorldBounds = new Bounds(this.m_farm.Position3f.AddZ((Fix32) (this.m_farm.Prototype.Layout.LayoutSize.Z / 2)).ToVector3(), (this.m_farm.Prototype.Layout.LayoutSize.RotatedAroundZ(farm.Transform.Rotation) + RelTile3i.One).ToVector3());
      this.m_randomForSprinklers = randomProvider.GetNonSimRandomFor((object) this);
      this.initializeSprinklers();
      this.m_animHandler.LoadAnimationFor(this.gameObject);
      this.m_animHandler.SetSpeed(0.6f);
      gameLoopEvents.OnProjectChanged.AddNonSaveable<FarmMb>(this, new Action(this.reassignBuffers));
    }

    private void initializeSprinklers()
    {
      Transform transform = this.gameObject.transform;
      Lyst<GameObject> lyst = new Lyst<GameObject>(16);
      for (int index = 0; index < transform.childCount; ++index)
      {
        GameObject gameObject = transform.GetChild(index).gameObject;
        if (gameObject.name.StartsWith("Sprinkler", StringComparison.Ordinal))
          lyst.Add(gameObject);
      }
      if (lyst.IsEmpty)
      {
        if (this.m_farm.Prototype.Graphics.SprinklerPrefabPath.HasValue)
          Log.Error(string.Format("Farm '{0}' has sprinkler prefab but no sprinkler placeholders were found ", (object) this.m_farm.Prototype) + "empty game objects with 'Sprinkler' prefix.");
        this.m_sprinklerParticleSystems = Array.Empty<ParticleSystem>();
      }
      else if (this.m_farm.Prototype.Graphics.SprinklerPrefabPath.IsNone)
      {
        Log.Error(string.Format("Farm '{0}' has sprinkler placeholders but no sprinkler prefab was found.", (object) this.m_farm.Prototype));
        this.m_sprinklerParticleSystems = Array.Empty<ParticleSystem>();
      }
      else
      {
        GameObject sharedPrefabOrEmptyGo = this.m_assetsDb.GetSharedPrefabOrEmptyGo(this.m_farm.Prototype.Graphics.SprinklerPrefabPath.Value);
        this.m_sprinklerParticleSystems = new ParticleSystem[lyst.Count];
        this.m_sprinklerAngles = new float[lyst.Count];
        for (int index = 0; index < lyst.Count; ++index)
        {
          GameObject go = lyst[index];
          GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(sharedPrefabOrEmptyGo);
          gameObject.SetActive(true);
          gameObject.transform.SetParent(go.transform.parent, false);
          gameObject.transform.localPosition = go.transform.localPosition;
          ParticleSystem component = gameObject.GetComponent<ParticleSystem>();
          component.Stop();
          this.m_sprinklerParticleSystems[index] = component;
          go.Destroy();
        }
        if (!this.m_farm.IsIrrigating)
          return;
        this.m_nextSprinklerToStartIndex = this.m_sprinklerParticleSystems.Length;
        float constant = this.m_sprinklerParticleSystems[0].main.startLifetime.constant;
        for (int index = 0; index < this.m_sprinklerParticleSystems.Length; ++index)
        {
          this.startSprinklerAt(index, 1f);
          ParticleSystem sprinklerParticleSystem = this.m_sprinklerParticleSystems[index];
          sprinklerParticleSystem.Simulate(constant);
          sprinklerParticleSystem.Play();
        }
      }
    }

    private void reassignBuffers()
    {
      if (this.m_detailsDataBuffer == null)
        return;
      this.CropsMaterial?.SetBuffer(FarmMb.PROPERTIES_ID, this.m_detailsDataBuffer);
    }

    void IEntityMbWithSyncUpdate.SyncUpdate(GameTime time)
    {
      this.m_isSprinkling = this.m_farm.IsIrrigating;
      this.m_shouldAnimate = this.m_farm.ShouldAnimate;
      this.m_newGameSpeedMult = time.GameSpeedMult;
      this.m_animHandler.SyncUpdate(time);
      if (this.m_farm.CurrentCrop.HasValue)
      {
        Crop crop = this.m_farm.CurrentCrop.Value;
        this.m_isNewCrop = crop != this.m_currentCrop;
        if (this.m_isNewCrop)
        {
          this.m_growthFrom = this.m_growthTo = crop.GrowthPercent.ToFloat();
          this.m_drynessTo = this.m_drynessTo = crop.DrynessPercent.ToFloat();
        }
        else
        {
          this.m_growthFrom = this.m_growthTo;
          this.m_drynessFrom = this.m_drynessTo;
          this.m_growthTo = crop.GrowthPercent.ToFloat();
          this.m_drynessTo = crop.DrynessPercent.ToFloat();
        }
        this.m_currentCrop = (Option<Crop>) crop;
      }
      else
        this.m_currentCrop = Option<Crop>.None;
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      this.updateSprinklers(time.FrameTimeSec);
      this.m_animHandler.SetEnabled(this.m_shouldAnimate);
      this.m_animHandler.RenderUpdate(time);
      if (this.m_currentCrop.IsNone)
      {
        this.CropsDrawn = 0;
      }
      else
      {
        if (this.m_isNewCrop)
          this.initializeInstancedDrawFor(this.m_currentCrop.Value.Prototype);
        if ((UnityEngine.Object) this.CropsMaterial == (UnityEngine.Object) null)
        {
          Log.WarningOnce("Crops material is null! Is crop TODO?");
        }
        else
        {
          this.CropsMaterial.SetVector(FarmMb.GROWTH_ID, new Vector4(this.m_growthFrom.Lerp(this.m_growthTo, time.AbsoluteT), this.m_drynessFrom.Lerp(this.m_drynessTo, time.AbsoluteT)));
          Graphics.DrawMeshInstancedIndirect(this.CropsMeshShared, 0, this.CropsMaterial, this.WorldBounds, this.m_drawArgsBuffer, 0, (MaterialPropertyBlock) null, ShadowCastingMode.Off, true, 0, (Camera) null, LightProbeUsage.Off, (LightProbeProxyVolume) null);
          this.CropsDrawn = this.m_detailsData.Length;
        }
      }
    }

    private void updateSprinklers(float deltaTimeSec)
    {
      Assert.That<ParticleSystem[]>(this.m_sprinklerParticleSystems).IsNotNull<ParticleSystem[]>();
      if (this.m_nextSprinklerToStartIndex > 0)
      {
        float num = deltaTimeSec * 18f * (float) this.m_newGameSpeedMult;
        for (int index = 0; index < this.m_nextSprinklerToStartIndex; ++index)
        {
          float angle = this.m_sprinklerAngles[index] + num;
          this.m_sprinklerAngles[index] = angle;
          this.m_sprinklerParticleSystems[index].gameObject.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
        }
        if (this.m_oldGameSpeedMult != this.m_newGameSpeedMult)
        {
          this.m_oldGameSpeedMult = this.m_newGameSpeedMult;
          foreach (ParticleSystem sprinklerParticleSystem in this.m_sprinklerParticleSystems)
            sprinklerParticleSystem.main.simulationSpeed = (float) this.m_newGameSpeedMult;
        }
      }
      if (this.m_isSprinkling)
      {
        if (this.m_nextSprinklerToStartIndex >= this.m_sprinklerParticleSystems.Length)
          return;
        this.m_nextSprinklerDelaySec -= deltaTimeSec * (float) this.m_newGameSpeedMult;
        if ((double) this.m_nextSprinklerDelaySec > 0.0)
          return;
        this.m_nextSprinklerDelaySec = FarmMb.SPRINKLER_START_DELAY_DURATION_SEC;
        this.startSprinklerAt(this.m_nextSprinklerToStartIndex, (float) this.m_newGameSpeedMult);
        ++this.m_nextSprinklerToStartIndex;
      }
      else
      {
        if (this.m_nextSprinklerToStartIndex <= 0)
          return;
        this.m_nextSprinklerDelaySec -= deltaTimeSec * (float) this.m_newGameSpeedMult;
        if ((double) this.m_nextSprinklerDelaySec > 0.0)
          return;
        this.m_nextSprinklerDelaySec = FarmMb.SPRINKLER_STOP_DELAY_DURATION;
        --this.m_nextSprinklerToStartIndex;
        this.m_sprinklerParticleSystems[this.m_nextSprinklerToStartIndex].Stop();
      }
    }

    private void startSprinklerAt(int index, float simSpeed)
    {
      ParticleSystem sprinklerParticleSystem = this.m_sprinklerParticleSystems[index];
      sprinklerParticleSystem.main.simulationSpeed = simSpeed;
      sprinklerParticleSystem.Play();
      float angle = this.m_randomForSprinklers.NextFloat(0.0f, 360f);
      sprinklerParticleSystem.gameObject.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.up);
      this.m_sprinklerAngles[index] = angle;
    }

    public override void Destroy()
    {
      this.m_drawArgsBuffer?.Dispose();
      this.m_detailsDataBuffer?.Dispose();
      this.CropsMaterial.DestroyIfNotNull();
      this.CropsMaterial = (Material) null;
      base.Destroy();
    }

    private void initializeInstancedDrawFor(CropProto cropProto)
    {
      this.CropsMaterial = (Material) null;
      if (cropProto.Graphics.PrefabPath == "TODO")
      {
        Log.WarningOnce("Crops graphics prefab path is still TODO!");
      }
      else
      {
        GameObject sharedPrefabOrThrow = this.m_assetsDb.GetSharedPrefabOrThrow(cropProto.Graphics.PrefabPath);
        this.CropsMeshShared = sharedPrefabOrThrow.GetComponent<MeshFilter>().sharedMesh;
        Assert.That<Mesh>(this.CropsMeshShared).IsValidUnityObject<Mesh>("Crops prefab has no mesh.");
        this.CropsMaterial = UnityEngine.Object.Instantiate<Material>(sharedPrefabOrThrow.GetComponent<MeshRenderer>().sharedMaterial);
        Assert.That<Material>(this.CropsMaterial).IsValidUnityObject<Material>("Crops prefab has no material.");
        this.CropsMaterial.SetBuffer(FarmMb.PROPERTIES_ID, this.m_detailsDataBuffer);
        this.m_drawArgs[0] = this.CropsMeshShared.GetIndexCount(0);
        this.m_drawArgs[2] = this.CropsMeshShared.GetIndexStart(0);
        this.m_drawArgs[3] = this.CropsMeshShared.GetBaseVertex(0);
        this.m_drawArgsBuffer.SetData((Array) this.m_drawArgs);
        this.CropsMaterial.SetVector(FarmMb.HEIGHT_SAG_NORMAL_ID, new Vector4(this.m_farm.Position3f.Height.ToUnityUnits(), 0.2f, 0.0f, 1f));
        this.CropsMaterial.SetVector(FarmMb.WIND_PARAMS_ID, new Vector4(0.4f * cropProto.Graphics.WindTimeScale, 2f * cropProto.Graphics.WindWaviness, 1.5f * cropProto.Graphics.WindAmplitude, 0.0f));
        TileTransform transform = this.m_farm.Transform;
        EntityLayout layout = this.m_farm.Prototype.Layout;
        ImmutableArray<RelTile2f> cropPositions = this.m_farm.Prototype.Graphics.CropPositions;
        Tile3i position = this.m_farm.Transform.Position;
        this.m_randomForCrops.SeedFast(position.X, position.Y, position.Z, this.m_farm.LifetimePlantedCropsCount);
        Tile2f xy = this.m_farm.GetCenter().Xy;
        for (int index = 0; index < cropPositions.Length; ++index)
        {
          Vector2 vector2 = (xy + layout.TransformRelativeF_Point(cropPositions[index], transform) + this.m_randomForCrops.NextRelTile2f(-FarmMb.POSITION_RANDOMNESS_TILES, FarmMb.POSITION_RANDOMNESS_TILES)).ToVector2();
          float radians = this.m_randomForCrops.NextFloat() * 6.28318548f;
          float rotationCos = MafiMath.Cos(radians);
          float rotationSin = MafiMath.Sin(radians);
          float scale = (float) (1.0 + (double) cropProto.Graphics.ScaleVariation * (double) this.m_randomForCrops.NextFloat());
          float growthRand = -0.1f * this.m_randomForCrops.NextFloat();
          this.m_detailsData[index] = new FarmMb.DetailInstanceData(vector2.x, vector2.y, scale, rotationCos, rotationSin, growthRand);
        }
        this.m_detailsDataBuffer.SetData((Array) this.m_detailsData);
      }
    }

    public FarmMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_randomForCrops = new XorRsr128PlusGenerator(RandomGeneratorType.NonSim, 0UL, 1UL);
      this.m_animHandler = new LoopAnimationHandler();
      this.m_drawArgs = new uint[5];
      this.m_oldGameSpeedMult = -1;
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    static FarmMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      FarmMb.POSITION_RANDOMNESS_TILES = 0.15.ToFix32();
      FarmMb.GROWTH_ID = Shader.PropertyToID("_GrowthXDryY");
      FarmMb.HEIGHT_SAG_NORMAL_ID = Shader.PropertyToID("_HeightXDrynessSagYFakeNormalW");
      FarmMb.PROPERTIES_ID = Shader.PropertyToID("_Properties");
      FarmMb.WIND_PARAMS_ID = Shader.PropertyToID("_WindParams");
      FarmMb.SPRINKLER_START_DELAY_DURATION_SEC = 0.2f;
      FarmMb.SPRINKLER_STOP_DELAY_DURATION = 0.1f;
    }

    /// <summary>
    /// Per-instance data that is passed to GPU. Layout of this struct must match the `MeshProperties` struct
    /// in the shader.
    /// </summary>
    [ExpectedStructSize(24)]
    private struct DetailInstanceData
    {
      public readonly float X;
      public readonly float Y;
      public readonly float RotationCos;
      public readonly float RotationSin;
      public readonly float Scale;
      public readonly float GrowthRand;

      public DetailInstanceData(
        float x,
        float y,
        float scale,
        float rotationCos,
        float rotationSin,
        float growthRand)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        this.X = x;
        this.Y = y;
        this.RotationCos = rotationCos;
        this.RotationSin = rotationSin;
        this.Scale = scale;
        this.GrowthRand = growthRand;
      }
    }
  }
}
