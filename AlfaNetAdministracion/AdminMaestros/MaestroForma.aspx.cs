﻿using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class AlfaNetAdministracion_AdminMaestros_MaestroForma : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string Admon = Request["Admon"];
            if (Admon == "S")
            {
                ((MainMaster)this.Master).hidemenu();
            }
            else
            {
                ((MainMaster)this.Master).showmenu();
            }
            this.DVForma.Visible = true;
            this.TCForma.ActiveTabIndex = 0;
            this.TCForma.Tabs[1].Visible = false;


        }
        else
        {

            //DropDownList ddlst = (DropDownList)DVForma.FindControl("DropDownList1");
            //RadioButtonList rblst = (RadioButtonList)DVForma.FindControl("RbtnLst");

            //if (rblst != null)
            //{
            //    if (rblst.SelectedValue.ToString() == "0")
            //    {
            //        ddlst.Visible = false;
            //    }
            //    else
            //    {
            //        ddlst.Visible = true;
            //    }
            //}

        }


    }

    protected void ImgBtnFind_Click(object sender, ImageClickEventArgs e)
    {
        if (TxtForma.Text != "")
        {
            if (TxtForma.Text.Contains(" | "))
            {
                this.HFCodigoSeleccionado.Value = TxtForma.Text.Remove(TxtForma.Text.IndexOf(" | "));
                this.DVForma.ChangeMode(DetailsViewMode.ReadOnly);



                //DSGrupoSQL.GrupoDataTable DTGrupo = new DSGrupoSQL.GrupoDataTable();
                //GrupoBLL ObjGrupo = new GrupoBLL();

                //DTGrupo = ObjGrupo.GetGrupoByID(HFCodigoSeleccionado.Value);
                ////DTGrupo = ObjTAGrp.GetGroupById(HFCodigoSeleccionado.Value);

                //DataRow[] rows = DTGrupo.Select();

                //this.RbtnLstPermiso.SelectedValue = rows[0].ItemArray[6].ToString().Trim();
            }
        }
    }

    protected void DVDepartamento_DataBound(Object sender, EventArgs e)
    {
        if (DVForma.DataItemCount.ToString() == "0")
        {
            this.DVForma.ChangeMode(DetailsViewMode.Insert);
        }
        else if (this.DVForma.CurrentMode == DetailsViewMode.ReadOnly)
        {
            Label Lb = (Label)DVForma.FindControl("Label2");
            if (Lb.Text == "1")
            {
                CheckBox Ch = (CheckBox)DVForma.FindControl("CheckBox1");
                Ch.Checked = true;
                Ch.Enabled = false;
            }
        }
        else if (this.DVForma.CurrentMode == DetailsViewMode.Edit)
        {
            Label LblCodProce = (Label)DVForma.FindControl("Label1");

            DSFormaTableAdapters.Forma_ReadExisteFormaTableAdapter TAFormaExiste = new DSFormaTableAdapters.Forma_ReadExisteFormaTableAdapter();
            DSForma.Forma_ReadExisteFormaDataTable DTFormaExiste = new DSForma.Forma_ReadExisteFormaDataTable();
            DTFormaExiste = TAFormaExiste.GetForma_ReadExisteForma(LblCodProce.Text);

            Label LblProce = (Label)DVForma.FindControl("Label3");
            TextBox TxtProce = (TextBox)DVForma.FindControl("TextBox1");
            Label LblProceMsg = (Label)DVForma.FindControl("Label13");

            if (DTFormaExiste.Count == 0)
            {
                LblProce.Visible = false;
                TxtProce.Visible = true;
                LblProceMsg.Visible = false;
            }
            else
            {
                LblProce.Visible = true;
                LblProceMsg.Visible = true;
                TxtProce.Visible = true;
            }

            TextBox Txt = (TextBox)DVForma.FindControl("TextBox2");
            if (Txt.Text == "1")
            {
                CheckBox Ch = (CheckBox)DVForma.FindControl("CheckBox1");
                Ch.Checked = true;
                Ch.Enabled = true;
            }

            //    TextBox TxtPadre = (TextBox)DVForma.FindControl("TextBox1");
            //    if (TxtPadre.Text != "")
            //    {
            //        RadioButtonList RBLPadre = (RadioButtonList)DVForma.FindControl("RbtnLstSelPadre");
            //     RBLPadre.SelectedValue = "1";
            //     TxtPadre.Visible = true;

            //     }
            //    else
            //    {
            //        RadioButtonList RBLPadre = (RadioButtonList)DVForma.FindControl("RbtnLstSelPadre");
            //     RBLPadre.SelectedValue = "0";
            //     TxtPadre.Visible = false;
            //    }
        }

    }
    protected void DVDepartamento_ItemInserted(Object sender, DetailsViewInsertedEventArgs e)
    {
        if (e.Exception != null)
        {
            //Display a user-friendly message
            this.LblMessageBox.Text = "Ocurrio un problema al tratar de adicionar el registro. ";
            Exception inner = e.Exception.InnerException;
            this.LblMessageBox.Text += ErrorHandled.FindError(inner);
            this.LblMessageBox.Text += e.Exception.Message.ToString();
            this.MPEMensaje.Show();

            //Indicate that exception has been handled
            e.ExceptionHandled = true;

            //Keep the row in insert mode
            e.KeepInInsertMode = true;
        }
        else if (e.Exception == null)
        {
            this.DVForma.DataBind();
            this.LblMessageBox.Text = "Registro Adicionado";
            this.MPEMensaje.Show();
        }
    }
    protected void DVDepartamento_ItemUpdated(Object sender, DetailsViewUpdatedEventArgs e)
    {
        if (e.Exception != null)
        {
            //Display a user-friendly message
            this.LblMessageBox.Text = "Ocurrio un problema al tratar de actualizar el registro. ";
            Exception inner = e.Exception.InnerException;
            this.LblMessageBox.Text += ErrorHandled.FindError(inner);
            this.LblMessageBox.Text += e.Exception.Message.ToString();
            this.MPEMensaje.Show();

            //Indicate that exception has been handled
            e.ExceptionHandled = true;

            //Keep the row in edit mode
            e.KeepInEditMode = true;
        }
        else if (e.Exception == null)
        {
            this.DVForma.DataBind();
            this.LblMessageBox.Text = "Registro Editado";
            this.MPEMensaje.Show();
        }
    }
    protected void DVDepartamento_ItemDeleted(Object sender, DetailsViewDeletedEventArgs e)
    {
        if (e.Exception != null)
        {
            //Display a user-friendly message
            this.LblMessageBox.Text = "No se pudo eliminar el registro. ";
            Exception inner = e.Exception.InnerException;
            this.LblMessageBox.Text += ErrorHandled.FindError(inner);
            this.LblMessageBox.Text += e.Exception.Message.ToString();
            this.MPEMensaje.Show();

            //Indicate that exception has been handled
            e.ExceptionHandled = true;

        }
        else if (e.Exception == null)
        {
            this.DVForma.DataBind();
            this.LblMessageBox.Text = "Registro Eliminado";
            this.MPEMensaje.Show();
        }
    }

    protected void RadBtnLstFindby_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.RadBtnLstFindby.SelectedValue.ToString() == "1")
            this.AutoCompleteDepartamento.ServiceMethod = "GetFormaTextByTextnull";
        if (this.RadBtnLstFindby.SelectedValue.ToString() == "2")
            this.AutoCompleteDepartamento.ServiceMethod = "GetFormaTextByIdnull";
    }

    protected void ImgBtnUpdateActualizar_Click(object sender, ImageClickEventArgs e)
    {
        CheckBox Ch = (CheckBox)DVForma.FindControl("CheckBox1");
        if (Ch.Checked == true)
        {
            TextBox Txt = (TextBox)DVForma.FindControl("TextBox2");
            Txt.Text = "1";

            this.GrupoByIdDataSource.UpdateParameters["FormaHabilitar"].DefaultValue = "1";
        }
        else
        {
            TextBox Txt = (TextBox)DVForma.FindControl("TextBox2");
            Txt.Text = "0";
            this.GrupoByIdDataSource.UpdateParameters["FormaHabilitar"].DefaultValue = "0";
        }
        this.GrupoByIdDataSource.UpdateParameters["Original_FormaCodigo"].DefaultValue = HFCodigoSeleccionado.Value;
        //this.GrupoByIdDataSource.UpdateParameters["GrupoPermiso"].DefaultValue = RbtnLstPermiso.SelectedValue.ToString();
    }
    protected void ImgBtnEdit_Click(object sender, ImageClickEventArgs e)
    {
        //TextBox TxtBox = (TextBox)DVCiudad.FindControl("TxtPais");

        //if (TxtBox.Text != "")
        //{
        //    if (TxtBox.Text.Contains(" | "))
        //    {
        //        TxtBox.Text = TxtBox.Text.Remove(TxtBox.Text.IndexOf(" | "));
        //    }
        //}
        // this.GrupoByIdDataSource.UpdateParameters["GrupoPermiso"].DefaultValue = RbtnLstPermiso.SelectedValue.ToString();
        //this.GrupoByIdDataSource.UpdateParameters["Original_GrupoCodigo"].DefaultValue = HFCodigoSeleccionado.Value;
    }
    protected void ImgBtnInsert_Click(object sender, ImageClickEventArgs e)
    {
        CheckBox Ch = (CheckBox)DVForma.FindControl("CheckBox1");
        if (Ch.Checked == true)
        {
            TextBox Txt = (TextBox)DVForma.FindControl("TextBox2");
            Txt.Text = "1";
            this.GrupoByIdDataSource.InsertParameters["FormaHabilitar"].DefaultValue = "1";
        }
        else
        {
            TextBox Txt = (TextBox)DVForma.FindControl("TextBox2");
            Txt.Text = "0";
            this.GrupoByIdDataSource.InsertParameters["FormaHabilitar"].DefaultValue = "0";
        }
        //this.GrupoByIdDataSource.InsertParameters["GrupoPermiso"].DefaultValue = RbtnLstPermiso.SelectedValue.ToString();

    }
    protected void ImgBtnNew_Click(object sender, ImageClickEventArgs e)
    {
        this.TxtForma.Text = "";
    }
    protected void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        this.GrupoByIdDataSource.UpdateParameters["Original_FormaCodigo"].DefaultValue = HFCodigoSeleccionado.Value;
        this.TxtForma.Text = "";
        this.Label7.Text = "¿Va a eliminar la Accion seleccionada esta seguro?" + " ";
        this.MPEPregunta.Show();

    }


    protected void RbtnLstPermiso_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DSGrupoSQLTableAdapters.GrupoTableAdapter ObjTAGrpPer = new DSGrupoSQLTableAdapters.GrupoTableAdapter();
        //DSGrupoSQL.GrupoDataTable DTGrupoPer = new DSGrupoSQL.GrupoDataTable();

        //DTGrupoPer = ObjTAGrpPer.GetGroupById(HFCodigoSeleccionado.Value);

        //DataRow[] rows = DTGrupoPer.Select();
        //String Padre;

        //if (rows[0].ItemArray[2].ToString() == "")
        //    Padre = null;
        //else
        //    Padre = rows[0].ItemArray[2].ToString();

        //GrupoBLL ObjGrupo = new GrupoBLL();
        //bool correcto =  ObjGrupo.UpdateGrupo(rows[0].ItemArray[1].ToString(),
        //                                                Padre,
        //                                                Convert.ToInt32(rows[0].ItemArray[3].ToString()),
        //                                                rows[0].ItemArray[5].ToString(),
        //                                                this.RbtnLstPermiso.SelectedValue,
        //                                                rows[0].ItemArray[0].ToString());




    }
    protected void TCGrupo_ActiveTabChanged(object sender, EventArgs e)
    {
        if (this.TCForma.ActiveTabIndex.ToString() == "1")
        {
            if (this.HFCodigoSeleccionado.Value.Length.ToString() == "0")
            {
                this.LblMessageBox.Text = "No ha seleccionado un expediente";
                this.MPEMensaje.Show();

            }
            else
            {
                this.RbtnLstPermiso.Enabled = true;

                //Label Lb = (Label)DVExpedientePermiso.FindControl("Label1");
                //Lb.Text = HFCodigoSeleccionado.Value;

            }
        }
    }
    protected void RbtnLstSelPadre_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox TxtBox = (TextBox)DVForma.FindControl("TextBox1");
        RadioButtonList rblst = (RadioButtonList)DVForma.FindControl("RbtnLstSelPadre");

        if (rblst != null)
        {
            if (rblst.SelectedValue.ToString() == "0")
            {
                TxtBox.Visible = false;
                TxtBox.Text = null;
            }
            else
            {
                TxtBox.Visible = true;
            }

        }
    }
    protected void Button1_Click1(object sender, EventArgs e)
    {
        FormaBLL Forma = new FormaBLL();
        int Correcto;

        try
        {

            Correcto = Forma.DeleteForma(HFCodigoSeleccionado.Value);
        }
        catch (Exception Error)
        {
            this.LblMessageBox.Text = "Ocurrio un problema al tratar de eliminar el registro. ";
            this.MPEMensaje.Show();
        }

        //this.DVDepartamento.DataBind();
        this.LblMessageBox.Text = "Registro Eliminado";
        this.MPEMensaje.Show();
        this.TxtForma.Text = "";
    }
    protected void CheckBox2_CheckedChanged1(object sender, EventArgs e)
    {

        FormView f1 = (FormView)DVForma.FindControl("FVAutoNum");
        CheckBox Ch = (CheckBox)f1.FindControl("CkAuto");
        if (Ch.Checked == true)
        {
            TextBox Txt = (TextBox)DVForma.FindControl("TextBox1");
            TextBox Tx2 = (TextBox)f1.FindControl("TxCons");
            Txt.Text = Tx2.Text.ToString();
        }
        else
        {
            TextBox Txt = (TextBox)DVForma.FindControl("TextBox1");
            Txt.Text = "";
        }
    }
}

