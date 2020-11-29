using System.Text.Json;

namespace Arctic.NHibernateExtensions
{
    public class NHibernateOptions
    {
        /// <summary>
        /// 指示是否在执行语句前检查事务。设为 true 时，如果执行数据库语句前未打开事务，则会抛出异常。
        /// 这是为了防止忘记打开事务。默认值为 false。
        /// </summary>
        public bool CheckTransaction { get; set; } = true;



        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        }
    }


}
