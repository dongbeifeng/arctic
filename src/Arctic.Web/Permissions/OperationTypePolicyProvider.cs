using Microsoft.AspNetCore.Authorization;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Arctic.Web
{
    internal class OperationTypePolicyProvider : IAuthorizationPolicyProvider
    {
        readonly ILogger _logger;

        const string POLICY_PREFIX = "OPTYPE_";
        IOperaionTypePermissions _opTypePermissions;

        public OperationTypePolicyProvider(IOperaionTypePermissions opTypePermissions, ILogger logger)
        {
            _opTypePermissions = opTypePermissions;
            _logger = logger;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return Task.FromResult<AuthorizationPolicy>(null);
        }


        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                string opType = policyName.Substring(POLICY_PREFIX.Length);
                var roles = _opTypePermissions.GetAllowedRoles(opType).ToArray();
                if (roles.Length > 0)
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireRole(roles)
                        .Build();
                    return Task.FromResult(policy);
                }
                else
                {
                    _logger.Debug("没有为 {opType} 设置角色", opType);
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(x => false)
                        .Build();
                    return Task.FromResult(policy);
                }
            }

            return Task.FromResult<AuthorizationPolicy>(null);
        }

    }

}
