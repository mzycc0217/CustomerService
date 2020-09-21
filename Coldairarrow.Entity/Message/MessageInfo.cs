using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coldairarrow.Entity
{
    /// <summary>
    /// MessageInfo
    /// </summary>
    [Table("MessageInfo")]
    public class MessageInfo
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        public String Id { get; set; }

        /// <summary>
        /// 发送者Id
        /// </summary>
        public String SendId { get; set; }

        /// <summary>
        /// 接收者Id
        /// </summary>
        public String ReceiveId { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        public String News { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// DTAE
        /// </summary>
        public DateTime? DTAE { get; set; }

        /// <summary>
        /// state
        /// </summary>
        public Int32? state { get; set; }

        /// <summary>
        /// Data1
        /// </summary>
        public String Data1 { get; set; }

        /// <summary>
        /// Data2
        /// </summary>
        public String Data2 { get; set; }
     

  

    }
}