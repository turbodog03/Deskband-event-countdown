using System.IO;

public static class Global
{
    // types
    public enum LOG_LEVELS { INFO, ERROR, SILENT };

    // path and file
    public static string APP_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
    public static string GLOBAL_LOG_FILE_NAME = "app.log";
    public static string SOUND_FILE_NAME = "Sound.wav";

    // styles
    public static float EVENT_LABEL_FONT_SIZE = 11f;
    public static float COMPELETED_LABEL_FONT_SIZE = 11f;

    public static string COMPELETED_LABEL_FONT = "苹方-简";
    
    // settings
    public static int MIN_COUNT_MIN = 1;
    public static LOG_LEVELS LOG_LEVEL = LOG_LEVELS.SILENT;

}
