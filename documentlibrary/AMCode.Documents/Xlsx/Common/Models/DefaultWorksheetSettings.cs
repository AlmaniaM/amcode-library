namespace AMCode.Documents.Xlsx
{
    public class DefaultWorksheetSettings
    {
        public double DefaultRowHeight { get; set; } = 15.0;
        public double DefaultColumnWidth { get; set; } = 8.43;
        public FontSettings DefaultFont { get; set; } = new FontSettings();
        public bool ShowGridlines { get; set; } = true;
        public bool ShowRowColHeaders { get; set; } = true;
    }
}
