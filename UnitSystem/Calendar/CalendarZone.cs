using System;
using System.Linq;

namespace FoundryRulesAndUnits.Units;

/// <summary>What a wall reading in the spring-forward hole (a time the clock skips) should mean.</summary>
public enum WallGap
{
    /// <summary>It names no instant: <see cref="CalendarZone.ToInstant"/> throws, TryToInstant returns false.</summary>
    Refuse,
    /// <summary>Move it forward by the gap's length (02:30 becomes 03:30): "the clock jumped".</summary>
    ShiftForward,
}

/// <summary>What a wall reading in the fall-back hour (a time the clock shows twice) should mean.</summary>
public enum WallOverlap
{
    /// <summary>Its first occurrence (still on the earlier, summer offset).</summary>
    Earlier,
    /// <summary>Its second occurrence.</summary>
    Later,
    /// <summary>It is ambiguous: refuse it.</summary>
    Refuse,
}

/// <summary>
/// A named time zone's rules — what turns a wall reading into an instant and back (MOMENT_CALENDAR_TIMESTAMP_SPECIFICATION.md,
/// Version 1.1). Wraps the platform's <see cref="TimeZoneInfo"/>; IANA ids (<c>America/New_York</c>) and Windows ids both resolve.
/// Holds no cache of its own — the platform already caches zones.
/// DST choices are always arguments, because they are an application's policy, not this library's guess.
/// </summary>
public class CalendarZone
{
    public CalendarZone(TimeZoneInfo info) => Info = info ?? throw new ArgumentNullException(nameof(info));

    public static CalendarZone Find(string id) => new(TimeZoneInfo.FindSystemTimeZoneById(id));
    public static CalendarZone Utc { get; } = new(TimeZoneInfo.Utc);

    public TimeZoneInfo Info { get; }
    public string Id => Info.Id;

    /// <summary>The current instant. Pass a <see cref="TimeProvider"/> to fix the clock in a test or a simulation.</summary>
    public Moment Now(TimeProvider? clock = null) => ToWallInstant((clock ?? TimeProvider.System).GetUtcNow());

    /// <summary>The wall reading of an instant in this zone.</summary>
    public Moment ToWall(Moment instant) => Moment.Wall(TimeZoneInfo.ConvertTime(instant.ToDateTimeOffset(), Info).DateTime);
    public Moment ToWall(DateTimeOffset instant) => Moment.Wall(TimeZoneInfo.ConvertTime(instant, Info).DateTime);

    /// <summary>An instant, read at this zone's offset for it.</summary>
    public Moment ToWallInstant(DateTimeOffset instant) => Moment.FromInstant(TimeZoneInfo.ConvertTime(instant, Info));

    /// <summary>The reading never happens here: the clock skips it (spring forward).</summary>
    public bool IsGap(Moment wall) => Info.IsInvalidTime(Reading(wall));

    /// <summary>The reading happens twice here: the clock shows it again (fall back).</summary>
    public bool IsOverlap(Moment wall) => Info.IsAmbiguousTime(Reading(wall));

    /// <summary>The instant a wall reading names in this zone. Throws when the policy refuses it.</summary>
    public Moment ToInstant(Moment wall, WallGap gap = WallGap.Refuse, WallOverlap overlap = WallOverlap.Earlier) =>
        TryToInstant(wall, out var instant, gap, overlap)
            ? instant
            : throw new ArgumentException($"{wall} is {(IsGap(wall) ? "skipped" : "shown twice")} by the clock in {Id}, and the policy refuses it.", nameof(wall));

    public bool TryToInstant(Moment wall, out Moment instant, WallGap gap = WallGap.Refuse, WallOverlap overlap = WallOverlap.Earlier)
    {
        instant = default;
        var local = Reading(wall);
        if (Info.IsInvalidTime(local))
        {
            if (gap == WallGap.Refuse) return false;
            // The gap's length is the jump in offset across it; moving forward by it lands on the reading the clock shows.
            var before = Info.GetUtcOffset(local.AddHours(-12));
            var after = Info.GetUtcOffset(local.AddHours(12));
            local = local + (after - before);
            if (Info.IsInvalidTime(local)) return false;
        }
        TimeSpan offset;
        if (Info.IsAmbiguousTime(local))
        {
            if (overlap == WallOverlap.Refuse) return false;
            var offsets = Info.GetAmbiguousTimeOffsets(local);
            // The larger offset is the earlier instant (still on summer time).
            offset = overlap == WallOverlap.Earlier ? offsets.Max() : offsets.Min();
        }
        else offset = Info.GetUtcOffset(local);
        instant = Moment.FromInstant(new DateTimeOffset(local, offset));
        return true;
    }

    /// <summary>The wall midnight that starts the reading's week (Monday by default).</summary>
    public static Moment WeekStart(Moment wall, DayOfWeek first = DayOfWeek.Monday)
    {
        var back = ((int)wall.Weekday - (int)first + 7) % 7;
        return Moment.Wall(wall.Date.AddDays(-back));
    }

    /// <summary>The wall midnight that starts the week an instant falls in, read in this zone.</summary>
    public Moment WeekStartOf(Moment instant, DayOfWeek first = DayOfWeek.Monday) => WeekStart(ToWall(instant), first);

    /// <summary>
    /// Wall-clock time from a wall epoch to an instant's reading here — the difference of two readings, which is what wall-clock
    /// rules mean ("Monday 00:00 to Wednesday 09:00" is 57 hours on the wall even when the clocks moved). Elapsed time is
    /// instant − instant.
    /// </summary>
    public Duration WallSince(Moment epoch, Moment instant) => ToWall(instant) - RequireWall(epoch);

    /// <summary>The inverse of <see cref="WallSince"/>: the instant at epoch + a wall-clock span, with explicit DST choices.</summary>
    public Moment InstantAt(Moment epoch, MeasuredValue wallSpan, WallGap gap = WallGap.Refuse, WallOverlap overlap = WallOverlap.Earlier) =>
        ToInstant(RequireWall(epoch) + Seconds(wallSpan), gap, overlap);

    public bool TryInstantAt(Moment epoch, MeasuredValue wallSpan, out Moment instant, WallGap gap = WallGap.Refuse, WallOverlap overlap = WallOverlap.Earlier) =>
        TryToInstant(RequireWall(epoch) + Seconds(wallSpan), out instant, gap, overlap);

    public override string ToString() => Id;

    private static Duration Seconds(MeasuredValue span) => Moment.Span(TimeSpan.FromSeconds(span.As("s")));

    private static Moment RequireWall(Moment wall) => wall.IsWall
        ? wall
        : throw new ArgumentException($"{wall} is an instant; a wall reading (no offset) was expected.", nameof(wall));

    private static DateTime Reading(Moment wall) => RequireWall(wall).Reading;
}
