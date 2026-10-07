using System.Globalization;
using MinhaBiblioteca.Models;

namespace MinhaBiblioteca.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var status = value as string;
        return status switch
        {
            StatusLivro.QueroLer => Color.FromArgb("#D35400"), // Laranja/Âmbar
            StatusLivro.Lendo => Color.FromArgb("#2980B9"),    // Azul
            StatusLivro.Lido => Color.FromArgb("#27AE60"),     // Verde
            _ => Colors.Gray
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
