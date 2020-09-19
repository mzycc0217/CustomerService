using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Dto
{
    public class Demolist
    {
        /// <summary>
        /// 客服Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 客户数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string Name { get; set; }
    }
}
