// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Inspectors.InspectorContext
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Priorities;
using Mafi.Core.Factory.MechanicalPower;
using Mafi.Core.Input;
using Mafi.Core.Notifications;
using Mafi.Core.Population;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Core.Terrain.Designation;
using Mafi.Core.Vehicles;
using Mafi.Unity.Camera;
using Mafi.Unity.Entities;
using Mafi.Unity.InputControl.Inspectors.Buildings;
using Mafi.Unity.InputControl.Inspectors.Vehicles;
using Mafi.Unity.InputControl.Messages;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.Terrain.Designation;
using Mafi.Unity.UserInterface;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Inspectors
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class InspectorContext
  {
    public readonly CameraController CameraController;
    public readonly IUnityInputMgr InputMgr;
    public readonly InspectorController MainController;
    public readonly EntityHighlighter Highlighter;
    public readonly ICalendar Calendar;
    public readonly IEntitiesManager EntitiesManager;
    public readonly IInputScheduler InputScheduler;
    public readonly IAssetTransactionManager AssetsManager;
    public readonly UpointsManager UpointsManager;
    public readonly IShaftManager ShaftManager;
    public readonly GlobalPrioritiesManager PrioritiesManager;
    public readonly UnreachableTerrainDesignationsManager UnreachablesManager;
    public readonly SurfaceDesignationsRenderer SurfaceDesignationsRenderer;
    public readonly INotificationsManager NotificationsManager;
    public readonly MessagesCenterController MessagesCenter;
    public readonly VehiclesManager VehiclesManager;
    public readonly VehiclesReplacementMap VehiclesReplacementMap;
    public readonly RecipesBookController RecipesBookController;
    public readonly UnlockedProtosDbForUi UnlockedProtosDbForUi;
    public readonly ProtosDb ProtosDb;
    public readonly UiBuilder Builder;
    public readonly OceanAreasOverlayRenderer OceanOverlayRenderer;
    public readonly LinesFactory LinesFactory;
    public readonly ShortcutsManager ShortcutsManager;

    public InspectorContext(
      CameraController cameraController,
      IUnityInputMgr inputMgr,
      InspectorController mainController,
      NewInstanceOf<EntityHighlighter> highlighter,
      ICalendar calendar,
      IEntitiesManager entitiesManager,
      IInputScheduler inputScheduler,
      IAssetTransactionManager assetsManager,
      UpointsManager upointsManager,
      IShaftManager shaftManager,
      GlobalPrioritiesManager prioritiesManager,
      UnreachableTerrainDesignationsManager unreachablesManager,
      SurfaceDesignationsRenderer surfaceDesignationsRenderer,
      UnlockedProtosDbForUi unlockedProtosDbForUi,
      ProtosDb protosDb,
      UiBuilder builder,
      RecipesBookController recipesBookController,
      INotificationsManager notificationsManager,
      MessagesCenterController messagesCenter,
      VehiclesManager vehiclesManager,
      VehiclesReplacementMap vehiclesReplacementMap,
      OceanAreasOverlayRenderer oceanOverlayRenderer,
      LinesFactory linesFactory,
      ShortcutsManager shortcutsManager)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.CameraController = cameraController;
      this.InputMgr = inputMgr;
      this.MainController = mainController;
      this.Highlighter = highlighter.Instance;
      this.Calendar = calendar;
      this.EntitiesManager = entitiesManager;
      this.InputScheduler = inputScheduler;
      this.AssetsManager = assetsManager;
      this.UpointsManager = upointsManager;
      this.ShaftManager = shaftManager;
      this.PrioritiesManager = prioritiesManager;
      this.UnreachablesManager = unreachablesManager;
      this.SurfaceDesignationsRenderer = surfaceDesignationsRenderer;
      this.UnlockedProtosDbForUi = unlockedProtosDbForUi;
      this.ProtosDb = protosDb;
      this.Builder = builder;
      this.RecipesBookController = recipesBookController;
      this.NotificationsManager = notificationsManager;
      this.MessagesCenter = messagesCenter;
      this.VehiclesManager = vehiclesManager;
      this.VehiclesReplacementMap = vehiclesReplacementMap;
      this.OceanOverlayRenderer = oceanOverlayRenderer;
      this.LinesFactory = linesFactory;
      this.ShortcutsManager = shortcutsManager;
    }
  }
}
