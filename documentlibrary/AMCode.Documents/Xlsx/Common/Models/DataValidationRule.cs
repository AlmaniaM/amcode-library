namespace AMCode.Documents.Xlsx
{
    public class DataValidationRule
    {
        public string Type { get; set; }
        public string Operator { get; set; }
        public string Formula1 { get; set; }
        public string Formula2 { get; set; }
        public bool ShowInputMessage { get; set; }
        public string InputTitle { get; set; }
        public string InputMessage { get; set; }
        public bool ShowErrorMessage { get; set; }
        public string ErrorTitle { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorStyle { get; set; } = "Stop";
    }
}
