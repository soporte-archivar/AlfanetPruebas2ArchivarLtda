using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
//using WebServiceAlfaBDU;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DSRadicadoTableAdapters;
using DSGrupoSQLTableAdapters;
using System.Text.RegularExpressions;
//using System.Web.Mail;
using System.Net.Mail;

using ASP;
using Microsoft;
using DSRadicadoTableAdapters;
using DSGrupoSQLTableAdapters;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Collections.Generic;
using AjaxControlToolkit;
using System.Text;
using iTextSharp.text.html.simpleparser;

public partial class PQR_Form : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
     {
         //this.tretTipoDePQRTram.Visible = false;
         if (!IsPostBack)
         {
             ctIdentificacion.Enabled = false;
             ctNombreProcedencia.Enabled = false;
             ctEmail.Enabled = false;
             ctTelefono.Enabled = false;
             ctDireccion.Enabled = false;
             ddlPais.Enabled = false;
             cddPais.Enabled = false;
             ddlDepartamento.Enabled = false;
             cddDepartamento.Enabled = false;
             ctDepartamento.Enabled = false;
             ddlCiudad.Enabled = false;
             cddCiudad.Enabled = false;
             ctCiudad.Enabled = false;
             ddlTipoDePQR.Enabled = false;
             ctDetalle.Enabled = false;
             this.tr_tipo_tramite.Attributes.Add("style", "display:none");


         }
         else
         {

             ctIdentificacion.Enabled = true;
             ctNombreProcedencia.Enabled = true;
             ctEmail.Enabled = true;
             ctTelefono.Enabled = true;
             ctDireccion.Enabled = true;
             ddlPais.Enabled = true;
             cddPais.Enabled = true;
             ddlDepartamento.Enabled = true;
             cddDepartamento.Enabled = true;
             ctDepartamento.Enabled = true;
             ddlCiudad.Enabled = true;
             cddCiudad.Enabled = true;
             ctCiudad.Enabled = true;
             ddlTipoDePQR.Enabled = true;
             ctDetalle.Enabled = true;

             SqlParameter parameter = new SqlParameter("@NaturalezaCodigoPadre", ddlTipoDePQR.SelectedValue);


             if (parameter.Value == "")
             {

                 DSNaturalezaSQLTableAdapters.NaturalezaTableAdapter solicitud = new DSNaturalezaSQLTableAdapters.NaturalezaTableAdapter();

                 ddlTipoDePQR.DataTextField = "NaturalezaNombre";
                 ddlTipoDePQR.DataValueField = "NaturalezaCodigo";
                 ddlTipoDePQR.DataSource = solicitud.GetNaturalezaByPQR();
                 ddlTipoDePQR.DataBind();

                 System.Web.UI.WebControls.ListItem lisolicitud = new System.Web.UI.WebControls.ListItem("Seleccione una solicitud", "-1");
                 ddlTipoDePQR.Items.Insert(0, lisolicitud);
             }

         }
        this.ctTelefono.Attributes.Add("onkeypress", "return validarSoloNumeros(event)");
        this.ctIdentificacion.Attributes.Add("onkeypress", "return validarIdentificacion(event)");
        this.ctDetalle.Attributes.Add("onkeypress", "ConteoCaracteres('" + this.ctDetalle.ClientID  + "', '" + this.etMaximoCaracteres.ClientID + "', 300)");
        this.ctDetalle.Attributes.Add("onblur", "ConteoCaracteres('" + this.ctDetalle.ClientID + "', '" + this.etMaximoCaracteres.ClientID + "', 300)");
        this.ddlTipoDeIdentificacion.Attributes.Add("onchange", "DefineEstiloCCSenListaDesplegable(this, 'WaterMarkedDDL', 'cajasTexto')");
        this.ddlTipoDePQR.Attributes.Add("onchange", "DefineEstiloCCSenListaDesplegable(this, 'WaterMarkedDDL', 'cajasTexto')");
    }
    protected void ddlPais_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ("170" == this.ddlPais.SelectedValue) 
        {
            this.etDepartamento.Text = "DEPARTAMENTO";
            this.ddlDepartamento.Visible = true;
            this.ctDepartamento.Visible = false;
            this.cvDepartamento.ControlToValidate = this.ddlDepartamento.ClientID;

            this.ddlCiudad.Visible = true;
            this.ctCiudad.Visible = false;
            this.cvCiudad.ControlToValidate = this.ddlCiudad.ClientID;

        }
        else
        {
            this.etDepartamento.Text = "PROVINCIA";
            this.ddlDepartamento.Visible = false;
            this.ctDepartamento.Visible = true;
            this.cvDepartamento.ControlToValidate = this.ctDepartamento.ClientID;

            this.ddlCiudad.Visible = false;
            this.ctCiudad.Visible = true;
            this.cvCiudad.ControlToValidate = this.ctCiudad.ClientID;
        }
        
    }
    protected void ddlTipoDePQR_SelectedIndexChanged(object sender, EventArgs e)
    {

        DSNaturalezaSQLTableAdapters.NaturalezaTableAdapter tramite = new DSNaturalezaSQLTableAdapters.NaturalezaTableAdapter();

        ddlTipoDePQRTram.DataTextField = "NaturalezaNombre";
        ddlTipoDePQRTram.DataValueField = "NaturalezaCodigo";
        SqlParameter parameter = new SqlParameter("@NaturalezaCodigoPadre", ddlTipoDePQR.SelectedValue);
        ddlTipoDePQRTram.DataSource = tramite.GetNaturalezaByPQRTram(parameter.Value.ToString());
        ddlTipoDePQRTram.DataBind();

        if (ddlTipoDePQRTram.SelectedItem != null)
        {
            this.tr_tipo_tramite.Attributes.Add("style", "display:normal");
            System.Web.UI.WebControls.ListItem litramite = new System.Web.UI.WebControls.ListItem("Seleccione un Tramite", "-1");
            ddlTipoDePQRTram.Items.Insert(0, litramite);
            ddlTipoDePQRTram.SelectedValue = "-1";
        }
        else
        {
            this.tr_tipo_tramite.Attributes.Add("style", "display:none");
            trAdjuntar_doc.Attributes.Add("style", "display:normal");
            adjuntar_doc1.Attributes.Add("style", "display:none");
        }

        if (this.ddlTipoDePQR.SelectedValue == "99999999")
        {
            tr_identificacion.Attributes.Add("style", "display:none");
            tr_procedencia.Attributes.Add("style", "display:none");
            tr_email.Attributes.Add("style", "display:none");
            tr_telefono.Attributes.Add("style", "display:none");
            tr_pais.Attributes.Add("style", "display:none");
            tr_departamento.Attributes.Add("style", "display:none");
            tr_ciudad.Attributes.Add("style", "display:none");
            tr_direccion.Attributes.Add("style", "display:none");
            tr_tipo_pqr.Attributes.Add("style", "display:none");
            ctDetalle.Enabled = true;
            this.tr_tipo_tramite.Attributes.Add("style", "display:none");
            this.ddlTipoDeIdentificacion.SelectedValue = "sa";
        }
    }
    //protected void ddlTipoDePQR_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Label2.Visible = false;
    //    List<CascadingDropDownNameValue> values =
    //        new List<CascadingDropDownNameValue>();
    //    NaturalezaBLL ObjNaturalezas = new NaturalezaBLL();

    //    foreach (DataRow dr in ObjNaturalezas.GetNaturalezaByPQRTram(ddlTipoDePQR.SelectedValue))
    //    {
    //        string mNaturalezaNombre = (string)dr["NaturalezaNombre"];
    //        string mNaturalezaCodigo = (string)dr["NaturalezaCodigo"];
    //        values.Add(new CascadingDropDownNameValue(
    //          mNaturalezaNombre, mNaturalezaCodigo));
    //    }
    //    if (values.Count > 0)

    //    //if (null != this.ddlTipoDePQR.SelectedValue)
    //    {
    //        //this.tretTipoDePQRTram.Attributes.Add("style", "display:normal");
    //        //this.ddlTipoDePQRTram.Visible = true;
    //        this.cvTipoPQRTram.ControlToValidate = this.ddlTipoDePQRTram.ClientID;
    //        this.adjuntar_doc1.Visible = false;            
    //    }
    //    else
    //    {
    //    //    //this.ddlTipoDePQRTram.Visible = false;
    //    //    //this.cvTipoPQR.Visible = false;
    //    //    //this.etTipoDePQRTram.Visible = false;
    //        //this.tretTipoDePQRTram.Visible = false;
    //        this.trAdjuntar_doc.Visible = true;
    //        this.Label3.Visible = false;
    //        this.Label1.Visible = false;
    //        this.Table2.Visible = false;
    //    }
    //}

    protected void ddlTipoDeIdentificacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(ddlTipoDeIdentificacion.SelectedValue  == "sa")
        {
            
             tr_identificacion.Attributes.Add("style","display:none");
             tr_procedencia.Attributes.Add("style","display:none");
             tr_email.Attributes.Add("style","display:none");
             tr_telefono.Attributes.Add("style","display:none");
             tr_pais.Attributes.Add("style","display:none");
             tr_departamento.Attributes.Add("style","display:none");
             tr_ciudad.Attributes.Add("style","display:none");
             tr_direccion.Attributes.Add("style","display:none");
             tr_tipo_pqr.Attributes.Add("style","display:none");
             ctDetalle.Enabled = true;
             this.tr_tipo_tramite.Attributes.Add("style", "display:none");
             this.ddlTipoDePQR.SelectedValue = "99999999";


        }
        else if(ddlTipoDeIdentificacion.SelectedValue =="---Seleccione un Tipo de Documento---"){
            ctIdentificacion.Enabled = false;
             ctNombreProcedencia.Enabled = false;
             ctEmail.Enabled = false;
             ctTelefono.Enabled = false;
             ctDireccion.Enabled = false;
             ddlPais.Enabled = false;
             cddPais.Enabled = false;
             ddlDepartamento.Enabled = false;
             cddDepartamento.Enabled = false;
             ctDepartamento.Enabled = false;
             ddlCiudad.Enabled = false;
             cddCiudad.Enabled = false;
             ctCiudad.Enabled = false;
             ddlTipoDePQR.Enabled = false;
             ctDetalle.Enabled = false;
             tr_tidentificacion.Attributes.Add("style","display:table-row");
             tr_procedencia.Attributes.Add("style","display:table-row");
             tr_email.Attributes.Add("style","display:table-row");
             tr_telefono.Attributes.Add("style","display:table-row");
             tr_pais.Attributes.Add("style","display:table-row");
             tr_departamento.Attributes.Add("style","display:table-row");
             tr_ciudad.Attributes.Add("style","display:table-row");
             tr_direccion.Attributes.Add("style","display:table-row");
             tr_tipo_pqr.Attributes.Add("style","display:table-row");
             infoLabel.Attributes.Add("style","color: Red; font-size: small; width: 100%");
             this.ddlTipoDePQR.SelectedIndex = 0;
        }
        else{
            tr_tidentificacion.Attributes.Add("style","display:table-row");
             tr_procedencia.Attributes.Add("style","display:table-row");
             tr_email.Attributes.Add("style","display:table-row");
             tr_telefono.Attributes.Add("style","display:table-row");
             tr_pais.Attributes.Add("style","display:table-row");
             tr_departamento.Attributes.Add("style","display:table-row");
             tr_ciudad.Attributes.Add("style","display:table-row");
             tr_direccion.Attributes.Add("style","display:table-row");
             tr_tipo_pqr.Attributes.Add("style","display:table-row");
             infoLabel.Attributes.Add("style","color: Red; font-size: small; width: 100%");
             this.ddlTipoDePQR.SelectedIndex = 0;
             ctDetalle.Enabled = true;
        }
    }
    protected void ctIdentificacion_TextChanged(object sender, EventArgs e)
    {
        /*pregunta si existe en Alfanet*/
        //Creamos un tableAdapter
        DSProcedenciaSQLTableAdapters.ProcedenciaTableAdapter TAProcedencias = new DSProcedenciaSQLTableAdapters.ProcedenciaTableAdapter();
        //Creamos un DataTable
        DSProcedenciaSQL.ProcedenciaDataTable DTProcedencia = new DSProcedenciaSQL.ProcedenciaDataTable();
        DTProcedencia = TAProcedencias.GetProcedenciaIdV2DataBy(this.ctIdentificacion.Text.Trim().ToString(), ddlTipoDeIdentificacion.SelectedValue.ToString().Trim());
        //DTProcedencia = ProcedenciaBLL.GetProcedenciaByID(this.txtUserId.Text.Trim().ToString());

        //Si la procedencia ya existe en base de datos
        if (DTProcedencia.Rows.Count == 1)
        {
            DSProcedenciaSQL.ProcedenciaRow DRProcedencia = (DSProcedenciaSQL.ProcedenciaRow)DTProcedencia.Rows[0];
            this.ctNombreProcedencia.Text = DRProcedencia.ProcedenciaNombre;
            this.ctTelefono.Text = DRProcedencia.ProcedenciaTelefono1;
            this.ctEmail.Text = DRProcedencia.ProcedenciaMail1;
            this.ctDireccion.Text = DRProcedencia.ProcedenciaDireccion;

            //this.UpdPnNombreProcedencia.Update();
            //this.UpdPnTelefono.Update();
            //this.UpdPnEmail.Update();
            //this.UpdPnDireccion.Update();
        }
        else if(DTProcedencia.Rows.Count == 0)
        {
            this.ctNombreProcedencia.Text = "";
            this.ctTelefono.Text = "";
            this.ctEmail.Text = "";
            this.ctDireccion.Text = "";
        }
    }
    protected void btEnviarPQR_Click(object sender, EventArgs e)
     {
        btEnviarPQR.Enabled = false;
        Boolean ExisteProcedencia = false, ErrorDeValidacion = true;
        int ActualizaProcedencia = 0;
        string CiudadCodigo = "";
        
        //VALIDAMOS LOS CAMPOS DILIGENCIADOS POR EL USUARIO
        //Validando El tipo de Identificacion
        string[] TiposDeId = {"cc", "ti", "ce", "nit", "pas","sa"};
        
        foreach (string tipoDeID in TiposDeId )
        {
            if (ddlTipoDeIdentificacion.SelectedValue == tipoDeID)
                ErrorDeValidacion = false;
        }
        if (ErrorDeValidacion) 
        {
            ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            this.etMsgPopuMensaje.Text = "Error: No ha elegido un Tipo de Identificación Valido";
			this.ddlTipoDePQRTram.SelectedValue = "-1";
            return;
        }
        //ErrorDeValidacion = true;
        
        if ("0" == ddlTipoDeIdentificacion.SelectedValue || "" == ddlTipoDeIdentificacion.SelectedValue)
        {
            this.cvTipoIdentificacion.Visible = true;
        }
               
        
        if (Request.Files.Count > 0 )
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFile PostedFile = Request.Files[i];                        
                    
                    if (PostedFile.ContentLength > 0)
                    {
                        string FileName = System.IO.Path.GetFileName(PostedFile.FileName);

                        if (!(FileName.ToString().ToLower().Contains(".jpg") ||
                        FileName.ToString().ToLower().Contains(".pdf") ||
                        FileName.ToString().ToLower().Contains(".gif") ||
                        FileName.ToString().ToLower().Contains(".png") ||
                        FileName.ToString().ToLower().Contains(".doc") ||
                        FileName.ToString().ToLower().Contains(".zip") ||
                        FileName.ToString().ToLower().Contains(".rar") ||
                        FileName.ToString().ToLower().Contains(".jpeg") ||
                        FileName.ToString().ToLower().Contains(".docx")))

                        {
                                ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                                this.etMsgPopuMensaje.Text = "Error: El tipo de archivo seleccionado no es valido. Los Formatos válidos son: Imagen (jpg,jpeg,gif,png), Texto (doc,docx,pdf), compresión de archivos (zip, rar)";
                                this.ddlTipoDePQRTram.SelectedValue = "-1";
								return;
                        }
                    }
                    else
                    {
                        DSNaturalezaSQLTableAdapters.Naturaleza_ReadAnexoByPQRTramTableAdapter TaAnexo = new DSNaturalezaSQLTableAdapters.Naturaleza_ReadAnexoByPQRTramTableAdapter();

                        DataTable dtAnexo = TaAnexo.GetNaturaleza_ReadAnexo(ddlTipoDePQRTram.SelectedValue);

                        //dtAnexo.ItemArray[2];

                        //string Anexo = "";

                        DataRow[] item;
                        item = dtAnexo.Select();

                        if(item[i].ItemArray[2].ToString() == "1")
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                            this.etMsgPopuMensaje.Text = "Error: Debes anexar todas las imagenes solicitadas";
                            this.ddlTipoDePQRTram.SelectedValue = "-1";
							return;
                        }

                        //Datarow[] aa = dtAnexo.Select();

                        //foreach (DataRow item in dtAnexo.Rows)
                        //{
                        //    Anexo = item[2].ToString();

                            //if (Anexo == "1")
                            //{
                            //    ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
                            //    this.etMsgPopuMensaje.Text = "Error: Debes anexar todas las imagenes solicitadas";
                            //    return;
                            //}

                        //}
                    }
            }
        }

        try
        {
            //NOS PREPARAMOS PARA INSERTAR DATOS
            //Obtenemos el GrupoCodigo de los Radicados
            rutinas Rutinas = new rutinas();
            string GrupoRadicado = ((DataTable)Rutinas.rtn_traer_tbtablas_por_Id("GRUPORAD")).Rows[0][0].ToString().Trim();

            //si es solicitud anonima
            //if (ddlTipoDeIdentificacion.SelectedValue == "sa" || this.ddlTipoDePQR.SelectedValue == "99999999")
            //{
            //    this.ctIdentificacion.Text = "99999999999";
            //    this.ctCiudad.Text = "999999999999";
            //    this.ctDepartamento.Text = "999999999";
            //    this.cddPais.SelectedValue = "99999999";

            //    this.ctNombreProcedencia.Text = "USUARIO ANONIMO";
            //    this.ctDireccion.Text = "99999999";
            //    this.ctTelefono.Text = "99999999";
            //    this.ctEmail.Text = "anonimo@anonimo.com";
            //    this.ddlTipoDePQR.SelectedValue = "99999999";

            //}
            //Verificamos que no se haya grabado un radicado identico 
            RadicarTramiteInTableAdapters.Radicado_RadicadoExistentePQRTableAdapter TARadicadoExiste = new RadicarTramiteInTableAdapters.Radicado_RadicadoExistentePQRTableAdapter();
            //Esta parte no se implementa porque uno de los parametros en la consulta es la fecha del radicado y esta siempre va a cambiar de registro en registro

            //Verificamos si la procedencia ya existe en Alfanet
            DSProcedenciaSQLTableAdapters.ProcedenciaTableAdapter TAProcedencias = new DSProcedenciaSQLTableAdapters.ProcedenciaTableAdapter();
            //Creamos un DataTable
            DSProcedenciaSQL.ProcedenciaDataTable DTProcedencia = new DSProcedenciaSQL.ProcedenciaDataTable();
            DTProcedencia = TAProcedencias.GetProcedenciaIdV2DataBy(this.ctIdentificacion.Text.Trim().ToString(), ddlTipoDeIdentificacion.SelectedValue.ToString().Trim());
            if (1 == DTProcedencia.Rows.Count)
            {
                ExisteProcedencia = true;

            }


            //Verificamos que si se eligio un pais diferente de colombia,
            //la ciudad suministrada exista en base de datos o de no crear la ciudad
            if ("170" != this.ddlPais.SelectedValue)
                CiudadCodigo = ((DataTable)Rutinas.rtn_verificar_CiudadPQR(this.ctCiudad.Text, this.ctDepartamento.Text, this.ddlPais.SelectedValue)).Rows[0][0].ToString().Trim();
            else if("99999999" == this.cddPais.SelectedValue)
                CiudadCodigo = cddPais.SelectedValue;
            else
                CiudadCodigo = ddlCiudad.SelectedValue;



            //Verificamos la existencia de la procedencia, si no existe hay que crearla, si existe
            //ver si cambio para hacer la actualización
            ActualizaProcedencia = (ExisteProcedencia) ? 1 : 0;
			
			//Creación Captcha John Vela 11/08/2016 para evitar Robot Informaticos
            String CaptchaString = Session["CAPTCHA"].ToString();


            if (this.TxtBCaptcha.Text.Equals(CaptchaString))
            {
                this.LblCaptchaError.Text = "";
            }
            else
            {
                this.LblCaptchaError.Text = "Captcha invalido";   
				this.ddlTipoDePQRTram.SelectedValue = "-1";				
            }

            //...[Insertamos La procedencia y el radicado]
            //Este paso actualiza o inserta la procedencia y a la vez crea el radicado
            //Creamos el TableAdapter
            RadicarTramiteInTableAdapters.insertarprocedenciaTableAdapter TAInsertProcyRad = new RadicarTramiteInTableAdapters.insertarprocedenciaTableAdapter();
            //Creamos el DataTable
            RadicarTramiteIn.insertarprocedenciaDataTable DTInsertProcyRad = new RadicarTramiteIn.insertarprocedenciaDataTable();
            //Obtenemos datos de una plantialla
            DSRadicadoTableAdapters.PlantillaPQRTableAdapter TAPlantillaPQR = new DSRadicadoTableAdapters.PlantillaPQRTableAdapter();
            DSRadicado.PlantillaPQRDataTable DTPlantillaPQR = new DSRadicado.PlantillaPQRDataTable();
            DTPlantillaPQR = TAPlantillaPQR.GetPlantillaPQR();
            DSRadicado.PlantillaPQRRow DRPlantPQR = (DSRadicado.PlantillaPQRRow)DTPlantillaPQR.Rows[0];
            String TipoPQR;

            TipoPQR = ddlTipoDePQR.SelectedValue;

            if (ddlTipoDePQRTram.SelectedValue != "")
            {
                TipoPQR = ddlTipoDePQRTram.SelectedValue;                
            }
            else
            {
                TipoPQR = ddlTipoDePQR.SelectedValue;
            }
            DSNaturalezaSQLTableAdapters.NaturalezaTableAdapter ObjTANat = new DSNaturalezaSQLTableAdapters.NaturalezaTableAdapter();
            DSNaturalezaSQL.NaturalezaDataTable DTNAturaleza = new DSNaturalezaSQL.NaturalezaDataTable();
            DTNAturaleza = ObjTANat.GetNaturalezaById(TipoPQR);
            string depCodPqr = DTNAturaleza[0].NaturalezaDependenciaPQR;
            //Validamos que se envien los datos completos a la BD --John Vela 14/06/2016--
            if (ctIdentificacion.Text != null & ctNombreProcedencia.Text != null & ctDireccion.Text != null & CiudadCodigo != null & ctTelefono.Text != null & ctEmail.Text != null & TipoPQR != null & ctDetalle.Text != null & ctIdentificacion.Text != "" & ctNombreProcedencia.Text != "" & ctDireccion.Text != "" & CiudadCodigo != "" & ctTelefono.Text != "" & ctEmail.Text != "" & TipoPQR != "" & ctDetalle.Text != "" & this.TxtBCaptcha.Text.Equals(CaptchaString))
            {
                DTInsertProcyRad = TAInsertProcyRad.GetinsertpqrBy(ctIdentificacion.Text, ctNombreProcedencia.Text, ctDireccion.Text, CiudadCodigo, ctTelefono.Text, "", ctEmail.Text, "", null, "", TipoPQR, depCodPqr, DRPlantPQR.ExpedienteCodigo, ctDetalle.Text, DateTime.Now, ddlTipoDeIdentificacion.SelectedValue, ActualizaProcedencia.ToString());
            }
            if(this.ddlTipoDePQR.SelectedValue == "99999999" || ddlTipoDeIdentificacion.SelectedValue == "sa")
            {
                DTInsertProcyRad = TAInsertProcyRad.GetinsertpqrBy("99999999999", "USUARIO ANONIMO", "99999999", "999999999999", "99999999", null, "anonimo@anonimo.com", "", null, "", this.ddlTipoDePQR.SelectedValue.ToString(), depCodPqr, DRPlantPQR.ExpedienteCodigo, ctDetalle.Text, DateTime.Now,"", "0");
            }
			string coderror = DTInsertProcyRad[0].ErrorNumber;
            if (!string.IsNullOrEmpty(coderror))
                throw new ApplicationException(DTInsertProcyRad[0].ErrorMessage);

            //Si hasta aqui todo va bien, hay que crear el documento pdf a partir del detalle del radicado
            string CodigoDocumento = DTInsertProcyRad[0].RadicadoCodigo.ToString();
            create_pdf(GrupoRadicado, CodigoDocumento, ddlTipoDeIdentificacion.SelectedValue, ctIdentificacion.Text,
                       ctNombreProcedencia.Text, ctDireccion.Text, ddlCiudad.SelectedValue,ddlDepartamento.SelectedValue, ctTelefono.Text, ctEmail.Text, ctDetalle.Text);

            //Ahora se suben los archivos que el usuario adjunto
            DSImagenTableAdapters.RadicadoImagenTableAdapter TARadicadoImagen = new DSImagenTableAdapters.RadicadoImagenTableAdapter();
            DSImagenTableAdapters.ImagenRutaTableAdapter TAImgRuta = new DSImagenTableAdapters.ImagenRutaTableAdapter();
            DSImagen.ImagenRutaDataTable DTImgRuta = new DSImagen.ImagenRutaDataTable();
            Object CodigoRuta = TAImgRuta.SelectRutaCodigoByAnioMesGrupo(DateTime.Today.Year, DateTime.Today.Month, "1");
            int codigoR = Convert.ToInt32(CodigoRuta); //41;//Convert.ToInt32(CodigoRuta);
            String Grupo = "Radicados";
            String Ano = DateTime.Today.Year.ToString();
            String Mes = DateTime.Today.Month.ToString();
            String PathVirtual = HttpContext.Current.Server.MapPath("~/AlfaNetRepositorioImagenes/" + Grupo + "/" + Ano + "/" + Mes + "/");


            if (null != Request.Files)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (this.Request.Files[i].FileName != "")
                    {
                        if (!Directory.Exists(PathVirtual))
                            Directory.CreateDirectory(PathVirtual);

                        if (CodigoRuta == null)
                        {
                            TAImgRuta.Insert(1, "", DateTime.Today.Year, DateTime.Today.Month, 1, PathVirtual);
                            CodigoRuta = TAImgRuta.SelectRutaCodigoByAnioMesGrupo(DateTime.Today.Year, DateTime.Today.Month, "1");
                            codigoR = Convert.ToInt32(CodigoRuta);
                        }
                        string NombreImagen = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Request.Files[i].FileName;
                        NombreImagen = NormalizeFileName(NombreImagen);

                        if (NombreImagen.Contains(""))
                        {
                            NombreImagen = NombreImagen.Replace("", "-");
                        }
                        if (NombreImagen.Contains("#"))
                        {
                            NombreImagen = NombreImagen.Replace("#", "-");
                        }
                        if (NombreImagen.Contains("%"))
                        {
                            NombreImagen = NombreImagen.Replace("%", "-");
                        }

                        if(NombreImagen.Length >= 39 )
                        { 
                            String[] Extension = NombreImagen.Split('.');
                            String TipoArchivo = Extension[Extension.Length - 1];
                            NombreImagen = NombreImagen.Substring(0, 35) + "." + TipoArchivo;
                        }

                        TARadicadoImagen.InsertRadicadoImagen(GrupoRadicado, Convert.ToInt32(CodigoDocumento), NombreImagen, codigoR);
                        Request.Files[i].SaveAs(PathVirtual + NombreImagen);

                    }
                }
            }

            MailBLL Correo = new MailBLL();
        //    MembershipUser usuario;
        //    DSUsuarioTableAdapters.UsuariosxdependenciaTableAdapter ObjTaUsuarioxDependencia = new DSUsuarioTableAdapters.UsuariosxdependenciaTableAdapter();
        //    DSUsuario.UsuariosxdependenciaDataTable DTUsuariosxDependencia = new DSUsuario.UsuariosxdependenciaDataTable();

        //    DTUsuariosxDependencia = ObjTaUsuarioxDependencia.GetUsuariosxDependenciaByDependencia(DDLNaturalezaDependenciaPQR.SelectedValue);
        //    if (DTUsuariosxDependencia.Count > 0)
        //    {
        //        DataRow[] rows = DTUsuariosxDependencia.Select();
        //        System.Guid a = new Guid(rows[0].ItemArray[0].ToString().Trim());
        //        usuario = Membership.GetUser(a);
            string url = "https://alfanet.fbscgr.gov.co/alfanetprueba/PQR_find.aspx?";
            string Body = "El Fondo de bienestar social de la Contraloría General de la Republica" + 
			"<BR>" + "le informa que su solicitud ha sido radicada con el" + "<b> número " + CodigoDocumento.ToString() + "</b>" +
            "<BR>" + " Fecha y hora de Radicación: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + 
			"<BR>" + "Procedencia: " + ctNombreProcedencia.Text.ToString() + 
			"<BR>" + "<BR>" + "Tenga presente este número de solicitud para consultar su trámite."+
            "<BR>"+"Para conocer el estado de su solicitud ingrese al siguiente enlace"+
			"<a href="+"https://alfanet.fbscgr.gov.co/alfanetprueba/PQR_find.aspx?"+">"+" Consultar"+"</a>";
			
           // Correo.EnvioCorreo("mesadeservicio@mintic.gov.co", ctEmail.Text, "Radicado Nro" + " " + CodigoDocumento.ToString(), Body, true, "1");
         //   }

          /*  try
           {*/
                //Configuración del Mensaje
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient();
                SmtpServer.Host="smtp.gmail.com";
                SmtpServer.EnableSsl = true;
                SmtpServer.Port = 587; //Puerto que utiliza Gmail para sus servicios
                //Especificamos las credenciales con las que enviaremos el mail
                SmtpServer.Credentials = new System.Net.NetworkCredential("alfanetpruebas@gmail.com", "gujjqkjimwglgdfa");
                //Especificamos el correo desde el que se enviará el Email y el nombre de la persona que lo envía
                mail.From = new MailAddress("gestiondocumental@fbscgr.gov.co", "PQR Fondo de bienestar social de la Contraloría General de la Republica", Encoding.UTF8);
                //Aquí ponemos el asunto del correo
                mail.Subject = "Radicado Nro" + " " + CodigoDocumento.ToString();
                //Aquí ponemos el mensaje que incluirá el correo

                mail.Body = "Bogotá, " + DateTime.Now.ToLongDateString() + "<BR><BR>" + "Señor(a)" + "<BR>" + ctNombreProcedencia.Text + "<BR><BR>" + "<BR><BR>" + "Cordial Saludo," + "<BR><BR>" +
                   "El Fondo de Bienestar Social de la Contraloría General de la República le informa que su solicitud ha sido radicada en nuestro sistema de Gestión Documental Alfanet" + 
                   "<BR>" + "<BR>" + "<b> Número de Radicado " + CodigoDocumento.ToString() + "</b>" +
                   "<BR>" + " Fecha y hora de Radicación: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + 
                   "<BR>"  + "Procedencia: " + ctNombreProcedencia.Text.ToString() + 
                   "<BR>" + "Detalle: " + ctDetalle.Text.ToString() + 
                   "<BR>" + "<BR>" + "Tenga presente este número de solicitud para consultar su trámite." +
                   "<BR>" + "Si desea conocer el estado de su solicitud ingrese al siguiente enlace: " +
                   "<a href=" + "https://alfanet.fbscgr.gov.co/alfanetprueba/PQR_find.aspx" + ">" + "Consultar" + "</a>" +
                   "<BR>" + "<BR>" +
                   "Nota: Favor no responder este correo, este medio solo es utilizado para informar sobre su trámite ante el Fondo de Bienestar Social de la Contraloría General de la República." + "<BR>" + "<BR>" +
                   "<b> Aviso de Privacidad </b>" + 
                   "<BR>" + "En cumplimiento con lo establecido en la Ley 1581 de 2012 sobre Protección de Datos Personales" + "<BR>" +
                   "y su Decreto Reglamentario 1377 de 2013, el Fondo de Bienestar Social de la Contraloría General de la República informa que garantiza la confidencialidad" + "<BR>" +
                   "de los datos personales facilitados por los usuarios y su tratamiento de acuerdo con la legislación sobre protección de datos de carácter personal;" + "<BR>" +
                   "siendo de uso exclusivo de la entidad y trasladados a terceros con autorización previa del usuario." + "<BR>";
                mail.IsBodyHtml = true;
                //Especificamos a quien enviaremos el Email, no es necesario que sea Gmail, puede ser cualquier otro proveedor
                if(ctEmail.Text != "")
                {
                    mail.To.Add(ctEmail.Text);

                    //Si queremos enviar archivos adjuntos tenemos que especificar la ruta en donde se encuentran
                    //mail.Attachments.Add(new Attachment(@"C:\Documentos\carta.docx"));
                    //Configuracion del SMTP




                    SmtpServer.Send(mail);
                }

                //return true;
           /* }
            catch (Exception ex)
            {
                //return false;
            }*/


          /*  MailMessage msg = new MailMessage();

            msg.To = this.ctEmail.Text;

            msg.From = "alfanet.archivar@gmail.com";

            msg.Subject = "El asunto del mail";

            msg.Body = "Este es el contenido del email";

            msg.Priority = MailPriority.High;

            msg.BodyFormat = MailFormat.Text; //o MailFormat.Html */

            //Si todo ha salido bien, Enviamos un mensaje en pantalla y blanqueamos el formulario
            string mensajeExitoso = "";
            if ("nit" == ddlTipoDeIdentificacion.SelectedValue)
                mensajeExitoso = "Señores ";
            else
                mensajeExitoso = "Señor(a) ";

            mensajeExitoso += ctNombreProcedencia.Text + ", su solicitud ha sido radicada con el No. "
                           + CodigoDocumento.ToString() + ", tenga en cuenta este número para hacerle seguimiento a su solicitud.</br> <b>Aviso Importante:</b> Los radicados generados después de las 5 pm (hora colombiana) serán gestionados con entusiasmo y eficiencia al siguiente día hábil. Agradecemos tu comprensión y paciencia. ¡Estamos aquí para ayudarte!";

            ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            this.etMsgPopuMensaje.Text = mensajeExitoso;
            this.btMsgPupup.OnClientClick = "parent.location.href='http://www.fbscgr.gov.co/'; return false;";
            //this.etMsgPopuMensaje.ForeColor = System.Drawing.Color.DodgerBlue;
            

            
            //Si todo salio bien blanqueamos el Formulario
            this.ddlTipoDeIdentificacion.SelectedIndex = 0;
            this.ctIdentificacion.Text = "";
            this.ctNombreProcedencia.Text = "";
            this.ctDireccion.Text = "";
            this.ctTelefono.Text = "";
            this.ctEmail.Text = "";
            this.ddlPais.SelectedIndex= 0;
            this.ddlTipoDePQR.SelectedIndex = 0;
            this.ctDetalle.Text = "";

        }
        // catch(Exception excepcion)
        // {
            // ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            // this.pnMsgPopup.Visible = true;
            // this.etMsgPopuMensaje.Text = "Error: Ocurrio un error al guardar la PQR " + excepcion.Message;
            
        // }
		catch(Exception excepcion)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "key", "launchModal();", true);
            this.pnMsgPopup.Visible = true;
            if (excepcion.Message == "No se pudieron habilitar las restricciones. Una o varias filas contienen valores que infringen las restricciones NON-NULL, UNIQUE o FOREIGN-KEY.")
            
                this.etMsgPopuMensaje.Text = "Error: Ocurrio un error al guardar la PQR. Por favor valide los datos del formulario.";
				this.ddlTipoDePQRTram.SelectedValue = "-1";
            
            if (excepcion.Message == "No hay ninguna fila en la posición 0.")

                this.etMsgPopuMensaje.Text = "Error: Ocurrio un error al guardar la PQR. Por favor valide los datos del formulario.";
				this.ddlTipoDePQRTram.SelectedValue = "-1";
                       
        }

        //------------------------------
    }
    public static string NormalizeFileName(string fileName)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(
             new string(System.IO.Path.GetInvalidFileNameChars())
        );
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return System.Text.RegularExpressions.Regex.Replace(fileName, invalidRegStr, "");
    }

    /*Metodo para como primer documento adjunto el detalle del radicado*/
    protected void create_pdf(string GrupoRadicado, string CodigoDocumento, string tipoid, string nid, string nombre, string dir,string ciudad,string departamento, string telefono, string correo, string detalle)
    {
        /*Obtener Departamento*/

        DSDepartamentoSQLTableAdapters.DepartamentoTableAdapter TADepartamento = new DSDepartamentoSQLTableAdapters.DepartamentoTableAdapter();
        DataTable dtDepartamento = TADepartamento.GetDepartamentoById(departamento);
        string nombredepartamento = "";
        foreach (DataRow item in dtDepartamento.Rows)
        {
            nombredepartamento = item[1].ToString();
        }

        /*Obtener la descripcion de la ciudad*/
        DSCiudadSQLTableAdapters.CiudadTableAdapter TACiudad = new DSCiudadSQLTableAdapters.CiudadTableAdapter();
        DataTable ciudadd = TACiudad.GetCiudadById(ciudad);
        string nombreciudad="";
        foreach (DataRow item in ciudadd.Rows)
        {
            nombreciudad = item[1].ToString();
        }
        
        

        /*Guardarlo en alfanet*/
        DSImagenTableAdapters.RadicadoImagenTableAdapter TARadicadoImagen = new DSImagenTableAdapters.RadicadoImagenTableAdapter();
        DSImagenTableAdapters.ImagenRutaTableAdapter TAImgRuta = new DSImagenTableAdapters.ImagenRutaTableAdapter();
        DSImagen.ImagenRutaDataTable DTImgRuta = new DSImagen.ImagenRutaDataTable();

        Object CodigoRuta = TAImgRuta.SelectRutaCodigoByAnioMesGrupo(DateTime.Today.Year, DateTime.Today.Month, "1");
        int codigoR = Convert.ToInt32(CodigoRuta);
        String Grupo = "Radicados";
        String Ano = DateTime.Today.Year.ToString();
        String Mes = DateTime.Today.Month.ToString();

        String PathVirtual = HttpContext.Current.Server.MapPath("~/AlfaNetRepositorioImagenes/" + Grupo + "/" + Ano + "/" + Mes + "/");
        /**/
        Document document = new Document();
        if (!Directory.Exists(PathVirtual))
        {
            Directory.CreateDirectory(PathVirtual);
        }
        PdfWriter.GetInstance(document, new FileStream(PathVirtual + "soporterad" + CodigoDocumento + ".pdf", FileMode.OpenOrCreate));
        document.Open();
        document.Add(new Paragraph("Documentos de soporte"));
        document.Add(new Paragraph(""));
        document.Add(new Paragraph("Nombre : " + nombre));
        document.Add(new Paragraph("Identificación : " + nid));
        document.Add(new Paragraph("N° Radicado : " + CodigoDocumento));
		document.Add(new Paragraph("Fecha y Hora : "  + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString()));
        document.Add(new Paragraph("E-Mail : " + correo));
        document.Add(new Paragraph("Teléfono : " + telefono));
        document.Add(new Paragraph("Dirección : " + dir));
        document.Add(new Paragraph("Ciudad : " + nombreciudad));
        document.Add(new Paragraph("Departamento : " + nombredepartamento));
        document.Add(new Paragraph("Detalle : "));
        document.Add(new Paragraph(detalle));
        document.Close();


        if (CodigoRuta == null)
        {
            TAImgRuta.Insert(1, "", DateTime.Today.Year, DateTime.Today.Month, 1, PathVirtual);
            CodigoRuta = TAImgRuta.SelectRutaCodigoByAnioMesGrupo(DateTime.Today.Year, DateTime.Today.Month, "1");
            codigoR = Convert.ToInt32(CodigoRuta);
            if (Directory.Exists(PathVirtual))
            {
                TARadicadoImagen.InsertRadicadoImagen(GrupoRadicado, Int32.Parse(CodigoDocumento), "soporterad" + CodigoDocumento + ".pdf", codigoR);
                //this.devExUpLoadArchivo.UploadedFiles[i].SaveAs(PathVirtual + this.devExUpLoadArchivo.UploadedFiles[i].FileName);
            }
            else
            {
                Directory.CreateDirectory(PathVirtual);
                TARadicadoImagen.InsertRadicadoImagen(GrupoRadicado, Int32.Parse(CodigoDocumento), "soporterad" + CodigoDocumento + ".pdf", codigoR);
                //this.devExUpLoadArchivo.UploadedFiles[i].SaveAs(PathVirtual + this.devExUpLoadArchivo.UploadedFiles[i].FileName);
            }
        }
        else
        {
            if (Directory.Exists(PathVirtual))
            {
                TARadicadoImagen.InsertRadicadoImagen(GrupoRadicado, Int32.Parse(CodigoDocumento), "soporterad" + CodigoDocumento + ".pdf", codigoR);
                //this.devExUpLoadArchivo.UploadedFiles[i].SaveAs(PathVirtual + this.devExUpLoadArchivo.UploadedFiles[i].FileName);
            }
            else
            {
                Directory.CreateDirectory(PathVirtual);
                TARadicadoImagen.InsertRadicadoImagen(GrupoRadicado, Int32.Parse(CodigoDocumento), "soporterad" + CodigoDocumento + ".pdf", codigoR);
                //this.devExUpLoadArchivo.UploadedFiles[i].SaveAs(PathVirtual + this.devExUpLoadArchivo.UploadedFiles[i].FileName);

            }
        }

        /*Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-disposition", "attachment; filename=" + "devjoker.pdf");
        Response.WriteFile(HttpContext.Current.Server.MapPath("~/devjoker.pdf"));
        Response.Flush();
        Response.Close();*/

        //File.Delete(Server.MapPath("~/devjoker.pdf"));


    }
    public void Page_Error(object Sender, EventArgs e)
     {
        Exception objErr = Server.GetLastError().GetBaseException();
        string err = "<b>Error Caught in Page_Error event</b><hr><br>" +
                "<br><b>Error in: </b>" + Request.Url.ToString() +
                "<br><b>Error Message: </b>" + objErr.Message.ToString() +
                "<br><b>Stack Trace:</b><br>" +
                          objErr.StackTrace.ToString();
        Response.Write(err.ToString());
        Server.ClearError();
    }
    //using System;


//namespace WebMail
//{
 //   class email
   // {
      /*  protected void Main(string[] args)
        {
            try 
            {
                MailMessage oMsg = new MailMessage();
        
                oMsg.From = "sender@somewhere.com";
            
                oMsg.To = "recipient@somewhere.com";
                oMsg.Subject = "Send Using Web Mail";
                
              
                oMsg.BodyFormat = MailFormat.Html;
                
         
                oMsg.Body = "<HTML><BODY><B>Hello World!</B></BODY></HTML>";
                
                // ADJUNTO
                String sFile = @"C:\temp\Hello.txt";  
                MailAttachment oAttch = new MailAttachment(sFile, MailEncoding.Base64);
  
                oMsg.Attachments.Add(oAttch);

                
                SmtpMail.SmtpServer = "MySMTPServer";
                SmtpMail.Send(oMsg);

                oMsg = null;
                oAttch = null;
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }*/
   // }
//} 
    protected void btLimpiarFormulario_Click(object sender, EventArgs e)
    {
        //Si todo salio bien blanqueamos el Formulario
       /* this.ddlTipoDeIdentificacion.SelectedIndex = 0;
        this.ctIdentificacion.Text = "";
        this.ctNombreProcedencia.Text = "";
        this.ctDireccion.Text = "";
        this.ctTelefono.Text = "";
        this.ctEmail.Text = "";
        this.ddlPais.SelectedIndex = 0;
        this.ddlTipoDePQR.SelectedIndex = 0;
        this.ctDetalle.Text = "";*/
        Response.Redirect("PQR_form.aspx");
    }
    protected void ctDetalle_TextChanged(object sender, EventArgs e)
    {

    }
    protected void ddlTipoDePQRTram_SelectedIndexChanged(object sender, EventArgs e)
    {
        //DSNaturalezaSQLTableAdapters.NaturalezaTableAdapter tramite = new DSNaturalezaSQLTableAdapters.NaturalezaTableAdapter();

        //ddlTipoDePQRTram.DataTextField = "NaturalezaNombre";
        //ddlTipoDePQRTram.DataValueField = "NaturalezaCodigo";
        //SqlParameter parameter = new SqlParameter("@NaturalezaCodigoPadre", ddlTipoDePQRTram.SelectedValue);
        //ddlTipoDePQRTram.DataSource = tramite.GetNaturalezaByPQRTram(parameter.Value.ToString());
        //ddlTipoDePQRTram.DataBind();

        DSNaturalezaSQLTableAdapters.Naturaleza_ReadAnexoByPQRTramTableAdapter TaAnexo = new DSNaturalezaSQLTableAdapters.Naturaleza_ReadAnexoByPQRTramTableAdapter();
        DataTable dtAnexo = TaAnexo.GetNaturaleza_ReadAnexo(ddlTipoDePQRTram.SelectedValue);
        string Anexo = "";
        //this.Table2.Visible = true;
        foreach (DataRow item in dtAnexo.Rows)
        {
            Label2.Visible = true;
            this.adjuntar_doc1.Attributes.Add("style", "display:normal");
            this.tr_tipo_tramite.Attributes.Add("style", "display:normal");
            trAdjuntar_doc.Attributes.Add("style", "display:none");
            Anexo = item[1].ToString();
            TableCell Cel1 = new TableCell();
            TableCell Cel2 = new TableCell();
            TableCell Cel = new TableCell();
            HtmlTitle LblOpcional = new HtmlTitle();
            string Opcional = item[2].ToString();
            Label lblObligatorio = new Label();


            if (Opcional == "0")
            {
                //LblOpcional.Visible = true;
                LblOpcional.Text = " (OPCIONAL)";
                Cel1.Text = Anexo;
				Cel.Attributes.Add("style", "Border:0;");
            }
            else
            {
                //lblObligatorio.Visible = true;
                lblObligatorio.Text = "*";
                lblObligatorio.Attributes.Add("style", "color:Red;font-weight:bold;");
                Cel1.Text = Anexo;
                Cel.Controls.Add(lblObligatorio);
                Cel.Attributes.Add("style", "Border:0;");
                //CheckBox ChBImagenes = new CheckBox();


            }

            HtmlInputFile file = new HtmlInputFile();
            Cel2.Controls.Add(file);
            TableRow tRow = new TableRow();
            tRow.Cells.Add(Cel);
            tRow.Cells.Add(Cel1);
            tRow.Cells.Add(Cel2);
            Table2.Rows.Add(tRow);

        }

    }
    //protected void ddlTipoDePQRTram_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DSNaturalezaSQLTableAdapters.Naturaleza_ReadAnexoByPQRTramTableAdapter TaAnexo = new DSNaturalezaSQLTableAdapters.Naturaleza_ReadAnexoByPQRTramTableAdapter();
    //    DataTable dtAnexo = TaAnexo.GetNaturaleza_ReadAnexo(ddlTipoDePQRTram.SelectedValue);
    //    string Anexo = "";
    //    this.Table2.Visible = true;
    //    foreach (DataRow item in dtAnexo.Rows)
    //    {
    //        Label2.Visible = true;
    //        //tretTipoDePQRTram.Visible = true;
    //        //this.adjuntar_doc1.Visible = true;
    //        //ddlTipoDePQRTram.Visible = true;
    //        //trAdjuntar_doc.Visible = false;
    //        Anexo = item[1].ToString();
    //        TableCell Cel1 = new TableCell();
    //        TableCell Cel2 = new TableCell();
    //        TableCell Cel = new TableCell();
    //        HtmlTitle LblOpcional = new HtmlTitle();
    //        string Opcional = item[2].ToString();
    //        Label lblObligatorio = new Label();
            

    //        if (Opcional == "0")
    //        {                
    //            //LblOpcional.Visible = true;
    //            LblOpcional.Text = " (OPCIONAL)";                               
    //            Cel1.Text = Anexo;                
    //        }
    //        else
    //        {
    //            //lblObligatorio.Visible = true;
    //            lblObligatorio.Text = "*";
    //            lblObligatorio.Attributes.Add("style", "color:Red;font-weight:bold;"); 
    //            Cel1.Text = Anexo;
    //            Cel.Controls.Add(lblObligatorio);
    //            //CheckBox ChBImagenes = new CheckBox();


    //        }
            
    //        HtmlInputFile file = new HtmlInputFile();
    //        Cel2.Controls.Add(file);
    //        TableRow tRow = new TableRow();
    //        tRow.Cells.Add(Cel);
    //        tRow.Cells.Add(Cel1);
    //        tRow.Cells.Add(Cel2);
    //        Table2.Rows.Add(tRow);

    //    }

    //}
    protected void Linkpaginaweb_Click(object sender, EventArgs e)
    {
        string _open = "window.open('https://www.fbscgr.gov.co/index.php?idcategoria=7329', '_blank' , 'top=100,left=100, width=1000,height=800,status=yes, resizable=yes,scrollbars=yes');";
        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
    }
}
