# FoundryRulesAndUnits - Future Enhancements

*Running list of enhancement ideas discovered during development*

---

## Enhancement 1: ASCII Aliases for Unicode Unit Symbols

**Date Identified:** January 3, 2026  
**Context:** Conversational Product Configuration with LLM agents

**Issue:**
- Unicode symbols (Ω, μ, °) are difficult for humans to type via keyboard
- Original design used only Unicode symbols: "Ω", "kΩ", "μF"
- LLMs have no problem with Unicode but humans need ASCII alternatives

**Proposed Solution:**
Add ASCII aliases that map to Unicode symbols:
- `"ohm"` → `"Ω"`
- `"kohm"` → `"kΩ"`
- `"Mohm"` → `"MΩ"`
- `"micro"` → `"μ"` (prefix)
- `"deg"` → `"°"` (for temperature/angles)

**Implementation Approach:**
1. Add ASCII unit codes to UnitDefinition specifications
2. Parser accepts both: `units(1000, ohm)` and `units(1000, Ω)`
3. Display still uses Unicode for professional appearance
4. Best of both worlds: human-friendly input, beautiful output

**Benefit:**
- Human users can type `units(1000, ohm)`
- LLM agents can naturally use `units(1000, Ω)`
- Both work, parser normalizes internally
- Formulas look professional when displayed

**Priority:** Medium (not blocking, workaround exists: use plain numbers for resistance)

---

## Enhancement Template

**Date Identified:** [Date]  
**Context:** [What we were working on]

**Issue:**
[Description of the problem or limitation]

**Proposed Solution:**
[How to address it]

**Implementation Approach:**
[Technical details]

**Benefit:**
[Why this matters]

**Priority:** [High/Medium/Low]

---

*Add new enhancements above this line*
