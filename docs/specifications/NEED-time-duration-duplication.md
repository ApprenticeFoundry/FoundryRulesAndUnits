# Need: `Time` and `Duration` are near-duplicate unit families

**Status:** Filed, not decided. Not a spec — no design ambiguity, just an unreconciled overlap
worth someone's deliberate call before more code depends on either one.

## What's there

`UnitSystem/UnitTypes/Time.cs` and `UnitSystem/UnitTypes/Duration.cs` are both plain
`MeasuredValue` subclasses, both registered against `s`/`min`/`hr`(/`day` in most unit-system
specs), both with the same `+`/`-`/`*`/`/` operators. `Duration` additionally has two cross-family
operators `Time` lacks (`Duration × Speed -> Length`, `Duration × Power -> Energy`). Otherwise
they are functionally the same type under two names. `UnitFamilyName.WorkTime` is a third,
related family (hour-based, work-day scaled) that also measures an elapsed span.

Found while investigating `FoundryFrameworkLab/Instances/Relationship_Instance.kn`'s
`Since|date:` line — see `MOMENT_CALENDAR_TIMESTAMP_SPECIFICATION.md`, filed the same day. Neither
`Time` nor `Duration` is a disguised point-in-time type; both are elapsed-span types, confirmed by
reading both files in full.

## The actual question, for whoever picks this up

Is the duplication accidental (one should be deprecated in favor of the other, existing usages
migrated), or intentional-but-undocumented (e.g. `Time` for "how long something takes" contexts,
`Duration` for "an interval used in a physics formula" contexts — a distinction that exists in
code comments or call-site convention but was never written down anywhere)? Nobody should guess;
grep every real usage of both across the workspace and let the actual call sites answer it before
touching either class.

## Deliberately not addressed here

This is a filing, not a decision. No option analysis, no recommendation — that's the next
person's job once the real usage pattern (or absence of one) is known.
