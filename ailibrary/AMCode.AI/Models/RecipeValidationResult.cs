namespace AMCode.AI.Models;

/// <summary>
/// Result of recipe validation
/// </summary>
public class RecipeValidationResult
{
    /// <summary>
    /// Overall validation score (0.0 to 1.0)
    /// </summary>
    public double Score { get; set; }

    /// <summary>
    /// Whether the recipe passed validation
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// List of validation issues found
    /// </summary>
    public List<ValidationIssue> Issues { get; set; } = new();

    /// <summary>
    /// List of validation warnings
    /// </summary>
    public List<ValidationWarning> Warnings { get; set; } = new();

    /// <summary>
    /// Validation timestamp
    /// </summary>
    public DateTime ValidatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Recipe title for reference
    /// </summary>
    public string RecipeTitle { get; set; } = string.Empty;

    /// <summary>
    /// Additional validation metadata
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// A validation issue that prevents the recipe from being valid
/// </summary>
public class ValidationIssue
{
    /// <summary>
    /// Issue type/category
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Issue severity
    /// </summary>
    public ValidationSeverity Severity { get; set; }

    /// <summary>
    /// Issue message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Field that has the issue
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// Suggested fix
    /// </summary>
    public string SuggestedFix { get; set; } = string.Empty;
}

/// <summary>
/// A validation warning that doesn't prevent validity but should be noted
/// </summary>
public class ValidationWarning
{
    /// <summary>
    /// Warning type/category
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Warning message
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Field that has the warning
    /// </summary>
    public string Field { get; set; } = string.Empty;

    /// <summary>
    /// Suggested improvement
    /// </summary>
    public string SuggestedImprovement { get; set; } = string.Empty;
}

/// <summary>
/// Validation severity levels
/// </summary>
public enum ValidationSeverity
{
    /// <summary>
    /// Critical issue that makes recipe unusable
    /// </summary>
    Critical,

    /// <summary>
    /// Major issue that significantly impacts quality
    /// </summary>
    Major,

    /// <summary>
    /// Minor issue that has minimal impact
    /// </summary>
    Minor
}
