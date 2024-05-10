// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.Vehicles.AttachmentMb
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Core;
using Mafi.Core.Vehicles.Trucks;
using Mafi.Unity.Entities.Dynamic;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.Vehicles
{
  public class AttachmentMb : MonoBehaviour, IAttachmentMbFriend
  {
    private MbPool<AttachmentMb> m_attachmentPool;

    public AttachmentProto Proto { get; private set; }

    public void Initialize(AttachmentProto attachment)
    {
      this.Proto = attachment.CheckNotNull<AttachmentProto>();
    }

    /// <summary>
    /// Called on the main thread when both sim and update threads are in sync. Limit processing in this method to
    /// absolute minimum.
    /// </summary>
    public virtual void SyncUpdate(DynamicGroundEntityMb parent)
    {
    }

    /// <summary>Called once very frame on the main thread.</summary>
    public virtual void RenderUpdate(GameTime time)
    {
    }

    public virtual void SimUpdateEnd(DynamicGroundEntityMb parent)
    {
    }

    public virtual void SetTruck(Truck truck)
    {
    }

    void IAttachmentMbFriend.SetPool(MbPool<AttachmentMb> attachmentPool)
    {
      Assert.That<MbPool<AttachmentMb>>(this.m_attachmentPool).IsNull<MbPool<AttachmentMb>>();
      this.m_attachmentPool = attachmentPool.CheckNotNull<MbPool<AttachmentMb>>();
    }

    void IAttachmentMbFriend.ReturnToPool()
    {
      AttachmentMb mb = this;
      this.m_attachmentPool.ReturnInstance(ref mb);
    }

    /// <summary>Reset before we return current instance to the pool.</summary>
    public virtual void Reset()
    {
    }

    public AttachmentMb()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
