// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.UserInterface.Components.SingleRecipeObserver
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Factory.Recipes;
using Mafi.Core.Syncers;
using Mafi.Unity.InputControl.RecipesBook;
using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using System;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.UserInterface.Components
{
  internal class SingleRecipeObserver : IUiElement
  {
    public readonly IUiUpdater Updater;
    private readonly Panel m_container;

    public GameObject GameObject => this.m_container.GameObject;

    public RectTransform RectTransform => this.m_container.RectTransform;

    public SingleRecipeObserver(
      IUiElement parent,
      UiBuilder builder,
      Option<RecipesBookController> recipeBookController,
      Func<Option<IRecipeForUi>> recipeProvider)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      SingleRecipeObserver singleRecipeObserver = this;
      this.m_container = builder.NewPanel("Recipe", parent);
      RecipeView.Cache recipeViewsCache = new RecipeView.Cache((IUiElement) this.m_container, builder, recipeBookController);
      Option<RecipeView> lastRecipe = (Option<RecipeView>) Option.None;
      this.Updater = UpdaterBuilder.Start().Observe<Option<IRecipeForUi>>(recipeProvider).Observe<bool>((Func<bool>) (() => builder.DurationNormalizer.IsNormalizationOn)).Do((Action<Option<IRecipeForUi>, bool>) ((proto, _) =>
      {
        if (proto.IsNone)
          return;
        if (lastRecipe.HasValue)
          lastRecipe.Value.Hide<RecipeView>();
        lastRecipe = (Option<RecipeView>) recipeViewsCache.GetView(proto.Value);
        lastRecipe.Value.PutTo<RecipeView>((IUiElement) closure_0.m_container);
        lastRecipe.Value.UpdateNormalization();
      })).Build();
    }
  }
}
