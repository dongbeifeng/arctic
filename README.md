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

## 前端

* `IListArgs<T>` 接口的字段名称是适应 [Ant design pro](https://pro.ant.design/) 设计的。
