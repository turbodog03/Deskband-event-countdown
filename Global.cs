using System.IO;

public static class Global
{
    public static string APP_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

    public static float EVENT_LABEL_FONT_SIZE = 11f;
    public static float COMPELETED_LABEL_FONT_SIZE = 11f;

    public static string COMPELETED_LABEL_FONT= "苹方-简";

    public static int MIN_COUNT_MIN = 1;
    
}

