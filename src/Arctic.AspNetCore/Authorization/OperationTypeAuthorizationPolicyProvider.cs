// Copyright 2020-2021 王建军
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

namespace Arctic.AspNetCore
{
    public class OperationTypeAuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        readonly ILogger _logger;
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public OperationTypeAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options, ILogger logger)
        {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            _logger = logger;
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="policyName">由 <see cref="OperationTypeAttribute"/> 传递的授权策略名。</param>
        /// <returns></returns>
        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX.Value, StringComparison.OrdinalIgnoreCase))
            {
                string operationType = policyName[POLICY_PREFIX.Value.Length..];
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .RequireAssertion(context => 
                        context.User.IsInRole("admin")      // 允许管理员做任何操作
                        || context.User.HasClaim(ClaimTypes.AllowedOperationType, operationType)   // 非管理员需要具备当前操作类型的权限
                    )
                    .Build();
                
                return Task.FromResult<AuthorizationPolicy?>(policy);
            }
            
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }

    }

}
