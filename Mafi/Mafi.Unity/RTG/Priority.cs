// Decompiled with JetBrains decompiler
// Type: RTG.Priority
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using ut12pZTdSUNA6wM24P;

#nullable disable
namespace RTG
{
  public class Priority
  {
    private int _priority;

    public int Value
    {
      get => this._priority;
      set => this._priority = value;
    }

    public static int Lowest => int.MaxValue;

    public static int Highest => int.MinValue;

    public void MakeLowest() => this._priority = Priority.Lowest;

    public void MakeHighest() => this._priority = Priority.Highest;

    public void MakeLowerThan(Priority priority) => this._priority = priority.Value + 1;

    public void MakeHigherThan(Priority priority) => this._priority = priority.Value - 1;

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj)
    {
      return (object) (obj as Priority) != null && this._priority == ((Priority) obj)._priority;
    }

    public int CompareTo(Priority other) => this._priority.CompareTo(other.Value);

    public static bool operator ==(Priority firstPriority, Priority secondPriority)
    {
      return firstPriority.Value == secondPriority.Value;
    }

    public static bool operator !=(Priority firstPriority, Priority secondPriority)
    {
      return firstPriority.Value != secondPriority.Value;
    }

    public static bool operator >(Priority firstPriority, Priority secondPriority)
    {
      return firstPriority.Value < secondPriority.Value;
    }

    public static bool operator >=(Priority firstPriority, Priority secondPriority)
    {
      return firstPriority.Value <= secondPriority.Value;
    }

    public static bool operator <(Priority firstPriority, Priority secondPriority)
    {
      return firstPriority.Value > secondPriority.Value;
    }

    public static bool operator <=(Priority firstPriority, Priority secondPriority)
    {
      return firstPriority.Value >= secondPriority.Value;
    }

    public Priority()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
    }
  }
}
