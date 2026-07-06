using System;
using System.Text;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// Exponent vector over the 8 base dimension axes, one signed byte per axis, packed into a
/// single long — the "prime factor form" of a dimension (SPEC_DIMENSIONAL_PEDIGREE §3).
///
/// Multiplication of quantities adds exponents; division subtracts; POW scales; SQRT halves
/// (valid only when every exponent is even). A zero packed value means dimensionless.
///
/// Canonical notation is the classical dimensional form: ASCII canonical "L2*M*T-2",
/// Unicode display "L²MT⁻²" — the same ASCII-canonical/Unicode-display policy as unit
/// symbols. Compose in packed form; notate lazily. Strings are NEVER built on the
/// evaluation hot path — only for errors, display, and reports.
///
/// Axis order (fixed, canonical): L, M, T, K(Θ), I, N, $, D.
/// There is deliberately NO angle axis — Torque-vs-Energy ambiguity is answered by the
/// human/AI, not by a pseudo-dimension (spec §11).
/// </summary>
public readonly struct DimensionalSignature : IEquatable<DimensionalSignature>
{
    private readonly long _packed;

    private const int LaneCount = 8;

    // ASCII canonical axis letters, in canonical order (lane 0..7)
    private static readonly char[] CanonicalAxis = { 'L', 'M', 'T', 'K', 'I', 'N', '$', 'D' };
    // Unicode display axis letters (Θ for temperature)
    private static readonly string[] DisplayAxis = { "L", "M", "T", "Θ", "I", "N", "$", "D" };

    public static readonly DimensionalSignature Dimensionless = default;

    public bool IsDimensionless => _packed == 0;

    private DimensionalSignature(long packed) => _packed = packed;

    /// <summary>
    /// Construct from per-axis exponents. Each must fit a signed byte; ±4 covers all real physics.
    /// </summary>
    public static DimensionalSignature Of(
        int length = 0, int mass = 0, int time = 0, int temperature = 0,
        int current = 0, int quantity = 0, int currency = 0, int data = 0)
    {
        Span<int> exps = stackalloc int[LaneCount] { length, mass, time, temperature, current, quantity, currency, data };
        long packed = 0;
        for (int lane = 0; lane < LaneCount; lane++)
        {
            var e = exps[lane];
            if (e < sbyte.MinValue || e > sbyte.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(exps), $"Dimensional exponent {e} out of range for lane {lane}");
            packed |= ((long)(byte)(sbyte)e) << (lane * 8);
        }
        return new DimensionalSignature(packed);
    }

    /// <summary>Exponent on a single lane (0..7 in canonical axis order).</summary>
    public int Exponent(int lane)
    {
        if (lane < 0 || lane >= LaneCount) throw new ArgumentOutOfRangeException(nameof(lane));
        return (sbyte)((_packed >> (lane * 8)) & 0xFF);
    }

    /// <summary>a × b → exponents add.</summary>
    public DimensionalSignature Multiply(DimensionalSignature other) => Combine(other, +1);

    /// <summary>a ÷ b → exponents subtract.</summary>
    public DimensionalSignature Divide(DimensionalSignature other) => Combine(other, -1);

    private DimensionalSignature Combine(DimensionalSignature other, int sign)
    {
        long packed = 0;
        for (int lane = 0; lane < LaneCount; lane++)
        {
            var e = Exponent(lane) + sign * other.Exponent(lane);
            if (e < sbyte.MinValue || e > sbyte.MaxValue)
                throw new OverflowException($"Dimensional exponent overflow on axis {CanonicalAxis[lane]}");
            packed |= ((long)(byte)(sbyte)e) << (lane * 8);
        }
        return new DimensionalSignature(packed);
    }

    /// <summary>a^n → exponents scale. Integer exponents only.</summary>
    public DimensionalSignature Pow(int n)
    {
        long packed = 0;
        for (int lane = 0; lane < LaneCount; lane++)
        {
            var e = Exponent(lane) * n;
            if (e < sbyte.MinValue || e > sbyte.MaxValue)
                throw new OverflowException($"Dimensional exponent overflow on axis {CanonicalAxis[lane]}");
            packed |= ((long)(byte)(sbyte)e) << (lane * 8);
        }
        return new DimensionalSignature(packed);
    }

    /// <summary>
    /// √a → exponents halve. Valid only when every exponent is even — sqrt(L²)→L is legal,
    /// sqrt(L³) or sqrt(Energy) has no clean dimension (spec §10.1 A10/A11).
    /// Returns false instead of throwing so the evaluator decides how to report.
    /// </summary>
    public bool TrySqrt(out DimensionalSignature root)
    {
        long packed = 0;
        for (int lane = 0; lane < LaneCount; lane++)
        {
            var e = Exponent(lane);
            if ((e & 1) != 0)
            {
                root = Dimensionless;
                return false;
            }
            packed |= ((long)(byte)(sbyte)(e / 2)) << (lane * 8);
        }
        root = new DimensionalSignature(packed);
        return true;
    }

    public bool Equals(DimensionalSignature other) => _packed == other._packed;
    public override bool Equals(object? obj) => obj is DimensionalSignature other && Equals(other);
    public override int GetHashCode() => _packed.GetHashCode();
    public static bool operator ==(DimensionalSignature left, DimensionalSignature right) => left.Equals(right);
    public static bool operator !=(DimensionalSignature left, DimensionalSignature right) => !left.Equals(right);

    /// <summary>
    /// ASCII canonical notation: fixed axis order, '*'-joined, exponent 1 omitted —
    /// "L2*M*T-2". Dimensionless is "1". String equality ⇔ signature equality.
    /// Lazy — never called on the evaluation hot path.
    /// </summary>
    public string ToCanonical()
    {
        if (IsDimensionless) return "1";
        var sb = new StringBuilder(16);
        for (int lane = 0; lane < LaneCount; lane++)
        {
            var e = Exponent(lane);
            if (e == 0) continue;
            if (sb.Length > 0) sb.Append('*');
            sb.Append(CanonicalAxis[lane]);
            if (e != 1) sb.Append(e);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Unicode display notation: "L²MT⁻²". Same dual-form policy as unit symbols
    /// (ASCII canonical, Unicode display). Lazy — display/reporting only.
    /// </summary>
    public string ToDisplay()
    {
        if (IsDimensionless) return "1";
        var sb = new StringBuilder(16);
        for (int lane = 0; lane < LaneCount; lane++)
        {
            var e = Exponent(lane);
            if (e == 0) continue;
            sb.Append(DisplayAxis[lane]);
            if (e != 1) AppendSuperscript(sb, e);
        }
        return sb.ToString();
    }

    private static void AppendSuperscript(StringBuilder sb, int e)
    {
        if (e < 0) { sb.Append('⁻'); e = -e; }   // ⁻
        foreach (var digit in e.ToString())
            sb.Append(digit switch
            {
                '1' => '¹', '2' => '²', '3' => '³',
                '4' => '⁴', '5' => '⁵', '6' => '⁶',
                '7' => '⁷', '8' => '⁸', '9' => '⁹',
                _ => '⁰'
            });
    }

    /// <summary>
    /// Parse the ASCII canonical form back to a signature (round-trip of ToCanonical).
    /// Accepts "1" for dimensionless. Rejects unknown axis letters and malformed exponents.
    /// </summary>
    public static bool TryParseCanonical(string? text, out DimensionalSignature signature)
    {
        signature = Dimensionless;
        if (string.IsNullOrWhiteSpace(text)) return false;
        text = text.Trim();
        if (text == "1") return true;

        long packed = 0;
        foreach (var token in text.Split('*'))
        {
            if (token.Length == 0) return false;
            int lane = Array.IndexOf(CanonicalAxis, token[0]);
            if (lane < 0) return false;

            int exponent = 1;
            if (token.Length > 1 && !int.TryParse(token.AsSpan(1), out exponent))
                return false;
            if (exponent < sbyte.MinValue || exponent > sbyte.MaxValue) return false;

            // duplicate axis in one string is malformed
            if (((packed >> (lane * 8)) & 0xFF) != 0) return false;
            packed |= ((long)(byte)(sbyte)exponent) << (lane * 8);
        }

        signature = new DimensionalSignature(packed);
        return true;
    }

    public override string ToString() => ToCanonical();
}
