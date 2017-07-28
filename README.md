# linq-helper
Abstraction layer over Linq with cache capabilities.

For using SqlRuntimeCacheManager you first need to enable notifications on database.

ALTER DATABASE database_name SET TRUSTWORTHY ON WITH ROLLBACK IMMEDIATE
ALTER DATABASE database_name SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE
ALTER AUTHORIZATION ON DATABASE::database_name TO sa

TODO work
1- Publish to nuget.org
2- Add a configuration to switch between 'System.Web.Cache' and 'System.Runtime.Cache'.
3- Add an example of usage.
4- Remove HttpContext dependency (if possible).
5- Receive sqlCacheDependency/dabase/name as parameter.
6- Remove sqlCacheDependency if not necesary with 'System.Runtime.Cache'.