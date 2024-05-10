// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Entities.Static.LiftMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Static;
using Mafi.Core.Factory.Lifts;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Entities.Static
{
  internal class LiftMb : StaticEntityMb
  {
    public void Initialize(Lift lift)
    {
      this.Initialize((ILayoutEntity) lift);
      Transform resultTransform;
      if (lift.Prototype.HeightDelta.IsNegative)
        this.transform.TryFindChild(lift.Prototype.Graphics.AscendingChildEnabled, out resultTransform);
      else
        this.transform.TryFindChild(lift.Prototype.Graphics.DescendingChildEnabled, out resultTransform);
      if ((Object) resultTransform == (Object) null)
        return;
      resultTransform.gameObject.Destroy();
    }

    public LiftMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
