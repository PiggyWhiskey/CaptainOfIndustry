// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.MachineWithSignMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class MachineWithSignMb : 
    LayoutEntityWithSignMb,
    IEntityMbWithRenderUpdate,
    IEntityMb,
    IDestroyableEntityMb
  {
    private Machine m_machine;

    public void Initialize(AssetsDb assetsDb, Machine machine)
    {
      this.Initialize(assetsDb, (Mafi.Core.Entities.Static.Layout.LayoutEntity) machine);
      this.m_machine = machine;
      this.m_signChildName = "sign";
      Assert.That<Mafi.Core.Entities.Static.Layout.LayoutEntity>(this.m_entity).IsNotNull<Mafi.Core.Entities.Static.Layout.LayoutEntity>();
      this.initializeSign(0.7692308f);
    }

    void IEntityMbWithRenderUpdate.RenderUpdate(GameTime time)
    {
      if (this.m_entity.IsDestroyed)
        return;
      Option<RecipeProto> recipeInProgress = this.m_machine.LastRecipeInProgress;
      if (recipeInProgress.IsNone && this.m_productToRender.HasValue)
      {
        this.m_productToRender = Option<ProductProto>.None;
        this.updateSign();
      }
      else
      {
        recipeInProgress = this.m_machine.LastRecipeInProgress;
        if (!recipeInProgress.HasValue || !(this.m_machine.LastRecipeInProgress.Value.OutputsAtEnd.First.Product != this.m_productToRender))
          return;
        Assert.That<int>(this.m_machine.LastRecipeInProgress.Value.OutputsAtEnd.Length).IsEqualTo(1, "Showing recipe sign for recipe with multiple outputs");
        this.m_productToRender = (Option<ProductProto>) this.m_machine.LastRecipeInProgress.Value.OutputsAtEnd.First.Product;
        this.updateSign();
      }
    }

    public MachineWithSignMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
