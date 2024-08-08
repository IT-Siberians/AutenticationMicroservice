﻿using Domain.ValueObjects.BaseEntities;
using System.Text.RegularExpressions;

namespace Domain.ValueObjects.ValueObjects;
/// <summary>
/// Базовый элемент Имя пользователя(никнейм)
/// </summary>
public class Username: ValueObject<string>
{
    /// <summary>
    /// Конструктор для EF
    /// </summary>
    protected Username() : base(string.Empty)
    {

    }
    /// <summary>
    /// Базовый элемент Хэшированный пароль
    /// </summary>
    /// <param name="value">Строка хранящаяся в элементе и проходящая валидацию на соответствие правилам Хэшированного пароля</param>

    public Username(string value): base(value)
    {
        
    }
    public const int MinNameLength = 3;//TODO:выделить в отдельные статические файлы
    public const int MaxNameLength = 30;//TODO:выделить в отдельные статические файлы
    private const string ValidNamePattern = "(^[a-zA-Z_-]+$)";//TODO:выделить в отдельные статические файлы
    /// <summary>
    /// Метод проверки соответствия правилам базового имени пользователя(никнейма)
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="ArgumentNullException">Нулевая или пустая строка в параметрах метода</exception>
    /// <exception cref="ArgumentOutOfRangeException">Несоответствие длине имени пользователя(никнейма)</exception>
    /// <exception cref="ArgumentException">Несоответствие паттерну имени пользователя(никнейму)</exception>
    protected override void Validate(string value)
    {
        if ((value == null) ||
            (string.IsNullOrWhiteSpace(value)))
            throw new ArgumentNullException(nameof(value), "Username cannot null or empty");

        if (value.Length > MaxNameLength)
            throw new ArgumentOutOfRangeException(nameof(value),
                $"Invalid username  length. Maximum length is {MaxNameLength}. Username value: {value}");

        if (value.Count(char.IsLetterOrDigit) < MinNameLength)
            throw new ArgumentOutOfRangeException(nameof(value),
            $"Invalid username  length. the minimum number of alphanumeric characters is equal to {MinNameLength}. Username value: {value}");
        
        var isMatch = Regex.Match(value, ValidNamePattern, RegexOptions.IgnoreCase);
        if (!isMatch.Success)
                throw new ArgumentException($"The username contains invalid characters. Username value: {value}");//TODO:FormatException или кастомные исключения
    }
}