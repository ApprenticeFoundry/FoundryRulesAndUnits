# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased] — additive, no version bump yet

### Added — calendar time (`UnitSystem/Calendar/`, spec `docs/specifications/MOMENT_CALENDAR_TIMESTAMP_SPECIFICATION.md` v1.1)

- `Moment`: a point on the calendar or clock, not a `MeasuredValue`. Either an instant (has an offset) or a wall reading (no offset).
  ISO-8601 in and out, round-tripping the form given; ordering; `Moment − Moment → Duration`; `Moment ± Duration/Time → Moment`;
  `Date`, `TimeOfDay`, `Weekday`, `StartOfDay`, `SinceStartOfDay`. Mixing an instant with a wall reading throws.
- `CalendarZone`: a named zone's rules (IANA or Windows id) — `ToWall`, `ToInstant`/`TryToInstant` with explicit `WallGap`
  (Refuse, ShiftForward) and `WallOverlap` (Earlier, Later, Refuse) policies, `IsGap`/`IsOverlap`, `WeekStart`/`WeekStartOf`,
  `WallSince`/`InstantAt` (wall-clock spans as `Duration`), `Now(TimeProvider?)`.
- First user: BarnWiz's native rebuild, which deleted its hand-rolled `TimeZoneInfo` code in favour of these.

## [11.1.0] - Latest

For the most recent changes, see the individual version changelogs in [docs/changelogs/](docs/changelogs/).

## Historical Versions

- [Version 11.1.0](docs/changelogs/CHANGELOG_11.1.0.md)
- [Version 11.0.0](docs/changelogs/CHANGELOG_11.0.0.md)
- [Version 10.11.0](docs/changelogs/CHANGELOG_10.11.0.md) 
- [Version 10.10.0](docs/changelogs/CHANGELOG_10.10.0.md)
- [Version 10.9.0](docs/changelogs/CHANGELOG_10.9.0.md)
- [Version 10.8.2](docs/changelogs/CHANGELOG_10.8.2.md)
- [Version 10.8.1](docs/changelogs/CHANGELOG_10.8.1.md)
- [Version 10.8.0](docs/changelogs/CHANGELOG_10.8.0.md)
- [Version 10.7.0](docs/changelogs/CHANGELOG_10.7.0.md)
- [Version 10.4.1](docs/changelogs/CHANGELOG_10.4.1.md)
- [Version 10.0.0](docs/changelogs/CHANGELOG_10.0.0.md)
- [Version 9.3.0](docs/changelogs/CHANGELOG_9.3.0.md)