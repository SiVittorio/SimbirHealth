using System;
using SimbirHealth.Data.Models.Account;

namespace SimbirHealth.Data.SharedResponses.Account;

public record AcccountResponse(Guid Guid, string FirstName, string LastName, List<Role> Roles);
