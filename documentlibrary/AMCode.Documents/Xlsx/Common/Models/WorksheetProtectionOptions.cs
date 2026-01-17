namespace AMCode.Documents.Xlsx
{
    public class WorksheetProtectionOptions
    {
        public string Password { get; set; }
        public bool AllowSelectLockedCells { get; set; } = true;
        public bool AllowSelectUnlockedCells { get; set; } = true;
        public bool AllowFormatCells { get; set; }
        public bool AllowFormatColumns { get; set; }
        public bool AllowFormatRows { get; set; }
        public bool AllowInsertColumns { get; set; }
        public bool AllowInsertRows { get; set; }
        public bool AllowInsertHyperlinks { get; set; }
        public bool AllowDeleteColumns { get; set; }
        public bool AllowDeleteRows { get; set; }
        public bool AllowSort { get; set; }
        public bool AllowAutoFilter { get; set; }
        public bool AllowPivotTables { get; set; }
    }
}
