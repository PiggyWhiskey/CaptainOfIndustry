// Decompiled with JetBrains decompiler
// Type: Mafi.Logging.Graylog.UdpGelfClient
// Assembly: Mafi, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: B4163D2E-70BA-4761-B75B-031A8F4CC526
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Sockets;
using System.Text;

#nullable disable
namespace Mafi.Logging.Graylog
{
  public class UdpGelfClient : IGelfClient, IDisposable
  {
    private Option<UdpClient> m_udpClient;
    private readonly Random m_random;
    private readonly JsonWriter m_jsonWriter;
    private readonly MemoryStream m_gzipStreamTmp;
    private readonly byte[] m_idTmp;
    private readonly byte[] m_udpChunk;

    public bool IsOperational => this.m_udpClient != (UdpClient) null;

    public UdpGelfClient(string host)
    {
      MBiHIp97M4MqqbtZOh.RFLpSOptx();
      this.m_random = new Random();
      this.m_jsonWriter = new JsonWriter(256);
      this.m_gzipStreamTmp = new MemoryStream();
      this.m_idTmp = new byte[8];
      this.m_udpChunk = new byte[8192];
      // ISSUE: explicit constructor call
      base.\u002Ector();
      try
      {
        this.m_udpClient = (Option<UdpClient>) new UdpClient(host, 12201);
      }
      catch (SocketException ex)
      {
        this.m_udpClient = Option<UdpClient>.None;
        Log.Warning("Failed to initialize UDP client, no internet?");
      }
      catch (Exception ex)
      {
        this.m_udpClient = Option<UdpClient>.None;
        Log.Exception(ex, "Failed to initialize UDP client.");
      }
    }

    public bool TrySendMessage(GelfMessage message)
    {
      if (this.m_udpClient.IsNone)
        return false;
      message.ToJson(this.m_jsonWriter);
      try
      {
        byte[] messageBytes = Encoding.UTF8.GetBytes(this.m_jsonWriter.GetJsonAndClear());
        if (messageBytes.Length > 512)
          messageBytes = this.compressMessage(messageBytes);
        return this.trySendChunkedMessage(messageBytes, this.m_udpClient.Value);
      }
      catch (Exception ex)
      {
        Log.Exception(ex);
        return false;
      }
    }

    private byte[] compressMessage(byte[] messageBytes)
    {
      this.m_gzipStreamTmp.Position = 0L;
      using (GZipStream gzipStream = new GZipStream((Stream) this.m_gzipStreamTmp, CompressionMode.Compress, true))
        gzipStream.Write(messageBytes, 0, messageBytes.Length);
      return this.m_gzipStreamTmp.ToArray();
    }

    private bool trySendChunkedMessage(byte[] messageBytes, UdpClient udpClient)
    {
      int num = messageBytes.Length.CeilDiv(8180);
      if (num > 128)
      {
        Log.Error(string.Format("GELF message ({0} bytes) contains {1} chunks, ", (object) messageBytes.Length, (object) num) + string.Format("exceeding the maximum of {0}.", (object) 128));
        return false;
      }
      this.m_random.NextBytes(this.m_idTmp);
      for (int index = 0; index < num; ++index)
      {
        this.m_udpChunk[0] = (byte) 30;
        this.m_udpChunk[1] = (byte) 15;
        Array.ConstrainedCopy((Array) this.m_idTmp, 0, (Array) this.m_udpChunk, 2, 8);
        this.m_udpChunk[10] = (byte) index;
        this.m_udpChunk[11] = (byte) num;
        int sourceIndex = index * 8180;
        int length = (messageBytes.Length - sourceIndex).Min(8180);
        Array.ConstrainedCopy((Array) messageBytes, sourceIndex, (Array) this.m_udpChunk, 12, length);
        udpClient.Send(this.m_udpChunk, 12 + length);
      }
      return true;
    }

    public void Dispose()
    {
      if (!this.m_udpClient.HasValue)
        return;
      this.m_udpClient.Value.Dispose();
      this.m_udpClient = Option<UdpClient>.None;
    }
  }
}
