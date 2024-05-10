// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.InputControl.LayoutEntityPlacing.LayoutEntitySlotPlacerHelper
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core;
using Mafi.Core.Entities.Static.Layout;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.InputControl.LayoutEntityPlacing
{
  public class LayoutEntitySlotPlacerHelper
  {
    private readonly ImmutableArray<ILayoutEntitySlotBasedValidator> m_slotBasedValidators;
    private readonly LinesFactory m_linesFactory;
    private readonly Lyst<LayoutEntitySlot> m_availableSlots;
    private readonly Lyst<LayoutEntitySlotPlacerHelper.SlotOutline> m_slotOutlines;
    private long m_snapDistanceSqr;

    public Tile3i LastSetPosition { get; private set; }

    public Rotation90 LastSetRotation { get; private set; }

    public bool LastSetReflection { get; private set; }

    public TileTransform Transform { get; private set; }

    public LayoutEntitySlotPlacerHelper(
      AllImplementationsOf<ILayoutEntitySlotBasedValidator> allSlotBasedValidators,
      LinesFactory linesFactory)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      this.m_availableSlots = new Lyst<LayoutEntitySlot>();
      this.m_slotOutlines = new Lyst<LayoutEntitySlotPlacerHelper.SlotOutline>();
      // ISSUE: reference to a compiler-generated field
      this.\u003CTransform\u003Ek__BackingField = TileTransform.Identity;
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.m_slotBasedValidators = allSlotBasedValidators.Implementations;
      this.m_linesFactory = linesFactory;
    }

    public void Activate(ILayoutEntityProto proto)
    {
      Assert.That<Lyst<LayoutEntitySlot>>(this.m_availableSlots).IsEmpty<LayoutEntitySlot>();
      this.m_snapDistanceSqr = (proto.Layout.LayoutSize.Xy / 2).LengthSqr;
      this.m_availableSlots.Clear();
      foreach (ILayoutEntitySlotBasedValidator slotBasedValidator in this.m_slotBasedValidators)
      {
        if (slotBasedValidator.CanHandle(proto))
          this.m_availableSlots.AddRange(slotBasedValidator.GetAvailableSlots(proto));
      }
      this.m_slotOutlines.Clear();
      foreach (LayoutEntitySlot availableSlot in this.m_availableSlots)
        this.m_slotOutlines.Add(new LayoutEntitySlotPlacerHelper.SlotOutline(this.m_linesFactory, availableSlot.Transform, proto.Layout));
    }

    public void ActivateNoSlotsSnapping()
    {
      this.m_snapDistanceSqr = 0L;
      this.m_availableSlots.Clear();
      this.m_slotOutlines.Clear();
    }

    public void Deactivate()
    {
      this.m_availableSlots.Clear();
      foreach (LayoutEntitySlotPlacerHelper.SlotOutline slotOutline in this.m_slotOutlines)
        slotOutline.ReturnToPool();
      this.m_slotOutlines.Clear();
    }

    public void SetTransform(TileTransform transform)
    {
      this.LastSetPosition = transform.Position;
      this.LastSetRotation = transform.Rotation;
      this.LastSetReflection = transform.IsReflected;
      this.setTransformInternal();
    }

    private void setTransformInternal()
    {
      this.Transform = new TileTransform(this.LastSetPosition, this.LastSetRotation, this.LastSetReflection);
      if (this.m_availableSlots.IsEmpty)
        return;
      long num1 = long.MaxValue;
      LayoutEntitySlot layoutEntitySlot = new LayoutEntitySlot();
      foreach (LayoutEntitySlot availableSlot in this.m_availableSlots)
      {
        long num2 = this.LastSetPosition.DistanceSqrTo(availableSlot.Transform.Position);
        if (num2 < num1)
        {
          num1 = num2;
          layoutEntitySlot = availableSlot;
        }
      }
      this.Transform = num1 <= this.m_snapDistanceSqr ? layoutEntitySlot.GetSlotTransform(this.Transform) : this.Transform;
    }

    /// <summary>
    /// Sets the position and returns true if the position has changed.
    /// </summary>
    public bool SetPosition(Tile3i position)
    {
      if (position == this.LastSetPosition)
        return false;
      this.LastSetPosition = position;
      this.setTransformInternal();
      return true;
    }

    public bool SetRotation(Rotation90 rotation)
    {
      if (rotation == this.LastSetRotation)
        return false;
      this.LastSetRotation = rotation;
      this.setTransformInternal();
      return true;
    }

    public bool SetReflection(bool isReflected)
    {
      if (isReflected == this.LastSetReflection)
        return false;
      this.LastSetReflection = isReflected;
      this.setTransformInternal();
      return true;
    }

    private struct SlotOutline
    {
      private static readonly Color OUTLINE_COLOR;
      private readonly LineMb m_areaOutline;

      public SlotOutline(LinesFactory lineFactory, TileTransform transform, EntityLayout layout)
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        Vector3 vector3 = layout.GetCenter(transform).ToVector3();
        RelTile2i relTile2i = layout.CoreSize;
        relTile2i = relTile2i.Rotate(transform.Rotation);
        Vector2 vector2 = relTile2i.AbsValue.ToUnityUnits() / 2f - new Vector2(0.5f, 0.5f);
        Vector3[] vector3Array = ArrayPool<Vector3>.Get(5);
        vector3Array[0] = vector3Array[4] = vector3 + new Vector3(-vector2.x, 0.0f, -vector2.y);
        vector3Array[1] = vector3 + new Vector3(vector2.x, 0.0f, -vector2.y);
        vector3Array[2] = vector3 + new Vector3(vector2.x, 0.0f, vector2.y);
        vector3Array[3] = vector3 + new Vector3(-vector2.x, 0.0f, vector2.y);
        this.m_areaOutline = lineFactory.CreateLinePooled(vector3Array, 0.5f, LayoutEntitySlotPlacerHelper.SlotOutline.OUTLINE_COLOR);
        vector3Array.ReturnToPool<Vector3>();
      }

      public void ReturnToPool() => this.m_areaOutline.ReturnToPool();

      static SlotOutline()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        LayoutEntitySlotPlacerHelper.SlotOutline.OUTLINE_COLOR = ColorRgba.Green.ToColor();
      }
    }
  }
}
