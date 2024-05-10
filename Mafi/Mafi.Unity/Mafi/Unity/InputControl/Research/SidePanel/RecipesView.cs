// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Research.SidePanel.RecipesView
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.Syncers;
using Mafi.Core.UnlockingTree;
using Mafi.Localization;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using Mafi.Unity.UserInterface.Components;
using System;
using System.Collections.Generic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Research.SidePanel
{
  /// <summary>Displays recipes that are unlocked by a research.</summary>
  internal class RecipesView
  {
    private readonly UiBuilder m_builder;
    private readonly StackContainer m_parent;
    private readonly Action<float> m_onWidthChanged;
    private readonly StackContainer m_recipesContainer;
    private readonly RecipesView.Cache m_viewsCache;
    private readonly Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>> m_recipesUnlocksCache;
    private readonly Lyst<RecipesView.MachineWithRecipeView> m_unlockedRecipesCache;
    private readonly Lyst<RecipesView.MachineWithRecipeView> m_lockedRecipesCache;
    public float MaxRecipeWidth;

    public RecipesView(
      UiBuilder builder,
      RecipesBookController recipesBookController,
      StackContainer parent,
      Action<float> onWidthChanged,
      Action<IRecipeForUi> onLockClick = null,
      Action<LayoutEntityProto> onMachineClick = null)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_recipesUnlocksCache = new Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>();
      this.m_unlockedRecipesCache = new Lyst<RecipesView.MachineWithRecipeView>();
      this.m_lockedRecipesCache = new Lyst<RecipesView.MachineWithRecipeView>();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_builder = builder;
      this.m_parent = parent;
      this.m_onWidthChanged = onWidthChanged;
      this.m_recipesContainer = builder.NewStackContainer("Recipes", (IUiElement) parent).SetStackingDirection(StackContainer.Direction.TopToBottom).SetItemSpacing(5f).AppendTo<StackContainer>(parent, new float?(0.0f));
      this.m_viewsCache = new RecipesView.Cache((IUiElement) this.m_recipesContainer, builder, recipesBookController, onLockClick, onMachineClick);
    }

    public IUiUpdater CreateUpdater(Func<ResearchNode> nodeProvider)
    {
      return UpdaterBuilder.Start().Observe<ResearchNodeProto>((Func<ResearchNodeProto>) (() => nodeProvider().Proto)).Do(new Action<ResearchNodeProto>(this.update)).Build();
    }

    public IUiUpdater CreateUpdaterForGeneralRecipes(
      Func<ProductProto> productProvider,
      Func<ProductProto, Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>>> recipesProvider,
      Func<LayoutEntityProto, IRecipeForUi, bool> isLockedFunc)
    {
      return UpdaterBuilder.Start().Observe<ProductProto>(productProvider).Do((Action<ProductProto>) (x => this.update(x, recipesProvider(x), (Option<Func<LayoutEntityProto, IRecipeForUi, bool>>) isLockedFunc))).Build();
    }

    private void update(
      ProductProto product,
      Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>> recipes,
      Option<Func<LayoutEntityProto, IRecipeForUi, bool>> isLockedFunc)
    {
      this.m_recipesUnlocksCache.Clear();
      this.m_recipesUnlocksCache.AddRange(recipes);
      this.update(this.m_recipesUnlocksCache, isLockedFunc, false);
    }

    private void update(ResearchNodeProto nodeProto)
    {
      this.m_recipesUnlocksCache.Clear();
      foreach (IUnlockNodeUnit unit in nodeProto.Units)
      {
        if (unit is RecipeUnlock recipeUnlock && !recipeUnlock.HideInUI)
          this.m_recipesUnlocksCache.Add(new KeyValuePair<LayoutEntityProto, IRecipeForUi>((LayoutEntityProto) recipeUnlock.MachineProto, (IRecipeForUi) recipeUnlock.Proto));
        if (unit is ProtoUnlock protoUnlock)
        {
          foreach (Proto unlockedProto in protoUnlock.UnlockedProtos)
          {
            if (unlockedProto is IRecipeForUi recipeForUi && recipeForUi is LayoutEntityProto key)
              this.m_recipesUnlocksCache.Add<LayoutEntityProto, IRecipeForUi>(key, recipeForUi);
          }
        }
      }
      this.update(this.m_recipesUnlocksCache, (Option<Func<LayoutEntityProto, IRecipeForUi, bool>>) Option.None, true);
    }

    private void update(
      Lyst<KeyValuePair<LayoutEntityProto, IRecipeForUi>> recipes,
      Option<Func<LayoutEntityProto, IRecipeForUi, bool>> isLockedFunc,
      bool alignToTheRight)
    {
      this.m_recipesContainer.StartBatchOperation();
      this.m_recipesContainer.ClearAll(true);
      this.m_unlockedRecipesCache.Clear();
      this.m_lockedRecipesCache.Clear();
      this.MaxRecipeWidth = 0.0f;
      foreach (KeyValuePair<LayoutEntityProto, IRecipeForUi> recipe in recipes)
      {
        RecipesView.MachineWithRecipeView view = this.m_viewsCache.GetView(recipe.Value);
        RecipeView recipeView = view.RecipeView;
        view.SetMachine(recipe.Key);
        view.RecipeView.UpdateNormalization();
        if (isLockedFunc.HasValue)
        {
          bool isVisible = isLockedFunc.Value(recipe.Key, recipe.Value);
          view.SetLockedOverlayVisibility(isVisible);
          if (isVisible)
            this.m_lockedRecipesCache.Add(view);
          else
            this.m_unlockedRecipesCache.Add(view);
        }
        else
          this.m_unlockedRecipesCache.Add(view);
        this.MaxRecipeWidth = this.MaxRecipeWidth.Max(recipeView.GetDynamicWidth());
      }
      if (recipes.IsNotEmpty)
        this.MaxRecipeWidth += 88f;
      foreach (RecipesView.MachineWithRecipeView machineWithRecipeView in this.m_unlockedRecipesCache)
      {
        machineWithRecipeView.AppendTo<RecipesView.MachineWithRecipeView>(this.m_recipesContainer, new float?(this.m_builder.Style.RecipeDetail.Height));
        machineWithRecipeView.Show<RecipesView.MachineWithRecipeView>();
      }
      foreach (RecipesView.MachineWithRecipeView machineWithRecipeView in this.m_lockedRecipesCache)
      {
        machineWithRecipeView.AppendTo<RecipesView.MachineWithRecipeView>(this.m_recipesContainer, new float?(this.m_builder.Style.RecipeDetail.Height));
        machineWithRecipeView.Show<RecipesView.MachineWithRecipeView>();
      }
      foreach (KeyValuePair<LayoutEntityProto, IRecipeForUi> recipe in recipes)
      {
        RecipeView recipeView = this.m_viewsCache.GetView(recipe.Value).RecipeView;
        if (alignToTheRight)
          recipeView.SetLeftIndent(this.MaxRecipeWidth - recipeView.GetDynamicWidth());
        else
          recipeView.SetLeftIndent(88f);
      }
      this.m_recipesContainer.FinishBatchOperation();
      this.m_onWidthChanged(this.MaxRecipeWidth);
    }

    public class MachineWithRecipeView : IUiElement
    {
      public const int WIDTH = 88;
      private readonly UiBuilder m_builder;
      private readonly Option<Action<IRecipeForUi>> m_onLockIconClick;
      public readonly RecipeView RecipeView;
      private readonly Panel m_machineIcon;
      private Option<Panel> m_lockedOverlay;
      private readonly TitleTooltip m_machineName;
      private readonly IconContainer m_categoryIcon;

      public GameObject GameObject => this.RecipeView.GameObject;

      public RectTransform RectTransform => this.RecipeView.RectTransform;

      private LayoutEntityProto Machine { get; set; }

      public MachineWithRecipeView(
        IUiElement parent,
        UiBuilder builder,
        RecipesBookController recipesBookController,
        IRecipeForUi recipe,
        Action<IRecipeForUi> onLockIconClick = null,
        Action<LayoutEntityProto> onMachineClick = null)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
        RecipesView.MachineWithRecipeView machineWithRecipeView = this;
        this.m_builder = builder;
        this.m_onLockIconClick = (Option<Action<IRecipeForUi>>) onLockIconClick;
        this.RecipeView = new RecipeView(parent, builder, (Option<RecipesBookController>) recipesBookController, recipe, noBorders: true);
        this.m_machineIcon = builder.NewPanel("MachineIcon").PutToLeftMiddleOf<Panel>((IUiElement) this.RecipeView, new Vector2(45f, 45f), Offset.Left(5f));
        this.m_machineName = new TitleTooltip(builder);
        this.m_machineName.SetMaxWidthOverflow(40);
        this.m_machineName.SetExtraOffsetFromBottom(13);
        if (onMachineClick != null)
          this.m_machineIcon.OnClick((Action) (() => onMachineClick(machineWithRecipeView.Machine)));
        this.m_categoryIcon = builder.NewIconContainer("CategoryIcon").PutToLeftBottomOf<IconContainer>((IUiElement) this.RecipeView, new Vector2(28f, 28f), Offset.Left(50f) + Offset.Bottom(5f));
        builder.NewTxt("Divider").SetText(":").SetTextStyle(new TextStyle((ColorRgba) 11579568, 16, new FontStyle?(FontStyle.Bold))).SetAlignment(TextAnchor.MiddleLeft).PutToLeftOf<Txt>((IUiElement) this.RecipeView, 5f, Offset.Left(85f) + Offset.TopBottom(5f));
        this.RecipeView.OnRecipeEnter += new Action(this.showMachineName);
        this.RecipeView.OnRecipeLeave += new Action(this.hideMachineName);
      }

      public void SetMachine(LayoutEntityProto proto)
      {
        this.Machine = proto;
        this.m_machineIcon.SetBackground(proto.Graphics.IconPath, new ColorRgba?((ColorRgba) 14606046));
        this.m_machineName.SetText((LocStrFormatted) proto.Strings.Name);
        if (!proto.Graphics.Categories.IsNotEmpty)
          return;
        this.m_categoryIcon.SetIcon(proto.Graphics.Categories.First.IconPath);
      }

      public void SetLockedOverlayVisibility(bool isVisible)
      {
        if (!isVisible)
        {
          if (!this.m_lockedOverlay.HasValue)
            return;
          this.m_lockedOverlay.Value.Hide<Panel>();
        }
        else if (this.m_lockedOverlay.IsNone)
        {
          this.m_lockedOverlay = (Option<Panel>) this.m_builder.NewPanel("Overlay").SetBackground(2500134.ToColorRgba().SetA((byte) 150)).PutTo<Panel>((IUiElement) this);
          this.m_builder.NewBtn("Lock").SetButtonStyle(this.m_builder.Style.Global.IconBtnWhite).SetIcon("Assets/Unity/UserInterface/General/Locked128.png").OnClick((Action) (() =>
          {
            Action<IRecipeForUi> valueOrNull = this.m_onLockIconClick.ValueOrNull;
            if (valueOrNull == null)
              return;
            valueOrNull(this.RecipeView.Recipe);
          })).PutToLeftBottomOf<Btn>((IUiElement) this.m_lockedOverlay.Value, 20.Vector2(), Offset.BottomLeft(4f, 4f));
        }
        else
          this.m_lockedOverlay.Value.SetVisibility<Panel>(true);
      }

      private void showMachineName() => this.m_machineName.Show((IUiElement) this.m_machineIcon);

      private void hideMachineName() => this.m_machineName.Hide();
    }

    public class Cache : ViewsCache<IRecipeForUi, RecipesView.MachineWithRecipeView>
    {
      private readonly IUiElement m_parent;
      private readonly RecipesBookController m_recipesBookController;
      private readonly Action<IRecipeForUi> m_onLockClick;
      private readonly Action<LayoutEntityProto> m_onMachineClick;

      public Cache(
        IUiElement parent,
        UiBuilder builder,
        RecipesBookController recipesBookController,
        Action<IRecipeForUi> onLockClick = null,
        Action<LayoutEntityProto> onMachineClick = null)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector(builder);
        this.m_parent = parent;
        this.m_recipesBookController = recipesBookController;
        this.m_onLockClick = onLockClick;
        this.m_onMachineClick = onMachineClick;
      }

      protected override RecipesView.MachineWithRecipeView CreateView(
        UiBuilder builder,
        IRecipeForUi data)
      {
        return new RecipesView.MachineWithRecipeView(this.m_parent, builder, this.m_recipesBookController, data, this.m_onLockClick, this.m_onMachineClick);
      }
    }
  }
}
