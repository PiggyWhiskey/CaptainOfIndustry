// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Utils.BitmapFont5Px
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Collections;
using System;
using System.Linq;

#nullable disable
namespace Mafi.Core.Utils
{
  public static class BitmapFont5Px
  {
    public const int AVG_CHAR_WIDTH = 4;
    public const int AVG_CHAR_HEIGHT = 6;
    private static readonly uint[] GLYPHS;

    public static void DrawString(
      string str,
      Action<Vector2i> setPixel,
      bool vertical = false,
      bool centered = false,
      int? maxLines = null)
    {
      Option<Vector2i[]> option = Option<Vector2i[]>.None;
      if (centered)
      {
        int num = 0;
        Lyst<int> ll = new Lyst<int>();
        foreach (char ch in str)
        {
          if (ch == '\n')
          {
            ll.Add(num);
            num = 0;
            if (maxLines.HasValue && ll.Count > maxLines.Value)
              break;
          }
          else if (ch != '\r' && ch >= ' ')
            ++num;
        }
        if (num > 0)
          ll.Add(num);
        option = (Option<Vector2i[]>) ll.Select<int, Vector2i>((Func<int, int, Vector2i>) ((lineLen, i) => vertical ? new Vector2i(-(ll.Count * 4 - 1) / 2 + 4 * i + 2, lineLen * 6 / 2) : new Vector2i(-(lineLen * 4 / 2) + 1, (ll.Count * 6 - 1) / 2 - 6 * i - 3 - 1))).ToArray<Vector2i>();
      }
      int num1 = 0;
      int num2 = 0;
      int index1 = 0;
      if (option.HasValue && option.Value.Length != 0)
      {
        Vector2i vector2i = option.Value[0];
        num1 = vector2i.X;
        num2 = vector2i.Y;
      }
      foreach (char index2 in str)
      {
        if ((int) index2 >= BitmapFont5Px.GLYPHS.Length)
        {
          Log.Warning(string.Format("Invalid char: {0} ({1})", (object) index2, (object) (int) index2));
        }
        else
        {
          switch (index2)
          {
            case '\n':
              ++index1;
              if (maxLines.HasValue && index1 > maxLines.Value)
                return;
              if (option.HasValue && index1 < option.Value.Length)
              {
                Vector2i vector2i = option.Value[index1];
                num1 = vector2i.X;
                num2 = vector2i.Y;
                continue;
              }
              if (vertical)
              {
                num1 += 4;
                num2 = 0;
                continue;
              }
              num1 = 0;
              num2 -= 6;
              continue;
            case '\r':
              continue;
            default:
              if (index2 < ' ')
              {
                Log.Warning(string.Format("Invalid non-printable char: {0} ({1})", (object) index2, (object) (int) index2));
                continue;
              }
              uint num3 = BitmapFont5Px.GLYPHS[(int) index2];
              Assert.That<uint>(num3).IsNotZero<char, int>("Unknown char '{0}' ({1})", index2, (int) index2);
              int num4 = (int) num3 & 15;
              uint num5 = num3 >> 4;
              Assert.That<long>((long) num5 & (long) ~((1 << num4 * 5) - 1)).IsZero("Invalid encoding, incorrect width?");
              for (int index3 = 0; index3 < 5; ++index3)
              {
                int num6 = num4 - 1;
                while (num6 >= 0)
                {
                  if (((int) num5 & 1) != 0)
                    setPixel(new Vector2i(num1 + num6, num2 + index3));
                  --num6;
                  num5 >>= 1;
                }
              }
              if (vertical)
              {
                num2 -= 6;
                continue;
              }
              num1 += num4 + 1;
              continue;
          }
        }
      }
    }

    static BitmapFont5Px()
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      BitmapFont5Px.GLYPHS = new uint[177]
      {
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        1U,
        465U,
        368643U,
        184204965U,
        249635U,
        272915U,
        209937621U,
        385U,
        6802U,
        9570U,
        43651U,
        23811U,
        49U,
        7171U,
        17U,
        84291U,
        505587U,
        91283U,
        404083U,
        403683U,
        375955U,
        497891U,
        236195U,
        469283U,
        174755U,
        175331U,
        161U,
        177U,
        86291U,
        58243U,
        279875U,
        403491U,
        6928500U,
        179923U,
        441059U,
        176803U,
        440035U,
        499315U,
        499267U,
        234211U,
        376531U,
        477555U,
        75427U,
        375507U,
        299635U,
        390867U,
        440019U,
        177827U,
        440899U,
        177843U,
        441043U,
        231651U,
        477475U,
        374515U,
        374691U,
        374739U,
        371411U,
        376099U,
        469747U,
        15026U,
        280851U,
        13682U,
        172035U,
        115U,
        9218U,
        179923U,
        441059U,
        176803U,
        440035U,
        499315U,
        499267U,
        234211U,
        376531U,
        477555U,
        75427U,
        375507U,
        299635U,
        390867U,
        440019U,
        177827U,
        440899U,
        177843U,
        441043U,
        231651U,
        477475U,
        374515U,
        374691U,
        374739U,
        371411U,
        376099U,
        469747U,
        217395U,
        497U,
        410979U,
        4539397U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        0U,
        174083U
      };
    }
  }
}
