using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using iDigitalTechnology.Models;
using DbManager.Extensions;
using iDigitalTechnology.BusinessLogic;
using Microsoft.Owin.Security;

namespace MYD_web_site.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        //登录
        public async Task<ActionResult> Login()
        {
            var user =await Request.GetOwinContext().Authentication.AuthenticateAsync("Application");
            if (user != null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(string user,string password)
        {
            var dic = new Dictionary<string, object> {["Code"] = "0", ["Msg"] = "登录成功"};

            #region 验证信息

            if (string.IsNullOrWhiteSpace(user))
            {
                dic["Code"] = "-1";
                dic["Msg"] = "请填写用户名";
                return Json(dic);
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                dic["Code"] = "-1";
                dic["Msg"] = "请填写密码";
                return Json(dic);
            }
            var userModel = await UserBL.SelectUserInfoByAccount(user);
            if (userModel == null)
            {
                dic["Code"] = "-102";
                dic["Msg"] = "用户不存在";
                return Json(dic);
            }
            if (password.HashPassWord() != userModel.UserPwd)
            {
                dic["Code"] = "-103";
                dic["Msg"] = "密码错误";
                return Json(dic);
            }

            #endregion

            #region 记录登录信息

            var authentication = HttpContext.GetOwinContext().Authentication;
            authentication.SignIn(
                new AuthenticationProperties(/*new Dictionary<string, string>() { { "ID", user.ID + "" } }*/)
                {
                    IsPersistent = true, //记住账户
                    ExpiresUtc = DateTime.Now.AddDays(1),
                    RedirectUri = "http://baidu.com"
                },
                    new ClaimsIdentity(new[] {
                            new Claim(ClaimsIdentity.DefaultNameClaimType, userModel.UserId.ToString()),
                            new Claim("UserID",userModel.UserId.ToString())
                    }, "Application")
                );

            #endregion

            return Json(dic);
        }

        //注册
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(string Account, string Email, string UserPwd, string PwdRepeat,string IDCard,string UserAddress,string Phone,int Gender=1)
        {
            var dic = new Dictionary<string, object>();

            #region 验证信息
            //if (string.IsNullOrWhiteSpace(Account))
            //{
            //    dic["Code"] = "-1";
            //    dic["Msg"] = "请填写用户名";
            //    return Json(dic);
            //}
            
            if (string.IsNullOrWhiteSpace(PwdRepeat))
            {
                dic["Code"] = "-1";
                dic["Msg"] = "请填写确认密码";
                return Json(dic);
            }
            if (!UserPwd.Equals(PwdRepeat))
            {
                dic["Code"] = "-1";
                dic["Msg"] = "2次填写密码不一致";
                return Json(dic);
            }
            if (string.IsNullOrWhiteSpace(IDCard))
            {
                dic["Code"] = "-1";
                dic["Msg"] = "请填写身份证编号";
                return Json(dic);
            }else if (IDCard.Length != 18)
            {
                dic["Code"] = "-1";
                dic["Msg"] = "请填写正确的身份证编号";
                return Json(dic);
            }
            else
            {
                var user = await UserBL.SelectUserInfoByIDCard(IDCard);
                if (user != null)
                {
                    dic["Code"] = "-1";
                    dic["Msg"] = "身份证已被注册，请更换身份证注册";
                    return Json(dic);
                }
            }
            if (string.IsNullOrWhiteSpace(Phone))
            {
                dic["Code"] = "-1";
                dic["Msg"] = "请填写手机号码";
                return Json(dic);
            }else if(!new Regex(@"^[1]+\d{10}").IsMatch(Phone))
            {
                dic["Code"] = "-1";
                dic["Msg"] = "请填写正确的手机号码";
                return Json(dic);
            }
            else
            {
                var user = await UserBL.SelectUserInfoByPhone(Phone);
                if (user != null)
                {
                    dic["Code"] = "-1";
                    dic["Msg"] = "手机号码已被注册，请更换手机号码注册";
                    return Json(dic);
                }
            }
            if (!string.IsNullOrWhiteSpace(Email) && !(new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$").IsMatch(Email)))
            {
                dic["Code"] = "-1";
                dic["Msg"] = "请填写正确的邮箱";
                return Json(dic);
            }
            else if (!string.IsNullOrWhiteSpace(Email) && (new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$").IsMatch(Email)))
            {
                var user = await UserBL.SelectUserInfoByEmail(Email);
                if (user != null)
                {
                    dic["Code"] = "-1";
                    dic["Msg"] = "邮箱已被注册，请更换邮箱注册";
                    return Json(dic);
                }
            }

            #endregion
            try
            {
                var result = await UserBL.CreateUser(new UserObjectModel {Email = Email, UserPwd = UserPwd, Gender = Gender ,UserAddress=UserAddress,IDCard=IDCard,Phone=Phone});
                if (result > 0)
                {
                    dic["Code"] = "0";
                    dic["Msg"] = "注册成功";
                }
            }
            catch(Exception ex)
            {
                dic["Code"] = "-100";
                dic["Msg"] = $"注册失败{ex.Message}";
            }
            return Json(dic);
        }
    }

}