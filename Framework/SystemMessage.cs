using Dapper.FastCrud;
using Entity.RanMaster;
using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Constant
{
    public class SystemMessage
    {
        private static SystemMessage _instance { get; set; }
        public static SystemMessage Instance
        {
            get
            {
                return _instance ?? (_instance = new SystemMessage());
            }
        }
        public static SystemMessage NewInsance(ObjectContext context)
        {
            return _instance ?? (_instance =  new SystemMessage(context));
        }
        public SystemMessage(ObjectContext context)
        {
            this.MapSetting(context);
        }
        public SystemMessage()
        {
        }
        private void MapSetting(ObjectContext context)
        {
            if(context.RanMaster.State == System.Data.ConnectionState.Closed)
            {
                context.RanMaster.Open();
            }
            using(var transaction = context.RanMaster.BeginTransaction())
            {
                try
                {
                    var setting = context.RanMaster.Find<SystemDefine>(state => state.AttachToTransaction(transaction));
                    var settings = typeof(MessageContants).GetFields();
                    foreach (var item in settings)
                    {
                        if (!setting.Any(x=>x.key == item.Name))
                        {
                            context.RanMaster.Insert(new SystemDefine
                            {
                                key = item.Name,
                                value = item.GetValue(null).ToString()
                            }, state => state.AttachToTransaction(transaction));
                        }
                    }
                    foreach (var item in setting)
                    {
                        var prop = typeof(MessageContants).GetField(item.key);
                        if (prop != null)
                        {
                            prop.SetValue(item, item.value);
                        }
                    }
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    if (context.RanMaster.State == System.Data.ConnectionState.Open)
                    {
                        context.RanMaster.Close();
                    }
                }
            }
            
        }
    }
}
