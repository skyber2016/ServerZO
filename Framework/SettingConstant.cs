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
    public class SettingConstant
    {
        private static SettingConstant _instance { get; set; }
        public static SettingConstant Instance
        {
            get
            {
                return _instance ?? (_instance = new SettingConstant());
            }
        }
        public static SettingConstant NewInsance(ObjectContext context)
        {
            return _instance ?? (_instance =  new SettingConstant(context));
        }
        public SettingConstant(ObjectContext context)
        {
            this.MapSetting(context);
        }
        public SettingConstant()
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
                    var setting = context.RanMaster.Find<Setting>(state => state.AttachToTransaction(transaction));
                    var settings = typeof(MessageDefine).GetFields();
                    foreach (var item in settings)
                    {
                        if (!setting.Any(x=>x.key == item.Name))
                        {
                            context.RanMaster.Insert(new Setting
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
        public IDictionary<string,string> GetSetting()
        {
            var dic = new Dictionary<string, string>();
            var prop =  typeof(MessageDefine).GetFields();
            foreach (var item in prop)
            {
                dic[item.Name] = item.GetValue(null).ToString();
            }
            return dic;
        }
    }
}
