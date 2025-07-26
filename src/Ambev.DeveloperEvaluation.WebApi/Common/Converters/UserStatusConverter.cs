using System.Text.Json;
using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Common.Converters;

/// <summary>
/// Converter customizado para UserStatus que aceita tanto string quanto número
/// </summary>
public class UserStatusConverter : JsonConverter<UserStatus>
{
    public override UserStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
                return UserStatus.Unknown;

            // Tenta converter string para enum (case-insensitive)
            if (Enum.TryParse<UserStatus>(stringValue, ignoreCase: true, out var enumValue))
                return enumValue;

            // Se não conseguir converter, retorna Unknown
            return UserStatus.Unknown;
        }
        
        if (reader.TokenType == JsonTokenType.Number)
        {
            var numberValue = reader.GetInt32();
            
            // Verifica se o número é um valor válido do enum
            if (Enum.IsDefined(typeof(UserStatus), numberValue))
                return (UserStatus)numberValue;
                
            return UserStatus.Unknown;
        }

        throw new JsonException($"Não é possível converter o valor para UserStatus. Esperado string ou número.");
    }

    public override void Write(Utf8JsonWriter writer, UserStatus value, JsonSerializerOptions options)
    {
        // Sempre escreve como string para manter consistência na saída
        writer.WriteStringValue(value.ToString());
    }
}
