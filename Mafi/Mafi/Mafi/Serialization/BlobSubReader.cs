// Decompiled with JetBrains decompiler
// Type: Mafi.Serialization.BlobSubReader
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Streams;
using System.IO;

#nullable disable
namespace Mafi.Serialization
{
  public class BlobSubReader : BlobReader
  {
    public readonly ReadOnlySubStream SubStream;

    public bool IsDone => this.SubStream.IsDone;

    public bool IsNotDone => !this.IsDone;

    public BlobSubReader(ReadOnlySubStream subStream, int loadedSaveVersion)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      // ISSUE: explicit constructor call
      base.\u002Ector((Stream) subStream, loadedSaveVersion);
      this.SubStream = subStream;
    }

    public void SkipRest() => this.SubStream.SkipRest();
  }
}
