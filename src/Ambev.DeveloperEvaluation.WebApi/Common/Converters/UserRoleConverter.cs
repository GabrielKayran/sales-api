using System.Text.Json;
using System.Text.Json.Serialization;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Common.Converters;


public class UserRoleConverter : JsonConverter<UserRole>
{
    public override UserRole Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
                return UserRole.None; 

            if (Enum.TryParse<UserRole>(stringValue, ignoreCase: true, out var enumValue))
                return enumValue;

            return UserRole.None;
        }
        
        if (reader.TokenType == JsonTokenType.Number)
        {
            var numberValue = reader.GetInt32();
            

            if (Enum.IsDefined(typeof(UserRole), numberValue))
                return (UserRole)numberValue;
                
            return UserRole.None;
        }

        throw new JsonException($"Não é possível converter o valor para UserRole. Esperado string ou número.");
    }

    public override void Write(Utf8JsonWriter writer, UserRole value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
