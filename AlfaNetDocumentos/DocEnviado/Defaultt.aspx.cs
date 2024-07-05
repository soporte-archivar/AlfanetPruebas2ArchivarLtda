using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Neodynamic.WebControls.ImageDraw;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections;
using iTextSharp.text.html.simpleparser;
using System.Net.Mail;
using System.Text.RegularExpressions;

public partial class _Defaultt :System.Web.UI.Page
{
    rutinas ejecutar = new rutinas();
    DataTable tabla = new DataTable();
    string GrpDocReg = "";
    // string GrpDocReg = "2";
    string ModuloLog = "Registro Plantillas";
    string ConsecutivoCodigo = "7";
    string ConsecutivoCodigoErr = "4";
    string ActividadLogCodigoErr = "Error";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.ExceptionDetails.Text = "";

        GrpDocReg = this.DropDownListGrupo.SelectedValue.ToString(); //  "2";

        System.Data.DataTable tabla = new DataTable();

        try
        {

            if (!IsPostBack)
            {
                this.DateFechaRad.Text = DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
                //this.TreeVDestino.Attributes["onClick"] = "return OnTreeDestinoClick(event);";
                this.TreeVDestino.Attributes["onClick"] = "return OnCheckBoxCheckChanged(event," + txtFullName.ClientID + ");";
                //this.TreeVDestino.Attributes["onClick"] = "return OnCheckBoxCheckChanged(event);";
                this.TreeVRemite.Attributes["onClick"] = "return OnTreeClick(event);";
                this.TreeVExpediente.Attributes["OnClick"] = "return OnTreeExpedienteClick(event)";
                this.TreeVMedio.Attributes["onClick"] = "return OnTreeMedioClick(event);";
                this.TreeVNaturaleza.Attributes["onClick"] = "return OnTreeNaturalezaClick(event);";
                this.TreeVSerie.Attributes["onClick"] = "return OnTreeSerieClick(event);";
                //this.txtEmail.Attributes["OnTextChanged"] = "return updateImageCard();";
                
                // llena el cuadro de lista grupo
                string codigodelgruporadicados = "";
                string codigodelgruporegistros = "";

                tabla = ejecutar.rtn_traer_tbtablas_por_Id("GRUPOREG");
                codigodelgruporegistros = tabla.Rows[0][0].ToString().Trim();
                tabla = ejecutar.rtn_traer_grupos_por_codigopadre(codigodelgruporegistros);
                if (tabla.Rows.Count == 0)
                {
                    tabla = ejecutar.rtn_traer_grupos_por_Id(codigodelgruporegistros);
                }
                this.DropDownListGrupo.DataTextField = "gruponombre";
                this.DropDownListGrupo.DataValueField = "grupocodigo";
                this.DropDownListGrupo.DataSource = tabla;
                this.DropDownListGrupo.DataBind();
                this.DropDownListGrupo.SelectedIndex = 0;



                // lista llena con gel grupo de radicacoines
                tabla = ejecutar.rtn_traer_tbtablas_por_Id("GRUPORAD");
                codigodelgruporadicados = tabla.Rows[0][0].ToString().Trim();
                tabla = ejecutar.rtn_traer_grupos_por_codigopadre(codigodelgruporadicados);
                if (tabla.Rows.Count == 0)
                {
                    tabla = ejecutar.rtn_traer_grupos_por_Id(codigodelgruporadicados);
                }
                this.DropDownListGrupoFuente.DataTextField = "gruponombre";
                this.DropDownListGrupoFuente.DataValueField = "grupocodigo";
                this.DropDownListGrupoFuente.DataSource = tabla;
                this.DropDownListGrupoFuente.DataBind();
                this.DropDownListGrupoFuente.SelectedIndex = 0;


                Application["grupo"] = this.DropDownListGrupoFuente.SelectedValue.ToString();
                Application["gruporegistro"] = this.DropDownListGrupo.SelectedValue.ToString();
                DDLPlantilla.Items.Clear();
                DDLPlantilla.Items.Add(new System.Web.UI.WebControls.ListItem("Seleccione", "-1"));
                
                DDLPlantilla.DataValueField = "Codigo";
                DDLPlantilla.DataTextField = "Descripcion";
                cargarPlantillas();
                if (Session["textoPlantilla"] != null)
                {
                    Editor.Text = Session["textoPlantilla"].ToString();
                    Session.Remove("textoPlantilla");
                    //DDLPlantilla.SelectedValue = Session["Plantilla_CodPlantilla"].ToString();
                }
                DDLPlantilla.SelectedValue = "-1";
            }
            else
            {
               
            }
        }
       
        catch (Exception Error)
        {
         this.ExceptionDetails.Text = "Problema" + Error;
        }
    }
    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioButtonList1.SelectedValue == "1")
        {
            this.ACEDestino.ServiceMethod = "GetDependenciaByText";
            this.txtFullName.Text = "";
            this.ListBoxEnterar.Items.Clear();
            this.Paneldep.Visible = true;
            this.ImgFindDestino.Visible = false;
           
        }
        if (RadioButtonList1.SelectedValue == "0")
        {
            this.ACEDestino.ServiceMethod = "GetProcedenciaByTextNombre";
            this.txtFullName.Text = "";
            this.ListBoxEnterar.Items.Clear();
            this.Paneldep.Visible = true;
            this.ImgFindDestino.Visible = false;
        }
        RadioButtonList1.Focus();
        this.SetFocus(RadioButtonList1);        
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        
        this.ExceptionDetails.Text = "";

        string WFAccion = "1";
        string RegistroCodigo = "1";
        try
        {
            String RadCodString;
            RadCodString = TxtRadFuente.Text;
            if (RadCodString == "")
                RadCodString = null;

            if (RadCodString != null)
            {
                if (RadCodString.Contains(" | "))
                {
                    RadCodString = RadCodString.Remove(RadCodString.IndexOf(" | "));
                }
            }

            Document document = new Document();
            PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~/devjoker.pdf"), FileMode.OpenOrCreate));
            document.Open();

            GuardarImg Imgs = new GuardarImg();
            String FileName = "";

            RegistroBLL ObjReg = new RegistroBLL();

            String ANO = DateTime.Today.Year.ToString();
            String MES = DateTime.Today.Month.ToString();

            if (RadioButtonList1.SelectedValue == "1")
            {
                if (this.TreeVDestino.CheckedNodes.Count > 0)
                {

                    foreach (TreeNode myNode in this.TreeVDestino.CheckedNodes)
                    {
                        RegistroCodigo = ObjReg.AddRegistro(GrpDocReg, Convert.ToDateTime(DateFechaRad.Text.ToString()), null, myNode.Text.ToString(), txtPhone.Text.ToString(), TxtNaturaleza.Text.ToString(), Convert.ToInt32(RadCodString), "Registro Automatico con Plantilla", null, null, null, null, Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString(), TxtExpediente.Text.ToString(), TxtMedioRecibo.Text.ToString(), TextBox1.Text.ToString(), null, RadioButtonList1.Text.ToString(), WFAccion, Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToInt32(RadioButtonList1.Text.ToString()), null, "0");
                        
                        //foreach (System.Web.UI.WebControls.ListItem Item in ListBoxFuente.Items)
                        //{
                        //    if (Item.Text != null)
                        //    {
                        //        if (Item.Text.Contains(" | "))
                        //        {
                        //            Item.Text = Item.Text.Remove(Item.Text.IndexOf(" | "));
                        //        }
                        //    }
                        //    DSRadicadoFuenteSQLTableAdapters.RadicadoFuente_ReadRadicadoFuenteTableAdapter TARadFuente = new DSRadicadoFuenteSQLTableAdapters.RadicadoFuente_ReadRadicadoFuenteTableAdapter();
                        //    TARadFuente.Insert(Convert.ToInt32(RegistroCodigo), Convert.ToInt32(Item.Text));
                        //}
                        this.insertarradicadosfuente(RegistroCodigo);

                        Imgs.GuardarImgs("Registro Automatico con Plantilla", myNode.Text, this.RadioButtonList1.SelectedValue.ToString(), RegistroCodigo + myNode.Value + ".png", RegistroCodigo, this.DropDownListGrupo.SelectedValue);
                        FileName = RegistroCodigo + myNode.Value + ".png";
                        //String Separador = @"\";
                        String PathFolder = HttpContext.Current.Server.MapPath("~/AlfaNetRepositorioImagenes/Registros/" + ANO + "/" + MES + "/");
                        //String PathFolder = @"F:\AlfaNet\AlfaNetRepositorioImagenes" + Separador + "Registros" + Separador + "2009" + Separador + "5" + Separador;
                        String PathString = PathFolder + FileName;
                        iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(@PathString);
                        png.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                        png.ScalePercent(80);
                        document.Add(png);

                        this.LblMessageBox.Text += string.Format("Se Genero el documento {0}", RegistroCodigo);
                        this.LblMessageBox.Text += " y su imagen <br />";

                    }
                }
                else if (this.txtFullName.Text != "Multiple")
                {
                    RegistroCodigo = ObjReg.AddRegistro(GrpDocReg, Convert.ToDateTime(DateFechaRad.Text.ToString()), null, this.txtFullName.Text.ToString(), txtPhone.Text.ToString(), TxtNaturaleza.Text.ToString(), Convert.ToInt32(RadCodString), "Registro Automatico con Plantilla", null, null, null, null, Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString(), TxtExpediente.Text.ToString(), TxtMedioRecibo.Text.ToString(), TextBox1.Text.ToString(), null, RadioButtonList1.Text.ToString(), WFAccion, Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToInt32(RadioButtonList1.Text.ToString()), null, "0");

                    this.insertarradicadosfuente(RegistroCodigo);
                    FileName = RegistroCodigo+ txtFullName.Text.Remove(txtFullName.Text.IndexOf(" | ")) + ".png";
                    Imgs.GuardarImgs("Registro Automatico con Plantilla", txtFullName.Text, this.RadioButtonList1.SelectedValue.ToString(), FileName, RegistroCodigo, this.DropDownListGrupo.SelectedValue);
                     //FileName = FileName+ ".png";
                     //String Separador = @"\";
                    String PathFolder = HttpContext.Current.Server.MapPath("~/AlfaNetRepositorioImagenes/Registros/" + ANO + "/" + MES + "/");
                    String PathString = PathFolder + FileName;
                    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(@PathString);
                    png.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                    png.ScalePercent(80);
                    document.Add(png);

                    this.LblMessageBox.Text += string.Format("Se Genero el documento {0}", RegistroCodigo);
                    this.LblMessageBox.Text += " y su imagen <br />";

                } 
                
              
            }           
            else if(RadioButtonList1.SelectedValue == "0")
            {
                foreach (System.Web.UI.WebControls.ListItem Procedencia in this.ListBoxEnterar.Items)
                {
                    RegistroCodigo = ObjReg.AddRegistro(GrpDocReg, Convert.ToDateTime(DateFechaRad.Text.ToString()), Procedencia.Text, null, txtPhone.Text.ToString(), TxtNaturaleza.Text.ToString(), Convert.ToInt32(RadCodString), "Registro Automatico con Plantilla", null, null, null, null, Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString(), TxtExpediente.Text.ToString(), TxtMedioRecibo.Text.ToString(), TextBox1.Text.ToString(), null, RadioButtonList1.Text.ToString(), WFAccion, Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToInt32(RadioButtonList1.Text.ToString()), null, "0");

                    // insertar radicados fuente
                    this.insertarradicadosfuente(RegistroCodigo);

                    //foreach (System.Web.UI.WebControls.ListItem Item in ListBoxFuente.Items)
                    //{
                    //    if (Item.Text != null)
                    //    {
                    //        if (Item.Text.Contains(" | "))
                    //        {
                    //            Item.Text = Item.Text.Remove(Item.Text.IndexOf(" | "));
                    //        }
                    //    }
                    //    DSRadicadoFuenteSQLTableAdapters.RadicadoFuente_ReadRadicadoFuenteTableAdapter TARadFuente = new DSRadicadoFuenteSQLTableAdapters.RadicadoFuente_ReadRadicadoFuenteTableAdapter();
                    //    TARadFuente.Insert(Convert.ToInt32(RegistroCodigo), Convert.ToInt32(Item.Text));
                    //}
                    String Proce = Procedencia.Text.Remove(Procedencia.Text.IndexOf(" | "));
                    Imgs.GuardarImgs("Registro Automatico con Plantilla", Proce, this.RadioButtonList1.SelectedValue.ToString(), RegistroCodigo + Proce + ".png", RegistroCodigo, this.DropDownListGrupo.SelectedValue);
                    FileName = RegistroCodigo + Proce + ".png";
                    //String Separador = @"\";
                    String PathFolder = HttpContext.Current.Server.MapPath("~/AlfaNetRepositorioImagenes/Registros/" + ANO + "/" + MES + "/");
                    //String PathFolder = @"F:\AlfaNet\AlfaNetRepositorioImagenes" + Separador + "Registros" + Separador + "2009" + Separador + "5" + Separador;
                    String PathString = PathFolder + FileName;
                    iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(@PathString);
                    png.Alignment = iTextSharp.text.Image.MIDDLE_ALIGN;
                    png.ScalePercent(80);
                    document.Add(png);

                    this.LblMessageBox.Text += string.Format("Se Genero el documento {0}", RegistroCodigo);
                    this.LblMessageBox.Text += " y su imagen <br />";
                }
            }
            this.MPEMessage.Show();

            document.Close();
            

            //this.LblMessageBox.Text = "Registro Nro" + " " + RegistroCodigo;
            

            //MailBLL Correo = new MailBLL();
            //MembershipUser usuario;
            //DSUsuarioTableAdapters.UsuariosxdependenciaTableAdapter ObjTaUsuarioxDependencia = new DSUsuarioTableAdapters.UsuariosxdependenciaTableAdapter();
            //DSUsuario.UsuariosxdependenciaDataTable DTUsuariosxDependencia = new DSUsuario.UsuariosxdependenciaDataTable();
            //DTUsuariosxDependencia = ObjTaUsuarioxDependencia.GetUsuariosxDependenciaByDependencia(this.txtPhone.Text.Remove(txtPhone.Text.IndexOf(" | ")).ToString().Trim());
            //if (DTUsuariosxDependencia.Count > 0)
            //{
            //    DataRow[] rows = DTUsuariosxDependencia.Select();
            //    System.Guid a = new Guid(rows[0].ItemArray[0].ToString().Trim());
            //    usuario = Membership.GetUser(a);
            //    string Body = "Tiene un nuevo Registro Nro " + RegistroCodigo + "<BR>" + " Fecha de Tramite: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + "<BR>" + "Dependencia: " + this.txtPhone.Text.ToString() + "<BR>" + "Naturaleza: " + this.TxtNaturaleza.Text.ToString().Trim() + "<BR>";
            //    Correo.EnvioCorreo("alfanetpruebas@gmail.co", usuario.Email, "Registro Nro" + " " + RegistroCodigo, Body, true, "1");
            //}
        }
        catch (Exception Error)
        {
            this.ExceptionDetails.Text = "Problema" + Error;
        }
    }
    protected void RBEnterarA_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RBEnterarA.SelectedValue == "T")
        {
            this.ListBoxEnterar.Items.Clear();
            this.txtFullName.Text = "";
            this.txtFullName.ReadOnly = true;

            this.ImgBtnAdd.Enabled = false;
            this.ImgBtnDelete.Enabled = false;
            this.ListBoxEnterar.Items.Add("Todas | Todas");
        }
        else
        {
            this.ListBoxEnterar.Items.Clear();
            this.txtFullName.Text = "";
            this.txtFullName.ReadOnly = false;

            this.ImgBtnAdd.Enabled = true;
            this.ImgBtnDelete.Enabled = true;
        }
    }
    private void guardar()
    {
        
    }
    protected void TreeVDestino_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        try
        {
            ArbolesBLL ObjArbolDep = new ArbolesBLL();
            DSDependenciaSQL.DependenciaByTextDataTable DTDependencia = new DSDependenciaSQL.DependenciaByTextDataTable();
            DTDependencia = ObjArbolDep.GetDependenciaTree(e.Node.Value);
            PopulateNodes(DTDependencia, e.Node.ChildNodes, "DependenciaCodigo", "DependenciaNombre");
        }
        catch (Exception Error)
        {
            //throw Exception(Error.Message);
            this.ExceptionDetails.Text = "Problema" + Error;
        }
    }
    private void PopulateNodes(DataTable dt, TreeNodeCollection nodes, String Codigo, String Nombre)
    {
        foreach (DataRow dr in dt.Rows)
        {
            TreeNode tn = new TreeNode();
            //dr["title"].ToString();
            tn.Text = dr[Codigo].ToString() + " | " + dr[Nombre].ToString();
            tn.Value = dr[Codigo].ToString();
            tn.NavigateUrl = "javascript:void(  0 );";
            nodes.Add(tn);

            //If node has child nodes, then enable on-demand populating
            tn.PopulateOnDemand = (Convert.ToInt32(dr["childnodecount"]) > 0);
        }
    }
   
    protected void ImgBtnAddFuente_Click(object sender, ImageClickEventArgs e)
    {
        this.ListBoxFuente.Items.Add(this.DropDownListGrupoFuente.SelectedValue + "--" + this.TxtRadFuente.Text.ToString());
        this.TxtRadFuente.Text = "";
        this.TxtRadFuente.Focus();
    }
    protected void ImgBtnDeleteFuente_Click(object sender, ImageClickEventArgs e)
    {
        this.ListBoxFuente.Items.Remove(this.ListBoxFuente.SelectedItem);
        this.ListBoxFuente.Focus();
    }
    protected void RBFuente_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RBFuente.SelectedValue == "T")
        {
            this.ListBoxFuente.Items.Clear();
            this.TxtRadFuente.Text = "";
            this.TxtRadFuente.ReadOnly = true;
            this.ImgBtnAddFuente.Enabled = false;
            this.ImgBtnDeleteFuente.Enabled = false;
            this.ListBoxFuente.Items.Add("Todas | Todas");
        }
        else
        {
            this.ListBoxFuente.Items.Clear();
            this.TxtRadFuente.Text = "";
            this.TxtRadFuente.ReadOnly = false;
            this.ImgBtnAddFuente.Enabled = true;
            this.ImgBtnDeleteFuente.Enabled = true;
        }
    }
    protected void TreeVMedio_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        ArbolesBLL ObjArbolMed = new ArbolesBLL();
        DSMedioSQL.MedioByTextDataTable DTMedio = new DSMedioSQL.MedioByTextDataTable();
        DTMedio = ObjArbolMed.GetMedioTree(Int32.Parse(e.Node.Value));
        PopulateNodes(DTMedio, e.Node.ChildNodes, "MedioCodigo", "MedioNombre");
    }
    protected void TreeVNaturaleza_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        ArbolesBLL ObjArbolNat = new ArbolesBLL();
        DSNaturalezaSQL.NaturalezaByTextDataTable DTNaturaleza = new DSNaturalezaSQL.NaturalezaByTextDataTable();
        DTNaturaleza = ObjArbolNat.GetNaturalezaTree(e.Node.Value);
        PopulateNodes(DTNaturaleza, e.Node.ChildNodes, "NaturalezaCodigo", "NaturalezaNombre");
    }
    public void TreeVExpediente_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        ArbolesBLL ObjArbolExp = new ArbolesBLL();
        DSExpedienteSQL.ExpedienteByTextDataTable DTExpediente = new DSExpedienteSQL.ExpedienteByTextDataTable();
        DTExpediente = ObjArbolExp.GetExpedienteTree(e.Node.Value);
        PopulateNodes(DTExpediente, e.Node.ChildNodes, "ExpedienteCodigo", "ExpedienteNombre");
    }
    protected void TreeVSerie_TreeNodePopulate(object sender, TreeNodeEventArgs e)
    {
        ArbolesBLL ObjArbolSer = new ArbolesBLL();
        DSSerieSQL.SerieByTextDataTable DTSerie = new DSSerieSQL.SerieByTextDataTable();
        DTSerie = ObjArbolSer.GetSerieTree(e.Node.Value);
        PopulateNodes(DTSerie, e.Node.ChildNodes, "SerieCodigo", "SerieNombre");
    }

    protected void ImgBtnAdd_Click(object sender, ImageClickEventArgs e)
    {
        this.ListBoxEnterar.Items.Add(this.txtFullName.Text.ToString());
        this.txtFullName.Text = "";
        if (this.ListBoxEnterar.Items.Count > 0)
            this.RFVDestino.Enabled = false;
        else
            this.RFVDestino.Enabled = true;

    }
    protected void ImgBtnDelete_Click(object sender, ImageClickEventArgs e)
    {
        this.ListBoxEnterar.Items.Remove(this.ListBoxEnterar.SelectedItem);

        if (this.ListBoxEnterar.Items.Count > 0)
            this.RFVDestino.Enabled = false;
        else
            this.RFVDestino.Enabled = true;
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.ContentType = "application/pdf";
        Response.AddHeader("Content-disposition", "attachment; filename=" + "devjoker.pdf");
        Response.WriteFile(HttpContext.Current.Server.MapPath("~/devjoker.pdf"));
        Response.Flush();
        Response.Close();
    }
    protected void DropDownListGrupo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Application["gruporegistro"] = this.DropDownListGrupo.SelectedValue.ToString();
    }

    protected void DropDownListGrupoFuente_SelectedIndexChanged(object sender, EventArgs e)
    {
        Application["grupo"] = this.DropDownListGrupoFuente.SelectedValue.ToString();
    }
    protected void insertarradicadosfuente(string RegistroCodigo)
    {
        string grupofuente = "";
        foreach (System.Web.UI.WebControls.ListItem Item in ListBoxFuente.Items)
        {
            string xcodigo = "";
            if (Item.Text != null)
            {
                if (Item.Text.Contains(" | "))
                {

                    int correr = 0;
                    correr = Item.Text.IndexOf("|") - (Item.Text.IndexOf("--") + 2);
                    grupofuente = Item.Text.Substring(0, Item.Text.IndexOf("--"));
                    xcodigo = Item.Text.Substring(Item.Text.IndexOf("--") + 2, correr);

                }
            }
            DSRadicadoFuenteSQLTableAdapters.RadicadoFuente_ReadRadicadoFuenteTableAdapter TARadFuente = new DSRadicadoFuenteSQLTableAdapters.RadicadoFuente_ReadRadicadoFuenteTableAdapter();
            TARadFuente.Insert(GrpDocReg, Convert.ToInt32(RegistroCodigo), grupofuente, Convert.ToInt32(xcodigo));

        }


    }
    protected void DDLPlantilla_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (DDLPlantilla.SelectedItem.ToString() != "Seleccione")
            {
                string codPlantilla = DDLPlantilla.SelectedValue.ToString();
                Business intelligence = new Business();
                DataSet ds = new DataSet();
                ds = intelligence.TraerHTMLPlantilla(codPlantilla);
                string codHtml = ds.Tables[0].Rows[0]["HTML"].ToString();
                string radicadoCodigo = Request.QueryString["RadicadoCodigo"];
                //int OpcPlantilla = 1;
                if (radicadoCodigo == null)
                {
                    radicadoCodigo = "-1";
                    //radicadoCodigo = "0";
                }
                remplazarValores(codHtml, radicadoCodigo);
                LMessagePlantilla.Text = "";
            }
            else
            {
                Editor.Text = "";
                LMessagePlantilla.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
       
    }
    private void remplazarValores(string codHtml, string radicadoCodigo)
    {
        try
        {
            if (radicadoCodigo != "-1")
            {
                Business intelligence = new Business();
                DataSet ds = new DataSet();
                Session.Add("TablaRelacion", "Registro");
                //Editor.Text = codHtml;
                //int valor = Editor.Text.IndexOf("##");
                while (codHtml.IndexOf("##") >= 0)
                {
                    Int32 vFrom = codHtml.IndexOf("##");
                    Int32 vTo = codHtml.IndexOf("##", vFrom + 1);
                    string TxtReplace = codHtml.Substring(vFrom + 2, vTo - vFrom - 2);

                    Int32 mSeparador = TxtReplace.IndexOf("/");
                    string mTablaSecundaria = TxtReplace.Substring(0, mSeparador);
                    string mCampo = TxtReplace.Substring(mSeparador + 1);
                    string Tabla = "Registro";


                    ds = intelligence.CombinaCampos(Tabla, mCampo, mTablaSecundaria, radicadoCodigo);
                    if (ds.Tables[0].Rows.Count >= 0)
                    {
                        try
                        {
                            codHtml = codHtml.Replace("##" + TxtReplace + "##", ds.Tables[0].Rows[0][1].ToString());

                        }
                        catch (Exception)
                        {

                            codHtml = codHtml.Replace("##" + TxtReplace + "##", " ");
                        }

                    }
                    //Editor.Text = "hola";
                }
            }
            Editor.Text = codHtml;
            Session["textoPlantilla"] = Editor.Text;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void IBARegistrar_Click(object sender, ImageClickEventArgs e)
    {
        string ActLogCod = "INSERTAR";
        string radicadoCodigo = Request.QueryString["RadicadoCodigo"];
        if (DDLPlantilla.SelectedValue == "-1")
        {
            upmessagealfa.Update();
            this.LblMessageBox.Text = "Debe Seleccionar una Plantilla";

           // this.MPEMessage.Enabled = true;
           // this.MPEMessage.Show();
            //
            //LMessagePlantilla.Text = "Debe Seleccionar una Plantilla";
            //message();
        }
        else
        {
            string plantilla = Editor.Text;
            Session["textoPlantilla"] = plantilla;
            //string codPlantilla = DDLPlantilla.SelectedValue.ToString();
            string radicado = "";
            if (radicadoCodigo == null || radicadoCodigo == "")
            {
                Session["OpcPlantilla"] = "1";
            }
            else
            {
                Session["OpcPlantilla"] = "0";
                radicado = Request.QueryString["RadicadoCodigo"];
            }
            string depUsuario = Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();
            Session["depUsuario"] = depUsuario;
            Session["Plantilla_CodPlantilla"] = DDLPlantilla.SelectedValue.ToString();
            //Response.Redirect("~/Plantillas/Plantillas/Page_RegistrarPlantilla.aspx?RadicadoCodigo=" + radicado + "&depUsuario=" + depUsuario);
            Response.Redirect("~/plantillas/plantillas/Page_FinalizarPlantilla.aspx?radicado=" + radicadoCodigo + "&plantilla=" + Request.QueryString["codPlantilla"] + "&depusuario=" + Request.QueryString["depUsuario"]);
            //LOG INSERTAR 
            string Descr = DDLPlantilla.SelectedItem.Text;
            DateTime Fecha = DateTime.Now;
            //OBTENER CONSECUTIVO DE LOGS
            DSGrupoSQLTableAdapters.ConsecutivoLogsTableAdapter Consecutivos = new DSGrupoSQLTableAdapters.ConsecutivoLogsTableAdapter();
            DSGrupoSQL.ConsecutivoLogsDataTable Conse = new DSGrupoSQL.ConsecutivoLogsDataTable();
            Conse = Consecutivos.GetConseActual(ConsecutivoCodigo);
            DataRow[] fila = Conse.Select();
            string x = fila[0].ItemArray[0].ToString();
            string LOG = Convert.ToString(x);
            int Tipo = 2;
            bool ChkEstado = true;
            string UserName = Profile.GetProfile(Profile.UserName).UserName.ToString();
            DSUsuarioTableAdapters.UserIdByUserNameTableAdapter objUsr = new DSUsuarioTableAdapters.UserIdByUserNameTableAdapter();
            string UsrId = objUsr.Aspnet_UserIDByUserName(UserName).ToString();
            DateTime FechaFin = DateTime.Now;
            Int64 LogId = Convert.ToInt64(LOG);
            string IP = Session["IP"].ToString();
            string NombreEquipo = Session["Nombrepc"].ToString();
            System.Web.HttpBrowserCapabilities nav = Request.Browser;
            string Navegador = nav.Browser.ToString() + " Version: " + nav.Version.ToString();
            //Insert de Log registrar plantilla workflow
            DSLogAlfaNetTableAdapters.LogAlfaNetTableAdapter InsertaLog = new DSLogAlfaNetTableAdapters.LogAlfaNetTableAdapter();
            InsertaLog.InsertLogPlantilla(LogId, Fecha, UserName, ActLogCod, ModuloLog, Session["Plantilla_CodPlantilla"].ToString(), Descr, Tipo, ChkEstado, plantilla, IP, NombreEquipo, Navegador);
            //Actualiza consecutivo
            DSGrupoSQLTableAdapters.ConsecutivoLogsTableAdapter ConseLogs = new DSGrupoSQLTableAdapters.ConsecutivoLogsTableAdapter();
            ConseLogs.GetConsecutivos(ConsecutivoCodigo);
        }
    }
    protected void btnCerrar_Click(object sender, ImageClickEventArgs e)
    {
       // this.MPEMessage.Enabled = false;
    }


    private void cargarPlantillas()
    {
        try
        {
            Business intelligence = new Business();
            DataSet ds = new DataSet();
            DataSet DSPlantilla = new DataSet();
            string dependenciaCodigo = Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();
            ds = intelligence.cargarPlantillas(dependenciaCodigo);

            foreach (DataRow item in ds.Tables[0].Rows)
            {
                if (item["estado"].ToString() == "True")
                {
                    DDLPlantilla.Items.Add(new System.Web.UI.WebControls.ListItem(item.ItemArray[1].ToString(), item.ItemArray[0].ToString()));
                }
            }
            DDLPlantilla.DataBind();
            if (Session["Plantilla_CodPlantilla"] != null)
            {
                DDLPlantilla.SelectedValue = Session["Plantilla_CodPlantilla"].ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private DatosRegistro llenarDatosRegistro(System.Web.UI.WebControls.ListItem TxtDestino)
    {
        try
        {
            Business intelligence = new Business();
            string procedenciaCodDestino;
            string dependenciaCodDestino;
            int radicadoCodigo = 0;
            string grupoCodigo = "2";
            DateTime wFMovimientoFecha = System.DateTime.Now;
            if (RadioButtonList1.SelectedValue == "1")
            {
                procedenciaCodDestino = TxtDestino.Text.Remove(TxtDestino.Text.IndexOf(" | "));
                dependenciaCodDestino = null;
            }
            else
            {
                procedenciaCodDestino = null;
                dependenciaCodDestino = TxtDestino.Text.Remove(TxtDestino.Text.IndexOf(" | "));
            }
            //string procedenciaCodDestino = TxtDestino.Text.Remove(TxtDestino.Text.IndexOf(" | "));
            //string dependenciaCodDestino = null;
            string dependenciaCodigo = Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();// Request.QueryString["depUsuario"];// Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();
            string naturalezaCodigo = "";
            string registroDetalle = "Registro Automatico con Plantilla";
            string registroGuia = null;
            string registroEmpGuia = null;
            string anexoExtRegistro = null;
            string logDigitador = Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();// Request.QueryString["depUsuario"];// Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();
            string expedienteCodigo = "";
            string medioCodigo = "";
            string serieCodigo = "";
            string regPesoEnvio = null;
            string regValorEnvio = null;
            string registroTipo;
            if (RadioButtonList1.SelectedValue == "1")
            {
                registroTipo = "0";
            }
            else
            {
                registroTipo = "1";
            }
            //string registroTipo = "0";
            string wFAccionCodigo = "13";
            DateTime wFMovimientoFechaEst = System.DateTime.Now;
            DateTime wFMovimientoFechaFin = System.DateTime.Now;
            int wFMovimientoTipo = 0;
            string wFMovimientoNotas = null;
            string wFMovimientoMultitarea = "0";
            string userId = "";

            //MembershipUser user = Membership.GetUser();

            //if (TxtRadicadoFuente.Text.Contains(" | "))
            //{
            //    radicadoCodigo = Convert.ToInt32(TxtRadicadoFuente.Text.Remove(TxtRadicadoFuente.Text.IndexOf(" | ")));
            //}
            //else if (TxtRadicadoFuente.Text.Trim() == "")
            //{
            //    radicadoCodigo = 0;
            //}
            //else
            //{
            //    radicadoCodigo = Convert.ToInt32(TxtRadicadoFuente.Text.Trim());
            //}
            if (dependenciaCodigo.ToString() == "" || dependenciaCodigo.ToString() == null)
            {
                string username = "TLINEA";
                string dep = intelligence.ReadUsersByUserName(username);
                //Object CodigoRuta = user.ProviderUserKey;
                //userId = Convert.ToString(CodigoRuta);
            }
            else
            {
                userId = intelligence.obtenerUserId(dependenciaCodigo.ToString());
                //userId = logDigitador;
            }

            if (TxtNaturaleza.Text.Contains(" | "))
            {
                naturalezaCodigo = TxtNaturaleza.Text.Remove(TxtNaturaleza.Text.IndexOf(" | "));
            }

            if (TxtExpediente.Text.Contains(" | "))
            {
                expedienteCodigo = TxtExpediente.Text.Remove(TxtExpediente.Text.IndexOf(" | "));
            }

            if (TxtMedioRecibo.Text.Contains(" | "))
            {
                medioCodigo = TxtMedioRecibo.Text.Remove(TxtMedioRecibo.Text.IndexOf(" | "));
            }

            if (TextBox1.Text.Contains(" | "))
            {
                serieCodigo = TextBox1.Text.Remove(TextBox1.Text.IndexOf(" | "));
            }

            if (dependenciaCodigo == "")
            {
                dependenciaCodigo = "100"; //OJO ESTO DEBE CAMBIAR EN PRODUCCION.
            }

            if (RadioButtonList1.SelectedValue == "0")
            {
                if (dependenciaCodDestino == "" || dependenciaCodDestino == null)
                {
                    dependenciaCodDestino = Request.QueryString["depUsuario"];// Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();

                }
                if (Session["Plantilla_Opcion"] != null && Session["Plantilla_Opcion"].ToString() == "1")//no esta aun
                {
                    procedenciaCodDestino = TxtDestino.Text.Remove(TxtDestino.Text.IndexOf(" | "));
                }
                else
                {
                    procedenciaCodDestino = null;
                    wFMovimientoTipo = 1;
                }
            }
            else
            {
                if (TxtDestino.Text.Contains(" | "))
                {
                    procedenciaCodDestino = TxtDestino.Text.Remove(TxtDestino.Text.IndexOf(" | "));
                }
            }

            DatosRegistro datosRegistro = new DatosRegistro();
            datosRegistro.grupoCodigo = grupoCodigo;
            datosRegistro.wFMovimientoFecha = wFMovimientoFecha;
            datosRegistro.procedenciaCodDestino = procedenciaCodDestino;
            datosRegistro.dependenciaCodDestino = dependenciaCodDestino;
            datosRegistro.dependenciaCodigo = dependenciaCodigo;
            datosRegistro.naturalezaCodigo = naturalezaCodigo;
            datosRegistro.radicadoCodigo = radicadoCodigo;
            datosRegistro.registroDetalle = registroDetalle;
            datosRegistro.registroGuia = registroGuia;
            datosRegistro.registroEmpGuia = registroEmpGuia;
            datosRegistro.anexoExtRegistro = anexoExtRegistro;
            datosRegistro.logDigitador = logDigitador;
            datosRegistro.expedienteCodigo = expedienteCodigo;
            datosRegistro.medioCodigo = medioCodigo;
            datosRegistro.serieCodigo = serieCodigo;
            datosRegistro.regPesoEnvio = regPesoEnvio;
            datosRegistro.regValorEnvio = regValorEnvio;
            datosRegistro.registroTipo = registroTipo;
            datosRegistro.wFAccionCodigo = wFAccionCodigo;
            datosRegistro.wFMovimientoFechaEst = wFMovimientoFechaEst;
            datosRegistro.wFMovimientoFechaFin = wFMovimientoFechaFin;
            datosRegistro.wFMovimientoTipo = wFMovimientoTipo;
            datosRegistro.wFMovimientoNotas = wFMovimientoNotas;
            datosRegistro.wFMovimientoMultitarea = wFMovimientoMultitarea;
            datosRegistro.userId = userId;
            return datosRegistro;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void IBAceptar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            string imagen = Session["textoPlantilla"].ToString();//contenido;
            HTMLWorker.ParseToList(new StringReader(imagen), new StyleSheet());
            foreach (IElement E in HTMLWorker.ParseToList(new StringReader(imagen), new StyleSheet()))
            {
                //if (TxtDestino.Text.Equals("") || TxtNaturaleza.Text.Equals("") || TxtExpediente.Text.Equals("") || TxtMedio.Text.Equals("") || TxtSerie.Text.Equals(""))
                //{
                //    //LMessagePlantilla.Text = "Por favor llenar todos los datos requeridos.";
                //    message();
                //}
                //else
                //{
                    //int registro = crearRegistro();
                    //DatosRegistro datos = new DatosRegistro();
                    //datos = llenarDatosRegistro();
                    //Session["datos"] = datos;

                    //LMessagePlantilla.Text = "Se Registro el documento con el n�mero: " + registro;
                    //crearRadicadoFuente(registro);
                    //insertarPlantilla(Request.QueryString["codPlantilla"], Request.QueryString["depUsuario"], Session["texto"].ToString());
                    //string procedencia = TxtDestino.Text.Remove(TxtDestino.Text.IndexOf(" | "));
                    //if (RBLDestino.SelectedValue == "1")
                    //{
                    //    procedencia += ".1";
                    //}
                    //else
                    //{
                    //    procedencia += ".2";
                    //}
                    //if (TxtRadicadoFuente.Text.Trim() != "")
                    //{
                    //    string rad = string.Empty;
                    //    if (TxtRadicadoFuente.Text.Contains(" | "))
                    //    {
                    //        rad = TxtRadicadoFuente.Text.Remove(TxtRadicadoFuente.Text.IndexOf(" | ")).Trim();
                    //    }
                    //    else
                    //    {
                    //        rad = TxtRadicadoFuente.Text.Trim();
                    //    }
                    //    string html = remplazarValores(Session["textoPlantilla"].ToString(), rad);
                    //    Session["textoPlantilla"] = html;
                    //}
                    //else
                    //{
                    //    string html = remplazarValores2(Session["textoPlantilla"].ToString());
                    //    Session["textoPlantilla"] = html;
                    //}
                    //message();

                    //Response.Redirect("Page_FinalizarPlantilla.aspx?radicado=" + TxtRadicadoFuente.Text.Trim() + "&plantilla=" + Request.QueryString["codPlantilla"] + "&depusuario=" + Request.QueryString["depUsuario"] + "&procedencia=" + procedencia);

                }
           // }
        }
        catch (Exception ex)
        {
            //if (ex.Message.Contains("is not a recognized imageformat"))
            //{

            //    upmessagealfa.Update();
            //    this.LblMessageBox.Text = "La plantilla contiene una imagen no valida en el editor de texto, favor intentar de nuevo";
            //    this.MPEMessage.Enabled = true;
            //    this.MPEMessage.Show();
            //}
            //else if (ex.Message.Contains("The URI prefix is not recognized"))
            //{

            //    upmessagealfa.Update();
            //    this.LblMessageBox.Text = "La plantilla contiene una imagen no valida en el editor de texto, favor intentar de nuevo.";
            //    this.MPEMessage.Enabled = true;
            //    this.MPEMessage.Show();
            //}
            //else if (ex.Message.Contains("Object reference not set to an instance of an object"))
            //{

            //    upmessagealfa.Update();
            //    this.LblMessageBox.Text = "La plantilla contiene una imagen no valida en el editor de texto, favor intentar de nuevo.";
            //    this.MPEMessage.Enabled = true;
            //    this.MPEMessage.Show();
            //}
            //else if (ex.Message.Contains("The specified path, file name, or both are too long"))
            //{

            //    upmessagealfa.Update();
            //    this.LblMessageBox.Text = "La plantilla contiene una imagen no valida en el editor de texto, favor intentar de nuevo.";
            //    this.MPEMessage.Enabled = true;
            //    this.MPEMessage.Show();
            //}
            //else if (ex.Message.Contains("Unable to cast object of type "))
            //{

            //    upmessagealfa.Update();
            //    this.LblMessageBox.Text = "La plantilla contiene una imagen no valida en el editor de texto, favor intentar de nuevo.";
            //    this.MPEMessage.Enabled = true;
            //    this.MPEMessage.Show();
            //}
            //else if (ex.Message.Contains("The number of columns in PdfPTable constructor must be greater than zero."))
            //{

            //    upmessagealfa.Update();
            //    this.LblMessageBox.Text = "La plantilla contiene una imagen no valida en el editor de texto, favor intentar de nuevo.";
            //    this.MPEMessage.Enabled = true;
            //    this.MPEMessage.Show();
            //}
            //else if (ex.Message.Contains("The remote server returned an error"))
            //{

            //    upmessagealfa.Update();
            //    this.LblMessageBox.Text = "La plantilla contiene una imagen no valida en el editor de texto, favor intentar de nuevo.";
            //    this.MPEMessage.Enabled = true;
            //    this.MPEMessage.Show();
            //}
            //else
            //{

            //    upmessagealfa.Update();
            //    this.LblMessageBox.Text = "Error en el sistema: " + ex.Message;
            //    this.MPEMessage.Enabled = true;
            //    this.MPEMessage.Show();
            //}
        }
    }

    private string CreatePDFDocument(string nameFile, string registro, byte[] imgFirma)
    {
        try
        {
            //obtener variables pdf
            DataSet ds = new DataSet();
            Business BI = new Business();
            string codplantilla = Session["Plantilla_CodPlantilla"].ToString();
            string dependencia = Session["depUsuario"].ToString();
            string cliente = "Archivar Ltda";
            string fechaRad = DateTime.Today.Year.ToString() + "-" + DateTime.Today.Month.ToString() + "-" + DateTime.Today.Day.ToString();
            string horaRad = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            //ds = BI.buscarPlnatilla(codplantilla, dependencia);
            //string contenido = Session["textoPlantilla"].ToString(); //ds.Tables[0].Rows[0]["HTML"].ToString();

            string target = createPath() + nameFile;
            //creacion pdf                
            Document document = new Document(PageSize.LETTER, 90f, 80f, 120f, 100f);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(target, FileMode.Create));
            ItsEvents e = new ItsEvents();
            writer.PageEvent = e;
            document.Open();

            // iTextSharp.text.Image header = iTextSharp.text.Image.GetInstance(Server.MapPath(@"~\Plantillas\Plantillas\Imagenes\Encabezado.png"));
            // header.ScalePercent(75f);
            // header.SetAbsolutePosition(document.PageSize.Width - 72f - 540f, document.PageSize.Height - 128f);

            // iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(Server.MapPath(@"~\Plantillas\Plantillas\Imagenes\Pie.png"));
            // footer.ScalePercent(49f);
            // footer.SetAbsolutePosition(document.PageSize.Width - 77f - 540f, document.PageSize.Height - 68f - 700f);
            // document.Add(header);
            // document.Add(footer);

            iTextSharp.text.Rectangle rect = new iTextSharp.text.Rectangle(document.PageSize.Width - 160f, 640f, 120f, 80f);
            rect.BorderWidth = 10f;
            rect.BorderColor = iTextSharp.text.BaseColor.BLACK;

            PdfContentByte cb = writer.DirectContent;

            PdfPTable table = new PdfPTable(1);
            PdfPCell cel1 = new PdfPCell(new Phrase(cliente, FontFactory.GetFont("arial", 6, BaseColor.BLACK)));
            cel1.Border = iTextSharp.text.Rectangle.TOP_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER;
            cel1.BorderWidthLeft = 0.9f;
            cel1.VerticalAlignment = 1;
            cel1.HorizontalAlignment = 1;
            cel1.BorderColor = BaseColor.LIGHT_GRAY;
            cel1.Colspan = 4;
            table.AddCell(cel1);

            PdfPCell cel2 = new PdfPCell(new Paragraph("Fecha: " + fechaRad.ToString() + "         Hora: " + horaRad.ToString(), FontFactory.GetFont("arial", 6, BaseColor.BLACK)));
            cel2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            cel2.BorderWidthLeft = 0.9f;
            cel2.BorderColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cel2);

            PdfPCell cel3 = new PdfPCell(new Paragraph("Registro No: " + registro, FontFactory.GetFont("arial", 6, BaseColor.BLACK)));
            cel3.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            cel3.BorderWidthLeft = 1f;
            cel3.BorderColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cel3);

            Business intelligence = null;
            DataSet DSRadicado = null;
            DataSet DSProcedencia = null;
            DataSet DSCiudad = null;

            intelligence = new Business();
            DSRadicado = new DataSet();
            DSProcedencia = new DataSet();
            DSCiudad = new DataSet();

            string radicado = Session["registro"].ToString();

            if (radicado.Contains(" |"))
            {
                radicado = radicado.Remove(radicado.IndexOf(" |"));
            }

            DSRadicado = intelligence.obtenerDatosRegistro(radicado);

            string codProcedencia = DSRadicado.Tables[0].Rows[0]["ProcedenciaCodDestino"].ToString();
            DSProcedencia = intelligence.obtenerDatosProcedencia(codProcedencia);





            string procNonbre = "";
            string procDireccion = "";
            string ciudadnombre = "";
            string DepartamentoNombre = "";
            DataSet DSRegistro = null;
            DSRegistro = new DataSet();
            DSRegistro = intelligence.obtenerDatosRegistro(registro);

            codProcedencia = DSRegistro.Tables[0].Rows[0]["ProcedenciaCodDestino"].ToString();
            if (codProcedencia != "")
            {
                DSProcedencia = intelligence.obtenerDatosProcedencia(codProcedencia);
                procNonbre = DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString();

                string codciudad = DSProcedencia.Tables[0].Rows[0]["CiudadCodigo"].ToString();
                DSCiudad = intelligence.obtenerDatosCiudad(codciudad);
                ciudadnombre = DSCiudad.Tables[0].Rows[0]["CiudadNombre"].ToString();
                DepartamentoNombre = DSCiudad.Tables[0].Rows[0]["DepartamentoNombre"].ToString();

                procDireccion = DSProcedencia.Tables[0].Rows[0]["ProcedenciaDireccion"].ToString();
            }
            else
            {
                string DependenciaCod = DSRegistro.Tables[0].Rows[0]["DependenciaCodDestino"].ToString();
                string DependenciaNom = DSRegistro.Tables[0].Rows[0]["dependencianombre1"].ToString();
                procNonbre = DependenciaCod + " " + DependenciaNom;
                procDireccion = "13001 CARTAGENA";
                ciudadnombre = "13001 CARTAGENA";
                DepartamentoNombre = "13000 BOLIVAR";
            }


            PdfPCell cel4 = new PdfPCell(new Paragraph("Destino: " + procNonbre, FontFactory.GetFont("arial", 6, BaseColor.BLACK)));
            cel4.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            cel4.BorderWidthLeft = 1f;
            cel4.BorderColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cel4);

            PdfPCell cel5 = new PdfPCell(new Paragraph("Dirección: " + procDireccion, FontFactory.GetFont("arial", 6, BaseColor.BLACK)));
            cel5.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            cel5.BorderWidthLeft = 1f;
            cel5.BorderColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cel5);

            PdfPCell cel6 = new PdfPCell(new Paragraph("" + ciudadnombre + " - " + DepartamentoNombre, FontFactory.GetFont("arial", 6, BaseColor.BLACK)));
            cel6.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER | iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
            cel6.BorderWidthLeft = 1f;
            cel6.BorderColor = BaseColor.LIGHT_GRAY;
            table.AddCell(cel6);

            //document.Add(table);

            table.LockedWidth = true;


            float[] width = { 150 };
            table.SetTotalWidth(width);
            table.WriteSelectedRows(0, 6, document.PageSize.Width - 200f, document.PageSize.Height - 120f, writer.DirectContent);
            cb.Stroke();

            string html = Session["textoPlantilla"].ToString();//contenido;
            Paragraph par = new Paragraph();
            par.Alignment = 3;
            foreach (IElement E in HTMLWorker.ParseToList(new StringReader(html), new StyleSheet()))
            {
                par.Add(E);
            }
            document.Add(par);

            iTextSharp.text.Image firma = iTextSharp.text.Image.GetInstance(imgFirma);
            firma.ScalePercent(60f);
            firma.SetAbsolutePosition(document.PageSize.Width - 20f - 500f, document.PageSize.Height - 60f - 600f);
            document.Add(firma);

            document.Close();
            return "OK";
            //writer.Close();
        }
        catch (Exception ex)
        {
            return "ERROR: al crear el Archivo PDF. " + ex.Message;
        }
        finally
        {
            //aca toca poner el cierre de los reader, el document etc, en caso de que ocurra algun error.
        }
    }
    private string createPath()
    {
        //string GGrupo = "2";

        DSImagenTableAdapters.RegistroImagenTableAdapter TARegistroImagen = new DSImagenTableAdapters.RegistroImagenTableAdapter();
        DSImagenTableAdapters.ImagenRutaTableAdapter TAImgRuta = new DSImagenTableAdapters.ImagenRutaTableAdapter();
        DSImagen.ImagenRutaDataTable DTImgRuta = new DSImagen.ImagenRutaDataTable();

        Object CodigoRuta = TAImgRuta.SelectRutaCodigoByAnioMesGrupo(DateTime.Today.Year, DateTime.Today.Month, "2");
        int codigoR = Convert.ToInt32(CodigoRuta);
        String Grupo = "Registros";
        String Ano = DateTime.Today.Year.ToString();
        String Mes = DateTime.Today.Month.ToString();

        string PathVirtual = HttpContext.Current.Server.MapPath("~/AlfaNetRepositorioImagenes/" + Grupo + "/" + Ano + "/" + Mes + "/");
        if (CodigoRuta == null)
        {
            TAImgRuta.Insert(1, "", DateTime.Today.Year, DateTime.Today.Month, 2, PathVirtual);
            //CodigoRuta = TAImgRuta.SelectRutaCodigoByAnioMesGrupo(DateTime.Today.Year, DateTime.Today.Month, "2");
            //codigoR = Convert.ToInt32(CodigoRuta);
            if (Directory.Exists(PathVirtual))
            {
                //TARegistroImagen.InsertRegistroImagen(GGrupo, Convert.ToInt32(this.HFNroDoc.Value), nombreArchivo, codigoR);
                //this.FileUpload1.SaveAs(PathVirtual + nombreArchivo);
                return PathVirtual;
            }
            else
            {
                Directory.CreateDirectory(PathVirtual);
                //TARegistroImagen.InsertRegistroImagen(GGrupo, Convert.ToInt32(this.HFNroDoc.Value), nombreArchivo, codigoR);
                //this.FileUpload1.SaveAs(PathVirtual + nombreArchivo);
                return PathVirtual;
            }
        }
        else
        {
            if (Directory.Exists(PathVirtual))
            {
                //TARegistroImagen.InsertRegistroImagen(GGrupo, Convert.ToInt32(this.HFNroDoc.Value), nombreArchivo, codigoR);
                //this.FileUpload1.SaveAs(PathVirtual + nombreArchivo);
                return PathVirtual;
            }
            else
            {
                Directory.CreateDirectory(PathVirtual);
                //TARegistroImagen.InsertRegistroImagen(GGrupo, Convert.ToInt32(this.HFNroDoc.Value), nombreArchivo, codigoR);
                //this.FileUpload1.SaveAs(PathVirtual + nombreArchivo);
                return PathVirtual;
            }
        }
    }

    protected void LBFirmar_Click(object sender, ImageClickEventArgs e)
    {
        //LBFirmar.Enabled = false;
        //if (Session["registro"].ToString()=="0")
        //if (Session["registro"])
        //if (String.IsNullOrEmpty(Session["registro"].ToString()))
        //{
            try
            {
                byte[] fImagenFirma;
                if (Session["fuImagenFirma"] != null)
                {
                    if (valImgFirma(Session["fuImagenFirma"].ToString()))
                    {
                    string rutaTemporal = Path.Combine(Server.MapPath("~/TemporalVisualisacion"), Session["fuImagenFirma"].ToString());
                    if (File.Exists(rutaTemporal))
                    {
                        // Leemos el contenido del archivo y lo convertimos en un array de bytes
                        fImagenFirma = File.ReadAllBytes(rutaTemporal);
                    }
                    else
                    {
                        // Manejamos el caso en el que el archivo temporal no existe
                        throw new FileNotFoundException("El archivo temporal no se encontró.");
                    }
                    //Creamos el registro
                    //DatosRegistro datos = new DatosRegistro();
                    //datos = (DatosRegistro)Session["datos"];
                    //string registro = registrar(datos);
                    //crearRadicadoFuente(Convert.ToInt32(registro));
                    ////s� el registro se realiza, creamos el pdf.
                    string registro = Session["registro"].ToString();
                        if (registro != "0")
                        {
                            string imagenName = "respuesta.pdf";
                            string nombreArchivo = 2 + "_" + registro + "_" + DateTime.Now.ToString("yyyyMMdd") + "_" + imagenName;
                            Session["ImgFirmaImageUrl"] = registro;
                            string creo = CreatePDFDocument(nombreArchivo, registro, fImagenFirma);
                            if (creo == "OK")
                            {
                                guardarArchivo(registro, nombreArchivo);
                                upmessagealfa.Update();
                                pnlBotonFirmar.Visible = false;
                            }
                            else
                            {
                                upmessagealfa.Update();
                                this.LblMessageBox.Text = "El sistema no pudo generar el archivo asociado al registro.";

                                this.MPEMessage.Enabled = true;
                                this.MPEMessage.Show();
                            }
                        }
                        else
                        {
                            LBFirmar.Enabled = true;
                            upmessagealfa.Update();
                            this.LblMessageBox.Text = "En este momento no es posible generar el registro, por favor intente mas tarde";
                            this.MPEMessage.Enabled = true;
                            this.MPEMessage.Show();
                            LBFirmar.Enabled = false;
                        }
                    }
                    else
                    {
                        LBFirmar.Enabled = true;
                        upmessagealfa.Update();
                        this.LblMessageBox.Text = "Seleccione un formato de imagen válido jpg � png)";
                        this.MPEMessage.Enabled = true;
                        this.MPEMessage.Show();
                    }
                }
                else
                {
                    LBFirmar.Enabled = true;
                    upmessagealfa.Update();
                    this.LblMessageBox.Text = "Seleccione la imagen de su firma en formato .jpg � .png";

                    this.MPEMessage.Enabled = true;
                    this.MPEMessage.Show();
                }
            }
            catch (Exception ex)
            {
                LBFirmar.Enabled = true;
                upmessagealfa.Update();
                this.LblMessageBox.Text = "Error en el sistema " + ex.Message;
                this.MPEMessage.Enabled = true;
                this.MPEMessage.Show();
            }
        //}
        //else
        //{
        //    LBFirmar.Enabled = true;
        //    pnlBotonFirmar.Visible = false;
        //    pnlBotonAdjuntarVisualizar.Visible = true;
        //}
    }
    private bool valImgFirma(string p)
    {
        if (p.ToLower().EndsWith(".png") || p.ToLower().EndsWith(".jpg"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void guardarArchivo(string registro,string nombreArchivo)
        {
            Business BI = new Business();
            DataSet ds = new DataSet();
            
            
                Business intelligence = new Business();
                DataSet datas = new DataSet();
                datas = intelligence.obtenerCodigoRuta(DateTime.Today.Year, DateTime.Today.Month, "2");
                string CodigoRuta = "";
                foreach (DataRow var in datas.Tables[0].Rows)
                {
                    CodigoRuta = datas.Tables[0].Rows[0]["ImagenRutaCodigo"].ToString();
                }
                
                string ano = DateTime.Today.Year.ToString();
                string mes = DateTime.Today.Month.ToString();

                String PathVirtual = ConfigurationSettings.AppSettings["RutaRepositorio"] + DateTime.Now.Year + @"\" + DateTime.Now.Month + @"\";

                if (CodigoRuta == null || CodigoRuta == "")
                {
                    intelligence.insertarImagen(1, "", DateTime.Today.Year, DateTime.Today.Month, 2, PathVirtual);

                    ds = intelligence.obtenerCodigoRuta(DateTime.Today.Year, DateTime.Today.Month, "2");

                    foreach (DataRow var in ds.Tables[0].Rows)
                    {
                        CodigoRuta = ds.Tables[0].Rows[0]["ImagenRutaCodigo"].ToString();
                    }
                    int codigoR = Convert.ToInt32(CodigoRuta);
                    if (Directory.Exists(PathVirtual))
                    {
                        intelligence.insertarRegistroImagen("2", Convert.ToInt32(registro), nombreArchivo, codigoR);
                    }
                    else
                    {
                        Directory.CreateDirectory(PathVirtual);
                        intelligence.insertarRegistroImagen("2", Convert.ToInt32(registro), nombreArchivo, codigoR);
                    }
                }
                else
                {
                    int codigoR = Convert.ToInt32(CodigoRuta);
                    if (Directory.Exists(PathVirtual))
                    {
                        intelligence.insertarRegistroImagen("2", Convert.ToInt32(registro), nombreArchivo, codigoR);
                    }
                    else
                    {
                        Directory.CreateDirectory(PathVirtual);
                        intelligence.insertarRegistroImagen("2", Convert.ToInt32(registro), nombreArchivo, codigoR);
                    }
                }
            
        }
        protected void LBAdjuntar_Click(object sender, EventArgs e)
        {
            //LNumRegistro.Text = firma.Value.ToString();
            //Session["ImgFirmaImageUrl"] = Session["NumRegistro"].ToString();
            //string registro = Session["Plantilla_RegistroNro"].ToString();
            //Session["Plantilla_RegistroNro"] = Session["Plantilla_RegistroNro"].ToString();
            //LMessagePlantilla.Text = registro;
            //message();
            //Page.RegisterStartupScript("script", "<script>window.open('../AlfaNetImagen/VisorImagenes/VisorImagenes.aspx?" + "DocumentoCodigo=" + Session["Plantilla_RegistroNro"].ToString() + "&GrupoCodigo=2&GrupoPadreCodigo=2&Admon=S&ImagenFolio=1' ,'Titulo','height=800', 'width=600')</script>");
            //ScriptManager.RegisterStartupScript(this, Page.GetType(), "script", "<script>window.open('../../AlfaNetImagen/VisorImagenes/VisorImagenes.aspx?" + "DocumentoCodigo=" + registro + "&GrupoCodigo=2&GrupoPadreCodigo=2&Admon=S&ImagenFolio=1' ,'Titulo','height=1000', 'width=2000')</script>", false);
		            //LNumRegistro.Text = firma.Value.ToString();
            //Session["ImgFirmaImageUrl"] = Session["NumRegistro"].ToString();
            string registro = Session["Plantilla_RegistroNro"].ToString();
            Session["Plantilla_RegistroNro"] = Session["Plantilla_RegistroNro"].ToString();
            //LMessagePlantilla.Text = registro;
            //message();
            //Page.RegisterStartupScript("script", "<script>window.open('../AlfaNetImagen/VisorImagenes/VisorImagenes.aspx?" + "DocumentoCodigo=" + Session["Plantilla_RegistroNro"].ToString() + "&GrupoCodigo=2&GrupoPadreCodigo=2&Admon=S&ImagenFolio=1' ,'Titulo','height=800', 'width=600')</script>");

            string ventana = "<script>window.open('../../AlfaNetImagen/VisorImagenes/VisorImagenes.aspx?" + "DocumentoCodigo=" + registro + "&GrupoCodigo=2&GrupoPadreCodigo=2&Admon=S&ImagenFolio=1' ,'Titulo','height=1000', 'width=2000', 'top=50', 'left=50')</script>";
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "script", ventana.ToString(), false);
        
        }
        protected void btnContinuarACopiarCorreos_Click(object sender, ImageClickEventArgs e)
        {
            pnlEnviarPorCorreo.Visible = true;
            //pnlBotonAdjuntarVisualizar.Visible = false;            
        }
        protected void LBCopiar_Click(object sender, EventArgs e)
        {
            DivCorreos.Attributes.Add("style", "display:inline-block; width: 480px;");
            PCorreos.Visible = true;
            /* System.Text.StringBuilder sb = new System.Text.StringBuilder();
             sb.Append(@"<script type='text/javascript'>");
             sb.Append(@"$('#document').ready(function() {");
             sb.Append(@"$('#DivCorreos').dialog({");
             sb.Append(@"modal: false,");
             sb.Append(@"autoOpen: true,");
             sb.Append(@"title: 'Vista Previa Documento',");
             sb.Append(@"open: function(type, data) {");
             sb.Append(@"$(this).parent().appendTo('form');");
             sb.Append(@"}");
             sb.Append(@"});");
             //sb.Append(@"$('#IBPlantillaSalir').click(function() {");
             //sb.Append(@"alert('hola');");
             //sb.Append(@"$('#DivCorreos').dialog('open');");
             //sb.Append(@"return false;");
             //sb.Append(@"});");
             sb.Append(@"});");
             sb.Append(@"</script>");
             ScriptManager.RegisterStartupScript(this, Page.GetType(), "JScript", sb.ToString(), false);*/

        }
        protected void LBEnviar_Click(object sender, EventArgs e)
        {
            Business intelligence = null;
            DataSet DSRadicado = null;
            DataSet DSRegistro = null;
            DataSet DSProcedencia = null;
            DataSet imagenes = null;
            DataSet codigoImagenRuta = null;
            DataSet correoDep = null;
            DataSet datosProcedencia = null;
            string expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            //MembershipUser usuario;
            MailMessage mail = null;
            Attachment attachment = null;
        try
        {
            intelligence = new Business();
            mail = new MailMessage();
            this.LblMessageBox.Text = "";
            DSRadicado = new DataSet();
            DSRegistro = new DataSet();
            DSProcedencia = new DataSet();
            ArrayList registroLista = Session["regisrtosLista"] as ArrayList;
            foreach (string registro in registroLista)
            {
                if (registro.Contains(" |"))
                {

                    registro.Remove(registro.IndexOf(" |"));
                }
                if (registro != "")
                {
                    string codProcedencia = "";

                    DSRegistro = intelligence.obtenerDatosRegistro(registro);
                    if(RadioButtonList1.SelectedValue == "0")
                    {
                        codProcedencia = DSRegistro.Tables[0].Rows[0]["ProcedenciaCodDestino"].ToString();
                        DSProcedencia = intelligence.obtenerDatosProcedencia(codProcedencia);
                        string procedenciaMail = DSProcedencia.Tables[0].Rows[0]["ProcedenciaMail1"].ToString();
                        string procNonbre = DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString();

                        if (procedenciaMail != "" || procedenciaMail != null)
                        {
                            if (Regex.IsMatch(procedenciaMail, expresion))
                            {
                                imagenes = new DataSet();
                                codigoImagenRuta = new DataSet();
                                //OBTENER IMAGENES DEL REGISTRO
                                string file = string.Empty;
                                string codigoRuta = string.Empty;
                                string ruta = string.Empty;
                                string imgs = string.Empty;
                                string imgsFinal = string.Empty;
                                imagenes = intelligence.obtenerImagenesRegistro("2", Convert.ToInt32(registro));
                                if (imagenes.Tables[0].Rows.Count != 0)
                                {
                                    foreach (DataRow item in imagenes.Tables[0].Rows)
                                    {
                                        file = item["RegistroImagenNombre"].ToString();
                                        codigoRuta = item["ImagenRutaCodigo"].ToString();
                                        codigoImagenRuta = intelligence.obtenerImagenruta(Convert.ToInt32(codigoRuta));
                                        foreach (DataRow items in codigoImagenRuta.Tables[0].Rows)
                                        {
                                            ruta = items["ImagenRutaPath"].ToString();

                                            imgs = ruta + file;
                                            mail.Attachments.Clear();
                                            attachment = new Attachment(imgs);
                                            mail.Attachments.Add(attachment);
                                        }
                                    }
                                }

                                string Body = "Tiene un nuevo Registro Nro. " + registro;
                                correoDep = new DataSet();
                                string mails = "";
                                datosProcedencia = new DataSet();
                                string ccmails = "";
                                string cmails = "";
                                string usersdep = "";
                                string[] emails;

                                foreach (System.Web.UI.WebControls.ListItem item in LBCorreos.Items)
                                {
                                    cmails = string.Empty;
                                    mails = item.ToString();
                                    emails = mails.Split('|');
                                    mails = emails[0];
                                    datosProcedencia = intelligence.obtenerDatosProcedencia(mails);
                                    if (datosProcedencia.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow row in datosProcedencia.Tables[0].Rows)
                                        {
                                            cmails = row["ProcedenciaMail1"].ToString(); //SE OBTIENE LA PROCEDENCIA.
                                        }
                                    }
                                    DataSet DSUsuarioxDependencia = new DataSet();
                                    DSUsuarioxDependencia = intelligence.obtenerUsuariosxDependencia(mails);
                                    if (DSUsuarioxDependencia.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (DataRow row in DSUsuarioxDependencia.Tables[0].Rows)
                                        {



                                            usersdep = row["userid"].ToString();
                                            System.Guid UId = new Guid(usersdep);

                                            MembershipUser user = Membership.GetUser(UId);
                                            cmails = user.Email.ToString();
                                        }
                                    }

                                    if (Regex.IsMatch(cmails, expresion))
                                    {
                                        if (Regex.Replace(cmails, expresion, String.Empty).Length == 0)
                                        {
                                            if (cmails.ToString() != "")
                                            {
                                                ccmails += cmails + ",";
                                            }
                                            if (DSUsuarioxDependencia.Tables[0].Rows.Count > 0 && datosProcedencia.Tables[0].Rows.Count > 0)
                                            {
                                                //ccmails = ccmails.Substring(0, ccmails.Length - 1);
                                            }
                                        }
                                    }
                                }
                                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Alfanet", System.Text.Encoding.UTF8);
                                if (Regex.IsMatch(procedenciaMail, expresion))
                                {
                                    if (Regex.Replace(procedenciaMail.ToString().Trim(), expresion, String.Empty).Length == 0)
                                    {
                                        mail.To.Clear();
                                        mail.To.Add(procedenciaMail);
                                        if (ccmails != "")
                                        {
                                            mail.CC.Add(ccmails);
                                        }

                                        mail.Subject = "Registro No: " + registro;
                                        mail.SubjectEncoding = System.Text.Encoding.UTF8;
                                        mail.Body = Body;
                                        mail.BodyEncoding = System.Text.Encoding.UTF8;
                                        SmtpServer.Port = 587;
                                        SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailFrom"], ConfigurationManager.AppSettings["EmailPass"]);
                                        SmtpServer.EnableSsl = true;

                                        SmtpServer.Send(mail);
                                        //LMessagePlantilla.Text = "La respuesta ha sido enviada exitosamente";
                                        //message();
                                        upmessagealfa.Update();
                                        this.LblMessageBox.Text += "La respuesta del registro " + registro + " ha sido enviada exitosamente. <br/>";

                                        this.MPEMessage.Enabled = true;
                                        this.MPEMessage.Show();
                                    }
                                    else
                                    {
                                        //return false;
                                    }
                                }
                                else
                                {
                                    //LMessagePlantilla.Text = "El correo Electronico de " + DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString() + " no es Válido.<br />";
                                    //message();
                                    upmessagealfa.Update();
                                    this.LblMessageBox.Text = "Registro " + registro + ": El correo Electronico de " + DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString() + " no es Válido.<br />";

                                    this.MPEMessage.Enabled = true;
                                    this.MPEMessage.Show();
                                }
                            }
                            else
                            {
                                lbtnPrint.Visible = true;
                                //LMessagePlantilla.Text = "El correo electrónico del destinatario: " + procNonbre + " no es válido. Debe enviar la respuesta en medio físico, la cual puede descargar aquí: ";

                                //message();
                                upmessagealfa.Update();
                                this.LblMessageBox.Text = "Registro " + registro + ":El correo electrónico del destinatario: " + procNonbre + " no es válido. Debe enviar la respuesta en medio físico, la cual puede descargar en la opción de Visualizar, Anexar, Imprimir";

                                this.MPEMessage.Enabled = true;
                                this.MPEMessage.Show();
                            }

                        }
                        else
                        {
                            //LMessagePlantilla.Text = "El correo electrónico del destinatario no existe. Debe enviar la respuesta en medio físico";
                            //message();
                            upmessagealfa.Update();
                            this.LblMessageBox.Text = "Registro " + registro + ":El correo electrónico del destinatario no existe. Debe enviar la respuesta en medio físico";

                            this.MPEMessage.Enabled = true;
                            this.MPEMessage.Show();
                        }
                    }
                    if (RadioButtonList1.SelectedValue == "1")
                    {
                        MailBLL Correo = new MailBLL();
                        MembershipUser usuario;
                        string codDependencia = DSRegistro.Tables[0].Rows[0]["DependenciaCodDestino"].ToString();
                        DSUsuarioTableAdapters.UsuariosxdependenciaTableAdapter ObjTaUsuarioxDependencia = new DSUsuarioTableAdapters.UsuariosxdependenciaTableAdapter();
                        DSUsuario.UsuariosxdependenciaDataTable DTUsuariosxDependencia = new DSUsuario.UsuariosxdependenciaDataTable();
                        DTUsuariosxDependencia = ObjTaUsuarioxDependencia.GetUsuariosxDependenciaByDependencia(codDependencia);
                        if (DTUsuariosxDependencia.Count > 0)
                        {
                            DataRow[] rows = DTUsuariosxDependencia.Select();
                            System.Guid a = new Guid(rows[0].ItemArray[0].ToString().Trim());
                            DSaspnet_UsersTableAdapters.Aspnet_Users_ReadUsersByUserNameTableAdapter objReadUser = new DSaspnet_UsersTableAdapters.Aspnet_Users_ReadUsersByUserNameTableAdapter();
                            DSaspnet_Users.Aspnet_Users_ReadUsersByUserNameDataTable TablaRead = new DSaspnet_Users.Aspnet_Users_ReadUsersByUserNameDataTable();
                            TablaRead = objReadUser.GetDataByUserId(a.ToString());
                            DataRow[] rowsUser = TablaRead.Select();
                            string Usrname = rowsUser[0].ItemArray[1].ToString();
                            usuario = Membership.GetUser(Usrname);

                            if (usuario.Email != "" || usuario.Email != null)
                            {
                                if (Regex.IsMatch(usuario.Email, expresion))
                                {
                                    imagenes = new DataSet();
                                    codigoImagenRuta = new DataSet();
                                    //OBTENER IMAGENES DEL REGISTRO
                                    string file = string.Empty;
                                    string codigoRuta = string.Empty;
                                    string ruta = string.Empty;
                                    string imgs = string.Empty;
                                    string imgsFinal = string.Empty;
                                    imagenes = intelligence.obtenerImagenesRegistro("2", Convert.ToInt32(registro));
                                    if (imagenes.Tables[0].Rows.Count != 0)
                                    {
                                        foreach (DataRow item in imagenes.Tables[0].Rows)
                                        {
                                            file = item["RegistroImagenNombre"].ToString();
                                            codigoRuta = item["ImagenRutaCodigo"].ToString();
                                            codigoImagenRuta = intelligence.obtenerImagenruta(Convert.ToInt32(codigoRuta));
                                            foreach (DataRow items in codigoImagenRuta.Tables[0].Rows)
                                            {
                                                ruta = items["ImagenRutaPath"].ToString();

                                                imgs = ruta + file;
                                                mail.Attachments.Clear();
                                                attachment = new Attachment(imgs);
                                                mail.Attachments.Add(attachment);
                                            }
                                        }
                                    }

                                    string Body = "Tiene un nuevo Registro Nro. " + registro;
                                    correoDep = new DataSet();
                                    string mails = "";
                                    datosProcedencia = new DataSet();
                                    string ccmails = "";
                                    string cmails = "";
                                    string usersdep = "";
                                    string[] emails;

                                    foreach (System.Web.UI.WebControls.ListItem item in LBCorreos.Items)
                                    {
                                        cmails = string.Empty;
                                        mails = item.ToString();
                                        emails = mails.Split('|');
                                        mails = emails[0];
                                        datosProcedencia = intelligence.obtenerDatosProcedencia(mails);
                                        if (datosProcedencia.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow row in datosProcedencia.Tables[0].Rows)
                                            {
                                                cmails = row["ProcedenciaMail1"].ToString(); //SE OBTIENE LA PROCEDENCIA.
                                            }
                                        }
                                        DataSet DSUsuarioxDependencia = new DataSet();
                                        DSUsuarioxDependencia = intelligence.obtenerUsuariosxDependencia(mails);
                                        if (DSUsuarioxDependencia.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow row in DSUsuarioxDependencia.Tables[0].Rows)
                                            {



                                                usersdep = row["userid"].ToString();
                                                System.Guid UId = new Guid(usersdep);

                                                MembershipUser user = Membership.GetUser(UId);
                                                cmails = user.Email.ToString();
                                            }
                                        }

                                        if (Regex.IsMatch(cmails, expresion))
                                        {
                                            if (Regex.Replace(cmails, expresion, String.Empty).Length == 0)
                                            {
                                                if (cmails.ToString() != "")
                                                {
                                                    ccmails += cmails + ",";
                                                }
                                                if (DSUsuarioxDependencia.Tables[0].Rows.Count > 0 && datosProcedencia.Tables[0].Rows.Count > 0)
                                                {
                                                    //ccmails = ccmails.Substring(0, ccmails.Length - 1);
                                                }
                                            }
                                        }
                                    }
                                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                                    mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Alfanet", System.Text.Encoding.UTF8);
                                    if (Regex.IsMatch(usuario.Email, expresion))
                                    {
                                        if (Regex.Replace(usuario.Email.ToString().Trim(), expresion, String.Empty).Length == 0)
                                        {
                                            mail.To.Clear();
                                            mail.To.Add(usuario.Email);
                                            if (ccmails != "")
                                            {
                                                mail.CC.Add(ccmails);
                                            }

                                            mail.Subject = "Registro No: " + registro;
                                            mail.SubjectEncoding = System.Text.Encoding.UTF8;
                                            mail.Body = Body;
                                            mail.BodyEncoding = System.Text.Encoding.UTF8;
                                            SmtpServer.Port = 587;
                                            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailFrom"], ConfigurationManager.AppSettings["EmailPass"]);
                                            SmtpServer.EnableSsl = true;

                                            SmtpServer.Send(mail);
                                            //LMessagePlantilla.Text = "La respuesta ha sido enviada exitosamente";
                                            //message();
                                            upmessagealfa.Update();
                                            this.LblMessageBox.Text += "La respuesta del registro " + registro + " ha sido enviada exitosamente. <br/>";

                                            this.MPEMessage.Enabled = true;
                                            this.MPEMessage.Show();
                                        }
                                        else
                                        {
                                            //return false;
                                        }
                                    }
                                    else
                                    {
                                        //LMessagePlantilla.Text = "El correo Electronico de " + DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString() + " no es Válido.<br />";
                                        //message();
                                        upmessagealfa.Update();
                                        this.LblMessageBox.Text = "Registro " + registro + ": El correo Electronico de " + DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString() + " no es Válido.<br />";

                                        this.MPEMessage.Enabled = true;
                                        this.MPEMessage.Show();
                                    }
                                }
                                else
                                {
                                    lbtnPrint.Visible = true;
                                    //LMessagePlantilla.Text = "El correo electrónico del destinatario: " + procNonbre + " no es válido. Debe enviar la respuesta en medio físico, la cual puede descargar aquí: ";

                                    //message();
                                    upmessagealfa.Update();
                                    this.LblMessageBox.Text = "Registro " + registro + ":El correo electrónico del destinatario: " + usuario.UserName + " no es válido. Debe enviar la respuesta en medio físico, la cual puede descargar en la opción de Visualizar, Anexar, Imprimir";

                                    this.MPEMessage.Enabled = true;
                                    this.MPEMessage.Show();
                                }

                            }
                            else
                            {
                                //LMessagePlantilla.Text = "El correo electrónico del destinatario no existe. Debe enviar la respuesta en medio físico";
                                //message();
                                upmessagealfa.Update();
                                this.LblMessageBox.Text = "Registro " + registro + ":El correo electrónico del destinatario no existe. Debe enviar la respuesta en medio físico";

                                this.MPEMessage.Enabled = true;
                                this.MPEMessage.Show();
                            }

                        }

                    }
                    //string codProcedencia = DSRegistro.Tables[0].Rows[0]["ProcedenciaCodDestino"].ToString();

                    //DSProcedencia = intelligence.obtenerDatosProcedencia(codProcedencia);
                    //string procedenciaMail = DSProcedencia.Tables[0].Rows[0]["ProcedenciaMail1"].ToString();
                    //string procNonbre = DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString();

                    //string registro = Session["registro"].ToString();
                    

                }
                else
                {
                    //string registro = Session["Plantilla_CodPlantilla"].ToString();
                    imagenes = new DataSet();
                    codigoImagenRuta = new DataSet();
                    //OBTENER IMAGENES DEL REGISTRO
                    string file = string.Empty;
                    string codigoRuta = string.Empty;
                    string ruta = string.Empty;
                    string imgs = string.Empty;
                    string imgsFinal = string.Empty;
                    imagenes = intelligence.obtenerImagenesRegistro("2", Convert.ToInt32(registro));
                    if (imagenes.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow item in imagenes.Tables[0].Rows)
                        {
                            file = item["RegistroImagenNombre"].ToString();
                            codigoRuta = item["ImagenRutaCodigo"].ToString();
                            codigoImagenRuta = intelligence.obtenerImagenruta(Convert.ToInt32(codigoRuta));
                            foreach (DataRow items in codigoImagenRuta.Tables[0].Rows)
                            {
                                ruta = items["ImagenRutaPath"].ToString();

                                imgs = ruta + file;
                                attachment = new Attachment(imgs);
                                mail.Attachments.Add(attachment);
                            }
                        }
                    }
                    string Body = "Tiene un nuevo Registro Nro. " + registro;
                    correoDep = new DataSet();
                    string mails = "";
                    datosProcedencia = new DataSet();
                    string ccmails = "";
                    string cmails = "";
                    string usersdep = "";
                    //string mailcc = "";
                    //string mensajefinal = "";
                    string[] emails;

                    foreach (System.Web.UI.WebControls.ListItem item in LBCorreos.Items)
                    {
                        cmails = string.Empty;
                        mails = item.ToString();
                        emails = mails.Split('|');
                        mails = emails[0];

                        datosProcedencia = intelligence.obtenerDatosProcedencia(mails);
                        if (datosProcedencia.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in datosProcedencia.Tables[0].Rows)
                            {
                                cmails = row["ProcedenciaMail1"].ToString(); //SE OBTIENE LA PROCEDENCIA.
                            }
                        }

                        DataSet DSUsuarioxDependencia = new DataSet();
                        DSUsuarioxDependencia = intelligence.obtenerUsuariosxDependencia(mails);
                        if (DSUsuarioxDependencia.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in DSUsuarioxDependencia.Tables[0].Rows)
                            {
                                usersdep = row["userid"].ToString();
                                //string U = Convert.ToString(usersdep);
                                System.Guid UId = new Guid(usersdep);

                                MembershipUser user = Membership.GetUser(UId);
                                cmails = user.Email.ToString();
                            }
                        }
                        if (Regex.IsMatch(cmails, expresion))
                        {
                            if (Regex.Replace(cmails, expresion, String.Empty).Length == 0)
                            {
                                if (cmails.ToString() != "")
                                {
                                    ccmails += cmails + ",";
                                }
                                if (DSUsuarioxDependencia.Tables[0].Rows.Count > 0 && datosProcedencia.Tables[0].Rows.Count > 0)
                                {
                                    //ccmails = ccmails.Substring(0, ccmails.Length - 1);
                                }
                            }
                        }

                        DSRegistro = intelligence.obtenerDatosRegistro(registro);
                        string codProcedencia = DSRegistro.Tables[0].Rows[0]["ProcedenciaCodDestino"].ToString();
                        string procedenciaMail = "";
                        if (codProcedencia != "")
                        {
                            DSProcedencia = intelligence.obtenerDatosProcedencia(codProcedencia);
                            string procNonbre = DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString();
                            procedenciaMail = DSProcedencia.Tables[0].Rows[0]["ProcedenciaMail1"].ToString();
                        }
                        else
                        {
                            string DependenciaCod = DSRegistro.Tables[0].Rows[0]["DependenciaCodDestino"].ToString();
                            //DataSet DSUsuarioxDependencia = new DataSet();
                            DSUsuarioxDependencia = intelligence.obtenerUsuariosxDependencia(DependenciaCod);

                            foreach (DataRow row in DSUsuarioxDependencia.Tables[0].Rows)
                            {
                                usersdep = row["userid"].ToString();
                                //string U = Convert.ToString(usersdep);
                                System.Guid UId = new Guid(usersdep);

                                MembershipUser user = Membership.GetUser(UId);
                                procedenciaMail = user.Email.ToString();
                            }

                        }
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailFrom"], "Alfanet", System.Text.Encoding.UTF8);

                        if (Regex.IsMatch(procedenciaMail, expresion))
                        {
                            if (Regex.Replace(procedenciaMail.ToString().Trim(), expresion, String.Empty).Length == 0)
                            {
                                mail.To.Add(procedenciaMail);
                                if (ccmails != "")
                                {
                                    mail.CC.Add(ccmails);
                                }

                                mail.Subject = "Registro No: " + registro;
                                mail.SubjectEncoding = System.Text.Encoding.UTF8;
                                mail.Body = Body;
                                mail.BodyEncoding = System.Text.Encoding.UTF8;
                                SmtpServer.Port = 587;
                                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailFrom"], ConfigurationManager.AppSettings["EmailPass"]);
                                SmtpServer.EnableSsl = true;
                                //SmtpServer.EnableSsl = true;

                                SmtpServer.Send(mail);
                                //LMessagePlantilla.Text = "La respuesta ha sido enviada exitosamente";
                                //message();
                                upmessagealfa.Update();
                                this.LblMessageBox.Text += " - La respuesta ha sido enviada exitosamente";

                                this.MPEMessage.Enabled = true;
                                this.MPEMessage.Show();
                            }
                            else
                            {
                                //return false;
                            }
                        }
                        else
                        {
                            //LMessagePlantilla.Text = "El correo Electronico de " + DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString() + " no es Válido.<br />";
                            //message();
                            upmessagealfa.Update();
                            this.LblMessageBox.Text = "El correo Electronico de " + DSProcedencia.Tables[0].Rows[0]["ProcedenciaNombre"].ToString() + " no es Válido.<br />";

                            this.MPEMessage.Enabled = true;
                            this.MPEMessage.Show();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //LMessagePlantilla.Text = "Falló el envío del correo electrónico con la respuesta. Descripción: "+ex.Message;
            //message();
            upmessagealfa.Update();
            this.LblMessageBox.Text = "Falló el envío del correo electrónico con la respuesta. Descripción: " + ex.Message;
            this.MPEMessage.Enabled = true;
            this.MPEMessage.Show();
        }
        }
        protected void btnAtrasEnviarEmail_Click(object sender, ImageClickEventArgs e)
        {
            if (lbtnPrint.Visible == true)
            {
                lbtnPrint.Visible = false;
            }
            //pnlBotonAdjuntarVisualizar.Visible = true;
            pnlEnviarPorCorreo.Visible = false;
            
        }
        protected void btnFinalizar_Click(object sender, ImageClickEventArgs e)
        {
            //Con este boton se esta cerrando desde javascript con el evento OnClientClick
            if (lbtnPrint.Visible == true)
            {
                lbtnPrint.Visible = false;
            }
            Session.Remove("textoPlantilla");
            //Session["textoPlantilla"]
        }
        protected void LBCerrar_Click(object sender, EventArgs e)
        {
            DivCorreos.Attributes.Add("style", "display:none");
            PCorreos.Visible = false;
        }
        protected void RBLCarreos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLCarreos.SelectedValue == "1")
            {
                TBCorreos.Text = string.Empty;
                this.ACECorreos.ServiceMethod = "GetProcedenciaByTextNombre";

                //this.PopCDestino.Enabled = true;
                //this.ImageButton12.Visible = false;
            }
            else if (RBLCarreos.SelectedValue == "0")
            {
                TBCorreos.Text = string.Empty;
                this.ACECorreos.ServiceMethod = "GetDependenciaByText";

                //this.PopCDestino.Enabled = false;
                //this.ImageButton12.Visible = true;
            }
        }
        protected void IBCarreos_Click(object sender, ImageClickEventArgs e)
        {
            string email = TBCorreos.Text;
            LBCorreos.Items.Add(email);
            TBCorreos.Text = "";
        }
        protected void IBCorreosBorrar_Click(object sender, ImageClickEventArgs e)
        {
            LBCorreos.Items.Remove(LBCorreos.SelectedItem);
        }
        protected void lbtnPrint_Click(object sender, EventArgs e)
        {
            string registro = Session["Plantilla_RegistroNro"].ToString();
            ScriptManager.RegisterStartupScript(this, Page.GetType(), "script", "<script>window.open('../../AlfaNetImagen/VisorImagenes/VisorImagenes.aspx?" + "DocumentoCodigo=" + registro + "&GrupoCodigo=2&GrupoPadreCodigo=2&Admon=S&ImagenFolio=1' ,'Titulo','height=1000', 'width=1000')</script>", false);
        }
        protected void btnSubir_Click(object sender, EventArgs e)
        {
            if (fuImagenFirma.HasFile)
            {
                try
                {
                    // Obtenemos el nombre del archivo
                    string nombreArchivo = Path.GetFileName(fuImagenFirma.FileName);
                    Session["fuImagenFirma"] = nombreArchivo;
                    // Guardamos la imagen en una ubicación temporal en el servidor
                    string rutaTemporal = Path.Combine(Server.MapPath("~/TemporalVisualisacion"), nombreArchivo);
                    fuImagenFirma.SaveAs(rutaTemporal);

                // Mostramos un mensaje de éxito o realizamos otras operaciones
                this.LblMessageBox.Text = "Imagen cargada satisfactoriamente.";
                this.MPEMessage.Enabled = true;
                this.MPEMessage.Show();
                this.fuImagenFirma.Enabled = false;
                }
                catch (Exception ex)
                {
                // Manejamos cualquier error que pueda ocurrir durante el proceso de carga
                this.LblMessageBox.Text = "Error al subir la imagen: " + ex.Message;
                this.MPEMessage.Enabled = true;
                this.MPEMessage.Show();
                }
            }
            else
            {
                // Se proporciona un mensaje si no se selecciona ningún archivo para cargar
                this.LblMessageBox.Text = "<br/> Seleccione una imagen para cargar.";
                this.MPEMessage.Enabled = true;
                this.MPEMessage.Show();
        }
        }
        protected void btnCrearRegistros_Click(object sender, EventArgs e)
    {
        try
        {
            //ScriptManager.RegisterStartupScript(this, GetType(), "BloquearInterfaz", "bloquearInterfaz();", true);
            RegistroBLL ObjReg = new RegistroBLL();
            string RegistroCodigo = "1";
            this.LblMessageBox.Text = "";

            if (DDLPlantilla.SelectedItem.ToString() != "Seleccione")
            {
                string codPlantilla = DDLPlantilla.SelectedValue.ToString();
                Business intelligence = new Business();
                DataSet ds = new DataSet();
                ds = intelligence.TraerHTMLPlantilla(codPlantilla);
                string codHtml = ds.Tables[0].Rows[0]["HTML"].ToString();
                //string radicadoCodigo = this.ListBoxEnterar.Items;
                string WFAccion = "1";
                String RadCodString;
                RadCodString = TxtRadFuente.Text;
                if (RadCodString == "")
                    RadCodString = null;

                if (RadCodString != null)
                {
                    if (RadCodString.Contains(" | "))
                    {
                        RadCodString = RadCodString.Remove(RadCodString.IndexOf(" | "));
                    }
                }
                ArrayList regisrtosList = new ArrayList();
                if (RadioButtonList1.SelectedValue == "0")
                {
                    foreach (System.Web.UI.WebControls.ListItem Procedencia in this.ListBoxEnterar.Items)
                    {
                        RegistroCodigo = ObjReg.AddRegistro(GrpDocReg, Convert.ToDateTime(DateFechaRad.Text.ToString()), Procedencia.Text, null, txtPhone.Text.ToString(), TxtNaturaleza.Text.ToString(), Convert.ToInt32(RadCodString), "Registro Automatico con Plantilla", null, null, null, null, Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString(), TxtExpediente.Text.ToString(), TxtMedioRecibo.Text.ToString(), TextBox1.Text.ToString(), null, RadioButtonList1.Text.ToString(), WFAccion, Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToInt32(RadioButtonList1.Text.ToString()), null, "0");
                        remplazarValores(codHtml, RegistroCodigo);
                        // insertar radicados fuente
                        this.insertarradicadosfuente(RegistroCodigo);
                        DatosRegistro datos = new DatosRegistro();
                        datos = llenarDatosRegistro(Procedencia);
                        Session["datos"] = datos;
                        Session["registro"] = RegistroCodigo;
                        regisrtosList.Add(RegistroCodigo);
                        Session["regisrtosLista"] = regisrtosList;
                        string depUsuario = Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();
                        Session["depUsuario"] = depUsuario;
                        Session["Plantilla_CodPlantilla"] = DDLPlantilla.SelectedValue.ToString();
                        LBFirmar_Click(this, new ImageClickEventArgs(314, 540));
                        this.LblMessageBox.Text += "<br/> El registro No. " + RegistroCodigo + " se ha creado correctamente. <br/>";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sometihg", "desbloquearInterfaz();", true);

                        DDLPlantilla.SelectedValue = "-1";
                        LMessagePlantilla.Text = "";
                        this.MPEMessage.Enabled = true;
                        this.MPEMessage.Show();
                    }
                }
                if (RadioButtonList1.SelectedValue == "1")
                {
                    foreach (System.Web.UI.WebControls.ListItem Procedencia in this.ListBoxEnterar.Items)
                    {
                        RegistroCodigo = ObjReg.AddRegistro(GrpDocReg, Convert.ToDateTime(DateFechaRad.Text.ToString()), null, Procedencia.Text, txtPhone.Text.ToString(), TxtNaturaleza.Text.ToString(), Convert.ToInt32(RadCodString), "Registro Automatico con Plantilla", null, null, null, null, Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString(), TxtExpediente.Text.ToString(), TxtMedioRecibo.Text.ToString(), TextBox1.Text.ToString(), null, RadioButtonList1.Text.ToString(), WFAccion, Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToDateTime(DateFechaRad.Text.ToString()), Convert.ToInt32(RadioButtonList1.Text.ToString()), null, "0");
                        remplazarValores(codHtml, RegistroCodigo);
                        // insertar radicados fuente
                        this.insertarradicadosfuente(RegistroCodigo);
                        DatosRegistro datos = new DatosRegistro();
                        datos = llenarDatosRegistro(Procedencia);
                        Session["datos"] = datos;
                        Session["registro"] = RegistroCodigo;
                        regisrtosList.Add(RegistroCodigo);
                        Session["regisrtosLista"] = regisrtosList;
                        string depUsuario = Profile.GetProfile(Profile.UserName).CodigoDepUsuario.ToString();
                        Session["depUsuario"] = depUsuario;
                        Session["Plantilla_CodPlantilla"] = DDLPlantilla.SelectedValue.ToString();
                        LBFirmar_Click(this, new ImageClickEventArgs(314, 540));
                        this.LblMessageBox.Text += "<br/> El registro No. " + RegistroCodigo + " se ha creado correctamente. <br/>";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sometihg", "desbloquearInterfaz();", true);

                        DDLPlantilla.SelectedValue = "-1";
                        LMessagePlantilla.Text = "";
                        this.MPEMessage.Enabled = true;
                        this.MPEMessage.Show();
                    }
                }
            }
            else
            {
                Editor.Text = "";
                LMessagePlantilla.Text = "";
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "sometihg", "desbloquearInterfaz();", true);

            DDLPlantilla.SelectedValue = "-1";
            LMessagePlantilla.Text = "";
            this.MPEMessage.Enabled = true;
            this.MPEMessage.Show();
            }
        catch (Exception ex)
        {
            throw ex;
        }
    }
        protected void btnCrearRegistros_Click2(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "BloquearInterfaz", "bloquearInterfaz();", true);

            // Lógica de procesamiento pesado aquí...

            //ScriptManager.RegisterStartupScript(this, GetType(), "DesbloquearInterfaz", "desbloquearInterfaz();", true);
    
        }
}