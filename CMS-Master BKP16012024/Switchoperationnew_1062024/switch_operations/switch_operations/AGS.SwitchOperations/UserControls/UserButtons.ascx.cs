using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AGS.SwitchOperations.UserControls
{
    public partial class UserButtons : System.Web.UI.UserControl
    {
        public event EventHandler AddClick, EditClick, ProcessClick, NextClick, PreviousClick, FirstClick, LastClick, RejectClick,UpdateClick,SaveClick,AcceptClick,ViewClick,SubmitClick,DeleteClick,CancelClick;
        public string AccessButtons { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDefault();
                //if (!string.IsNullOrEmpty(AccessButtons))
                //{
                //    string[] accessPrefix = AccessButtons.Split(',');
                //    foreach (string ap in accessPrefix)
                //    {
                //        switch (ap.Trim())
                //        {
                //            case "A": btnAdd_U.Visible = true; break;
                //            case "B": btnSubmit_U.Visible = true; break;
                //            case "C": btnAccept_U.Visible = true; break;
                //            case "D": btnDelete_U.Visible = true; break;
                //            case "E": btnEdit_U.Visible = true; break;
                //            case "R": btnReject_U.Visible = true; break;
                //            case "S": btnSave_U.Visible = true; break;
                //            case "U": btnUpdate_U.Visible = true; break;                            
                //            case "T": btnCancel_U.Visible = true; break;
                //            case "P": btnProcess_U.Visible = true; break;
                //        }
                //    }
                //}
            }
        }

        public void LoadDefault()
        {
            if (!string.IsNullOrEmpty(AccessButtons))
            {
                string[] accessPrefix = AccessButtons.Split(',');
                foreach (string ap in accessPrefix)
                {
                    switch (ap.Trim())
                    {
                        case "A": btnAdd_U.Visible = true; break;
                        case "B": btnSubmit_U.Visible = true; break;
                        case "C": btnAccept_U.Visible = true; break;
                        case "D": btnDelete_U.Visible = true; break;
                        case "E": btnEdit_U.Visible = true; break;
                        case "R": btnReject_U.Visible = true; break;
                        case "S": btnSave_U.Visible = true; break;
                        case "U": btnUpdate_U.Visible = true; break;
                        case "T": btnCancel_U.Visible = true; break;
                        case "P": btnProcess_U.Visible = true; break;
                    }
                }
            }
        }
        public void ShowButton(char pChar)
        {
            btnAdd_U.Visible = false;
            btnSubmit_U.Visible = false;
            btnAccept_U.Visible = false;
            btnDelete_U.Visible = false;
            btnEdit_U.Visible = false;
            btnReject_U.Visible = false;
            btnSave_U.Visible = false;
            btnUpdate_U.Visible = false;
            btnCancel_U.Visible = false;
            btnProcess_U.Visible = false;

            switch (pChar)
            {
                case 'A': btnAdd_U.Visible = true; break;
                case 'B': btnSubmit_U.Visible = true; break;
                case 'C': btnAccept_U.Visible = true; break;
                case 'D': btnDelete_U.Visible = true; break;
                case 'E': btnEdit_U.Visible = true; break;
                case 'R': btnReject_U.Visible = true; break;
                case 'S': btnSave_U.Visible = true; break;
                case 'U': btnUpdate_U.Visible = true; break;
                case 'T': btnCancel_U.Visible = true; break;
                case 'P': btnProcess_U.Visible = true; break;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (AddClick != null)
                AddClick(sender, e);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (EditClick != null)
                EditClick(sender, e);
        }

        protected void btnProcess_Click(object sender, EventArgs e)
        {
            if (ProcessClick != null)
                ProcessClick(sender, e);
        }

        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (FirstClick != null)
                FirstClick(sender, e);
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (PreviousClick != null)
                PreviousClick(sender, e);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (NextClick != null)
                NextClick(sender, e);
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (LastClick != null)
                LastClick(sender, e);
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (RejectClick != null)
                RejectClick(sender, e);
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (AcceptClick != null)
                AcceptClick(sender, e);
        }
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (UpdateClick != null)
                UpdateClick(sender, e);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (SaveClick != null)
                SaveClick(sender, e);
        }
        protected void btnView_Click(object sender, EventArgs e)
        {
            if (ViewClick != null)
                ViewClick(sender, e);
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (SubmitClick != null)
                SubmitClick(sender, e);
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (DeleteClick != null)
                DeleteClick(sender, e);
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (CancelClick != null)
                CancelClick(sender, e);
        }



    }
}