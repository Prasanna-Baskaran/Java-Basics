using AGS.SwitchOperations.BusinessLogics;
using AGS.SwitchOperations.BusinessObjects;
using AGS.Utilities;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace AGS.SwitchOperations
{
    public partial class ShowAccountBalanceDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ClsAccountBalanceBO objAccBal = new BusinessObjects.ClsAccountBalanceBO();
            objAccBal.CardNo = txtSearchCardNo.Value;
            objAccBal.BankId = Session["BankID"].ToString();
            DataTable ObjDTOutPut = new ClsAccountBalDetailsBAL().FunSearchAccountBalanceDetails(objAccBal);
            //hdnTransactionDetails.Value = createTable(ObjDTOutPut, RoleID);
            hdnAccountBalDetails.Value = ObjDTOutPut.ToHtmlTableString("", new AddedTableData[] { new AddedTableData() { index = 0 , hideColumnName = true } });
        }

    }
}