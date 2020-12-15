# Arctic

## 简介

Arctic 是基于 .net5 的 WebApi 后端框架，暂未包含前端部分，是作为自动化立体仓库 WMS 业务框架的基础结构开发的。

[![Build status](https://ci.appveyor.com/api/projects/status/l635nrq5oqjgolrr/branch/main?svg=true)](https://ci.appveyor.com/project/dongbeifeng/arctic/branch/main)

## 启动

在命令行中执行命令：

``` cmd
cd src\Arctic.Web
dotnet run
```

在浏览器中打开 <http://localhost:5000/swagger>。

## 技术选型

### 组件

* [Autofac](https://autofac.org/)
* [NHibernate](https://nhibernate.info/)
* [Dynamic LINQ](https://dynamic-linq.net/)
* [Serilog](https://serilog.net/)

### 工具

* [Swagger](https://swagger.io/)
* [xUnit](https://xunit.net/)
* [NSubstitute](https://nsubstitute.github.io/)

### 数据库

NHibernate 支持多种关系型数据库。程序开发时使用的数据库是 SQL Server 2019 Developer。

## 列表页

列表页有一个查询参数，名称形式为 `XListArgs`，查询参数包装了查询条件，排序条件和分页条件：

``` csharp

/// <summary>
/// 图书列表查询参数
/// </summary>
public class BookListArgs
{
    //
    // 使用 SearchArgAttribute，表示这个属性是查询条件
    // SearchMode.Like 表示使用模糊查询
    // 下面的属性表示 source.Title like this.Title
    // 
    /// <summary>
    /// 标题，支持模糊查找，使用 ? 表示一个字符，使用 * 表示任意个字符
    /// </summary>
    [SearchArg(SearchMode.Like)]
    public string? Title { get; set; }

    //
    // SourceProperty 表示在哪个属性上查询
    // SearchMode.GreaterOrEqual 表示 大于等于
    // 下面的属性表示 source.PublicationDate >= this.PublicationDateFrom
    // 
    /// <summary>
    /// 出版日期
    /// </summary>
    [SourceProperty("PublicationDate")]
    [SearchArg(SearchMode.GreaterOrEqual)]
    public DateTime? PublicationDateFrom { get; set; }


    /// <summary>
    /// 出版日期
    /// </summary>
    [SourceProperty("PublicationDate")]
    [SearchArg(SearchMode.Less)]
    public DateTime? PublicationDateTo { get; set; }

    // 
    // Sort 表示排序条件，字典的键表示排序字段，值表示排序顺序，
    // 例如 { "Title": "asc", PublicationDate: "desc" } 表示
    // ORDER BY Title ASC, PublicationDate DESC
    // 
    /// <summary>
    /// 排序字段
    /// </summary>
    public OrderedDictionary? Sort { get; set; }

    /// <summary>
    /// 基于 1 的当前页面。
    /// </summary>
    public int? Current { get; set; }

    /// <summary>
    /// 每页大小
    /// </summary>
    public int? PageSize { get; set; }

}

```

查询通过反射生成，列表页参数类不需要实现任何接口：

``` csharp

var pagedList = await _session.Query<Book>().SearchAsync(args, args.Sort, args.Current, args.PageSize);

```

## TODO

* ids4
* docker
* ELK
