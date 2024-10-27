using System;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Common.Services.Web.ExternalApiService;

namespace SimbirHealth.Common.Services.Web.AuthValidationService;

/// <summary>
/// Сервис для проверки данных из access токена
/// </summary>
public class AuthValidationService : IAuthValidationService
{
    private IDictionary<string, object>? _claims = null;
    private Guid _guidClaim;
    private string _rolesClaim;
    public AuthValidationService(){}

    /// <summary>
    /// Загрузить информацию из токена
    /// </summary>
    /// <param name="claims"></param>
    public void LoadClaims(IDictionary<string, object> claims)
    {
        _claims = claims;
        var guidClaim = (JsonElement)_claims![ClaimTypes.NameIdentifier];
        var rolesClaim = (JsonElement)_claims![ClaimTypes.Role];
        _guidClaim = guidClaim.Deserialize<Guid>();
        _rolesClaim = rolesClaim.Deserialize<string>();
    }

    /// <summary>
    /// Проверить, принадлежит ли пользователь одной из выбранных ролей
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    public bool ContainsSpecificRoles(List<string> roles){
        bool containsRole = false;
        foreach (var role in roles){
            containsRole = containsRole || _rolesClaim.Contains(role);
        }
        return containsRole;
    }

    /// <summary>
    /// Проверить, совпадает ли Guid из токена с запрашиваемым Guid
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public bool GuidsEqual(Guid guid) => _guidClaim != guid;
}
