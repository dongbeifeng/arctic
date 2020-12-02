using NHibernate.Mapping.ByCode;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Arctic.NHibernateExtensions
{
    public abstract class XModelMapper : ModelMapper
    {
        public XModelMapper()
        {
            BeforeMapManyToOne += (insp, prop, map) => {
                // 应用 RequiredAttribute 列不可空
                if (prop.LocalMember.IsDefined(typeof(RequiredAttribute), true))
                {
                    map.NotNullable(true);
                }
            };

            // 属性
            this.BeforeMapProperty += (insp, prop, map) =>
            {
                if (keywords.Any(x => x.Equals(prop.ToColumnName(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    map.Column("x" + prop.ToColumnName());
                }

                // 不可空值类型对应的列也不可空
                PropertyInfo? p = prop.LocalMember as PropertyInfo;
                if (p != null)
                {
                    if (IsValueTypeAndNotNullable(p.PropertyType))
                    {
                        map.NotNullable(true);
                    }
                }

                // 应用 RequiredAttribute 列不可空
                if (prop.LocalMember.IsDefined(typeof(RequiredAttribute), true))
                {
                    map.NotNullable(true);
                }

                // 长度
                var attr = prop.LocalMember.GetCustomAttributes(typeof(MaxLengthAttribute), true).OfType<MaxLengthAttribute>().FirstOrDefault();
                if (attr != null)
                {
                    map.Length(attr.Length);
                }
            };
        }


        private static bool IsValueTypeAndNotNullable(Type type)
        {
            if (type.IsValueType == false)
            {
                return false;
            }
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return false;
            }
            return true;
        }

        private static readonly List<string> keywords = new List<string> {
            "Number", "Thread", "Weight", "Category",
            "Exists","Operator", "Enabled",
            "Column","Level","Type","Message","Role","Name","Status",
            "Value", "Text", "State", "Desc", "Description", "No", "LineNo",
            "Area",
        };

    }


}
