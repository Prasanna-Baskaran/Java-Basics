using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGS.SwitchOperations.BusinessObjects
{
    public class ClsUserBO
    {
        public Int64 UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string UserRoleID { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string IsActive { get; set; }
        public string CreatedDate { get; set; }
        public Int64 CreatedByID { get; set; }
        public Int64 ModifiedByID { get; set; }
        public string Remark { get; set; }
        public int IntPara { get; set; }
    }
}