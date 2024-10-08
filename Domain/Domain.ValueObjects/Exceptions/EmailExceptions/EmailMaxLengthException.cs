﻿using static Common.Helpers.Constants.EmailConstants;
using static Domain.Helpers.EmailHelpers.EmailDomainMessages;

namespace Domain.ValueObjects.Exceptions.EmailExceptions;

/// <summary>
/// Исключительная ситуация создание Email длиннее допустимого
/// </summary>
/// <param name="valueLength">Длина создаваемого Email</param>
/// <param name="paramName">Название параметра, в котором произошло исключение</param>
internal class EmailMaxLengthException(int valueLength, string paramName)
    : ArgumentOutOfRangeException(
        paramName: paramName,
        string.Format(EMAIL_LONGER_MAX_LENGTH_ERROR, EMAIL_MAX_LENGTH, valueLength));