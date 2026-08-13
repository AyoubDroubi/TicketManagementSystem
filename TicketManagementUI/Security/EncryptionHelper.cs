using Jose;
using Newtonsoft.Json;
using System.Text;

namespace TicketManagementUI.Security;

public sealed class EncryptionHelper<T> where T : class
{
    private const int A256KwKeySizeBytes = 32;
    private readonly byte[] _secretKey;

    public EncryptionHelper(IConfiguration configuration)
    {
        var configuredKey = configuration["JWEKey"];

        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            throw new InvalidOperationException(
                "JWEKey is not configured. Supply it through environment variables, user-secrets, or a secure secret provider.");
        }

        _secretKey = Encoding.UTF8.GetBytes(configuredKey);

        if (_secretKey.Length != A256KwKeySizeBytes)
        {
            throw new InvalidOperationException(
                $"JWEKey must be exactly {A256KwKeySizeBytes} UTF-8 bytes for A256KW.");
        }
    }

    public string Encode(object value)
        => JWT.Encode(value, _secretKey, JweAlgorithm.A256KW, JweEncryption.A256CBC_HS512);

    public T Decode(string token)
    {
        var payload = JWT.Decode(token, _secretKey, JweAlgorithm.A256KW, JweEncryption.A256CBC_HS512);
        return JsonConvert.DeserializeObject<T>(payload)
            ?? throw new InvalidOperationException("The decrypted payload could not be deserialized.");
    }
}
