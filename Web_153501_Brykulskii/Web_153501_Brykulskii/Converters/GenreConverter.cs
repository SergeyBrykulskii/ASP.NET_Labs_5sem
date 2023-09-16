namespace Web_153501_Brykulskii.Converters;

public static class GenreConverter
{
    public static string? ConvertToRu(string? genre)
    {
        return genre switch
        {
            "portrait" => "Портрет",
            "landscape" => "Пейзаж",
            "marina" => "Марина",
            "still-life" => "Натюрморт",
            _ => null,
        };
    }

    public static string? ConvertToEn(string? genre)
    {
        return genre switch
        {
            "Портрет" => "portrait",
            "Пейзаж" => "landscape",
            "Марина" => "marina",
            "Натюрморт" => "still-life",
            _ => null,
        };
    }
}
