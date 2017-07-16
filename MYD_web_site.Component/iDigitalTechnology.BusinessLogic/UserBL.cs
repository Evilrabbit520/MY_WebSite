using iDigitalTechnology.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iDigitalTechnology.DataManage;

namespace iDigitalTechnology.BusinessLogic
{
    public class UserBL
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static async Task<int> CreateUser(UserObjectModel user)
        {
            return await User.CreateUser(user);
        }

        /// <summary>
        /// 通过Email查询用户信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static async Task<UserObjectModel> SelectUserInfoByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var dt=await User.SelectUserInfoByEmail(email);
            if (dt == null || dt.Rows.Count == 0)
                return null;

            return dt.Rows.OfType<DataRow>().Select(x => new UserObjectModel(x)).FirstOrDefault();
        }
        /// <summary>
        /// 通过手机号查询用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static async Task<UserObjectModel> SelectUserInfoByPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return null;

            var dt = await User.SelectUserInfoByPhone(phone);
            if (dt == null || dt.Rows.Count == 0)
                return null;

            return dt.Rows.OfType<DataRow>().Select(x => new UserObjectModel(x)).FirstOrDefault();
        }
        /// <summary>
        /// 通过身份证查询用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static async Task<UserObjectModel> SelectUserInfoByIDCard(string IDCard)
        {
            if (string.IsNullOrWhiteSpace(IDCard))
                return null;

            var dt = await User.SelectUserInfoByIDCard(IDCard);
            if (dt == null || dt.Rows.Count == 0)
                return null;

            return dt.Rows.OfType<DataRow>().Select(x => new UserObjectModel(x)).FirstOrDefault();
        }
        public static async Task<UserObjectModel> SelectUserInfoByAccount(string Account)
        {
            if (string.IsNullOrWhiteSpace(Account))
                return null;

            var dt = await User.SelectUserInfoByAccount(Account);
            if (dt == null || dt.Rows.Count == 0)
                return null;

            return dt.Rows.OfType<DataRow>().Select(x => new UserObjectModel(x)).FirstOrDefault();
        }

        
    }
}
