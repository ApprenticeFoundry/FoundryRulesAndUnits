# Moment (Calendar Timestamp) Specification

**Version:** 1.0
**Date:** 2026-09-21
**Status:** Proposed

## Executive Summary

This specification introduces a **Moment** type to FoundryRulesAndUnits — a specific point on the
calendar or clock (`2024-06-05T12:00:00Z`) — distinct from every existing time-related family in
this system, all of which measure an **elapsed span**, never a point. No code in this repo
implements this yet; this document exists to settle the design before any does.

## Motivation

Raised from `FoundryFrameworkLab/Instances/Relationship_Instance.kn:29` — an authored relationship
fact wanting to carry *when it became true*:
```
Relationship  @Wednesday has_father @Gomez
    Since|date: '2024-06-05T12:00:00Z'
```
`|date` does not exist as a unit, and — the more important finding — **it never should, as
currently conceived**. A unit in this system (`Time`, `Duration`, `WorkTime`, `Length`, `Mass`,
...) is a `MeasuredValue`: a magnitude in a `UnitGroup`, convertible by a linear (or otherwise
well-defined) scale factor against a base unit, meaningfully addable to another value of the same
family (`30 min + 30 min = 1 hr`). A calendar timestamp has none of these properties. There is no
meaningful "half of June 5th, 2024," no base-unit scale factor between "a Tuesday" and "a
Wednesday," and no operation that adds two moments together and produces a third moment. Bending
the unit-pipe mechanism to accept `date` would be a category error, not a missing registration —
confirmed by inspection, not assumption: `Time.cs` and `Duration.cs` are both plain `MeasuredValue`
subclasses with `+`/`-`/`*`/`/` operators over a linear unit scale, and neither is doing anything
resembling calendar arithmetic today.

**A related, secondary finding, filed separately (see the companion need below) rather than
folded into this spec:** `Time` and `Duration` are themselves near-identical duplicates today —
same base units, same operators, same behavior, `Duration` only ahead by two cross-family
operators. That duplication is a cleanup question with no real design ambiguity; introducing
`Moment` is a genuine new design question. Keeping them apart avoids entangling a simple fix with
a harder decision.

## What a Moment Is, and Is Not

A **Moment** is a point on a timeline — an instant identified by a calendar date and, optionally,
a time of day and a UTC offset (ISO-8601's own shape: `2024-06-05`, `2024-06-05T12:00:00Z`,
`2024-06-05T12:00:00-04:00`). It answers *when*, never *how much*.

It is explicitly **not**:
- A `MeasuredValue`. It has no unit family, no base-unit scale, no `UnitGroup`.
- Addable to another `Moment` (`June 5 + June 6` is meaningless).
- Directly comparable across unit systems the way `Length` is (SI vs IPS) — a moment's
  representation (calendar, timezone) is a formatting/parsing concern, not a measurement-system
  concern, so it sits outside the `SIUnitSystemSpecification`/`IPSUnitSystemSpecification`/etc.
  family entirely.

What it **can** meaningfully do:
- Subtract from another `Moment` to produce a `Duration` (`June 6 - June 5 = 1 day`) — this is
  the one real bridge between the two concepts, and the one place they interact.
- Compare (`<`, `<=`, `>`, `>=`, `==`) against another `Moment` — a genuine ordering, unlike two
  arbitrary durations of different unit families.
- Parse from, and format to, an ISO-8601 string — the wire format the worked example already uses.

## Options

### A. Extend `MeasuredValue`, give it a fabricated "unit family" (`date`) anyway

Cost: every `MeasuredValue` consumer (the unit-conversion machinery, the `UnitSystem` per-spec
base-unit tables) is built around linear scale factors between units of one family. A calendar
has none — "convert 1 `date` to `hours`" has no answer. Forcing this shape onto `Moment` would
mean either leaving most of `MeasuredValue`'s contract unimplemented (throwing on the operations
that don't apply) or silently defining nonsense conversions to fill the gap. Not picked.

### B. A new, separate value kind — `Moment` — parallel to `MeasuredValue` but not derived from it

`Moment` is its own root type: wraps a calendar instant (an ISO-8601-parseable value), offers
comparison operators and formatting, and exactly one arithmetic bridge
(`Moment - Moment -> Duration`, `Moment + Duration -> Moment`). It does not implement
`MeasuredValue`'s unit-conversion surface at all, because none of it applies. This mirrors how KN
notation's own `DataTable` schema already handles "a typed value that isn't a measured quantity"
— `datatype('text')`, `datatype('boolean')` sit beside numeric, unit-bearing columns without
pretending to be measured values either.

### C. Treat it as a plain string everywhere, defer the type question indefinitely

Cost: no ordering, no `Moment - Moment` duration arithmetic, no validation that the string is
actually a well-formed timestamp — defers the real capability this was raised to get, not just
the implementation of it. Not picked as the destination, though it remains the honest fallback
for any document written before this type exists (exactly `Relationship_Instance.kn`'s situation
today).

## Decision

**B**, pending review — this is a proposed spec, not yet accepted. `Moment` is a new, standalone
value kind, not a `MeasuredValue` subclass. It is out of scope for this document to also decide
where it is exposed from the KN notation layer (`datatype('moment')`? a dedicated literal
syntax?) — that is `FoundryFrameworkLab`'s own decision to make once this type exists, and is
being tracked there separately (Decisions/0050 and its open date-literal question,
`FoundryFrameworkLab/Instances/Relationship_Instance.kn:29`).

## Scope for Version 1.0

- `Moment`: wraps one calendar instant. Constructed from / formats to ISO-8601.
- Comparison operators: `<`, `<=`, `>`, `>=`, `==`, `!=`.
- `Moment - Moment -> Duration`.
- `Moment + Duration -> Moment`, `Moment - Duration -> Moment`.
- Timezone handling: store and round-trip the offset exactly as given (`Z`, `-04:00`, or none for
  a bare calendar date); do not silently normalize to UTC. A modeler recording "Since 2024-06-05"
  from an interview may not know or care about a timezone at all — the bare-date form must be
  legal, not just the full timestamp.

## Future Scope (Version 2.0)

- Calendar arithmetic beyond simple duration subtraction (add "1 month," respecting variable
  month length) — genuinely harder than anything in Version 1.0's scope, deferred on purpose.
- Recurring/periodic moments (a weekly reminder) — a different concept, not a Moment at all.
- Locale-aware formatting for display — Version 1.0 is ISO-8601 in and out, nothing else.

## Related

- `FoundryFrameworkLab/Decisions/0050-relationship-revived-as-a-bare-instance-fact-statement.md`
  — the KN notation work that surfaced this gap.
- `FoundryRulesAndUnits/docs/specifications/CURRENCY_AND_COST_UNITS_SPECIFICATION.md` — the
  precedent this document's format follows; also itself an example of a "system-independent"
  value family (Currency) that, like Moment, does not vary per unit system (SI/IPS/FPS/...).
- The `Time`/`Duration` duplication finding, filed separately, not folded into this spec's scope.
