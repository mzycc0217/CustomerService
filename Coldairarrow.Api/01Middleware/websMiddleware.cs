using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coldairarrow.Api.websokcets
{
    public class websMiddleware
    {
        private readonly RequestDelegate _next;

        public websMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public static Dictionary<string, WebSocket> webUser = new Dictionary<string, WebSocket>();

        /// <summary>
        /// 收发类
        /// </summary>
        /// <param name="context"></param>
        /// <param name="webSocket"></param>
        /// <returns></returns>
        #region Echo
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await AddUser(context, webSocket);

                    var buffer = new byte[1024 * 4];
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    while (!result.CloseStatus.HasValue)
                    {

                        //await User["ReceiveId"].SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                        ////发送消息
                        await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                        //接收消息
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    }
                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }

         
        }
        #endregion


        /// <summary>
        /// 删除没有存活的检测
        /// </summary>
        /// <returns></returns>
        public void DetectionAsync()
        {

            do
            {
                var keysd = webUser.Keys.ToList();


                for (int i = 0; i < keysd.Count; i++)
                {
                    if (webUser[keysd[i]].State==WebSocketState.Closed)
                    {
                       webUser[keysd[i]].Abort();
                        webUser.Remove(keysd[i]);
                    }
                }


            } while (webUser.Count!=0);

        }



        /// <summary>
        /// 添加人
        /// </summary>
        /// <returns></returns>
        public async Task AddUser(HttpContext context, WebSocket webSocket)
        {
            try
            {
                var Id = context.Request.Query["UserId"].ToString().Trim();

                //var pd = DateTime.Now.ToString();
                //var psd = "名称:" + Id + "时间"+pd;
               //  _logger.Error(psd);
                //if (string.IsNullOrWhiteSpace(Id))
                //{
                //    webSocket.Abort();
                //    return;
                //}
                if (Id != null)
                {
                    WebSocket web;
                    if (webUser.TryGetValue(Id, out web))
                    {
                        webUser.Remove(Id);
                        webUser.Add(Id, webSocket);
                    }
                    else
                    {
                        webUser.Add(Id, webSocket);
                    }
                }
                // Thread thread=new Thread(DetectionAsync());
             /// await  Task.Run(() => DetectionAsync());

            }
            catch (Exception EX)
            {

                throw;
            }
        }
             
    }
}
