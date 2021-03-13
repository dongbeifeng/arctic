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

using System;
using System.ComponentModel.DataAnnotations;

namespace Arctic.AppSeqs
{
    /// <summary>
    /// 自定义的应用程序级别的序列对象。
    /// </summary>
    public class AppSeq
    {
        public AppSeq(string seqName)
        {
            this.SeqName = seqName;
            this.NextVal = 1;
        }

        protected AppSeq()
        {
            SeqName = default!;
        }

        /// <summary>
        /// 序列名称。
        /// </summary>
        [Required]
        [MaxLength(50)]
        public virtual string SeqName { get; internal protected set; }

        /// <summary>
        /// 序列的下一个值。
        /// </summary>
        public virtual int NextVal { get; protected set; }

        /// <summary>
        /// 获取序列的下一个值。
        /// </summary>
        /// <returns></returns>
        public virtual int GetNextVal()
        {
            return this.NextVal++;
        }

    }
}
