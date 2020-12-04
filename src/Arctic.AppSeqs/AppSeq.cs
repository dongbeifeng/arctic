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
