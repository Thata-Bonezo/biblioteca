using System.Globalization;

namespace MinhaBiblioteca.Converters;

public class RatingToStarsConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int nota && nota > 0)
        {
            nota = Math.Clamp(nota, 1, 5);
            return new string('★', nota) + new string('☆', 5 - nota);
        }

        return "Sem avaliação";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
