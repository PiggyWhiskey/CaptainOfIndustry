// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Entities.Static.Layout.InvalidEntityLayoutException
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using System;
using System.Runtime.Serialization;

#nullable disable
namespace Mafi.Core.Entities.Static.Layout
{
  [Serializable]
  public class InvalidEntityLayoutException : Exception
  {
    public InvalidEntityLayoutException(
      string message,
      string token = null,
      int? line = null,
      int? col = null,
      Vector2i? tokenPos = null)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(message + (token != null ? " Token '" + token + "'" : "") + (line.HasValue ? string.Format(", line {0}", (object) line) : "") + (col.HasValue ? ", col " + token : "") + (tokenPos.HasValue ? string.Format(", line {0}, col {1}", (object) (tokenPos.Value.Y + 1), (object) (tokenPos.Value.X * 3 + 1)) : ""));
    }

    protected InvalidEntityLayoutException(SerializationInfo info, StreamingContext context)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: explicit constructor call
      base.\u002Ector(info, context);
    }
  }
}
