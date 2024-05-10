// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Factory.Zippers.MiniZipperValidator
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Entities.Validators;
using Mafi.Core.Factory.Transports;
using Mafi.Localization;
using System;

#nullable disable
namespace Mafi.Core.Factory.Zippers
{
  [GlobalDependency(RegistrationMode.AsAllInterfaces, false, false)]
  public class MiniZipperValidator : 
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator,
    IEntityPreAddValidator,
    IFactory<MiniZipperProto, EntityAddRequestData, LayoutEntityAddRequest>
  {
    private readonly TransportsManager m_transportsManager;
    private CanPlaceMiniZipperAtResult? m_lastCanAddResult;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public MiniZipperValidator(TransportsManager transportsManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_transportsManager = transportsManager;
    }

    public EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      this.m_lastCanAddResult = new CanPlaceMiniZipperAtResult?();
      if (!(addRequest.Proto is MiniZipperProto proto))
        return EntityValidationResult.Success;
      CanPlaceMiniZipperAtResult? result;
      LocStrFormatted error;
      if (!this.m_transportsManager.CanBuildMiniZipperAt(proto, addRequest.Transform.Position, out result, out error))
        return EntityValidationResult.CreateError(error);
      this.m_lastCanAddResult = result;
      return EntityValidationResult.Success;
    }

    public void PrepareForAdd()
    {
      if (!this.m_lastCanAddResult.HasValue)
        return;
      this.m_transportsManager.CutOutTransportForMiniZipper(this.m_lastCanAddResult.Value);
      this.m_lastCanAddResult = new CanPlaceMiniZipperAtResult?();
    }

    public LayoutEntityAddRequest Create(MiniZipperProto proto, EntityAddRequestData data)
    {
      return LayoutEntityAddRequest.GetPooledInstanceToCreateEntity((ILayoutEntityProto) proto, new EntityAddRequestData(data.Transform, ignoreForCollisions: data.IgnoreForCollisions.HasValue ? (Predicate<EntityId>) (x => data.IgnoreForCollisions.ValueOrNull(x) || this.m_transportsManager.IgnoreTransportsAndPillars(x)) : (Predicate<EntityId>) (x => this.m_transportsManager.IgnoreTransportsAndPillars(x)), recordTileErrors: data.RecordTileErrors));
    }
  }
}
