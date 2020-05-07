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
    public class ClientMessage
    {
        private static ClientMessage _instance { get; set; }
        public static ClientMessage Instance
        {
            get
            {
                return _instance ?? (_instance = new ClientMessage());
            }
        }
        public static ClientMessage NewInsance(ObjectContext context)
        {
            return _instance ?? (_instance =  new ClientMessage(context));
        }
        public ClientMessage(ObjectContext context)
        {
            this.MapSetting(context);
        }
        public ClientMessage()
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
                    var setting = context.RanMaster.Find<ClientDefine>(state => state.AttachToTransaction(transaction));
                    var settings = typeof(MessageDefine).GetFields();
                    foreach (var item in settings)
                    {
                        if (!setting.Any(x=>x.key == item.Name))
                        {
                            context.RanMaster.Insert(new ClientDefine
                            {
                                key = item.Name,
                                value = item.GetValue(null).ToString()
                            }, state => state.AttachToTransaction(transaction));
                        }
                    }
                    foreach (var item in setting)
                    {
                        var prop = typeof(MessageDefine).GetField(item.key);
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
