using Coldairarrow.Api.Dto;
using Coldairarrow.Business;
using Coldairarrow.Entity;
using Coldairarrow.Util;
using EFCore.Sharding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers
{
    [Route("/UserDecilmal/[controller]/[action]")]
    public class Base_UserDecimalController : BaseApiController
    {
        #region DI

        public Base_UserDecimalController(IBase_UserDecimalBusiness base_UserDecimalBus, IDbAccessor db)
        {
            _base_UserDecimalBus = base_UserDecimalBus;
            _db = db;
        }

        IBase_UserDecimalBusiness _base_UserDecimalBus { get; }
        IDbAccessor _db { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_UserDecimal>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _base_UserDecimalBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_UserDecimal> GetTheData(IdInputDTO input)
        {
            return await _base_UserDecimalBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_UserDecimal data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _base_UserDecimalBus.AddDataAsync(data);
            }
            else
            {
                await _base_UserDecimalBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_UserDecimalBus.DeleteDataAsync(ids);
        }

        #endregion


        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<Base_UserDecimal> wxlogins(Login login)
        {
            var url = "https://api.weixin.qq.com/sns/jscode2session?appid=wxa0e02c3ef617fc0c&secret=2f0cb4e82542d5453456108f7b8e615e&js_code=" + login.code + "&grant_type=authorization_code";
            var s = await Client_Post(url);
            //await GetUsers(s);
            if (s!=null)
            {
            var res = await _db.GetIQueryable<Base_UserDecimal>().FirstOrDefaultAsync(p => p.Openid == s);
            if (res == null)
            {
                Base_UserDecimal base_UserDecimal = new Base_UserDecimal
                {
                    Openid = s,
                    Name = login.NickName,
                    data3=0,
                    data4 = login.Img,//头像
                    data2 = "2"
                };
                InitEntity(base_UserDecimal);
              int i=  await _db.InsertAsync(base_UserDecimal);
                if (i>0)
                {
                    Menu_Servise menu_Servised = new Menu_Servise
                    {
                       // 客户
                        MenuID = "1235",
                        UserID = base_UserDecimal.Id

                    };
                    InitEntity(menu_Servised);
                    await _db.InsertAsync(menu_Servised);
                }
                return base_UserDecimal;
            }
            //else
            //{
            //    if (res.data2 == "3")
            //    {

            //        using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            //        {
            //            var c = redis.GetDatabase(0);
            //            await c.HashSetAsync(res.Id, "", "", When.Always, CommandFlags.None);
            //        }
            //    }
                return res;
                // }
            }
            else
            {
                Base_UserDecimal base_UserDecimal = new Base_UserDecimal();
                return base_UserDecimal;
            }

        }
      /// <summary>
      /// 获取用户信息
      /// </summary>
      /// <param name="requestUri"></param>
      /// <returns></returns>
        private  async Task<string> Client_Post(string requestUri)
        {

            //.net 4.5+使用HttpClient
            using (var httpClient = new HttpClient())
            {
             var responseJson =await httpClient.GetAsync(requestUri).Result.Content.ReadAsStringAsync();

            dynamic s = JsonConvert.DeserializeObject(responseJson);
            var openid = s.openid;


            return openid;
            }
        
            //序列化
            //  var result = responseJson.FromJson<TYTrainStopResponse>();

        }

        /// <summary>
        /// 获取用户信息(access_token)
        /// </summary>
        /// <param name="openid"></param>
        /// <returns></returns>
        private async Task<string> GetUsers(string openid)
        {

            using (HttpClient httpClient = new HttpClient())
            {

       //   https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET


                var pd = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxa0e02c3ef617fc0c&secret=2f0cb4e82542d5453456108f7b8e615e";

                var responseJsonsd =await httpClient.GetAsync(pd).Result.Content.ReadAsStringAsync();
                dynamic s = JsonConvert.DeserializeObject(responseJsonsd);
                var k = s.access_token;

                //.net 4.5+使用HttpClient
                 var requestUri = "https://api.weixin.qq.com/wxa/getpaidunionid?access_token="+ k+ "&openid=" + openid;
            
                var responseJson =await httpClient.GetAsync(requestUri).Result.Content.ReadAsStringAsync();

                 dynamic sds = JsonConvert.DeserializeObject(responseJson);


            }

              
           //  var openid = s.openid;


            return openid;
            //序列化
            //  var result = responseJson.FromJson<TYTrainStopResponse>();

        }
    }
}