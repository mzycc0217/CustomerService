using Coldairarrow.Business;
using Coldairarrow.Entity;
using Coldairarrow.Util;
using EFCore.Sharding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers
{
    [Route("/Message/[controller]/[action]")]
    public class Menu_ServiseController : BaseApiController
    {
        #region DI

        public Menu_ServiseController(IMenu_ServiseBusiness menu_ServiseBus,IDbAccessor db)
        {
            _menu_ServiseBus = menu_ServiseBus;
            _db = db;
        }

        IMenu_ServiseBusiness _menu_ServiseBus { get; }
        IDbAccessor _db { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Menu_Servise>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _menu_ServiseBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Menu_Servise> GetTheData(IdInputDTO input)
        {
            return await _menu_ServiseBus.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<List<ServiceTel>> GetMenuData(ServiceTel tel)
        {
            ///获取权限
            var ps =await  _db.GetIQueryable<Menu_Servise>().AsNoTracking().Where(p => p.UserID == tel.Id).ToListAsync();  
            //获取菜单
            var q = _db.GetIQueryable<ServiceTel>().AsNoTracking().Where(s=>s.Id!="0");
            List<ServiceTel> serviceTels = new List<ServiceTel>();
            foreach (var item in ps)
            {

                var st =  q.FirstOrDefault(t => t.Id == item.MenuID);



                serviceTels.Add(st);
               // ServiceTel serviceTel = new ServiceTel(); 



            }
            return serviceTels;
        }


        /// <summary>
        /// 设置客服
        /// </summary>
        /// <param name="menu_Servise"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        public async Task<string> SetKefu(Menu_Servise menu_Servise)
        {
            var msg = string.Empty;
            if (!string.IsNullOrWhiteSpace(menu_Servise.UserID))
            {
              var res = _db.GetIQueryable<Base_UserDecimal>().AsNoTracking().FirstOrDefault(p=>p.Id== menu_Servise.UserID);
                res.data2 = "3";
               await _db.UpdateAsync(res);
             

                Menu_Servise menu_Servised = new Menu_Servise
                {
                    MenuID = menu_Servise.MenuID,
                    UserID=menu_Servise.UserID
                    
                };
                InitEntity(menu_Servised);
              int s=  await _db.InsertAsync(menu_Servised);
                if (s>0)
                {
                    msg = "添加成功";
                }
                else
                {
                    msg = "添加失败";
                }
               

            }


            var resd = new
            {
                id="1",
                msg=msg

            };
            return resd.ToJson();

           
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Menu_Servise data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                InitEntity(data);

                await _menu_ServiseBus.AddDataAsync(data);
            }
            else
            {
                await _menu_ServiseBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _menu_ServiseBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}