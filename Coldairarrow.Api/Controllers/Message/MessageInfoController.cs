using AutoMapper.Internal;
using Coldairarrow.Api.Dto;
using Coldairarrow.Api.websokcets;
using Coldairarrow.Business;
using Coldairarrow.Entity;
using Coldairarrow.Util;
using EFCore.Sharding;
using LinqKit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using org.apache.zookeeper.data;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers
{
    [Route("/Message/[controller]/[action]")]
    public class MessageInfoController : BaseApiController
    {
        #region DI

        public MessageInfoController(IMessageInfoBusiness messageInfoBus, IDbAccessor db)
        {
            _messageInfoBus = messageInfoBus;
            _db = db;
        }

        IMessageInfoBusiness _messageInfoBus { get; }
        IDbAccessor _db { get; }
        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<MessageInfo>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _messageInfoBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<MessageInfo> GetTheData(IdInputDTO input)
        {
            return await _messageInfoBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(MessageInfo data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _messageInfoBus.AddDataAsync(data);

            }
            else
            {
                await _messageInfoBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]

        public async Task DeleteData(List<string> ids)
        {
            await _messageInfoBus.DeleteDataAsync(ids);
        }

        #endregion

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task SendMessage(Message message)
        {
            //进行数据库处理
            //var sta = 0;
            var rec = message.ReceiveId.Trim();
            var sen = message.SendId.Trim();

            MessageInfo info = new MessageInfo
            {
                Id = IdHelper.GetId(),
                SendId = message.SendId,
                ReceiveId = message.ReceiveId,
                News = message.News,
                Type = message.Type,
                DTAE = DateTime.Now,
            };
            //转成Byte数组
            var p = await _db.GetIQueryable<Base_UserDecimal>().AsNoTracking().FirstOrDefaultAsync(p => p.Id == message.SendId);
            message.Id = info.Id;
            message.base_UserDecimals = p;
            var ptime = DateTime.Now;
            var datas = ptime.ToLongDateString() + ptime.ToLongTimeString();
            message.DTAE = datas;

            try

            {

                info.state = 0;
                var s = string.Empty;
                byte[] aa;
                WebSocket web;
                if (websMiddleware.webUser.TryGetValue(message.ReceiveId, out web))
                {
                    if (websMiddleware.webUser[message.ReceiveId].State == WebSocketState.Open)
                    {
                        //已经处理的
                        info.Data1 = "0";
                        message.Data1 = "0";
                        s = JsonConvert.SerializeObject(message);

                        aa = Encoding.Default.GetBytes(s);
                        await websMiddleware.webUser[rec].SendAsync(new ArraySegment<byte>(aa, 0, aa.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        await websMiddleware.webUser[sen].SendAsync(new ArraySegment<byte>(aa, 0, aa.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    else
                    {
                        message.Data1 = "1";
                        //未处理的
                        info.Data1 = "1";
                        s = JsonConvert.SerializeObject(message);
                        aa = Encoding.Default.GetBytes(s);
                        await websMiddleware.webUser[sen].SendAsync(new ArraySegment<byte>(aa, 0, aa.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                       //  webServise.webUser[message.ReceiveId].Abort();
                        //  webServise.webUser.Remove(message.ReceiveId);
                        //  await webServise.webUser[message.ReceiveId].CloseAsync(WebSocketCloseStatus.NormalClosure, "正常移除", CancellationToken.None);

                    }

                }
                else
                {
                    message.Data1 = "1";
                    //未处理的
                    info.Data1 = "1";
                    s = JsonConvert.SerializeObject(message);
                    aa = Encoding.Default.GetBytes(s);

                    await websMiddleware.webUser[sen].SendAsync(new ArraySegment<byte>(aa, 0, aa.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                }


                await _messageInfoBus.AddDataAsync(info);
            }
            catch (Exception ex)
            {

                String mess = "发送失败" + ex.ToString();
                info.News = mess;
                //byte[] aad = Encoding.Default.GetBytes(mess);
                info.state = 20;
                //await webServise.webUser[message.SendId].SendAsync(new ArraySegment<byte>(aad, 0, aad.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                await _messageInfoBus.AddDataAsync(info);
                //throw;
            }
        }

        /// <summary>
        /// 查询还有没有客服//如果有客服则返回客服ID//如果没有则返回字符串文字
        /// </summary>
        /// <param name="UserDecimal"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<string> GetUsersd(Base_UserDecimal UserDecimal)
        {
            //获取在线客服
            try
            {

                List<Base_UserDecimal> vs = new List<Base_UserDecimal>();

                // webServise.webUser.Add("1306422215759106048", webSocket);
                List<string> test = new List<string>();
                if (websMiddleware.webUser.Keys.Count != 0)
                {
                    //  test.Add(webServise.webUser.Keys.ToString());
                    foreach (var item in websMiddleware.webUser.Keys)
                    {
                        test.Add(item);
                    }
                }

                for (int i = 0; i < test.Count; i++)
                {
                    var users = await _db.GetIQueryable<Base_UserDecimal>().FirstOrDefaultAsync(p => p.Id == test[i] && p.data2 == "3");
                    if (users != null)
                    {
                        vs.Add(users);
                    }


                }
                //foreach (var item in webServise.webUser)
                //{
                //    var usid = item.Key;
                //    var users = await _db.GetIQueryable<Base_UserDecimal>().FirstOrDefaultAsync(p => p.Id == usid && p.data2 == "3");
                //    if (users != null)
                //    {
                //        vs.Add(users);
                //    }
                //   }


                using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
                {
                    var c = redis.GetDatabase(0);
                    if (vs.Count == 0)
                    {
                        var users = await _db.GetIQueryable<Base_UserDecimal>().FirstOrDefaultAsync(p => p.Id != null && p.data2 == "3");
                        await c.HashSetAsync(users.Id, UserDecimal.Id, DateTime.Now.ToString(), When.Always, CommandFlags.None);
                        var resd = new
                        {
                            have = "1",
                            Id = users.Id,
                            msg = users.Name
                        };

                        return resd.ToJson();

                    }



                    List<Demolist> vs1 = new List<Demolist>();

                    //获取这个客服
                    foreach (var item in vs)
                    {

                        var count = c.HashGetAll(item.Id);
                        //获取这个客服下的客户数量
                        List<Object> demolist = new List<Object>();
                        if (count.Length == 0)
                        {
                            await c.HashSetAsync(item.Id, UserDecimal.Id, DateTime.Now.ToString(), When.Always, CommandFlags.None);

                            var resd = new
                            {
                                have = "1",
                                Id = item.Id,
                                msg = item.Name
                            };
                            return resd.ToJson();
                        }
                        else
                        {
                            foreach (var items in count)
                            {
                                var pw = items.ToString();
                                //  var hashmodel = JsonConvert.DeserializeObject(pw);
                                if (items != null)
                                {
                                    demolist.Add(items);
                                }

                            }
                            //客户对应的数量

                            vs1.Add(new Demolist
                            {
                                Name = item.Name,
                                Id = item.Id,
                                Count = demolist.Count
                            });

                        }


                    }


                    var pd = vs1.Min(t => t.Count);
                    var p = vs1.FirstOrDefault(s => s.Count == pd);
                    await c.HashSetAsync(p.Id, UserDecimal.Id, DateTime.Now.ToString(), When.Always, CommandFlags.None);
                    var res = new
                    {
                        have = "1",
                        Id = p.Id,
                        msg = p.Name
                    };

                    return res.ToJson();
                }





                //   await c.HashSetAsync(message.ReceiveId, message.SendId, message.SendId, When.Always, CommandFlags.None);



            }
            catch (Exception ex)
            {

                throw;
            }



        }



        /// <summary>
        /// 获取客服
        /// </summary>
        /// <param name="UserDecimal"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<Base_UserDecimal> GetAllKefu(Base_UserDecimal UserDecimal)
        {
            var message = "您好请问您有什么疑问呢？";
            var sd = JsonConvert.SerializeObject(message);
            var aa = Encoding.Default.GetBytes(sd);
            List<string> test = new List<string>();

            List<Base_UserDecimal> vs = new List<Base_UserDecimal>();
            if (websMiddleware.webUser.Keys.Count != 0)
            {
                //  test.Add(webServise.webUser.Keys.ToString());
                foreach (var item in websMiddleware.webUser.Keys)
                {
                    test.Add(item);
                }
            }

            for (int i = 0; i < test.Count; i++)
            {
                var users = await _db.GetIQueryable<Base_UserDecimal>().FirstOrDefaultAsync(p => p.Id == test[i] && p.data2 == "3");
                if (users != null)
                {
                    vs.Add(users);
                }

            }

            if (vs.Count == 0)
            {

                Base_UserDecimal ts = new Base_UserDecimal();
                var q = await _db.GetIQueryable<Base_UserDecimal>().Where(p => p.data2 == "3").ToListAsync();
                var t = q.Min(s => s.data3);
                if (t.Equals(0))
                {
                    var s = _db.GetIQueryable<Base_UserDecimal>().FirstOrDefault(p => p.data2 == "3");
                    s.data3 = s.data3 + 1;
                    await _db.UpdateAsync(s);
                    if (!string.IsNullOrWhiteSpace(UserDecimal.UserId))
                    {
                        if (websMiddleware.webUser[UserDecimal.UserId].State == WebSocketState.Open)
                        {
                            await websMiddleware.webUser[UserDecimal.UserId].SendAsync(new ArraySegment<byte>(aa, 0, aa.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    return s;
                }
                else
                {
                    var s = _db.GetIQueryable<Base_UserDecimal>().FirstOrDefault(p => p.data2 == "3");
                    s.data3 = s.data3 + 1;
                    await _db.UpdateAsync(s);
                    if (!string.IsNullOrWhiteSpace(UserDecimal.UserId))
                    {

                        if (websMiddleware.webUser[UserDecimal.UserId].State == WebSocketState.Open)
                        {
                            await websMiddleware.webUser[UserDecimal.UserId].SendAsync(new ArraySegment<byte>(aa, 0, aa.Length), WebSocketMessageType.Text, true, CancellationToken.None);

                        }
                    }
                    return s;
                }
            }
            else
            {
                var t = vs.Min(s => s.data3);
                var res = vs.FirstOrDefault(s => s.data3 == t);
                var result = _db.GetIQueryable<Base_UserDecimal>().FirstOrDefault(r => r.Id == res.Id);
                result.data3 = result.data3 + 1;


                if (!string.IsNullOrWhiteSpace(UserDecimal.UserId))
                {

                    if (websMiddleware.webUser[UserDecimal.UserId].State == WebSocketState.Open)
                    {
                        await websMiddleware.webUser[UserDecimal.UserId].SendAsync(new ArraySegment<byte>(aa, 0, aa.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                }
                await _db.UpdateAsync(result);
                return
                     res;




            }



        }
        /// <summary>
        /// 客服断开连接
        /// </summary>
        /// <param name="remove"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<string> BackLine(RemoveId remove)
        {
            using (ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("127.0.0.1:6379"))
            {

                var s = redis.GetDatabase(0);
                var p = await s.HashDeleteAsync(remove.KefuId, remove.UserId);
                Object res = new object();
                if (p)
                {
                    websMiddleware.webUser.Remove(remove.UserId);
                    res = new
                    {
                        mag = "移除成功"

                    };
                }
                res = new
                {
                    mag = "移除失败"

                };


                return res.ToJson();
            }

        }


        /// <summary>
        /// 客服获取历史消息 获取未处理的消息（p.Data1=1）
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<List<messageInfosd>> GetMessage(Message message)
        {
            //var reult = await _db.GetIQueryable<Base_UserDecimal>().FirstOrDefaultAsync(s => s.Id == message.ReceiveId);
         
                var res = _db.GetIQueryable<MessageInfo>().Where(p => p.Id != null);
                if (!string.IsNullOrWhiteSpace(message.ReceiveId) && !string.IsNullOrWhiteSpace(message.SendId))
                {
                    res = res.Where(p => p.ReceiveId == message.ReceiveId && p.SendId == message.SendId || p.SendId == message.ReceiveId && p.ReceiveId == message.SendId);
                }

                if (!string.IsNullOrWhiteSpace(message.Type))
                {
                    res = res.Where(p => p.Type == message.Type);
                }
                if (!string.IsNullOrWhiteSpace(message.Data1))
                {
                    res = res.Where(p => p.Data1 == p.Data1);
                }
                if (message.state != null)
                {
                    var t = await res.Where(p => p.state == message.state).Select(p => new messageInfosd
                    {

                        Id = p.Id,
                        SendId = p.SendId,
                        ReceiveId = p.ReceiveId,
                        News = p.News,
                        Data1 = p.Data1,
                        Data2 = p.Data2,
                        DTAE = p.DTAE,
                        Type = p.Type,
                        state = p.state,
                        //发送者
                        UserDecimal = _db.GetIQueryable<Base_UserDecimal>(true).SingleOrDefault(s => s.Id == p.SendId),
                        //接受者
                        base_UserDecimals = _db.GetIQueryable<Base_UserDecimal>(true).SingleOrDefault(s=>s.Id==p.ReceiveId),



                    }).OrderByDescending(p => p.DTAE).Skip((message.PageIndex - 1) * message.PageRow)
                       .Take(message.PageRow).ToListAsync();
                    var ks = t.OrderBy(s => s.DTAE).ToList();
                    return ks;
                }

            

            List<messageInfosd> messageInfosds = new List<messageInfosd>();

            return messageInfosds;


        }



        /// <summary>
        /// 客户获取历史消息(sendId)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<List<messageInfosd>> GetUserMessage(Message message)
        {

            var sd = _db.GetIQueryable<MessageInfo>().Where(p=>p.Id!=null) ;

            if (!string.IsNullOrWhiteSpace(message.SendId))
            {
                sd = sd.Where(p => p.SendId == message.SendId||p.ReceiveId==message.SendId);
            }

            if (!string.IsNullOrWhiteSpace(message.Type))
            {
                sd = sd.Where(p => p.Type == message.Type);
            }
            if (!string.IsNullOrWhiteSpace(message.Data1))
            {
                sd = sd.Where(p => p.Data1 == p.Data1);
            }
            if (message.state != null)
            {
            var   res = await sd.Where(p => p.state == message.state).Select(p => new messageInfosd
                {
                Id = p.Id,
                SendId = p.SendId,
                ReceiveId = p.ReceiveId,
                News = p.News,
                Data1 = p.Data1,
                Data2 = p.Data2,
                DTAE = p.DTAE,
                Type = p.Type,
                state = p.state,
                //发送者
                UserDecimal = _db.GetIQueryable<Base_UserDecimal>(true).SingleOrDefault(s => s.Id == p.SendId),
                //接受者
                base_UserDecimals = _db.GetIQueryable<Base_UserDecimal>(true).SingleOrDefault(s => s.Id == p.ReceiveId)

            }).OrderByDescending(p => p.DTAE).Skip((message.PageIndex - 1) * message.PageRow)
                .Take(message.PageRow).ToListAsync();
                var ks = res.OrderBy(s => s.DTAE).ToList();
                return ks;

            }



            List<messageInfosd> messageInfosds = new List<messageInfosd>();

            return messageInfosds;


            //var res = _db.GetIQueryable<MessageInfo>().Where(p => p.Id != null);
            //if (!string.IsNullOrWhiteSpace(message.SendId))
            //{
            //    res = res.Where(p => p.SendId == message.SendId || p.ReceiveId == message.SendId);
            //}

            //if (!string.IsNullOrWhiteSpace(message.Type))
            //{
            //    res = res.Where(p => p.Type == message.Type);
            //}
            //if (!string.IsNullOrWhiteSpace(message.Data1))
            //{
            //    res = res.Where(p => p.Data1 == p.Data1);
            //}
            //if (message.state != null)
            //{
            //    var t = await res.Where(p => p.state == message.state).Include(s=>s.listuser).OrderByDescending(p => p.DTAE).Skip((message.PageIndex - 1) * message.PageRow)
            //.Take(message.PageRow).ToListAsync();
            //    var ks = t.OrderBy(s => s.DTAE).ToList();
            //    return ks;
            //}

            //   List<MessageInfo> messages = new List<MessageInfo>();
            // return messages;
        }

        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task SetMessage(Message message)
        {
            var res = await _db.GetIQueryable<MessageInfo>().SingleOrDefaultAsync(p => p.Id == message.Id);
            List<string> vs = new List<string>();
            res.state = 3;
            vs.Add(res.state.ToString());
            await _db.UpdateAsync<MessageInfo>(res, vs);
        }


        /// <summary>
        /// 获取在线客户(用不到)
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<List<Base_UserDecimal>> GetUser()
        {
            ///获取普通客户
            List<Base_UserDecimal> vs = new List<Base_UserDecimal>();
            foreach (var item in websMiddleware.webUser)
            {
                var usid = item.Key;
                var users = await _db.GetIQueryable<Base_UserDecimal>().FirstOrDefaultAsync(p => p.Id == usid && p.data2 == "2");
                if (users != null)
                {
                    vs.Add(users);
                }

            }
            return vs;
        }
        /// <summary>
        /// 获取在线客服(用不到)
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<List<Base_UserDecimal>> GetUserCustom()
        {
            ///获取在线客服
            List<Base_UserDecimal> vs = new List<Base_UserDecimal>();
            foreach (var item in websMiddleware.webUser)
            {
                var usid = item.Key;
                var users = await _db.GetIQueryable<Base_UserDecimal>().FirstOrDefaultAsync(p => p.Id == usid && p.data2 == "3");
                if (users != null)
                {
                    vs.Add(users);
                }

            }
            return vs;
        }


        /// <summary>
        /// 移除(关闭连接对方的连接)
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<Object> RemoveUser(Base_UserDecimal base_)
        {
            try
            {
                // result.CloseStatusDescription
                websMiddleware.webUser[base_.Id].Abort();
                websMiddleware.webUser.Remove(base_.Id);
                return JsonConvert.DeserializeObject("移除成功");
            }
            catch (Exception)
            {

                throw;
            }


        }


        /// <summary>
        /// 设置为已读
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<string> SeteMessage(List<string> vs)
        {
            foreach (var item in vs)
            {
                var res = await _db.GetIQueryable<MessageInfo>().AsNoTracking().FirstOrDefaultAsync(p => p.Id == item);
                res.Data1 = "0";

                int i = await _db.UpdateAsync(res);
                if (i > 0)
                {
                    var resd = new
                    {
                        success = true,
                        msg = "更新成功"
                    };
                    return resd.ToJson();
                }
                else
                {
                    var resd = new
                    {
                        success = false,
                        msg = "更新失败"
                    };
                    return resd.ToJson();
                }
            }
            var resk = new
            {
                success = false,
                msg = "系统错误"
            };
            return resk.ToJson();


        }



        /// <summary>
        /// 优雅websocket关闭连接(自身的)
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task CloseUser(Base_UserDecimal base_)
        {
            websMiddleware.webUser[base_.Id].Abort();
            websMiddleware.webUser.Remove(base_.Id);

        }

    }
}