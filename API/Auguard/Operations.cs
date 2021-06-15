using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wework.Auguard
{
    public class Operations
    {
        public static OperationAuthorizationRequirement THEM =
      new OperationAuthorizationRequirement { Name = nameof(THEM) };
        public static OperationAuthorizationRequirement XEM =
            new OperationAuthorizationRequirement { Name = nameof(XEM) };
        public static OperationAuthorizationRequirement SUA =
            new OperationAuthorizationRequirement { Name = nameof(SUA) };
        public static OperationAuthorizationRequirement XOA =
            new OperationAuthorizationRequirement { Name = nameof(XOA) };
        //public static OperationAuthorizationRequirement OWNER =
        //    new OperationAuthorizationRequirement { Name = nameof(OWNER) };
        //public static OperationAuthorizationRequirement ADMIN =
        //new OperationAuthorizationRequirement { Name = nameof(ADMIN) };
        //public static OperationAuthorizationRequirement MEMBER =
        //new OperationAuthorizationRequirement { Name = nameof(MEMBER) };
    }
}
