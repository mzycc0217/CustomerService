using Coldairarrow.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Dto
{
    public class Message
    {
      

            public String Id { get; set; }
            /// <summary>
            /// 接收者Id
            /// </summary>
            public string ReceiveId { get; set; }
            public string SendId { get; set; }
            public string News { get; set; }

            public string DTAE { get; set; }
             public string Data1 { get; set; } 
           public Boolean Data2 { get; set; } = false;
            public string Type { get; set; }
            /// <summary>
            /// 消息状态
            /// </summary>
            public int? state { get; set; }

        /// <summary>
        /// 消息状态(已经发送，未接受到)
        /// </summary>
        public int? states { get; set; }

        /// <summary>
        /// 是否是客服
        /// </summary>
        public string Kefu { get; set; }

        /// <summary>
        /// 不用管
        /// </summary>
        public Base_UserDecimal base_UserDecimals { get; set; }
            /// <summary>
            /// 页码
            /// </summary>
            public int PageIndex { get; set; } = 1;

            /// <summary>
            /// 页码
            /// </summary>
            public int PageRow { get; set; } = 20;
        
    }
}
