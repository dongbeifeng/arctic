using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Arctic.Web
{
    internal class OperationTypePolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "OPTYPE_";
        readonly IOperaionTypePermissions _opTypePermissions;
        readonly ILogger _logger;
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public OperationTypePolicyProvider(IOperaionTypePermissions opTypePermissions, IOptions<AuthorizationOptions> options, ILogger logger)
        {
            _opTypePermissions = opTypePermissions;
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _logger = logger;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();


        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))
            {
                string opType = policyName[POLICY_PREFIX.Length..];
                var roles = _opTypePermissions.GetAllowedRoles(opType).ToArray();
                if (roles.Length > 0)
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireRole(roles)
                        .Build();
                    return Task.FromResult<AuthorizationPolicy?>(policy);
                }
                else
                {
                    _logger.Debug("没有为 {opType} 设置角色", opType);
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAssertion(x => false)
                        .Build();
                    return Task.FromResult<AuthorizationPolicy?>(policy);
                }
            }

            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

    }

}
