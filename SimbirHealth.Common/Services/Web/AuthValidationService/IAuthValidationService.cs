using System;

namespace SimbirHealth.Common.Services.Web.AuthValidationService;

public interface IAuthValidationService
{
    void LoadClaims(IDictionary<string, object> claims);
    bool ContainsSpecificRoles(List<string> roles);
    bool GuidsEqual(Guid guid);
}
