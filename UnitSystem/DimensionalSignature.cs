using System;
using System.Text;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// Exponent vector over the 9 base dimension axes, seven signed bits per axis, packed into a
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
/// Axis order (fixed, canonical): L, M, T, K(Θ), I, N, $, D, A(∠).
/// Angle is a SOFT axis (spec §3.6). An angle is a real quantity and also a ratio, so a radian is
/// 1: the axis keeps rad/s apart from Hz, and <see cref="AgreesApartFromAngle"/> lets it fold away
/// where a length or an energy absorbs it. Torque-vs-Energy is still answered by the human/AI.
/// </summary>
public readonly struct DimensionalSignature : IEquatable<DimensionalSignature>
{
    private readonly long _packed;

    private const int LaneCount = 9;
    private const int LaneBits = 7;
    private const long LaneMask = (1L << LaneBits) - 1;
    private const int MinExponent = -(1 << (LaneBits - 1));
    private const int MaxExponent = (1 << (LaneBits - 1)) - 1;
    private const int AngleLane = 8;

    // ASCII canonical axis letters, in canonical order (lane 0..8)
    private static readonly char[] CanonicalAxis = { 'L', 'M', 'T', 'K', 'I', 'N', '$', 'D', 'A' };
    // Unicode display axis letters (Θ for temperature, ∠ for angle)
    private static readonly string[] DisplayAxis = { "L", "M", "T", "Θ", "I", "N", "$", "D", "∠" };

    public static readonly DimensionalSignature Dimensionless = default;

    public bool IsDimensionless => _packed == 0;

    private DimensionalSignature(long packed) => _packed = packed;

    /// <summary>
    /// Construct from per-axis exponents. Each must fit seven signed bits; ±4 covers all real physics.
    /// </summary>
    public static DimensionalSignature Of(
        int length = 0, int mass = 0, int time = 0, int temperature = 0,
        int current = 0, int quantity = 0, int currency = 0, int data = 0, int angle = 0)
    {
        Span<int> exps = stackalloc int[LaneCount] { length, mass, time, temperature, current, quantity, currency, data, angle };
        long packed = 0;
        for (int lane = 0; lane < LaneCount; lane++)
        {
            var e = exps[lane];
            if (e < MinExponent || e > MaxExponent)
                throw new ArgumentOutOfRangeException(nameof(exps), $"Dimensional exponent {e} out of range for lane {lane}");
            packed |= Packed(e, lane);
        }
        return new DimensionalSignature(packed);
    }

    private static long Packed(int exponent, int lane) => (exponent & LaneMask) << (lane * LaneBits);

    /// <summary>Exponent on a single lane (0..8 in canonical axis order).</summary>
    public int Exponent(int lane)
    {
        if (lane < 0 || lane >= LaneCount) throw new ArgumentOutOfRangeException(nameof(lane));
        var bits = (int)((_packed >> (lane * LaneBits)) & LaneMask);
        return bits > MaxExponent ? bits - (1 << LaneBits) : bits;
    }

    /// <summary>The exponent of angle.</summary>
    public int AngleExponent => Exponent(AngleLane);

    /// <summary>This signature with its angle folded away: an angle is a ratio, so what is left is what the value measures.</summary>
    public DimensionalSignature WithoutAngle() => new(_packed & ~(LaneMask << (AngleLane * LaneBits)));

    /// <summary>
    /// True when the two agree on every axis but angle. Because a radian is 1, this is the test wherever two values must
    /// be the same kind of thing (spec §3.6): g / L adds to w * w, and sqrt(g / L) can be declared in rad/s.
    /// </summary>
    public bool AgreesApartFromAngle(DimensionalSignature other) => WithoutAngle() == other.WithoutAngle();

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
            if (e < MinExponent || e > MaxExponent)
                throw new OverflowException($"Dimensional exponent overflow on axis {CanonicalAxis[lane]}");
            packed |= Packed(e, lane);
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
            if (e < MinExponent || e > MaxExponent)
                throw new OverflowException($"Dimensional exponent overflow on axis {CanonicalAxis[lane]}");
            packed |= Packed(e, lane);
        }
        return new DimensionalSignature(packed);
    }

    /// <summary>
    /// √a → exponents halve. Valid only when every exponent is even — sqrt(L²)→L is legal,
    /// sqrt(L³) or sqrt(Energy) has no clean dimension (spec §10.1 A10/A11). An odd angle folds away
    /// first, since an angle is a ratio: a torsion spring's sqrt(k / I) is a rate.
    /// Returns false instead of throwing so the evaluator decides how to report.
    /// </summary>
    public bool TrySqrt(out DimensionalSignature root)
    {
        var source = (AngleExponent & 1) != 0 ? WithoutAngle() : this;
        long packed = 0;
        for (int lane = 0; lane < LaneCount; lane++)
        {
            var e = source.Exponent(lane);
            if ((e & 1) != 0)
            {
                root = Dimensionless;
                return false;
            }
            packed |= Packed(e / 2, lane);
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
            if (exponent < MinExponent || exponent > MaxExponent) return false;

            // duplicate axis in one string is malformed
            if (((packed >> (lane * LaneBits)) & LaneMask) != 0) return false;
            packed |= Packed(exponent, lane);
        }

        signature = new DimensionalSignature(packed);
        return true;
    }

    public override string ToString() => ToCanonical();
}
