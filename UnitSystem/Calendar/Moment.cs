using System;
using System.Globalization;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// A point on the calendar or clock — never a measured quantity (docs/specifications/MOMENT_CALENDAR_TIMESTAMP_SPECIFICATION.md).
/// Either an INSTANT (it carries a UTC offset, so it names one point on the timeline) or a WALL READING (floating: a date, or a
/// date and time of day, with no offset — what a calendar on the wall says, somewhere unspecified). A <see cref="CalendarZone"/>
/// turns one into the other. Mixing the two kinds in a comparison or a subtraction throws rather than guessing.
/// The one bridge to the unit system is <see cref="Duration"/>: moment − moment, moment ± duration.
/// </summary>
[Serializable]
public readonly struct Moment : IComparable<Moment>, IEquatable<Moment>
{
    private static readonly string[] WallFormats =
        ["yyyy-MM-dd", "yyyy-MM-ddTHH:mm", "yyyy-MM-ddTHH:mm:ss", "yyyy-MM-ddTHH:mm:ss.FFFFFFF"];

    private readonly DateTime _wall;      // the reading, Kind Unspecified
    private readonly TimeSpan? _offset;   // null: a wall reading
    private readonly bool _zulu;          // written with 'Z' rather than +00:00, kept so it round-trips as given

    private Moment(DateTime wall, TimeSpan? offset, bool hasTime, bool zulu)
    {
        _wall = DateTime.SpecifyKind(wall, DateTimeKind.Unspecified);
        _offset = offset;
        HasTime = hasTime || wall.TimeOfDay != TimeSpan.Zero;
        _zulu = zulu && offset == TimeSpan.Zero;
    }

    // ---- making one -------------------------------------------------------------------------------------------------

    /// <summary>An instant, read at its own offset.</summary>
    public static Moment FromInstant(DateTimeOffset instant) => new(instant.DateTime, instant.Offset, true, false);

    /// <summary>A wall reading: a date and a time of day, no zone.</summary>
    public static Moment Wall(DateTime reading) => new(reading, null, true, false);

    /// <summary>A wall reading of a whole date (a bare calendar date, as an interview might record it).</summary>
    public static Moment Wall(DateOnly date) => new(date.ToDateTime(TimeOnly.MinValue), null, false, false);

    /// <summary>A wall reading of a date at a time of day.</summary>
    public static Moment Wall(DateOnly date, TimeOnly time) => new(date.ToDateTime(time), null, true, false);

    /// <summary>ISO-8601: <c>2024-06-05</c>, <c>2024-06-05T12:00</c>, <c>2024-06-05T12:00:00Z</c>, <c>2024-06-05T12:00:00-04:00</c>.</summary>
    public static Moment Parse(string text) =>
        TryParse(text, out var moment) ? moment : throw new FormatException($"'{text}' is not an ISO-8601 date or timestamp.");

    public static bool TryParse(string? text, out Moment moment)
    {
        moment = default;
        if (string.IsNullOrWhiteSpace(text)) return false;
        var s = text.Trim();
        TimeSpan? offset = null;
        var zulu = false;
        if (s.EndsWith('Z') || s.EndsWith('z')) { offset = TimeSpan.Zero; zulu = true; s = s[..^1]; }
        else if (s.Length > 16 && s[^3] == ':' && (s[^6] == '+' || s[^6] == '-'))
        {
            if (!TimeSpan.TryParseExact(s[^5..], @"hh\:mm", CultureInfo.InvariantCulture, out var o)) return false;
            offset = s[^6] == '-' ? -o : o;
            s = s[..^6];
        }
        if (!DateTime.TryParseExact(s, WallFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var wall)) return false;
        if (offset is not null && s.Length == 10) return false;   // an offset on a bare date names no instant
        moment = new Moment(wall, offset, s.Length > 10, zulu);
        return true;
    }

    // ---- what it is ---------------------------------------------------------------------------------------------------

    /// <summary>True for a wall reading (no offset); false for an instant.</summary>
    public bool IsWall => _offset is null;
    public bool IsInstant => _offset is not null;
    public bool HasTime { get; }

    /// <summary>The offset of an instant; null for a wall reading.</summary>
    public TimeSpan? Offset => _offset;

    /// <summary>The reading: a wall reading's own, or an instant's at its own offset.</summary>
    public DateTime Reading => _wall;
    public DateOnly Date => DateOnly.FromDateTime(_wall);
    public TimeOnly TimeOfDay => TimeOnly.FromDateTime(_wall);
    public DayOfWeek Weekday => _wall.DayOfWeek;

    /// <summary>Midnight of this moment's date, of the same kind (an instant keeps its offset).</summary>
    public Moment StartOfDay => new(_wall.Date, _offset, false, _zulu);

    /// <summary>The reading's time of day as a span: how far the clock face is past midnight (wall time, whatever the kind).</summary>
    public Duration SinceStartOfDay => Span(_wall.TimeOfDay);

    /// <summary>The instant as a <see cref="DateTimeOffset"/>. A wall reading has none: ask a <see cref="CalendarZone"/>.</summary>
    public DateTimeOffset ToDateTimeOffset() => _offset is { } o
        ? new DateTimeOffset(_wall, o)
        : throw new InvalidOperationException($"{this} is a wall reading with no zone; resolve it with a CalendarZone first.");

    /// <summary>The same instant read at another offset (UTC by default).</summary>
    public Moment AtOffset(TimeSpan offset) => FromInstant(ToDateTimeOffset().ToOffset(offset));

    // ---- arithmetic: the bridge to units ----------------------------------------------------------------------------

    /// <summary>Instant − instant is elapsed time; wall − wall is wall-clock time. Mixing the two throws.</summary>
    public static Duration operator -(Moment later, Moment earlier) => Span(Ticks(later, earlier, "subtract"));

    public static Moment operator +(Moment moment, Duration span) => moment.Shift(Seconds(span));
    public static Moment operator -(Moment moment, Duration span) => moment.Shift(-Seconds(span));
    public static Moment operator +(Duration span, Moment moment) => moment.Shift(Seconds(span));
    public static Moment operator +(Moment moment, Time span) => moment.Shift(Seconds(span));
    public static Moment operator +(Time span, Moment moment) => moment.Shift(Seconds(span));
    public static Moment operator -(Moment moment, Time span) => moment.Shift(-Seconds(span));

    /// <summary>The span as a <see cref="Duration"/>, the unit system's own type, so callers read it in any unit (<c>As("hr")</c>).</summary>
    public static Duration Span(TimeSpan span) => UnitSystem.Shared().CreateUnit<Duration>(span.TotalSeconds, "s");

    private Moment Shift(double seconds) => new(_wall.AddTicks((long)Math.Round(seconds * TimeSpan.TicksPerSecond)), _offset, HasTime, _zulu);

    private static double Seconds(MeasuredValue span) => span.As("s");

    private static TimeSpan Ticks(Moment a, Moment b, string verb)
    {
        if (a.IsWall != b.IsWall)
            throw new InvalidOperationException($"Cannot {verb} {a} and {b}: one is an instant and one is a wall reading. Resolve the wall reading with a CalendarZone.");
        return a.IsWall ? a._wall - b._wall : a.ToDateTimeOffset() - b.ToDateTimeOffset();
    }

    // ---- ordering -----------------------------------------------------------------------------------------------------

    public int CompareTo(Moment other) => Math.Sign(Ticks(this, other, "compare").Ticks);

    /// <summary>Instants are equal when they name the same point, whatever offset each is written at.</summary>
    public bool Equals(Moment other) =>
        IsWall == other.IsWall && (IsWall ? _wall == other._wall : ToDateTimeOffset() == other.ToDateTimeOffset());

    public override bool Equals(object? obj) => obj is Moment m && Equals(m);
    public override int GetHashCode() => IsWall ? HashCode.Combine(0, _wall) : HashCode.Combine(1, ToDateTimeOffset().UtcTicks);

    public static bool operator ==(Moment a, Moment b) => a.Equals(b);
    public static bool operator !=(Moment a, Moment b) => !a.Equals(b);
    public static bool operator <(Moment a, Moment b) => a.CompareTo(b) < 0;
    public static bool operator <=(Moment a, Moment b) => a.CompareTo(b) <= 0;
    public static bool operator >(Moment a, Moment b) => a.CompareTo(b) > 0;
    public static bool operator >=(Moment a, Moment b) => a.CompareTo(b) >= 0;

    // ---- text ---------------------------------------------------------------------------------------------------------

    /// <summary>ISO-8601, round-tripping the form it was given: a bare date stays a bare date, 'Z' stays 'Z'.</summary>
    public override string ToString()
    {
        if (!HasTime && IsWall) return _wall.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var text = _wall.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFF", CultureInfo.InvariantCulture).TrimEnd('.');
        if (_offset is not { } o) return text;
        if (_zulu) return text + "Z";
        return text + (o < TimeSpan.Zero ? "-" : "+") + o.Duration().ToString(@"hh\:mm", CultureInfo.InvariantCulture);
    }
}
