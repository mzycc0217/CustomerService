using Coldairarrow.Business;
using Coldairarrow.Entity;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers
{
    [Route("/Message/[controller]/[action]")]
    public class ServiceTelController : BaseApiController
    {
        #region DI

        public ServiceTelController(IServiceTelBusiness serviceTelBus)
        {
            _serviceTelBus = serviceTelBus;
        }

        IServiceTelBusiness _serviceTelBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<ServiceTel>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _serviceTelBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<ServiceTel> GetTheData(IdInputDTO input)
        {
            return await _serviceTelBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(ServiceTel data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _serviceTelBus.AddDataAsync(data);
            }
            else
            {
                await _serviceTelBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _serviceTelBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}