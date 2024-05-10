// Decompiled with JetBrains decompiler
// Type: Mafi.Core.Population.Edicts.Edict
// Assembly: Mafi.Core, Version=0.6.3.0, Culture=neutral, PublicKeyToken=null
// MVID: 2D94E377-7747-46A4-A766-4DF7ABFEBD30
// Assembly location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.dll
// XML documentation location: D:\GameLibrary\SteamLibrary\steamapps\common\Captain of Industry\Captain of Industry_Data\Managed\Mafi.Core.xml

using eLxG93FZl5M3yHxFGb;
using Mafi.Core.Buildings.Offices;
using Mafi.Core.Entities;
using Mafi.Core.Prototypes;
using Mafi.Core.Simulation;
using Mafi.Localization;
using Mafi.Serialization;
using System;

#nullable disable
namespace Mafi.Core.Population.Edicts
{
  public abstract class Edict
  {
    public readonly EdictProto Prototype;
    private bool m_active;
    private bool m_currentMonthAlreadyPaidFor;
    private readonly UpointsManager m_upointsManager;
    private readonly ICalendar m_calendar;
    private readonly CaptainOfficeManager m_captainOfficeManager;
    private int m_numberOfDaysActive;

    /// <summary>Whether the edict was enabled by the player.</summary>
    public bool IsEnabled { get; private set; }

    /// <summary>
    /// Whether the edict is active which mean it is enabled + all its prerequisites are satisfied.
    /// </summary>
    [DoNotSave(0, null)]
    public bool IsActive
    {
      get => this.m_active;
      private set
      {
        bool flag = this.m_active != value;
        this.m_active = value;
        if (!flag)
          return;
        if (this.Prototype.MonthlyUpointsCost.IsNegative)
        {
          if (this.m_active)
          {
            this.m_calendar.NewDay.Add<Edict>(this, new Action(this.onNewDay));
            this.m_numberOfDaysActive = 0;
          }
          else
            this.m_calendar.NewDay.Remove<Edict>(this, new Action(this.onNewDay));
        }
        this.OnActiveChanged(this.m_active);
      }
    }

    public string LastReasonForNotBeingActive { get; private set; }

    public Edict(
      EdictProto edictProto,
      UpointsManager upointsManager,
      ICalendar calendar,
      CaptainOfficeManager captainOfficeManager)
    {
      MBiHIp97M4MqqbtZOh.rMWAw2OR8();
      // ISSUE: reference to a compiler-generated field
      this.\u003CLastReasonForNotBeingActive\u003Ek__BackingField = "";
      // ISSUE: explicit constructor call
      base.\u002Ector();
      this.Prototype = edictProto;
      this.m_upointsManager = upointsManager;
      this.m_calendar = calendar;
      this.m_captainOfficeManager = captainOfficeManager;
      calendar.NewMonth.Add<Edict>(this, new Action(this.OnNewMonth));
      captainOfficeManager.OnOfficeActiveChanged.Add<Edict>(this, new Action(this.onOfficeChanged));
    }

    protected abstract bool CanReactivateForNewMonth(out string reasonForNotActive);

    protected virtual void OnActiveChanged(bool isActiveNow)
    {
    }

    protected abstract Edict.EdictEnableCheckResult CanBeEnabledInternal();

    public Edict.EdictEnableCheckResult CanBeEnabled()
    {
      if (this.Prototype.IsAdvanced)
      {
        CaptainOffice valueOrNull = this.m_captainOfficeManager.CaptainOffice.ValueOrNull;
        if ((valueOrNull != null ? (valueOrNull.Prototype.SupportsAdvancedEdicts ? 1 : 0) : 0) == 0)
          return new Edict.EdictEnableCheckResult()
          {
            CanBeEnabled = false,
            Explanation = TrCore.EdictRequiresAdvancedOffice.TranslatedString
          };
      }
      return this.CanBeEnabledInternal();
    }

    private void onOfficeChanged()
    {
      if (!this.IsEnabled)
        return;
      if (this.IsActive && !this.m_captainOfficeManager.IsOfficeActive)
      {
        this.LastReasonForNotBeingActive = TrCore.CaptainOfficeNotAvailable.TranslatedString;
        this.IsActive = false;
      }
      else
      {
        if (this.IsActive || !this.m_captainOfficeManager.IsOfficeActive)
          return;
        this.tryReactivate();
      }
    }

    private void onNewDay()
    {
      Assert.That<Upoints>(this.Prototype.MonthlyUpointsCost).IsNegative();
      ++this.m_numberOfDaysActive;
    }

    private void OnNewMonth()
    {
      if (this.m_numberOfDaysActive > 0)
      {
        if (this.Prototype.MonthlyUpointsCost.IsNegative)
        {
          Upoints upoints = this.Prototype.MonthlyUpointsCost.Value.Abs().ScaledBy(Percent.FromRatio(this.m_numberOfDaysActive, 30)).Upoints();
          UpointsManager upointsManager = this.m_upointsManager;
          Proto.ID edict = IdsCore.UpointsCategories.Edict;
          Upoints generated = upoints;
          LocStr? nullable = new LocStr?(this.Prototype.Strings.Name);
          Upoints? max = new Upoints?();
          LocStr? extraTitle = nullable;
          upointsManager.GenerateUnity(edict, generated, max, extraTitle);
        }
        else
          Log.Error("numberOfDaysActive is positive for positive Unity?");
        this.m_numberOfDaysActive = 0;
      }
      this.m_currentMonthAlreadyPaidFor = false;
      this.tryReactivate();
      this.OnNewMonthInternal();
    }

    protected virtual void OnNewMonthInternal()
    {
    }

    private void tryReactivate()
    {
      if (!this.IsEnabled)
        this.IsActive = false;
      else if (!this.m_captainOfficeManager.IsOfficeActive)
      {
        this.LastReasonForNotBeingActive = "Captain's office is not available";
        this.IsActive = false;
      }
      else
      {
        string reasonForNotActive;
        bool flag = this.CanReactivateForNewMonth(out reasonForNotActive);
        this.LastReasonForNotBeingActive = reasonForNotActive;
        if (!flag)
          this.IsActive = false;
        else if (this.Prototype.MonthlyUpointsCost.IsNegative)
          this.IsActive = true;
        else if (this.m_currentMonthAlreadyPaidFor)
        {
          this.IsActive = true;
        }
        else
        {
          UpointsManager upointsManager = this.m_upointsManager;
          Proto.ID edict = IdsCore.UpointsCategories.Edict;
          Upoints abs = this.Prototype.MonthlyUpointsCost.Abs;
          LocStr? nullable = new LocStr?(this.Prototype.Strings.Name);
          Option<IEntity> consumer = new Option<IEntity>();
          LocStr? extraTitle = nullable;
          if (upointsManager.TryConsume(edict, abs, consumer, extraTitle))
          {
            this.m_currentMonthAlreadyPaidFor = true;
            this.IsActive = true;
          }
          else
          {
            this.IsActive = false;
            this.LastReasonForNotBeingActive = "Not enough unity";
          }
        }
      }
    }

    public void ToggleEnabled()
    {
      if (this.IsEnabled)
      {
        this.IsEnabled = false;
        this.IsActive = false;
      }
      else
      {
        if (!this.CanBeEnabled().CanBeEnabled)
          return;
        this.IsEnabled = true;
        this.tryReactivate();
      }
    }

    protected virtual void SerializeData(BlobWriter writer)
    {
      writer.WriteBool(this.IsEnabled);
      writer.WriteString(this.LastReasonForNotBeingActive);
      writer.WriteBool(this.m_active);
      writer.WriteGeneric<ICalendar>(this.m_calendar);
      CaptainOfficeManager.Serialize(this.m_captainOfficeManager, writer);
      writer.WriteBool(this.m_currentMonthAlreadyPaidFor);
      writer.WriteInt(this.m_numberOfDaysActive);
      UpointsManager.Serialize(this.m_upointsManager, writer);
      writer.WriteGeneric<EdictProto>(this.Prototype);
    }

    protected virtual void DeserializeData(BlobReader reader)
    {
      this.IsEnabled = reader.ReadBool();
      this.LastReasonForNotBeingActive = reader.ReadString();
      this.m_active = reader.ReadBool();
      reader.SetField<Edict>(this, "m_calendar", (object) reader.ReadGenericAs<ICalendar>());
      reader.SetField<Edict>(this, "m_captainOfficeManager", (object) CaptainOfficeManager.Deserialize(reader));
      this.m_currentMonthAlreadyPaidFor = reader.ReadBool();
      this.m_numberOfDaysActive = reader.ReadInt();
      reader.SetField<Edict>(this, "m_upointsManager", (object) UpointsManager.Deserialize(reader));
      reader.SetField<Edict>(this, "Prototype", (object) reader.ReadGenericAs<EdictProto>());
    }

    public struct EdictEnableCheckResult
    {
      public bool CanBeEnabled;
      public string Explanation;
    }
  }
}
