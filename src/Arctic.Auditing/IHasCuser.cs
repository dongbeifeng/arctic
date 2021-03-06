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

namespace Arctic.Auditing
{
    // TODO 重命名
    /// <summary>
    /// 表示数据具有创建人字段。
    /// </summary>
    public interface IHasCuser
    {
        /// <summary>
        /// 获取或设置数据的创建人。
        /// </summary>
        string cuser { get; set; }
    }

}
