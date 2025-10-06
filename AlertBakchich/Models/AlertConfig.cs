namespace AlertBakchich.Models
{
    public class AlertConfig
    {
        public int alertDuration { get; set; }
        public string messagePosition { get; set; } = "inside";
        public string animationType { get; set; } = "slide";
        public string animationDirection { get; set; } = "bottom";
        public string mediaType { get; set; } = "gif";
        public string mediaUrl { get; set; } = "";
        public int? mediaWidth { get; set; }
        public int? mediaHeight { get; set; }
        public string fontFamily { get; set; } = "'Arial', sans-serif";
        public int donorFontSize { get; set; }
        public string donorColor { get; set; } = "#ffffff";
        public int amountFontSize { get; set; }
        public string amountColor { get; set; } = "#ffd700";
        public int messageFontSize { get; set; }
        public string messageColor { get; set; } = "#ffffff";
        public string backgroundColor { get; set; } = "#000000";
        public int backgroundOpacity { get; set; }
        public bool showBorder { get; set; }
        public string borderColor { get; set; } = "#ffd700";
        public int borderWidth { get; set; }
        public bool textShadow { get; set; }
        public bool playSound { get; set; }
        public string soundUrl { get; set; } = "";
        public bool queueAlerts { get; set; }
        public int maxQueue { get; set; }
        public int? zIndexMedia { get; set; } = 1;
        public int? zIndexText { get; set; } = 2;
        public string textVertical { get; set; } = "50%";
        public string textHorizontal { get; set; } = "50%";
        public string textWidth { get; set; } = string.Empty;
        public string? customCss { get; set; } = string.Empty;
        public string? customJs { get; set; } = string.Empty;
        public bool loopVideo { get; set; } = false;
    }
}
