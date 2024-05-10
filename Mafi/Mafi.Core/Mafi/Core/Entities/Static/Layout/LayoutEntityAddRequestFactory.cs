// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutEntityAddRequestFactory
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [GlobalDependency(RegistrationMode.AsSelf, false, false)]
  public class LayoutEntityAddRequestFactory
  {
    private readonly IResolver m_resolver;

    public LayoutEntityAddRequestFactory(IResolver resolver)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_resolver = resolver;
    }

    public LayoutEntityAddRequest CreateRequestFor<TProto>(
      TProto proto,
      EntityAddRequestData data,
      EntityAddReason reason = EntityAddReason.New)
      where TProto : ILayoutEntityProto
    {
      if (this.m_resolver != null)
      {
        Option<LayoutEntityAddRequest> option = this.m_resolver.TryInvokeFactoryHierarchy<LayoutEntityAddRequest>((object) proto, (object) data);
        if (option.HasValue)
          return option.Value;
        Log.Error(string.Format("Failed to create request for '{0}'.", (object) proto));
      }
      return LayoutEntityAddRequest.GetPooledInstanceToCreateEntity((ILayoutEntityProto) proto, data, reason);
    }
  }
}
