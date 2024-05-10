// Decompiled with JetBrains decompiler
// Type: Mafi.Unity.MainMenu.CreditsScreen
// Assembly: Mafi.Unity, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: E477F8BD-C838-4DE6-805D-A367452B274A
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Unity.xml

using Mafi.Unity.UiFramework;
using Mafi.Unity.UiFramework.Components;
using Mafi.Unity.UserInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ut12pZTdSUNA6wM24P;

#nullable disable
namespace Mafi.Unity.MainMenu
{
  internal class CreditsScreen
  {
    private static readonly ColorRgba WHITE;
    private static readonly ColorRgba GRAY;
    public readonly Canvass Canvas;
    private static readonly string FOUNDERS_NAMES;
    private static readonly string[] ART_NAMES;
    private static readonly string[] PROGRAMMING_NAMES;
    private static readonly string[] COMMUNITY_MANAGEMENT_NAMES;
    private static readonly string[] SOUND_NAMES;
    private static readonly string[] SPECIAL_THANKS_NAMES;
    private static readonly KeyValuePair<string, string>[] STATS_DATA;
    private static readonly string[] COMMUNITY_MODS_NAMES;
    private static readonly string[] CONTENT_CREATORS_NAMES;
    private static readonly string[] WIKI_CONTRIB_NAMES;
    private static readonly KeyValuePair<string, string[]>[] COMMUNITY_TRANSLATORS_NAMES;
    private static readonly string[] BACKERS_NAMES;

    public bool IsDestroyed => !(bool) (UnityEngine.Object) this.Canvas.GameObject;

    public CreditsScreen(UiBuilder builder, float transparency = 0.0f)
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Canvas = builder.NewCanvas("Credits").SetRenderMode(RenderMode.ScreenSpaceOverlay).SetConstantPixelSize().SetSortOrder(100).MakeInteractive();
      builder.NewPanel("bg").SetBackground(new Color(0.0f, 0.0f, 0.0f, 1f - transparency)).PutTo<Panel>((IUiElement) this.Canvas);
      ScrollableContainer scrollableContainer = builder.NewScrollableContainer("Scroll").AddVerticalScrollbar().PutTo<ScrollableContainer>((IUiElement) this.Canvas);
      Panel panel = this.buildCredits(builder);
      scrollableContainer.AddItemTop((IUiElement) panel);
      float normDistancePerSec = 50f / (panel.GetHeight() - this.Canvas.GetHeight()).Max(1f);
      this.Canvas.GameObject.AddComponent<CreditsScreen.CreditsScrollerMb>().Initialize(scrollableContainer, normDistancePerSec);
    }

    private Panel buildCredits(UiBuilder builder)
    {
      Panel content = builder.NewPanel("Content");
      float y = this.Canvas.GetHeight() / 3f;
      image("Assets/Unity/UserInterface/MainMenu/CoiLogo-1024x80.png", new Vector2(1024f, 80f));
      if (builder.AssetsDb.ContainsAsset("Assets/Supporter/SupporterEdition-512x56.png"))
      {
        y += 20f;
        image("Assets/Supporter/SupporterEdition-512x56.png", new Vector2(512f, 56f));
      }
      y += 30f;
      customText("by MaFi Games", new TextStyle(CreditsScreen.WHITE, 22, new FontStyle?(FontStyle.Italic)));
      y += 50f;
      sectionTitle("Production\nGame design\nProgramming\nCommunity management");
      y += 5f;
      customText(CreditsScreen.FOUNDERS_NAMES.FormatInvariant((object) 22, (object) CreditsScreen.GRAY.ToHex()).ToUpperInvariant(), new TextStyle(ColorRgba.White, 44, new FontStyle?(FontStyle.Bold)), true);
      y += 20f;
      sectionWithNames("Programming", CreditsScreen.PROGRAMMING_NAMES, false);
      sectionWithNames("3D modeling and animation", CreditsScreen.ART_NAMES, false);
      sectionWithNames("Community management", CreditsScreen.COMMUNITY_MANAGEMENT_NAMES, false);
      sectionWithNames("Music and sound effects", CreditsScreen.SOUND_NAMES, false);
      sectionWithNames("Special thanks to", CreditsScreen.SPECIAL_THANKS_NAMES, false);
      y += 40f;
      Txt customText1 = createCustomText(((IEnumerable<string>) CreditsScreen.STATS_DATA.MapArray<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (x => x.Key + ":"))).JoinStrings("\n"), new TextStyle(CreditsScreen.GRAY, 22, new FontStyle?(FontStyle.Normal)), anchor: TextAnchor.UpperRight);
      customText1.PutRelativeTo<Txt>((IUiElement) content, customText1.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y).AddRight((float) ((double) customText1.GetWidth() / 2.0 + 4.0)));
      Txt customText2 = createCustomText(((IEnumerable<string>) CreditsScreen.STATS_DATA.MapArray<KeyValuePair<string, string>, string>((Func<KeyValuePair<string, string>, string>) (x => x.Value))).JoinStrings("\n"), new TextStyle(CreditsScreen.GRAY, 22, new FontStyle?(FontStyle.Normal)), anchor: TextAnchor.UpperLeft);
      customText2.PutRelativeTo<Txt>((IUiElement) content, customText2.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y).AddLeft((float) ((double) customText2.GetWidth() / 2.0 + 4.0)));
      y += customText1.GetHeight();
      y += 50f;
      IconContainer iconContainer = builder.NewIconContainer("line").SetColor(ColorRgba.Gray).SetSize<IconContainer>(new Vector2(this.Canvas.GetWidth() / 2f, 2f));
      iconContainer.PutRelativeTo<IconContainer>((IUiElement) content, iconContainer.GetSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y));
      y += 50f;
      customText("Additionally, we'd like to thank members of our amazing community.\nThanks for being with us from the very beginning!", new TextStyle(CreditsScreen.WHITE, 22, new FontStyle?(FontStyle.Italic)));
      y += 30f;
      sectionWithNames("Community moderators", CreditsScreen.COMMUNITY_MODS_NAMES, true);
      sectionWithNames("Content creators", CreditsScreen.CONTENT_CREATORS_NAMES, true);
      sectionWithNames("Most active Wiki contributors", CreditsScreen.WIKI_CONTRIB_NAMES, true);
      sectionTitle("Most active community translators");
      string names1;
      string names2;
      string names3;
      CreditsScreen.getStringsOfTranslators(out names1, out names2, out names3);
      float self = trNames(names1, -300);
      float num1 = trNames(names2, 0);
      float num2 = trNames(names3, 300);
      y += self.Max(num1).Max(num2);
      sectionTitle("Huge thanks to our most generous backers");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      listNames(CreditsScreen.BACKERS_NAMES, true, CreditsScreen.\u003C\u003EO.\u003C0\u003E__addFunnyPhrases ?? (CreditsScreen.\u003C\u003EO.\u003C0\u003E__addFunnyPhrases = new Func<int, string[], string[]>(CreditsScreen.addFunnyPhrases)), true);
      y += 100f;
      sectionTitle("... ok, now you can go back and\nfinish flattening the entire island.");
      y += this.Canvas.GetHeight() / 2f;
      content.SetHeight<Panel>(y);
      return content;

      void image(string path, Vector2 size)
      {
        IconContainer iconContainer = builder.NewIconContainer("COI logo").SetIcon(path).SetSize<IconContainer>(size);
        iconContainer.PutRelativeTo<IconContainer>((IUiElement) content, iconContainer.GetSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y));
        y += iconContainer.GetHeight();
      }

      void sectionTitle(string str)
      {
        y += 20f;
        Txt txt = builder.NewTxt("txt").SetText(str).SetTextStyle(new TextStyle(CreditsScreen.GRAY, 22, new FontStyle?(FontStyle.Normal))).SetAlignment(TextAnchor.UpperCenter).SetPreferredSize();
        txt.PutRelativeTo<Txt>((IUiElement) content, txt.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y));
        y += txt.GetHeight() + 10f;
      }

      Txt createNamesTxt(IEnumerable<string> names, int fontSize = 22, bool enableRichText = false)
      {
        Txt namesTxt = builder.NewTxt("txt").SetTextStyle(new TextStyle(CreditsScreen.WHITE, fontSize, new FontStyle?(FontStyle.Bold))).SetAlignment(TextAnchor.UpperCenter);
        if (enableRichText)
          namesTxt.EnableRichText();
        namesTxt.SetText(names.JoinStrings("\n")).SetPreferredSize();
        return namesTxt;
      }

      void listNames(
        string[] names,
        bool sort,
        Func<int, string[], string[]> preProcessColumn = null,
        bool enableRichText = false)
      {
        names = names.MapArray<string, string>((Func<string, string>) (x => x.ToUpperInvariant()));
        if (sort)
          Array.Sort<string>(names, (IComparer<string>) StringComparer.Ordinal);
        if (preProcessColumn == null)
          preProcessColumn = (Func<int, string[], string[]>) ((i, x) => x);
        if (names.Length > 14)
        {
          int num = names.Length.CeilDiv(3);
          Txt namesTxt1 = createNamesTxt((IEnumerable<string>) preProcessColumn(0, names.Slice<string>(0, num)), 16, enableRichText);
          namesTxt1.PutRelativeTo<Txt>((IUiElement) content, namesTxt1.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y).AddRight(300f));
          Txt namesTxt2 = createNamesTxt((IEnumerable<string>) preProcessColumn(1, names.Slice<string>(num, num)), 16, enableRichText);
          namesTxt2.PutRelativeTo<Txt>((IUiElement) content, namesTxt2.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y));
          Txt namesTxt3 = createNamesTxt((IEnumerable<string>) preProcessColumn(2, names.Slice<string>(2 * num)), 16, enableRichText);
          namesTxt3.PutRelativeTo<Txt>((IUiElement) content, namesTxt3.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y).AddLeft(300f));
          y += namesTxt1.GetHeight();
        }
        else if (names.Length > 5)
        {
          int num = names.Length.CeilDiv(2);
          Txt namesTxt4 = createNamesTxt((IEnumerable<string>) preProcessColumn(0, names.Slice<string>(0, num)), enableRichText: enableRichText);
          namesTxt4.PutRelativeTo<Txt>((IUiElement) content, namesTxt4.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y).AddRight(150f));
          Txt namesTxt5 = createNamesTxt((IEnumerable<string>) preProcessColumn(1, names.Slice<string>(num)), enableRichText: enableRichText);
          namesTxt5.PutRelativeTo<Txt>((IUiElement) content, namesTxt5.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y).AddLeft(150f));
          y += namesTxt4.GetHeight();
        }
        else
        {
          Txt namesTxt = createNamesTxt((IEnumerable<string>) preProcessColumn(0, names), enableRichText: enableRichText);
          namesTxt.PutRelativeTo<Txt>((IUiElement) content, namesTxt.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y));
          y += namesTxt.GetHeight();
        }
      }

      Txt createCustomText(string str, TextStyle style, bool enableRichText = false, TextAnchor anchor = TextAnchor.UpperCenter)
      {
        Txt txt = builder.NewTxt("txt");
        if (enableRichText)
          txt.EnableRichText();
        return txt.SetText(str).SetTextStyle(style).SetAlignment(anchor).SetPreferredSize();
      }

      void customText(string str, TextStyle style, bool enableRichText = false, TextAnchor anchor = TextAnchor.UpperCenter)
      {
        Txt customText = createCustomText(str, style, enableRichText, anchor);
        customText.PutRelativeTo<Txt>((IUiElement) content, customText.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y));
        y += customText.GetHeight();
      }

      void sectionWithNames(string section, string[] names, bool sort)
      {
        sectionTitle(section);
        listNames(names, sort);
      }

      float trNames(string names, int offset)
      {
        Txt txt = builder.NewTxt("txt").EnableRichText().SetText(names).SetTextStyle(new TextStyle(CreditsScreen.GRAY, 16)).SetAlignment(TextAnchor.LowerCenter).SetPreferredSize();
        txt.PutRelativeTo<Txt>((IUiElement) content, txt.GetPreferedSize(), HorizontalPosition.Center, VerticalPosition.Top, Offset.Top(y).AddLeft((float) offset));
        return txt.GetHeight();
      }
    }

    private static string[] addFunnyPhrases(int colI, string[] names)
    {
      string[] sourceArray;
      int num;
      switch (colI)
      {
        case 0:
          sourceArray = new string[6]
          {
            "<color=" + CreditsScreen.GRAY.ToHex() + ">",
            "Did you know that one",
            "of the internal names",
            "for this game was",
            "\"Capitalio\"?",
            "</color>"
          };
          num = 2 * names.Length / 4;
          break;
        case 1:
          sourceArray = new string[5]
          {
            "<color=" + CreditsScreen.GRAY.ToHex() + ">",
            "Wow, so many",
            "generous supporters!",
            "Thank you!</color> <color=" + ColorRgba.DarkRed.ToHex() + "><3</color>",
            ""
          };
          num = names.Length / 4;
          break;
        default:
          sourceArray = new string[5]
          {
            "<color=" + CreditsScreen.GRAY.ToHex() + ">",
            "Are you still here?",
            "Your excavators",
            "might be idle!",
            "</color>"
          };
          num = 3 * names.Length / 4;
          break;
      }
      string[] destinationArray = new string[names.Length + sourceArray.Length];
      Array.Copy((Array) names, 0, (Array) destinationArray, 0, num);
      Array.Copy((Array) sourceArray, 0, (Array) destinationArray, num, sourceArray.Length);
      Array.Copy((Array) names, num, (Array) destinationArray, num + sourceArray.Length, names.Length - num);
      return destinationArray;
    }

    private static void getStringsOfTranslators(
      out string names1,
      out string names2,
      out string names3)
    {
      KeyValuePair<string, string[]>[] array = ((IEnumerable<KeyValuePair<string, string[]>>) CreditsScreen.COMMUNITY_TRANSLATORS_NAMES).OrderBy<KeyValuePair<string, string[]>, string>((Func<KeyValuePair<string, string[]>, string>) (x => x.Key), (IComparer<string>) StringComparer.Ordinal).ToArray<KeyValuePair<string, string[]>>();
      int num1 = ((IEnumerable<KeyValuePair<string, string[]>>) array).Sum<KeyValuePair<string, string[]>>((Func<KeyValuePair<string, string[]>, int>) (x => 2 + x.Value.Length));
      int num2 = num1 / 3;
      int num3 = 0;
      int index;
      for (index = 0; index < array.Length; ++index)
      {
        int num4 = 2 + array[index].Value.Length;
        if (num2 - num4 >= 0)
        {
          ++num3;
          num2 -= num4;
        }
        else
          break;
      }
      int num5 = num1 / 3;
      int count = 0;
      for (; index < array.Length; ++index)
      {
        int num6 = 2 + array[index].Value.Length;
        ++count;
        num5 -= num6;
        if (num5 <= 0)
          break;
      }
      names1 = selectNames(array.Slice<KeyValuePair<string, string[]>>(0, num3));
      names2 = selectNames(array.Slice<KeyValuePair<string, string[]>>(num3, count));
      names3 = selectNames(array.Slice<KeyValuePair<string, string[]>>(num3 + count));

      static string selectNames(KeyValuePair<string, string[]>[] values)
      {
        return ((IEnumerable<KeyValuePair<string, string[]>>) values).Select<KeyValuePair<string, string[]>, string>((Func<KeyValuePair<string, string[]>, string>) (x =>
        {
          string str = ((IEnumerable<string>) x.Value).Select<string, string>((Func<string, string>) (y => y.ToUpperInvariant())).OrderBy<string, string>((Func<string, string>) (y => y), (IComparer<string>) StringComparer.Ordinal).JoinStrings("\n");
          return x.Key + "\n<b><color=#ffffff>" + str + "</color></b>\n";
        })).JoinStrings("\n");
      }
    }

    public void Destroy() => this.Canvas.GameObject.Destroy();

    static CreditsScreen()
    {
      xxhJUtQyC9HnIshc6H.OukgcisAbr();
      CreditsScreen.WHITE = ColorRgba.White;
      CreditsScreen.GRAY = ColorRgba.LightGray;
      CreditsScreen.FOUNDERS_NAMES = "Marek Fišer  <size={0}><color={1}>and</color></size>  Filip Pavliš";
      CreditsScreen.ART_NAMES = new string[3]
      {
        "Miroslav Jersenský",
        "Ognyan Bozhilov",
        "Tony Kihlberg"
      };
      CreditsScreen.PROGRAMMING_NAMES = new string[2]
      {
        "Jeremy Appleyard",
        "Collin Arnold"
      };
      CreditsScreen.COMMUNITY_MANAGEMENT_NAMES = new string[1]
      {
        "Zak Frazier"
      };
      CreditsScreen.SOUND_NAMES = new string[1]
      {
        "Ondřej Matějka"
      };
      CreditsScreen.SPECIAL_THANKS_NAMES = new string[3]
      {
        "Charlota Santini",
        "Jiří Královec",
        "Kejie Bao"
      };
      CreditsScreen.STATS_DATA = new KeyValuePair<string, string>[6]
      {
        new KeyValuePair<string, string>("Simulation", "Custom engine"),
        new KeyValuePair<string, string>("Rendering", "3rd party engine"),
        new KeyValuePair<string, string>("Lines of C# code", "500 000+"),
        new KeyValuePair<string, string>("Unit/regression tests", "4000+"),
        new KeyValuePair<string, string>("3D models", "750+"),
        new KeyValuePair<string, string>("Textures/icons", "3500+")
      };
      CreditsScreen.COMMUNITY_MODS_NAMES = new string[6]
      {
        "McRib",
        "Febejo",
        "Renncifer",
        "Mathijs",
        "Voske_123",
        "NZ_Wanderer"
      };
      CreditsScreen.CONTENT_CREATORS_NAMES = new string[12]
      {
        "Aavak",
        "Nilaus",
        "SpielbaerLP",
        "Orbital Potato",
        "Dan Field",
        "Glidercat",
        "JD Plays",
        "bbaljo",
        "Charlie Pryor",
        "Cringer",
        "BigWolfChris",
        "CaptainTech"
      };
      CreditsScreen.WIKI_CONTRIB_NAMES = new string[9]
      {
        "Thadius856",
        "Phoenixx",
        "NaK",
        "L32",
        "Vekking",
        "Konsi3108",
        "Jurryaany",
        "Brammigamer",
        "ABravePanda"
      };
      CreditsScreen.COMMUNITY_TRANSLATORS_NAMES = new KeyValuePair<string, string[]>[19]
      {
        new KeyValuePair<string, string[]>("Catalan", new string[2]
        {
          "Marc",
          "Juan José Navarro"
        }),
        new KeyValuePair<string, string[]>("Czech", new string[3]
        {
          "Dominik Bednář",
          "Mana",
          "Ondřej Brumar"
        }),
        new KeyValuePair<string, string[]>("German", new string[6]
        {
          "Moritz",
          "McRib",
          "Stefan Stritzke",
          "Arnold Thermann",
          "Fabian Bassi",
          "Olli"
        }),
        new KeyValuePair<string, string[]>("Spanish", new string[4]
        {
          "Miguel Ángel Cordero Marcos",
          "Fabio delgado",
          "Darko's Gaming",
          "Aaron Afonso Urbano"
        }),
        new KeyValuePair<string, string[]>("Estonian", new string[1]
        {
          "Fred Elkind"
        }),
        new KeyValuePair<string, string[]>("French", new string[8]
        {
          "Thomas Herr",
          "Marc Blumenstein",
          "Guillaume Houplain",
          "Xavier",
          "omycron",
          "Marc-André Chabot",
          "Jean-Louis",
          "Bernard imbeault"
        }),
        new KeyValuePair<string, string[]>("Hungarian", new string[5]
        {
          "Balazs Lengyel",
          "Németh Dávid",
          "Molnár László",
          "Hopper István",
          "Nagy Róbert"
        }),
        new KeyValuePair<string, string[]>("Italian", new string[2]
        {
          "Christian Benevento",
          "Michele Congia"
        }),
        new KeyValuePair<string, string[]>("Japanese", new string[3]
        {
          "Mechanical Hive Lives",
          "Frio",
          "Read air"
        }),
        new KeyValuePair<string, string[]>("Korean", new string[4]
        {
          "KaiLi",
          "Lynette",
          "Rean",
          "MNB"
        }),
        new KeyValuePair<string, string[]>("Dutch", new string[2]
        {
          "Mathijs",
          "Manuel de Heer"
        }),
        new KeyValuePair<string, string[]>("Polish", new string[7]
        {
          "Chalwa",
          "Patryk Trojanowski",
          "Krzysztof Ciszewski",
          "Patryk Górecki",
          "Tlx",
          "Wiktor Adamczyk",
          "Bartosz Mikołajczyk"
        }),
        new KeyValuePair<string, string[]>("Portugese (Brazilian)", new string[4]
        {
          "Thiago Oshima",
          "Ives Matheus Añes de Souza",
          "Victor Bortolucci",
          "Marlon Peterson Macedo Silvério Bueno"
        }),
        new KeyValuePair<string, string[]>("Russian", new string[6]
        {
          "Yan",
          "Алекс",
          "Dmitrii Barabanov",
          "nh",
          "Denis",
          "Kashtan"
        }),
        new KeyValuePair<string, string[]>("Swedish", new string[2]
        {
          "Febejo",
          "J Karlsson"
        }),
        new KeyValuePair<string, string[]>("Turkish", new string[2]
        {
          "dbl2010",
          "Te7557"
        }),
        new KeyValuePair<string, string[]>("Ukrainian", new string[3]
        {
          "Maksym",
          "Mykyta",
          "Oleksandr Kabachynskyi"
        }),
        new KeyValuePair<string, string[]>("Chinese (simplified)", new string[1]
        {
          "Kejie Bao"
        }),
        new KeyValuePair<string, string[]>("Chinese (traditional)", new string[5]
        {
          "Tass",
          "Tony",
          "鄭瑞杰",
          "Lo Yuan-Chen",
          "薄荷"
        })
      };
      CreditsScreen.BACKERS_NAMES = new string[1388]
      {
        "147263589",
        "3rw1n_89",
        "3vilPcDiva",
        "4WheelsWolf",
        "7ower3efender",
        "8bitnintendo",
        "[IOP] BobTheBuilder",
        "A.Be.Whiskey",
        "Aahzmandius Karrde",
        "Aaron Butt",
        "Aaron Cassella",
        "Aaron McGill",
        "Aaron Morris",
        "Aaron Pool",
        "Aaron Young",
        "Abracadavid",
        "Abraham Jiménez Snow IV",
        "AceNZ",
        "AchLassDoch",
        "Adam 'Yamakiroshi' Hay",
        "Adam Frigerio",
        "Adam McLeod",
        "Adam Szijgyarto aka Dosi",
        "AdamEPlays",
        "Adenn",
        "Admiral Old Thrasbarg",
        "Admiral Stino",
        "Admiral Zeek",
        "AdmiralSuirrel",
        "Adrian Brown",
        "Aeldred",
        "Agrau",
        "aimbotd",
        "AIMLESSINLIFE",
        "Ajax4052",
        "AkumaArmy",
        "Albin3k",
        "Aldiss5",
        "Alejandro Guayaquil",
        "Aleksis L.",
        "Alex 'SuperRodge' Rodgers",
        "Alex H",
        "Alex H.",
        "Alex Hill",
        "Alex Riley",
        "Alex Rustick",
        "Alex Scherer",
        "Alex Schlatter",
        "Alexander",
        "Alexander Acri",
        "Alexander Goncz",
        "Alexander Ranum Nilsen",
        "Alexander Soto",
        "alexander Tugend",
        "Alexandre CALMES",
        "Alexandre Genin",
        "AlgorRhythmX",
        "allas",
        "Alton Coe",
        "AmbientKiller",
        "Amled",
        "Ancio",
        "Anda",
        "Andaria",
        "Andre 'Barosh' Rosenberg",
        "Andre Jäger",
        "Andre Kurbjun",
        "Andreas",
        "Andreas Lindström",
        "Andrei Usenka",
        "Andrew",
        "Andrew Ebert",
        "Andrew Gipson",
        "Andrew J Dove",
        "Andrew P Chehayl",
        "Andrew Plays",
        "André Nordstokke",
        "Andy 'Ruckdog' Rucker",
        "Andy Lowe",
        "Andy Richards",
        "Andy the Mugen",
        "Andy Wood",
        "Andy77HL",
        "Annmarie Bell",
        "AnodeCathode",
        "Anonymous",
        "Anthony Aldcroft",
        "Anthony Thompson",
        "Antiker86 Fluffels",
        "Antonio Pereira García",
        "Aosek_cz",
        "ApocalypticApes",
        "Aradil Sunder",
        "Arcanopolis",
        "Architect_Blasen",
        "Ariedien",
        "Aristomache",
        "Armel Hotin",
        "Arne Callewaert",
        "Arne van der Lei",
        "Arnold Thermann/HerrKaleu",
        "Arnold Waasdorp",
        "ArokD",
        "Arthur Hammeke",
        "Arthur Stathoulis",
        "ArxArmA",
        "Ash Dobie",
        "Ashen",
        "asoftbird",
        "Atlas1990",
        "Aussie Joel",
        "AustereGrim",
        "Austin Clark",
        "Austin Power",
        "Averageaussiegamr",
        "Awaggy13",
        "Axel Hedvall",
        "Azanek",
        "Azzofan",
        "B. Mahoney",
        "B3nji_Playz",
        "babo",
        "BaDHABiT",
        "Barley Barbarossa",
        "Barry Robert Coleman",
        "Bartos",
        "Basox70",
        "Bass637",
        "Beathoven",
        "Beemer",
        "Bence Harcsa #Parivan",
        "Benjamin 'Cats' Eggert",
        "Benjamin Kröper",
        "Benjamin Merz",
        "BensseG",
        "Bernersenner",
        "Bernhard Reiber",
        "BiddyG",
        "BigPapaTanker",
        "BigWolfChris",
        "Billy & Finnley N",
        "BinaryKali",
        "Birbman",
        "Bjoern B.",
        "blackrequiem",
        "Blas Borde",
        "Blastmetal",
        "Blaubart",
        "BlindBruellaffe",
        "Blkdragon112",
        "Blockletsplay",
        "Bloodsheed",
        "Blottoo aka H-Man",
        "Bluethunder",
        "Bob Le FOU 55",
        "Bob1103",
        "Bobby Zuber",
        "Boberix",
        "Bobiika",
        "Boendalin Skjorsson",
        "Boothieboy",
        "Boredtrucker209",
        "Boris Bosnjak",
        "Borschi",
        "bradk2588",
        "Bradley Clay",
        "Braeden",
        "Brandner Wolfgang",
        "Brandon B.",
        "Brandon Brewster",
        "Brandon Kelly",
        "Brandon Roberts",
        "Breaking_Boell",
        "Brendon OToole",
        "Brethern",
        "Brett (ilsey188)",
        "Brett Hammel",
        "Brian 'Shaggy' McPartlin",
        "Brian P",
        "Brian Papile(PaPPy)",
        "Brian Smith",
        "BrianWChan",
        "Broken Knight",
        "BrrIce",
        "Bruinsma Excavating Ltd.",
        "Bruno Freitas",
        "Bruwt Force",
        "Bryan White",
        "bryanfury37",
        "Brycarno",
        "Bunnysanp",
        "Burnrate",
        "Buzzorth",
        "Børje Solvang",
        "C. Morning",
        "c0nnex",
        "Caeasarus",
        "Cajun comrade",
        "Calgar106",
        "Callum Jones",
        "Cameron Yeingst",
        "Canaan Al'Shala",
        "Captain Ferret",
        "CaptainKorker",
        "Captbone",
        "Carson Hawkins",
        "Cbates670",
        "CdnSpecialist",
        "Cedrik Vanwalleghem",
        "CeKanix",
        "Celebrian",
        "CementCatX",
        "certainOrder",
        "cha0tics",
        "Chad Schilling",
        "Chairman_Stanley Poston Jr",
        "Champalier",
        "Cheeesechunk",
        "CheshireesCat",
        "ChewyGranola",
        "ChibiclubTD",
        "Chibiketan",
        "Chikenade",
        "Chilled Gammer",
        "Chris Ball (Fallen Paladin)",
        "Chris Britt",
        "Chris Campo",
        "Chris Hochscheidt",
        "Chris McCoy",
        "Chris Neijnens",
        "Chris Simcock",
        "Chris Stanyon",
        "Chris Zeke To Tall trucker gaming",
        "Christian",
        "Christian Alex Breitenstein",
        "Christian Hentschel",
        "Christian Raspe",
        "Christoph Felix",
        "Christophe Defrain",
        "Christopher 'Kaj' Vaughn",
        "Christopher G Webb",
        "Christopher Hansen",
        "Claud Harmon",
        "Claudio Gadient, Switzerland",
        "Clothulhu",
        "Cody Allen",
        "Cole Beals",
        "Cole Sandidge",
        "Comet Streak The Deer Dergy",
        "CommanderShepard",
        "ComputerComa",
        "Comrade Silver",
        "Connor",
        "Connor 'HP' Kelly",
        "Connor Doornbos (DorniNerd)",
        "CoolFace",
        "Coolgye1801",
        "Coranark",
        "Corey Miller",
        "cpjet64",
        "Craig 'Wizzo' Fletcher",
        "Crisjeeeee",
        "Crouser88",
        "Crusi Gurbux",
        "Cryptoman100",
        "Curtis 'Blank_Savant' Kelly",
        "cy-one",
        "Cybarox",
        "Cyric313",
        "Cyril Schafflützel",
        "D Cross",
        "D00dDk",
        "Dafader",
        "DagranMc",
        "Dalton Wells",
        "Damien (Deathtopia) Mcskimming",
        "Damien Otis",
        "Damien Saubesty",
        "Dan Poulter",
        "Dan Reed",
        "dan9186",
        "Daniel Beck",
        "Daniel Carbajosa",
        "Daniel Davis",
        "Daniel Grewdahl",
        "Daniel J Havens Jr",
        "Daniel Liljekvist",
        "Daniel Marban",
        "Daniel Perry",
        "Daniel Radschun",
        "Daniel Seller",
        "Daniel Spaar",
        "Daniel Stahl",
        "Danny 'RevsUpRayes' Peters",
        "dapez",
        "DapperRex Studios",
        "Dario H. - Groover",
        "Dario Seifert",
        "Darken Nekros",
        "Darren Hobbs",
        "darren sharp",
        "Darth Wheaton",
        "Daryl Crain",
        "Dash Peters",
        "DatSparrow",
        "Dave Li",
        "Dave Sharman",
        "David Bernard",
        "David Brummer",
        "David Cumps",
        "David L. Ensign",
        "David Leather",
        "David Linscott",
        "David M. Holman",
        "David Pelichet",
        "David Pickard",
        "David Prince",
        "David Rosa",
        "David Stocker",
        "David Wolf",
        "Davidbest",
        "DavidT",
        "Davy Payen",
        "Dax",
        "dcjammer",
        "DeadlyDonuts",
        "Dean 'Tank' Morris",
        "DeathKnight250",
        "DeathRoams",
        "delias anthony",
        "Delvan",
        "Dennis Koopmann",
        "Denny 'Downtown' Brown",
        "Derek 'Gutentosh' Rose",
        "Derrickspartan1",
        "derStrategieNerd",
        "Dewboy101",
        "Dezboyrecordz",
        "DGECOMBAT",
        "Dgtlreaper",
        "Dirk Burger",
        "Doberman61CZ",
        "Doc Hogan",
        "Doesitbehind",
        "DokGrotznik",
        "Dokton",
        "DonB",
        "Donnti",
        "DooM98B13",
        "doomimp93",
        "Doomtone",
        "DOSFox",
        "Doug Seacrist",
        "DpierreS",
        "Dr_Blech",
        "DragoKen",
        "DragonKnight",
        "Drake narvaiz",
        "Drazzle",
        "DreadLord911",
        "Dren Throwdown",
        "Drew Swint",
        "Drew1PKT",
        "Ducks_Nero",
        "Dunbal",
        "Dunbant",
        "Duncanel",
        "Duratath",
        "Dustin D.",
        "Dustin W. Fisher",
        "dustinsplace",
        "Dwatsch",
        "Dwayne Taresh",
        "dwzivic",
        "Dynar",
        "e9",
        "Earl Squirrelson",
        "Ed Rochenski",
        "Eddie D Lowe",
        "Eddrick",
        "Edgar Law",
        "Einsteinert",
        "Eky",
        "Elesh",
        "Elouda Lightstar",
        "EndivaGER",
        "Ep1cShovel",
        "Erakol",
        "Eric 'Ergrak' Cable",
        "Eric Haarstad",
        "Eric Liggett",
        "Erick Christgau",
        "Ernest Murry",
        "Eryk Sołowiej",
        "Eugenio García",
        "euroyak",
        "Eveline De Ridder",
        "Evgenii Maksimov",
        "evilzug",
        "ExaltedEmu",
        "Exile1st",
        "ExultantShadow",
        "Ezra Sharp",
        "Ezzuqi",
        "Fabian",
        "Fabian Benzing",
        "Fabian DerGamer",
        "Fabian Freund",
        "FaCeLe5s",
        "Fadelessdeer55",
        "FallenSunGaming",
        "Farantis",
        "Farg_Ink_Uhl",
        "Fattercow",
        "Felix Bender",
        "Fellowslothb",
        "Felten",
        "Fenrii",
        "Ferdinand Röder",
        "Fernando",
        "FGsquared",
        "FibermanCz",
        "fidschi",
        "Figard Yoann",
        "Firasia",
        "Firemmann",
        "Fiskpinne",
        "Fivox",
        "Floreit",
        "Florent B",
        "Florian Scheinast",
        "Florian Schneider",
        "Fluffy Pinecone",
        "flying_panzer",
        "Foofles",
        "Forbid2",
        "Fox-Alpha",
        "Frami58",
        "Frank Hoffer",
        "Frank Iaquinto",
        "Frank Knabe",
        "Frank Mazza (Dragon327)",
        "Frank Wächter",
        "FrankGTB",
        "Frankimschrank",
        "FrAsS BrAsS",
        "Freak[TIK]",
        "Fred Schmid",
        "Freddoccino",
        "Freddykrüger",
        "Fredrik Svedberg",
        "FreedomFries",
        "Fregate84",
        "Fritz Lachmayr",
        "Gamgroll",
        "Gardiner, Brandon.",
        "Garret M",
        "Garrett Kabasa",
        "Gary Bertagnolli",
        "Gary Lewis",
        "Gary Locker",
        "Gary Rantz",
        "Gavin Wakefield",
        "Gaz Baker",
        "GejzirSoft",
        "Geoff Riley",
        "Geoffery Gordon",
        "Geordie Wiliams",
        "Georg Pichlmaier",
        "George Lykovitis",
        "George Pryor",
        "George Rowe",
        "Germanfragger",
        "Gersch",
        "Gevatters Djinn",
        "Ghezra",
        "Gianluca Varoli",
        "GigaWatt",
        "Gilles Bourgault",
        "Gingeroot",
        "Gizrah",
        "Glanggeddin",
        "Glufforg",
        "Gnrnr",
        "God's Favorite Ant",
        "Godfrey Tables",
        "GoldenBPrime",
        "Goose",
        "Gordo",
        "GottVater",
        "Grant R Parsons",
        "Gray Beers",
        "Greatcatania",
        "Greene",
        "GreenFox97",
        "Greg 'Ardias' Heaney",
        "Grendza-Donsky F.",
        "Grisu118",
        "Grooty - Philipp Rollett",
        "gropingforelmo",
        "GrumpyDee",
        "Gunsmithy",
        "Gérôme Wüthrich",
        "h3killa",
        "H4uklotz",
        "Hactarus",
        "Hamza Zafar",
        "Hanfei",
        "Hannes Le.",
        "Hannibal Donner",
        "hannyyjj",
        "Hans Christian 'Argondo'",
        "Hans Holmström",
        "Hans-Christian Braun",
        "Hanz55",
        "Hardy Family",
        "Harley S. aka: Does It Matter?",
        "hasecbinusr",
        "Hassan Ibraheem",
        "Hauke R.",
        "Hawkeye",
        "Haxor94",
        "HD_Kenny",
        "Heavens",
        "Hector",
        "heero1407",
        "Helge Kværnø",
        "HellWatcher",
        "Henrik Aaholm",
        "Hikaro Thojan",
        "Hikaru Usada",
        "hiphoss",
        "HoardusMaximus",
        "Hofu",
        "Honoured Enemy",
        "Honvik",
        "HoratioTV",
        "hover-227",
        "Howard C Newton",
        "hylko",
        "Hyperbyte",
        "IamtheBurnt",
        "Ian Kearney",
        "Ian Snow",
        "Icedcoldthie",
        "IceDragon",
        "Ieuan Kessels",
        "Igor RussKie",
        "IIIskywalkIII",
        "Illusion13",
        "ILLusive",
        "Imper1um",
        "infiniteseries",
        "Infotron",
        "ionatan83",
        "Ironeye",
        "iseac weller",
        "Isellacuras2",
        "ItsDarthChaos",
        "itsnotDave",
        "itworld.manager",
        "Ivan Jurkovic",
        "Ivan the Terrible",
        "J. Perry, P.E.",
        "J.W.Thomassen",
        "Jackson Johnston",
        "Jacob Palnick",
        "Jake Rylander",
        "Jakll",
        "James",
        "James 'PlumYeti' Owens",
        "James kempson",
        "James Thorp",
        "James Ulrich",
        "Jamie 'Macros' Carter",
        "Jan Erik Lisund",
        "Jan Horsky",
        "Jan Ribbink",
        "Jan Tuček",
        "Jan-B",
        "Jan-Niclas Matthiesen (Joe_Matthew)",
        "Jan-Phil Wagner",
        "japalie91",
        "jari de gamer",
        "Jaron Dempcy",
        "Jason 'IrishAssassinO1'",
        "Jason 'Jeteye' Rusch",
        "Jason 'NerdingAround' Cantu",
        "Jason Chandler",
        "Jason Cornellier",
        "Jason McClung",
        "Jason Omega001 Heynoski",
        "Jason Philipp",
        "Jason Van Cleave",
        "Jason Williams",
        "Jauser9",
        "jaxfrank",
        "Jayson Ponger",
        "Jean-Laurent Gendre",
        "Jeb",
        "Jeff Chapman",
        "Jeff Ferland",
        "Jeff Walter",
        "Jeffrey Cuppens",
        "JellyPotato",
        "Jelmergu",
        "Jeremiah Richardson",
        "Jeremy Adams (Aerient)",
        "Jeremy Lapp",
        "Jeroen Gaykema",
        "Jesper Bengtsson DK",
        "Jess W. Yost",
        "Jesse Konell",
        "Jesse LaPuma",
        "Jesse Smith",
        "Jhustblue",
        "JigsawJohn",
        "Jim Pryde",
        "Jimmie Gyllensteen",
        "jimmyjon711",
        "jjd5150",
        "Joakim Norman",
        "joao almeida",
        "joao miguel",
        "Jochen Gaule",
        "Joe 'kami' Kiles",
        "Joe Fusco",
        "Joel Merk",
        "Jofblood",
        "Johan Fogel",
        "Johannes Böhm (Syraneus)",
        "John 'AcesofDeath7' Mullens",
        "John Henson",
        "John Robert Gardner",
        "John Vanderbeck",
        "Johnathan Brown",
        "Johnpaul Probett",
        "JokerZ",
        "Jon Lunger",
        "Jonas Nilsson",
        "Jonas Pick",
        "Jonathan Bony",
        "Jonathan Donaldson",
        "Jonathan Gillette",
        "Jonathan Page",
        "Jonathan Sider",
        "Jonathan Yong",
        "Jonathon Forgeson",
        "joram pater",
        "Jordan Hepton",
        "Jorn van Moolenbroek",
        "Joseph C McGinty Jr",
        "Josh E",
        "Joshua V White",
        "JPerera",
        "Juan 'Chapel' Guzman",
        "Juergen Graebner",
        "Julian Maas",
        "Jurriaan van Ingen",
        "Justin Turriff",
        "Justin Wheeler",
        "Justin.g.H",
        "Jutnik",
        "Jye 'JyeGuru' Karl-Perry",
        "KaeTzcHieN",
        "KageKeeper",
        "Kagemura212 ~ Gradyjoe123",
        "Kaitlin",
        "Kaladur Ironforge",
        "KaMat",
        "Kamil Kachniarz 'Wilkolaczek'",
        "Kane",
        "Kaozmarine",
        "Karagos01",
        "Karl",
        "Kasey D.",
        "KaSo312",
        "Kaylee Beals (best wife ever)",
        "Kayleigh Adams",
        "Keegan Finn",
        "Keegan Pahl",
        "KEGJR",
        "Kelhek",
        "Ken 'Uxan' Shaffer Jr",
        "Kerminator",
        "Keru90",
        "Kevin Forman",
        "Kevin Potter (Klorf)",
        "Khaymenn",
        "Kholdan Staalstorm",
        "Kiky Landskron",
        "Kilinater",
        "Killdead (lore)",
        "Killer_369",
        "KingTr011",
        "Kizzy",
        "Klather",
        "Klaus_Ferrano",
        "kleiner_penner",
        "Koala With Hat",
        "koimeiji",
        "Konstantin Feichtinger",
        "Koubik",
        "Kris Hanson",
        "Kris Møllegaard",
        "Kris Pukdam",
        "Kristiana Moretti",
        "KristoffVB",
        "Krizzoula",
        "Kroddn",
        "Krokodiel89",
        "KROOKED",
        "Krtek",
        "kubeX",
        "Kuma supesu",
        "Kyle Flewellen",
        "Kyle Schiffbauer",
        "Kyle Schmidt",
        "Kámoš Drak",
        "Lakota",
        "Lance McComber",
        "LancelotTheLion",
        "Lara & Markus",
        "Larold",
        "Lars Grønset",
        "Lars Schreck",
        "Lars Walloscheck",
        "LateNightLounge",
        "Laurent",
        "Laurent Chervet aka Psychokiller1888",
        "LeBanditelle",
        "Lee 'Smiffles' Smith",
        "Lee Crabtree",
        "Lee Wilkinson",
        "Leftbak",
        "Leggo",
        "lejonet",
        "Lemy",
        "Leomedes",
        "leon king",
        "LeotoTV",
        "Lewis Guild",
        "LGnap",
        "Liam Welbourn",
        "LIE STFandring",
        "Lifernal",
        "LimitLessLindi",
        "Ling.Long.",
        "Liss Brunne",
        "Lithiea Litmar",
        "ljej1989",
        "Locke",
        "Locke4",
        "Lockelocoo.tv",
        "Loepie",
        "Loggerhead Distillery",
        "LoicB",
        "LoneWolf2_0",
        "Lonnie Littleton",
        "Lord Filip",
        "LordLatimer",
        "Loris Balsamo",
        "Los",
        "Louka Colantoni / Nonojump",
        "LoungeLizard",
        "LoyalExpert",
        "Luca Campana",
        "LuckyP0S",
        "Ludo J",
        "Luke Dodson",
        "Luna4804",
        "Lupus",
        "Luzifer van Rattus",
        "LYKO",
        "lyndon_p",
        "Lynk Michael",
        "Lyra Vemane",
        "Lysander232",
        "M. Brantley",
        "Mac :)",
        "Macek StarNet",
        "mack_29_",
        "Mad Shadow",
        "Madcaz _85",
        "Madox Labs",
        "Maenhout stijn",
        "Magarine - Banaranama - newEarth",
        "Magiclight",
        "Magnus Valdemar (magn1220)",
        "Maik Lottmann",
        "Mak",
        "Malcolm Graves",
        "Malossi",
        "ManicCLo",
        "Manuel Rauber",
        "Marcel Miesen (Kanalgrufti)",
        "Marcin Witkowski",
        "Marco Arweiler",
        "Marco Cunha",
        "Marco V Horn",
        "Mark 'Stronkie' Stronks",
        "Mark McClelland",
        "Mark Stefaniak",
        "Mark van Dijk",
        "Mark Watt",
        "MarkMik",
        "Markus Knoch",
        "Markus Solberg",
        "Maron M Möller",
        "MarPer !!",
        "Marsman141",
        "Martin",
        "Martin 'Pentacore' Claesson",
        "Martin Grov",
        "Martin Peck",
        "Martin Sipek",
        "MastMan",
        "Matheus 'Almaravarion' Czogalla",
        "Mathew Billman",
        "Mathijs Franken",
        "Mathijs Van Dijck",
        "Matic87",
        "matrix751",
        "Matthew Campbell",
        "Matthew Morris",
        "Matthias Ehret alias SARUEGG",
        "Mattias Brändström",
        "Mattias Thander",
        "Matzi",
        "Maxime Callens",
        "Maximilian",
        "Maximilian Angersbach",
        "Maximilian Stahl",
        "Maxwell1509",
        "Mazian",
        "McZlik",
        "MEDER Florian",
        "Medieval Peanut",
        "Meerschaut Nick",
        "Mefistus",
        "MegaXiong",
        "Meik Kimmel",
        "Melana Nighthawk",
        "Memanoth",
        "Menomoree Rochhuru",
        "Mental Kitten",
        "MFoil",
        "Michael 'Arctos' Robbe",
        "Michael 'Rannulf' Hernandez",
        "Michael Elkoremarr Lodahl",
        "Michael Goldstein",
        "Michael Hitchner",
        "Michael Loftis",
        "Michael Malone",
        "Michael Moriarty",
        "Michael Ottmers",
        "michaelleahy",
        "Micheal_Knight",
        "Miggidimic",
        "Migmimi (FR)",
        "Miguel (xXNibeLungOXx) Martínez",
        "Mihai2",
        "Miikka L.",
        "Mikael G Karlsson",
        "Mikael Kristensen",
        "Mike Laidlaw",
        "Mike-KNOWFEAR",
        "MikeH86",
        "Miles Billington",
        "Ming",
        "MirageCentury",
        "Mizutayio",
        "MJ Skopczynski",
        "mkay",
        "Mohammed F. Forsad",
        "Mojijojy",
        "Monarghel",
        "Monopolyman",
        "Montybill",
        "MooCop",
        "Morehei",
        "Morten Minus Mikkelsen",
        "Morti",
        "Mortuos Saeculi",
        "moshauer",
        "MotleyHavoc",
        "Moustachio Mike",
        "MoX!",
        "Mr ginger",
        "Mr. Scheiby",
        "MrBoFrost",
        "MrChupa",
        "Mrhappy15",
        "MrMeevin",
        "MrPitti",
        "MrSmoofy",
        "MrWhiteUA",
        "mt94plays",
        "mutton44",
        "Muzzy",
        "Mystichawk",
        "n-j",
        "Nadeem Gulzar",
        "Nargon",
        "Nate R",
        "Nathan Bunn",
        "Necrogami",
        "Ned Carlson",
        "neverfirstplayer",
        "Nicco M.",
        "Nick-I'mSoLost-Edwards",
        "Nico",
        "Nico Stark",
        "Nicolai Zernikow",
        "Nicolas Ahmed",
        "Nicolas Guay",
        "Niestegge Philipp",
        "Nightmare",
        "NihilDexter",
        "Nilz",
        "Noah Slifer - VaporStrike",
        "Noliv3",
        "nomad-89",
        "NORBERT MAECHLER",
        "Nordlicht1981",
        "Nuno Nogueira",
        "obj",
        "Obliwerator",
        "ObsidianPuncher0",
        "ODI22",
        "Oggy",
        "OHALICHAN",
        "Ohio Bryce",
        "oli-obk",
        "Omicronus",
        "Ondra Chrapek",
        "Ondřej 'Behe' Brumar",
        "Opirian",
        "Orcrowing",
        "orlandu84",
        "Oscar Segura",
        "OutcastSurvival",
        "Overtron",
        "Owen Morris",
        "Pabl1t0",
        "Paddywagon",
        "PainDragon",
        "Pal Mezei",
        "PandaWoods321",
        "Papat",
        "Patrick",
        "Patrick 1188",
        "Patrick Savage",
        "Patrick Service",
        "Patrick zockt",
        "Pattex_49th",
        "Paul Wood",
        "Pchute",
        "Pecomica",
        "Pedro TUGA",
        "PeguenoCZ",
        "Perfacktion",
        "Peter",
        "Peter Blattman-White",
        "Peter Friese",
        "Peter Savransky",
        "PetitKaillou",
        "PetterG",
        "Pezkenin",
        "PhantomCrash45",
        "Phaos72",
        "Philip Lotz",
        "PhuriousGeorge",
        "Pim aka Triple_xD",
        "pinguinfuss",
        "Piotr 'ThePiachu' Piasecki",
        "Piotr Cymbalski",
        "Pioupiou",
        "PJH",
        "pk",
        "PlatinumDragon11",
        "Plautinumtoast",
        "Plexo",
        "Pnus",
        "poetter",
        "PoiTEE",
        "Polaris14",
        "Pontus-Pilatzius",
        "Poppaj7213",
        "Pops",
        "PORTALIER Nicolas",
        "PowerWeb4You Internet Diensten",
        "PreacherBro33",
        "proff_ch405",
        "Pronga",
        "Protoss32521",
        "PseudoSquirrel",
        "Psy",
        "Pyromaniac",
        "Qitama",
        "QQx",
        "Queen of Heels Jes",
        "Quyen D Tran",
        "R Hendriks",
        "R3eaper",
        "Rabid Manatee",
        "Radagast",
        "Raffael Cornacchia",
        "Ralle030583",
        "Ramasuri (Rafael)",
        "Ramon van den Berg",
        "Rangetanks",
        "Raphael 'Xloouch' Gnädinger",
        "Rapier8472",
        "Raprei",
        "Raskil2000",
        "Rastou de la Rastignette",
        "Ratty Daddy",
        "Raver10101",
        "Ray Huber",
        "Rayanth",
        "Raz Barba",
        "Razor Alkonir White",
        "RealHenkOne",
        "RED XIII",
        "RedstoneRider",
        "Reemaï",
        "REIGN",
        "Remon Poortinga (Poorty)",
        "René Batgerel",
        "René M",
        "Repenix",
        "Rhyno-93",
        "Rhys Palmer",
        "Richard Brown",
        "Richard de vree",
        "Richard G.",
        "Richard Gauthier",
        "Richard Kolk",
        "Richard Murillo",
        "Richard Nixon",
        "Richard OLVERA",
        "Rick Osborn",
        "Rickie",
        "Ricky Chandler",
        "Ricky Singh",
        "Riff Zifnab",
        "rikky",
        "Rob (R2) Radune",
        "Rob Crawford",
        "Robert Bremer",
        "Robert Brittain",
        "Robert Buchenrieder",
        "Robert F",
        "Robert Sjulander",
        "Robin Ortryd",
        "Robotaube",
        "rodSeb",
        "Roger Lindskog",
        "Roger Waters",
        "Rollbacksmiley",
        "Roman 'Arathir' Mikala",
        "Roman Bartl",
        "Roman Keller",
        "Rome159",
        "Ron Pepper",
        "Ronon101",
        "Roy Abbott",
        "Rudi",
        "Rudolf Coi",
        "RunRabbitRun",
        "Rush",
        "RussianUzi",
        "Ryan craig",
        "Ryan L (GuiltyCrownShu)",
        "Ryan Noble",
        "saberslay",
        "Sailorclaw63",
        "SalatPirat",
        "Salt Shaker",
        "SaltyBallz",
        "Sam Borrett",
        "Sammybravo007",
        "Sander 'Xyphearos' van Deijl",
        "Sanga",
        "Sanka",
        "Sapphire_Knight",
        "Saramir",
        "Sarravi",
        "Sascha Hotopp",
        "Sascha Thomas",
        "Sckeptyk",
        "Scotland Tom",
        "Scott 'Hoeney' Lusk",
        "Scott C. Zielinski",
        "ScoutNL",
        "Scrapy",
        "Scyn",
        "Sean 'Reload15' Mason",
        "Sean M Pyeatt",
        "Sean Pretorius",
        "Sean Seguin",
        "Sean Semple",
        "SeanCZ",
        "Sebastian 'Iceberg' Berg",
        "Sebastian Eckel",
        "Sebastian Fischer",
        "Sebastian Schmitz",
        "Sebastian Seck",
        "Sebastian Süpple",
        "Sebilupu",
        "SenbinoWerewolf",
        "SentiCZ",
        "Seonnyn",
        "SeriusLP",
        "Sero",
        "Seth Magee",
        "Seton",
        "severalboxes",
        "Sgaggero",
        "SH5_Kaleun",
        "ShadowGlass",
        "Shadowkiller609",
        "Shane Carroll",
        "Shannon French",
        "Shiny",
        "shiva chirr",
        "Shivz",
        "Shudder",
        "Sibirientiger",
        "Sierra <3",
        "Silent Speaker",
        "Silent_Sentinel",
        "Silko Pillasch",
        "Silver Chicken",
        "simomies",
        "Simon",
        "SImon Cannings",
        "Simon Orlando",
        "Simon Rennfanz (Renncifer)",
        "Simon Yngve",
        "Simone Tex Camerini",
        "Simple1",
        "Sir_Dawe",
        "SirKewlaid",
        "sirnolte",
        "SirPat",
        "Skalidor",
        "Skatmaan",
        "Skellitor301",
        "Skinneh",
        "Skippy",
        "SMC",
        "Smedo (Andreas BeLa)",
        "Smiter1983",
        "Snake",
        "sNiPeCZ",
        "Sockenschuss",
        "Sonk",
        "sopleb",
        "SozeDK",
        "spacedreamer1978",
        "Sparkly Kitten",
        "sparsedot",
        "Splunkhead",
        "SSG (Darkpiccolo)",
        "ssgtsniper",
        "Steef",
        "Stef Brouwer",
        "Stefan 'Chapi' Köhler",
        "stelaire",
        "Stephen Weiblinger",
        "Steve Belton",
        "Steve Boegly (arhyx)",
        "Steve Bourget",
        "Steve C.",
        "Steve Cole",
        "Steve Crowther",
        "Steve Dark",
        "Steve Meerschaut",
        "Steven Jones",
        "Steven Rychetnik",
        "Steven Touhig UK",
        "Stian 'Shepherd'",
        "Stian Laszenko-Larsen",
        "Stian Levik",
        "Stijn Janssens",
        "StoicNate",
        "Stormlund",
        "Strato",
        "StreamR",
        "Strif3",
        "Stuart Elam",
        "Stubbe3KDK",
        "StuLeo",
        "Subjekt-Henning",
        "Sure. Svein Sivertsen (CaptainTech)",
        "SuurflieG",
        "Svein Magne Jørgensen",
        "Sven S.",
        "Sven von den Berken",
        "Sven-I-Am",
        "SwissMustang",
        "Syberian (Chad B)",
        "Syphyx",
        "SZPYTMA Cyril",
        "T Nnunley",
        "T-Bass M'cool",
        "T2k3",
        "Tactical Weasel",
        "Tanner Herrmann",
        "Tarawind",
        "tattooed666",
        "Tauran1506",
        "Taylor Houser",
        "Tazsonic",
        "TDIcelander",
        "TechRoss",
        "TekkieWelt",
        "TenderSausage",
        "TerminalWander",
        "Terrana",
        "Terrance Ballinger",
        "TerrBear",
        "TerroRichard",
        "TH3xR34P3R",
        "Thanatos-System",
        "The Gennai Family",
        "The-DayZi aka. Philipp Lubahn",
        "The88mm",
        "The_Thyphoon",
        "Theebu",
        "TheeGrandpoobah",
        "TheHalfPotato",
        "TheKraigen",
        "TheMostInternet",
        "Theodorus Marx",
        "TheOldGuy",
        "TheOrigin",
        "TheRealButtpie (Innocent nickname:)",
        "Therealmig",
        "ThereWasAnError",
        "TheSheiken",
        "Thibaud.A",
        "Thim Jonasson",
        "Thomas Amadeus Dobes",
        "Thomas Genese",
        "Thomas Klitgaard (TheFlame)",
        "Thomas Meraire",
        "Thomas Simaels",
        "Thomas WJ van der Ploeg",
        "Thorin Fireforge",
        "Thorsten 'Thodian' Weigel",
        "Thorsten M.",
        "Tim Crisp",
        "Tim Krupka",
        "Tim Lane",
        "Timbadur",
        "TimDC",
        "Timo",
        "Timothy Mautz",
        "Tino",
        "Tipunch",
        "TISSIER Alexandre",
        "Tobias Polti",
        "Toby Dagan",
        "Todd 'squeak' Hendricks",
        "Todd Titch",
        "todihk",
        "Tom FireDemon",
        "Tom Gray",
        "Tomáš Mio Novák",
        "Tomáš Vlk",
        "Toni_Sotto",
        "Torbakjunior",
        "Torben Ritter",
        "Touriste",
        "Toxicat",
        "Tr0mp",
        "Travis",
        "Travis Henderson",
        "Trey Jetter",
        "Tristan Reinhoudt",
        "Tromlog",
        "tubby.nl",
        "Tubman1",
        "Tucker Nelson",
        "Tunnel49",
        "tw0blu3c5",
        "Twinky69",
        "Twitch-Icokeh2oTV",
        "TygerSon13",
        "Tyrias",
        "U-Bolt",
        "Umaade",
        "Un Sanglier",
        "UnknownMrE",
        "Uno Fonseca",
        "UpperValleyLeatherGoods",
        "UrgentOrange",
        "Uthrax",
        "Vaffel",
        "Valentin MATHIOT",
        "Valerie Stone",
        "Valorion",
        "Vampy Destroyer Of Worlds",
        "van_nostra",
        "Vartue",
        "Vegard Nilsen",
        "VegardBH",
        "Veilus",
        "Veylenn",
        "Ville Sivonen",
        "Vincent",
        "Vincent Mineault",
        "vincentbp53",
        "viper799",
        "Virgil 'Muffin' Shelby",
        "Virukas",
        "Void Reaper",
        "Voraus",
        "Voske_123",
        "Víctor García Fuentes",
        "Walker Enterprises",
        "Wallace & Bimbo",
        "Walter Kramer",
        "WarCookie",
        "WarMadMax",
        "Waveman55",
        "Wendy H",
        "WerK",
        "Wes",
        "Wesley van Opstal",
        "WhiskeyActual",
        "WhiteNet",
        "Whompratt",
        "Wiildy",
        "Wil James",
        "Wild Dog Gamer",
        "wildonerbua",
        "Wildthing",
        "Willem Fischer",
        "William 'Wilper' Persson",
        "William Snow",
        "Willie Evans",
        "Wink",
        "Wobbly Boater",
        "Wojciech 'Forien' Szulc",
        "wolf45tv",
        "wolfie7009",
        "Wolfshooting",
        "WolfteinSK",
        "WollysWolfPack",
        "Woozeranto",
        "WortWizard",
        "Wouter-Kenis",
        "Wyatt R",
        "Xbamze",
        "xDeekay",
        "Xertian",
        "xopion",
        "Xtreme61391",
        "Xxdeebsxx",
        "Y. Fedoryshyn",
        "YEET",
        "Yeller Belli",
        "Yoann Figard",
        "Yomantwo",
        "Yomoto",
        "youforce",
        "Yuudachi",
        "Zabulon aka Andre",
        "Zachary Czubachowski",
        "Zahnen",
        "Zalen Galeth",
        "Zdeněk Novotný (DeznekCZ)",
        "Zeddikis",
        "Zedle",
        "Zeke Metcalf",
        "Zemas",
        "Zemerson",
        "ZeroSilver",
        "zeuocool",
        "Zranite",
        "zroku",
        "zukfroader",
        "Zytukin",
        "|X|BP W@RST34M"
      };
    }

    private class CreditsScrollerMb : MonoBehaviour
    {
      public float ScrollSpeed;
      private ScrollableContainer m_scrollableContainer;

      public void Initialize(ScrollableContainer scrollableContainer, float normDistancePerSec)
      {
        this.ScrollSpeed = -normDistancePerSec;
        this.m_scrollableContainer = scrollableContainer;
      }

      private void Update()
      {
        if (this.m_scrollableContainer == null || Input.anyKey || Input.anyKey)
          return;
        this.m_scrollableContainer.ScrollBy(this.ScrollSpeed * Time.unscaledDeltaTime);
      }

      public CreditsScrollerMb()
      {
        xxhJUtQyC9HnIshc6H.OukgcisAbr();
        // ISSUE: explicit constructor call
        base.\u002Ector();
      }
    }
  }
}
