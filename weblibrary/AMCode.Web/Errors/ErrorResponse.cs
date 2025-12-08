namespace AMCode.Web.Errors
{
    /// <summary>
    /// Standardized error response format
    /// </summary>
    public class ErrorResponse
    {
        public ErrorDetail Error { get; set; } = null!;
    }

    /// <summary>
    /// Error detail information
    /// </summary>
    public class ErrorDetail
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public object? Details { get; set; }
    }
}
