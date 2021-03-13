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

namespace Arctic.NHibernateExtensions
{
    public enum SearchMode
    {
        /// <summary>
        /// 指示判断源属性是否 等于 查询参数值
        /// </summary>
        Equal,

        /// <summary>
        /// 指示判断源属性是否 不等于 查询参数值
        /// </summary>
        NotEqual,

        /// <summary>
        /// 指示判断源属性是否 LIKE 查询参数值
        /// </summary>
        Like,

        /// <summary>
        /// 指示判断源属性是否 NOT LIKE 查询参数值
        /// </summary>
        NotLike, 
        
        /// <summary>
        /// 指示判断源属性是否 大于 查询参数值
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 指示判断源属性是否 大于等于 查询参数值
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// 指示判断源属性是否 小于 查询参数值
        /// </summary>
        LessThan,
        
        /// <summary>
        /// 指示判断源属性是否 小于等于 查询参数值
        /// </summary>
        LessThanOrEqual,
        
        /// <summary>
        /// 指示判断源属性是否 IN 查询参数值
        /// </summary>
        In,


        /// <summary>
        /// 指示判断源属性是否 NOT IN 查询参数值
        /// </summary>
        NotIn,
        
        /// <summary>
        /// 指示使用表达式作为谓词进行查询，表达式由查询参数名后跟 Expr 后缀命名的属性提供，
        /// 例如名为 LocationCode 的查询参数的表达式由名为 LocationCodeExpr 的属性提供
        /// </summary>
        Expression,

    }


}
