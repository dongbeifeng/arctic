// Copyright 2020 王建军
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Threading.Tasks;

namespace Arctic.Web
{
    internal class OperationTypePolicyProvider : IAuthorizationPolicyProvider
    {
        readonly IOperaionTypeAuthoriztion _permissions;
        readonly ILogger _logger;
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public OperationTypePolicyProvider(IOperaionTypeAuthoriztion permissions, IOptions<AuthorizationOptions> options, ILogger logger)
        {
            _permissions = permissions;
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _logger = logger;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();


        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX.Value, StringComparison.OrdinalIgnoreCase))
            {
                string opType = policyName[POLICY_PREFIX.Value.Length..];
                var roles = _permissions.GetAllowedRoles(opType).ToArray();
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
