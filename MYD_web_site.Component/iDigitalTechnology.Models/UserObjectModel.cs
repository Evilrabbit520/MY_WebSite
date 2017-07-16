using DbManager.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iDigitalTechnology.Models
{
    public class UserObjectModel : BaseModel
    {
        public UserObjectModel() : base()
        {

        }

        public UserObjectModel(DataRow dr) : base(dr)
        {

        }
        public Guid UserId { set; get; }
        public string Account { set; get; }
        public string UserPwd { set; get; }
        public string UserName { set; get; }
        public int Gender { set; get; }
        public string IDCard { set; get; }
        public string Email { set; get; }
        public string UserAddress { set; get; }
        public string Phone { set; get; }
    }
}
