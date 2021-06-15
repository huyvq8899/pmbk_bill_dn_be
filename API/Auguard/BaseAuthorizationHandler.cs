using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Services.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace wework.Auguard
{
    public class BaseAuthorizationHandler :AuthorizationHandler<OperationAuthorizationRequirement, string>
    {
        IFunction_RoleRespositories _IFunction_RoleRespositories;
        public BaseAuthorizationHandler(IFunction_RoleRespositories IFunction_RoleRespositories)
        {
            _IFunction_RoleRespositories = IFunction_RoleRespositories;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, string resource)
        {
            var rols = context.User.FindAll(ClaimTypes.Role);
            List<string> roles = new List<string>();
            foreach (var item in rols)
            {
                roles.Add(item.Value);
            }
            if (roles.Count > 0)
            {
                //
                var rs = await _IFunction_RoleRespositories.CheckPermission(roles[0], resource);
                if (rs == true || roles.Contains("OWNER")) context.Succeed(requirement);
                else context.Fail();
                //
            }
            else
            {
                context.Fail();
            }
        }
    }
}
