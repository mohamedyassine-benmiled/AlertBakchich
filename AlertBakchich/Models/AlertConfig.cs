namespace AlertBakchich.Models
{
    public class AlertConfig
    {
        public int alertDuration { get; set; } = 5;
        public string messagePosition { get; set; } = "inside";
        public string animationType { get; set; } = "slide";
        public string animationDirection { get; set; } = "bottom";
        public string mediaType { get; set; } = "gif";
        public string mediaUrl { get; set; } = "https://media0.giphy.com/media/xTiTnqUxyWbsAXq7Ju/giphy.gif?cid=3309efc1ow3a32mguza5i61t3c4ekuzcbx483p7vfc1nb25z&ep=v1_gifs_search&rid=giphy.gif&ct=g";
        public int? mediaWidth { get; set; } = 400;
        public int? mediaHeight { get; set; } = 300;
        public string fontFamily { get; set; } = "'Arial', sans-serif";
        public int donorFontSize { get; set; } = 32;
        public string donorColor { get; set; } = "#ffffff";
        public int amountFontSize { get; set; } = 48;
        public string amountColor { get; set; } = "#ffd700";
        public int messageFontSize { get; set; } = 24;
        public string messageColor { get; set; } = "#ffffff";
        public string backgroundColor { get; set; } = "#000000";
        public int backgroundOpacity { get; set; } = 80;
        public bool showBorder { get; set; }
        public string borderColor { get; set; } = "#ffd700";
        public int borderWidth { get; set; } = 3;
        public bool textShadow { get; set; }
        public bool playSound { get; set; }
        public string soundUrl { get; set; } = "https://www.myinstants.com/media/sounds/yt1s_WtO44NZ.mp3";
        public bool queueAlerts { get; set; }
        public int maxQueue { get; set; } = 10;
        public int? zIndexMedia { get; set; } = 1;
        public int? zIndexText { get; set; } = 2;
        public string textVertical { get; set; } = "50%";
        public string textHorizontal { get; set; } = "50%";
        public string textWidth { get; set; } = "100%";
        public string? customCss { get; set; } = string.Empty;
        public string? customJs { get; set; } = string.Empty;
        public bool loopVideo { get; set; } = false;
        public int audioVolume { get; set; } = 100;
    }
}
