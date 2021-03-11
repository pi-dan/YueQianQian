using FreeSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YueQianQian.Common;

namespace YueQianQian.Repository
{
    public class FreeSqlDB
    {
        private static IFreeSql fsql = null;
        //static FreeSqlDB()
        //{

        //    fsql = Get();
        //}
        public static IFreeSql Get(IWebHostEnvironment env)
        {
            if (fsql == null)
            {
                string dbType = AppSettingsHelper.GetJson("Configs/dbconfig.json", "DbContexts:DbType");
                string connectionString = AppSettingsHelper.GetJson("Configs/dbconfig.json", $"DbContexts:{dbType}:ConnectionString");

                DataType DbType = StringConvertToEnum<DataType>(dbType);
                fsql = new FreeSqlBuilder()
                                           .UseConnectionString(DbType, connectionString)
                                           .UseMonitorCommand(cmd => { if (env.IsDevelopment()) Console.WriteLine(cmd.CommandText); })
                                           .UseAutoSyncStructure(false).UseNoneCommandParameter(true)
                                           .Build();
                fsql.Aop.TraceBefore += (_, e) =>
                {
                    if (env.IsDevelopment()) Console.WriteLine($"----TraceBefore---{e.Identifier} {e.Operation}");
                };
                fsql.Aop.TraceAfter += (_, e) =>
                {
                    if (env.IsDevelopment())
                        Console.WriteLine($"----TraceAfter---{e.Identifier} {e.Operation} {e.Remark} {e.Exception?.Message} {e.ElapsedMilliseconds}ms\r\n");
                };
            }

            return fsql;
        }

        public static T StringConvertToEnum<T>(string str)
        {
            T result = default(T);
            try
            {
                result = (T)Enum.Parse(typeof(T), str);
            }
            catch (Exception ex)
            {
                return result;
            }
            return result;
        }
    }
}
