// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers.PopupProviderBase`1
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Prototypes;
using System;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers
{
  internal abstract class PopupProviderBase<T> : IPopupProvider where T : IProto
  {
    public Type SupportedType => typeof (T);

    public void PopulateView(MenuPopupView view, object item, bool isForResearch)
    {
      this.PopulateView(view, (T) item, isForResearch);
    }

    protected abstract void PopulateView(MenuPopupView view, T proto, bool isForResearch);

    protected PopupProviderBase()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
