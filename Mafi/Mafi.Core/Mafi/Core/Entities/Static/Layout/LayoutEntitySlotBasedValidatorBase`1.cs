// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.LayoutEntitySlotBasedValidatorBase`1
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Validators;
using Mafi.Localization;
using Mafi.Serialization;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [GenerateSerializer(false, null, 0)]
  public abstract class LayoutEntitySlotBasedValidatorBase<TEntityProto> : 
    ILayoutEntitySlotBasedValidator,
    IEntityAdditionValidator<LayoutEntityAddRequest>,
    IEntityAdditionValidator
    where TEntityProto : ILayoutEntityProto
  {
    private bool m_disableSlotValidation;

    public EntityValidatorPriority Priority => EntityValidatorPriority.Default;

    public abstract ImmutableArray<LayoutEntitySlot> GetAvailableSlots(TEntityProto proto);

    public ImmutableArray<LayoutEntitySlot> GetAvailableSlots(ILayoutEntityProto proto)
    {
      if (proto is TEntityProto proto1)
        return this.GetAvailableSlots(proto1);
      Log.Error("Attempting to get slots for unsupported entity proto '" + proto.GetType().Name + "'. Supported proto is '" + typeof (TEntityProto).Name + "'.");
      return ImmutableArray<LayoutEntitySlot>.Empty;
    }

    public bool CanHandle(ILayoutEntityProto proto) => proto is TEntityProto;

    public virtual EntityValidationResult CanAdd(LayoutEntityAddRequest addRequest)
    {
      if (this.m_disableSlotValidation || !this.CanHandle(addRequest.Proto))
        return EntityValidationResult.Success;
      foreach (LayoutEntitySlot availableSlot in this.GetAvailableSlots(addRequest.Proto))
      {
        if (availableSlot.IsCompatibleWith(addRequest.Transform))
          return EntityValidationResult.Success;
      }
      return EntityValidationResult.CreateError((LocStrFormatted) TrCore.AdditionError__NotInSlot);
    }

    public void DisableSlotValidation() => this.m_disableSlotValidation = true;

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.m_disableSlotValidation);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.m_disableSlotValidation = reader.ReadBool();
    }

    protected LayoutEntitySlotBasedValidatorBase()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
