# Branching & release discipline

*Written 2026-08-09 by Hawkeye, from Steve's philosophy, after the first night more than one
builder (Wright, on Debra's machine) pushed real work alongside Steve's own. Applies identically
across all six repos that build Polyphony: this one, `FoundryMicroCore`, `FoundryRulesAndUnits`,
`FoundryWorldsAndDrawings`, `FoundryMentorModeler`, and `FoundryMentorModeler.Complete`.*

## The two branches that matter

- **`master` is the release branch — what we'd show outside this team.** It has a version. It
  only moves when someone deliberately decides *this* is a release, by merging `develop` into it.
  Nobody commits to `master` directly, ever — not even a one-line fix.
- **`develop` is where the work happens.** It's the default branch — what you get building
  Polyphony day to day, what a fresh clone should land on. Everything real happens here or in a
  branch that feeds back into it.

**Right now, nothing has been released from `master` in most of these repos, and that's correct,
not stale.** A `master` that hasn't moved in weeks isn't behind — it's telling you truthfully that
nobody has called a release yet. Don't "freshen" it just to keep it moving; that defeats the whole
point of having it mean something specific.

## The flow

1. Branch off `develop` for anything new — a feature, an experiment, a fix you're not sure about
   yet. Name the branch for what it does.
2. Build it out on that branch. Nothing here is under pressure to be perfect; it's not `develop`
   yet.
3. When you're satisfied it works, merge it into `develop`. That's the bar: *satisfied it works*,
   not *satisfied it's finished* — `develop` is still the workbench, just the shared one.
4. `master` only moves when someone decides to cut a release: merge `develop` into `master`, tag
   the version, and that tag is what a deploy builds from — not "whatever `master`'s tip happens to
   be today."

## The one rule that's new because more than one hand is on this now

**When a change spans more than one of the six repos, use the *same branch name* in every repo it
touches.** The first time this mattered, it wasn't obvious: Wright's Polyphony branch and his
FoundryMicroCore branch had different names (`loom-dc-seats` and `loom-dc-agents`), and pairing them
required computing merge-bases by hand to work out they belonged together at all. A shared name
turns that into a glance instead of an investigation. Doesn't matter whose convention wins going
forward — matters that everyone touching more than one repo for the same change picks *one* name and
uses it everywhere that change lives.

## Why this is written down now

Steve's own words for why this wasn't written down before tonight: *"it's been you and me working
on things, and I knew where everything was."* One person holding the whole map in their head is a
real, working strategy — right up until a second person needs the map too. This document is that
map, made explicit the first night it needed to be.

## Issues & pull requests

The branch rules above say how to name a branch once it exists. They don't say how the other
builder finds out it's about to exist, or how it gets back into `develop` deliberately instead of
by direct push. That coordination layer — issue first, branch anchored to the issue number, PR
into `develop` — is in [ISSUE-AND-PR-WORKFLOW.spec.md](https://github.com/SteveStrong/Polyphony/blob/develop/ISSUE-AND-PR-WORKFLOW.spec.md).
Same six repos, same "nobody pushes to `master`" rule, one layer up.
