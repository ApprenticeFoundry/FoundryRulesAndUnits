namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Result of unit operation validation
    /// </summary>
    public class UnitOperationResult
	{
		public bool IsValid { get; set; }
		public bool IsWarning { get; set; }
		public string Message { get; set; } = "";
		public Type? ResultType { get; set; }

		public static UnitOperationResult Valid(string message, Type? resultType = null) =>
			new() { IsValid = true, Message = message, ResultType = resultType };

		public static UnitOperationResult Invalid(string message) =>
			new() { IsValid = false, Message = message };

		public static UnitOperationResult Warning(string message) =>
			new() { IsValid = true, IsWarning = true, Message = message };
	}
}