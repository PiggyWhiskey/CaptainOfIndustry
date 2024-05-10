// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers.DefaultProtoPopupProvider
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.Toolbar.MenuPopup.Providers
{
  internal class DefaultProtoPopupProvider : PopupProviderBase<IProto>
  {
    public DefaultProtoPopupProvider()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }

    protected override void PopulateView(MenuPopupView view, IProto proto, bool isForResearch)
    {
      view.SetTitle((LocStrFormatted) proto.Strings.Name);
      view.SetDescription((LocStrFormatted) proto.Strings.DescShort);
      if (!(proto is DrivingEntityProto drivingEntityProto))
        return;
      view.SetFuelConsumption(drivingEntityProto.GetFuelConsumedPer60());
    }
  }
}
