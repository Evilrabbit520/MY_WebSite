using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DbManager;
using DbManager.Extensions;
using iDigitalTechnology.Models;

namespace iDigitalTechnology.DataManage
{
    public class User
    {
        public static async Task<int> CreateUser(UserObjectModel user)
        {
            using (
                var cmd =
                    new SqlCommand(
                        @"INSERT	INTO [Digital Technology].dbo.Digital_Technology_Users ( UserId,Account, UserPwd, UserName, Gender, IDCard, Email, UserAddress, Phone,CreateDateTime,LastUpdateDateTime ) VALUES	( NEWID(),LEFT(ROUND(RAND()*100000+10000,0),5)+LEFT(ROUND(RAND()*100000+10000,0),5)+LEFT(ROUND(RAND()*1000+100,0),3), @UserPwd, @UserName, @Gender, @IDCard, @Email, @UserAddress, @Phone,GETDATE(),GETDATE()); ")
            )
            {
                cmd.Parameters.AddWithValue("@UserName", user.UserName);
                cmd.Parameters.AddWithValue("@UserPwd", user.UserPwd.HashPassWord());
                cmd.Parameters.AddWithValue("@Phone", user.Phone);
                cmd.Parameters.AddWithValue("@Gender", user.Gender);
                cmd.Parameters.AddWithValue("@IDCard", user.IDCard);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@UserAddress", user.UserAddress);
                return await DbHelper.ExecuteNonQueryAsync(cmd);
            }
        }

        public static async Task<DataTable> SelectUserInfoByEmail(string email)
        {
            using (var cmd=new SqlCommand("SELECT * FROM [Digital Technology].dbo.Digital_Technology_Users AS UO (NOLOCK) WHERE UO.Email=@Email"))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                return await DbHelper.ExecuteDataTableAsync(cmd);
            }
        }
        public static async Task<DataTable> SelectUserInfoByPhone(string phone)
        {
            using (var cmd = new SqlCommand("SELECT * FROM [Digital Technology].dbo.Digital_Technology_Users AS UO (NOLOCK) WHERE UO.Phone=@Phone"))
            {
                cmd.Parameters.AddWithValue("@Phone", phone);
                return await DbHelper.ExecuteDataTableAsync(cmd);
            }
        }
        public static async Task<DataTable> SelectUserInfoByIDCard(string IDCard)
        {
            using (var cmd = new SqlCommand("SELECT * FROM [Digital Technology].dbo.Digital_Technology_Users AS UO (NOLOCK) WHERE UO.IDCard=@IDCard"))
            {
                cmd.Parameters.AddWithValue("@IDCard", IDCard);
                return await DbHelper.ExecuteDataTableAsync(cmd);
            }
        }
        public static async Task<DataTable> SelectUserInfoByAccount(string Account)
        {
            using (var cmd = new SqlCommand("SELECT * FROM [Digital Technology].dbo.Digital_Technology_Users AS UO (NOLOCK) WHERE UO.Account=@Account"))
            {
                cmd.Parameters.AddWithValue("@Account", Account);
                return await DbHelper.ExecuteDataTableAsync(cmd);
            }
        }
    }
}
