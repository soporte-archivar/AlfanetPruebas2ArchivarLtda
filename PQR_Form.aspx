<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeFile="PQR_Form.aspx.cs" Inherits="PQR_Form" Culture="es-CO" UICulture="es-CO" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Radicacion PQR</title>
    <script language="javascript" type="text/javascript" src="PQRScripts.js">function TABLE1_onclick() {
}
    </script>
    <script type="text/javascript">
        //var counter = 0;
        //function AddFileUpload() {
        //    var div = document.createElement('DIV');
        //    div.innerHTML = '<br/><input id="file' + counter + '" name = "file' + counter + '" type="file" /><input id="Button' + counter + '" type="button" value="Remove" onclick = "RemoveFileUpload(this)" />';
        //    document.getElementById("FileUploadContainer").appendChild(div);
        //    counter++;
        //}
        var counter = 1;
        function AddFileUpload() {
            var div = document.createElement('DIV');
            div.innerHTML = '<br/><input id="file' + counter + '" name = "file' + counter + '" type="file" /><input id="Button' + counter + '" type="button" value="Remove" onclick = "RemoveFileUpload(this)" />';
            document.getElementById("FileUploadContainer").appendChild(div);
            document.getElementById("Button11").style.display = "none";
        }
        //function RemoveFileUpload(div) {
        //    document.getElementById("FileUploadContainer").removeChild(div.parentNode);
        //}
        function RemoveFileUpload(div) {
            document.getElementById("FileUploadContainer").removeChild(div.parentNode);
            document.getElementById("Button11").style.display = "inline";
        }
    </script>
    <%--Creación Scribe para actualizar captcha--%>
    <script type="text/javascript">
        function actucap() {
            obj = document.getElementById("cap");
            if (!obj) obj = window.document.all.cap;
            if (obj) {
                obj.src = "Captcha.aspx?" + Math.random();
            }
        }
    </script>
    <script src="jquery-1.4.4.js" type="text/javascript"></script>
    <script src="jquery.blockUI.js" type="text/javascript"></script>
    
        <script type="text/javascript">
            $(document).ready(function () {
                var selectValue = $("#ddlTipoDePQR").val();
                if (selectValue == "99999999") {
                    $('#correo').attr('disabled', true);
                }
                $('#check').click(function () {
                    var checkboxValues = "";
                    $('input[name="orderBox[]"]:checked').each(function () {
                        checkboxValues += $(this).val() + ",";
                    });
                    //eliminamos la última coma.
                    checkboxValues = checkboxValues.substring(0, checkboxValues.length - 1);
                    if (checkboxValues == 'datos,correo') {
                        $('#btEnviarPQR').attr('disabled', false);
                    } else if (checkboxValues == 'datos' && selectValue == "99999999") {
                        
                        $('#btEnviarPQR').attr('disabled', false);
                    }
                    else {
                        $('#btEnviarPQR').attr('disabled', true);
                    }
                });
                
            });

        </script>


        <script type="text/javascript">
            $(document).ready(function () {

                $('.prueba').click(function () {
                    $.blockUI({
                        css: {
                            border: 'none',
                            padding: '15px',
                            backgroundColor: '#000',
                            '-webkit-border-radius': '10px',
                            '-moz-border-radius': '10px',
                            opacity: .5,
                            color: '#fff'
                        }
                    });
                    // setTimeout($.unblockUI, 2000);
                });
            });
    </script>

            <script type="text/javascript">
                function check1(box) {
                    if (box.checked) {

                        document.getElementById("datos").checked = false

                    }
                }
                function check2(box) {
                    if (box.checked) {
                        document.getElementById("NOAutorizacion").checked = false

                    }
                }
</script>
    <script language="javascript" type="text/javascript">
        function numbersonly(myfield, e, dec) {
            var key;
            var keychar;

            if (window.event)
                key = window.event.keyCode;
            else if (e)
                key = e.which;
            else
                return true;
            keychar = String.fromCharCode(key);

            // control keys
            if ((key == null) || (key == 0) || (key == 8) ||
            (key == 9) || (key == 13) || (key == 27))
                return true;

                // numbers
            else if ((("0123456789").indexOf(keychar) > -1))
                return true;

                // decimal point jump
            else if (dec && (keychar == ".")) {
                myfield.form.elements[dec].focus();
                return false;
            }
            else
                return false;
        }
    </script>
    <style type="text/css">
        body {
            font-family: Calibri,Arial, Helvetica, sans-serif;
            color: #555;
            text-align: justify;
            line-height: normal;
            padding: 0% 2% 0% 2%;
            max-width: 100%;
            background-color: #ECE4D5;
        }

        .cajasTexto {
            width: 99%;
            color: black;
            border: 1px solid #BEBEBE;
            background-color: #FFF8E9;
        }

        .WaterMarkedTextBox {
            width: 99%;
            height: 100%;
            border: 1px solid #BEBEBE;
            color: gray;
            border: 1px solid #BEBEBE;
            background-color: #FFF8E9;
            /*font-size: 8pt;
            text-align: center;*/
        }

        .WaterMarkedDDL {
            /*border: 1px solid #BEBEBE;*/
            color: gray;
            width: 100%;
            border: 1px solid #BEBEBE;
            background-color: #FFF8E9;
            /*font-size: 8pt;
            text-align: center;*/
        }

        .modalBackground {
            background-color: Gray;
            filter: alpha(opacity=50);
            opacity: 0.50;
        }

        .auto-style4 {
            width: 25%;
            height: 44px;
        }
        .auto-style10 {
            width: 30%;
            height: 44px;
        }
        .auto-style11 {
            width: 45%;
            height: 44px;
        }
        
        input {
            background-color: #f90;
            border: medium none;
            border-radius: 5px;
            padding: 3px;
        }

        #head {
            border-bottom: dashed 1px #FF9900;
            padding-bottom: 5px;
            background-repeat: no-repeat;
            background-position: right center;
        }

        #titulo {
            font-size: 24px;
            color: #FFF;
            text-align: center;
            border-top-width: 1px;
            border-top-style: dashed;
            border-top-color: #FBB608;
            background-color: #F49A22;
        }

        #content {
            width: 90%;
            margin-right: auto;
            margin-left: auto;
            background-color: #FFF;
            padding: 5px;
            border-radius: 10px;
        }
        .auto-style14 {
            width: 30%;
        }
    </style>
</head>
<body id="content">
    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
	
	 <tr>
                <td style="vertical-align: top; text-align: center">
                    <br />
                        <img style="background-position: center center; border: 'none'; padding: inherit; margin: inherit; background-image: url('AlfaNetImagen/Logo/LogoEmpresaPQR.jpg'); " height="120" width="600" Border="1"/>
						
                    <br />
                </td>
            </tr>
	
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <cc1:ModalPopupExtender ID="ModalPopupAjax" runat="server" PopupControlID="pnMsgPopup"
                OkControlID="btMsgPupup" TargetControlID="pnMsgPopup" BackgroundCssClass="modalBackground">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="pnMsgPopup" runat="server" Width="560px" BackColor="ControlLightLight" Visible="true" Style="display: none;">
                <table width="100%" id="TABLE1" onclick="return TABLE1_onclick()">
                    <tr>
                        <td colspan="2" style="background-color: #ff8000;">
                            <span style="color: White; font-weight: bold;">
                                <asp:Label ID="etMsgPopupTitulo" runat="server" ForeColor="Black" Text="" Font-Size="14pt"></asp:Label></span></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="etMsgPopuMensaje" runat="server" ForeColor="Black" Font-Size="12pt"></asp:Label></td>
                    </tr>
                    <tr>
                        <td width="50%">&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btMsgPupup" Text="Aceptar" runat="server" CausesValidation="False" UseSubmitBehavior="False" /></td>
                    </tr>
                </table>
            </asp:Panel>
            <%--<img src="AlfaNetImagen/logoFondo.gif" alt="Logo Entidad" />--%>
        </div>
        <p>
            Los campos marcados con <span style="color: red;">*</span> son obligatorios.&nbsp;
        </p>
        <table width="100%">
            <tr>
                <td></td>
                <td>
                    <p style="color: Red; font-size: small; width: 100%" id="infoLabelNui" name="infoLabel" runat="server">
                        Recuerde que su solicitud debe utilizar siempre la misma identificación bien sea esta Cédula, Nit o Pasaporte.
                    </p>
                </td>
            </tr>
            <tr id="tr_tidentificacion" runat="server">
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etTipoIdentificacion" runat="server" Text="Tipo de identificación" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 45%">
                    <asp:DropDownList ID="ddlTipoDeIdentificacion" runat="server" CssClass="WaterMarkedDDL" AutoPostBack="True" OnSelectedIndexChanged="ddlTipoDeIdentificacion_SelectedIndexChanged">
                        <asp:ListItem>---Seleccione un Tipo de Documento---</asp:ListItem>
						<asp:ListItem Value="sa">Solicitud Anonima</asp:ListItem>
                        <asp:ListItem Value="cc">C&#233;dula de Ciudadan&#237;a</asp:ListItem>
                        <asp:ListItem Value="ti">Tarjeta de Identidad</asp:ListItem>
                        <asp:ListItem Value="ce">C&#233;dula de Extranjer&#237;a</asp:ListItem>
                        <asp:ListItem Value="nit">NIT</asp:ListItem>
                        <asp:ListItem Value="pas">Pasaporte</asp:ListItem>
                    </asp:DropDownList></td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator Font-Size="Small" ID="cvTipoIdentificacion" runat="server" ControlToValidate="ddlTipoDeIdentificacion"
                        Display="Dynamic" ErrorMessage="Debe seleccionar un tipo de identificación"> * Debe Seleccionar un Tipo de Identificacion</asp:RequiredFieldValidator></td>
            </tr>
            <tr id="tr_identificacion" runat="server">
                <td align="left" style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etNumIdentificacion" runat="server" Text="número de identificación" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 45%">
                    <asp:UpdatePanel ID="UpdPnIdentificacion" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="ctIdentificacion" runat="server" onKeyPress="return numbersonly(this, event)" CssClass="cajasTexto" AutoPostBack="True" OnTextChanged="ctIdentificacion_TextChanged"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="TBWIdentificacion" runat="server" TargetControlID="ctIdentificacion" WatermarkText="Numero de Identificacion" WatermarkCssClass="WaterMarkedTextBox">
                            </cc1:TextBoxWatermarkExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator Font-Size="Small" ID="cvIdentificacion" runat="server"
                        ControlToValidate="ctIdentificacion" Display="Dynamic" ErrorMessage="El número de identificación es requerido"> * El numero de identificacion es requerido</asp:RequiredFieldValidator></td>
            </tr>
            <tr id="tr_procedencia" runat="server">
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etNombreProcedencia" runat="server" Text="Nombres y Apellidos" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 45%">
                    <asp:UpdatePanel ID="UpdPnNombreProcedencia" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="ctNombreProcedencia" runat="server" CssClass="cajasTexto"></asp:TextBox><cc1:TextBoxWatermarkExtender ID="TBWNombreProcedencia" runat="server" WatermarkText="Nombres y Apellidos" TargetControlID="ctNombreProcedencia" WatermarkCssClass="WaterMarkedTextBox">
                            </cc1:TextBoxWatermarkExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator Font-Size="Small" ID="cvNombreProcedencia" runat="server"
                        ControlToValidate="ctNombreProcedencia" Display="Dynamic" ErrorMessage="Su nombre es requerido"> * Su nombre es requerido</asp:RequiredFieldValidator></td>
            </tr>
            <tr id="tr_email" runat="server">
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etEmail" runat="server" Text="Email" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 45%">
                    <asp:UpdatePanel ID="UpdPnEmail" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="ctEmail" runat="server" CssClass="cajasTexto" ValidationGroup="cvEmail"></asp:TextBox><cc1:TextBoxWatermarkExtender ID="TBWEmail" runat="server" TargetControlID="ctEmail" WatermarkText="Email, Ejemplo: email@correo.com" WatermarkCssClass="WaterMarkedTextBox">
                            </cc1:TextBoxWatermarkExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator Font-Size="Small" ID="cvEmail" runat="server"
                        ControlToValidate="ctEmail" Display="Dynamic" ErrorMessage="El correo electronico es requerido"> * El correo electronico es Requerido</asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="cvfEmail" runat="server" ControlToValidate="ctEmail"
                            Display="Dynamic" ErrorMessage="El correo electronico suministrado no es valido"
                            SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Font-Size="Small"> * El correo electronico suministrado no es valido</asp:RegularExpressionValidator></td>
            </tr>
            <tr id="tr_telefono" runat="server">
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etTelefono" runat="server" Text="Teléfono" EnableViewState="False" Font-Size="9pt"></asp:Label>
                </td>
                <td style="width: 45%">
                    <asp:UpdatePanel ID="UpdPnTelefono" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="ctTelefono" runat="server" CssClass="cajasTexto" CausesValidation="True"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                ID="TBWTelefono" runat="server" TargetControlID="ctTelefono" WatermarkText="Numero Telefonico" WatermarkCssClass="WaterMarkedTextBox">
                            </cc1:TextBoxWatermarkExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator Font-Size="Small" ID="cvTelefono" runat="server"
                        ControlToValidate="ctTelefono" Display="Dynamic" ErrorMessage="El Teléfono es requerido">* El telefono es requerido</asp:RequiredFieldValidator></td>
            </tr>
            <tr  id="tr_direccion" runat="server">
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etDireccion" runat="server" Text="Dirección" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 45%">
                    <asp:UpdatePanel ID="UpdPnDireccion" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="ctDireccion" runat="server" CssClass="cajasTexto"></asp:TextBox><cc1:TextBoxWatermarkExtender
                                ID="TBWDireccion" runat="server" TargetControlID="ctDireccion" WatermarkText="Direccion de Correspondencia" WatermarkCssClass="WaterMarkedTextBox">
                            </cc1:TextBoxWatermarkExtender>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator Font-Size="Small" ID="cvDireccion" runat="server"
                        ControlToValidate="ctDireccion" Display="Dynamic" ErrorMessage="La Dirección es requerida"> * La Direccion es requerida</asp:RequiredFieldValidator></td>
            </tr>
            <tr id="tr_pais" runat="server">
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etPais" runat="server" Text="País" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 45%">
                    <asp:DropDownList ID="ddlPais" runat="server" CssClass="WaterMarkedDDL" AutoPostBack="True" OnSelectedIndexChanged="ddlPais_SelectedIndexChanged">
                    </asp:DropDownList>
                    <cc1:CascadingDropDown ID="cddPais" runat="server" ServicePath="CascadinDropDown.asmx" ServiceMethod="GetPais" Category="Pais" PromptText="Seleccione un País ..." SelectedValue="170"
                        TargetControlID="ddlPais">
                    </cc1:CascadingDropDown>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator ID="cvPais" runat="server"
                        ControlToValidate="ddlPais" Display="Dynamic" ErrorMessage="Seleccione un País" Font-Size="Small"> * Seleccione un Pais</asp:RequiredFieldValidator></td>
            </tr>
            <tr id="tr_departamento" runat="server">
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etDepartamento" runat="server" Text="Departamento" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 45%">
                    <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="WaterMarkedDDL">
                    </asp:DropDownList><cc1:CascadingDropDown ID="cddDepartamento" runat="server" TargetControlID="ddlDepartamento" ServicePath="CascadinDropDown.asmx" ServiceMethod="GetDepartamentoByPais " ParentControlID="DDLPais" Category="Departamento" PromptText="Seleccione un Departamento..." SelectedValue="11">
                    </cc1:CascadingDropDown>
                    <asp:TextBox ID="ctDepartamento" runat="server" Visible="False" CssClass="cajasTexto"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="TBWDepartamento" runat="server" TargetControlID="ctDepartamento" WatermarkCssClass="WaterMarkedTextBox" WatermarkText="Ingrese la Provincia">
                    </cc1:TextBoxWatermarkExtender>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator ID="cvDepartamento" runat="server"
                        ControlToValidate="ddlDepartamento" Display="Dynamic" ErrorMessage="Seleccione un departamento" Font-Size="Small"> * Seleccione un departamento</asp:RequiredFieldValidator></td>
            </tr>
            <tr id="tr_ciudad" runat="server">
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etCiudad" runat="server" Text="Ciudad" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 45%">
                    <asp:DropDownList ID="ddlCiudad" runat="server" CssClass="WaterMarkedDDL">
                    </asp:DropDownList><cc1:CascadingDropDown ID="cddCiudad" runat="server" TargetControlID="ddlCiudad" ServicePath="CascadinDropDown.asmx" ServiceMethod="GetCiudadByDepartamento" ParentControlID="DDLDepartamento" Category="Ciudad" PromptText="Seleccione Ciudad..." SelectedValue="11001">
                    </cc1:CascadingDropDown>
                    <asp:TextBox ID="ctCiudad" runat="server" Visible="False" CssClass="cajasTexto"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="TBWCiudad" runat="server" TargetControlID="ctCiudad" WatermarkCssClass="WaterMarkedTextBox" WatermarkText="Ingrese la Ciudad de Residencia">
                    </cc1:TextBoxWatermarkExtender>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator ID="cvCiudad" runat="server"
                        ControlToValidate="ddlCiudad" Display="Dynamic" ErrorMessage="Seleccione una ciudad" Font-Size="Small"> * Seleccione una Ciudad</asp:RequiredFieldValidator></td>
            </tr>
        </table>
        <table width="75%">
            <tr>
                <td>
                    <p style="color: Red; font-size: small; width: 100%" id="infoLabel" name="infoLabel" runat="server">
                        La respuesta a su solicitud le será enviada a través de la Dirección de 
                            correspondencia o al correo electrónico, por lo tanto verifique que los 
                            datos se incluyeron correctamente. Si su solicitud es anónima, la respuesta 
                            será publicada mediante AVISO en la página inicial de PQRDS, que usted podrá 
                            consultar suministrando el número de radicado que le generará el sistema 
                            automáticamente al dar clic en 'Enviar', por favor tome nota de él y consérvelo.
                    </p>
                </td>
            </tr>
        </table>
        <table width="100%">
            <%--<tr>
                <td class="auto-style12"><span style="color: red;">*</span><asp:Label ID="etTipoDePQR" runat="server" Text="Tipo de Solicitud" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td class="auto-style13">
                    <asp:DropDownList ID="ddlTipoDePQR" runat="server" CssClass="WaterMarkedDDL">
                    </asp:DropDownList>
                    <cc1:CascadingDropDown ID="CCDTipoDePQR" runat="server" TargetControlID="ddlTipoDePQR" ServicePath="CascadinDropDown.asmx" ServiceMethod="GetNaturalezaByPQR" PromptText="Seleccione el tipo de Solicitud" Category="Naturaleza">
                    </cc1:CascadingDropDown>
                </td>
                <td class="auto-style7">
                    <asp:RequiredFieldValidator ID="cvTipoPQR" runat="server"
                        ControlToValidate="ddlTipoDePQR" Display="Dynamic" ErrorMessage="Seleccione un tipo de solicitud" Font-Size="Small"> * Seleccione un tipo de Solicitud</asp:RequiredFieldValidator></td>                
            </tr>--%>
            <%--<tr>
                <td class="auto-style10"><span style="color: red;">*</span><asp:Label ID="etTipoDePQRTram" runat="server" Text="Tipo de Tramite" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td class="auto-style11">
                    <asp:DropDownList ID="ddlTipoDePQRTram" runat="server" CssClass="WaterMarkedDDL" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoDePQRTram_SelectedIndexChanged" >
                    </asp:DropDownList>
                    <cc1:CascadingDropDown ID="CCDTipoDePQRTram" runat="server" TargetControlID="ddlTipoDePQRTram" ParentControlID="ddlTipoDePQR" ServicePath="CascadinDropDown.asmx" ServiceMethod="GetNaturalezaByPQRTram" PromptText="Seleccione el tipo de Solicitud" Category="Naturaleza">
                    </cc1:CascadingDropDown>
                </td>
                <td class="auto-style4">
                    <asp:RequiredFieldValidator ID="cvTipoPQRTram" runat="server"
                        ControlToValidate="ddlTipoDePQR" Display="Dynamic" ErrorMessage="Seleccione un tipo de tramite" Font-Size="Small"> * Seleccione un tipo de Solicitud</asp:RequiredFieldValidator></td>
            </tr>--%>
                        <tr id="tr_tipo_pqr" runat="server">
                <td style="height: 74px; width: 30%;"><span style="color: red;">*</span><asp:Label ID="etTipoDePQR" runat="server" Text="Tipo de Solicitud" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="height: 74px; width: 45%;">
                    <asp:DropDownList ID="ddlTipoDePQR" runat="server" CssClass="WaterMarkedDDL" OnSelectedIndexChanged="ddlTipoDePQR_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td style="height: 74px; width: 35%;">
                    <asp:RequiredFieldValidator ID="cvTipoPQR" runat="server"
                        ControlToValidate="ddlTipoDePQR" Display="Dynamic" ErrorMessage="Seleccione un tipo de solicitud" Font-Size="Small"> * Seleccione un tipo de Solicitud</asp:RequiredFieldValidator></td>
            </tr>
            <tr id="tr_tipo_tramite" runat="server">
                <td class="auto-style10"><span style="color: red;">*</span><asp:Label ID="etTipoDePQRTram" runat="server" Text="Tipo de Tramite" Font-Size="9pt"></asp:Label></td>
                <td class="auto-style11">

                    <asp:DropDownList ID="ddlTipoDePQRTram" runat="server" CssClass="WaterMarkedDDL"  OnSelectedIndexChanged="ddlTipoDePQRTram_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList>
                    <br />
                    <br />
                </td>
                <td class="auto-style4">
                    <asp:RequiredFieldValidator ID="cvTipoPQRTram" runat="server"
                        ControlToValidate="ddlTipoDePQR" Display="Dynamic" ErrorMessage="Seleccione un tipo de tramite" Font-Size="Small"> * Seleccione un tipo de Tramite</asp:RequiredFieldValidator>
                    <br />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width: 30%"><span style="color: red;">*</span><asp:Label ID="etDetalle" runat="server" Text="Asunto" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="text-align: left; font-size: 12px; width: 45%;">
                    <asp:TextBox ID="ctDetalle" runat="server" TextMode="MultiLine" CssClass="cajasTexto" MaxLength="300" Rows="6" OnTextChanged="ctDetalle_TextChanged" Height="58px"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender
                        ID="TBWDetalle" runat="server" TargetControlID="ctDetalle" WatermarkText="Escriba el detalle de su Peticion, Queja o Reclamo" WatermarkCssClass="WaterMarkedTextBox">
                    </cc1:TextBoxWatermarkExtender>
                    <span style="font-size: 12px">Puede ingresar hasta un máximo de 300 caracteres.</span>
                    <span>Caracteres ingresados: </span>
                    <asp:Label ID="etMaximoCaracteres" runat="server" Text="0"></asp:Label>, m<span>áximo
                            300 caracteres</span>
                </td>
                <td style="width: 35%">
                    <asp:RequiredFieldValidator ID="cvDetalle" runat="server"
                        ControlToValidate="ctDetalle" Display="Dynamic" ErrorMessage="Debe indicar el detalle de la solicitud" Font-Size="Small"> * Debe indicar el detalle de la solicitud</asp:RequiredFieldValidator></td>
            </tr>
        </table>
        <table width="100%">
            <tr id="adjuntar_doc1" runat="server" style="display: none">
                <td class="auto-style14">
                    <asp:Label ID="Label1" runat="server" Text="Adjunto" EnableViewState="False" Font-Size="9pt"></asp:Label>
                </td>
                <td style="width: 40%">
                    <asp:Label ID="Label2" runat="server" Text="Verifique que cumple con todos los documentos obligatorios (<b>*</b>) y adjuntarlos, en caso contrario. <b>NO SE REALIZA EL TRAMITE</b>" ForeColor="Red" Visible="false" ></asp:Label>
                    <asp:Table ID="Table2" runat="server" Border="1" BorderColor="Gray" BorderStyle="None" BorderWidth="1"></asp:Table>
                    <asp:Label ID="Label3" runat="server" Text="Formatos válidos: Imagen (jpg,jpeg,gif,png), Texto (doc,docx,pdf),  compresión de archivos  (zip, rar)" Font-Size="Smaller" Width="100%"></asp:Label>
                    <br />
                </td>
                <td style="width: 30%"></td>
            </tr>
            <tr id="trAdjuntar_doc" runat="server">
                <td class="auto-style14">
                    <asp:Label ID="etAdjunto" runat="server" Text="Adjunto" EnableViewState="False" Font-Size="9pt"></asp:Label></td>
                <td style="width: 40%">
                    <span style="font-family: Arial; font-size: smaller">Cargar Adjuntos</span>&nbsp;&nbsp;
                    <div id="FileUploadContainer" runat="server">
                        <!--FileUpload Controls will be added here -->
                    </div>
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="Button11" type="button" value="Adicionar Documentos" style="font-family: Arial; font-size: smaller" onclick="AddFileUpload()" /><br />
                    <br />
                    <br />
                    <asp:Label ID="etFormaArchivosValidos" runat="server" Text="Formatos válidos: Imagen (jpg,jpeg,gif,png), Texto (doc,docx,pdf),  compresión de archivos  (zip, rar)" Font-Size="Smaller" Width="100%"></asp:Label>
                    <br />
                </td>
                <td style="width: 30%"></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="check">                            
                        <span style="color: red;">*</span><input type="checkbox" onclick="check2(this)" name="orderBox[]" id="datos" value="datos" />
                        &nbsp;<asp:Label ID="LblAutDatos" Font-Bold="true" runat="server" Text="El funcionario, contratista y/o proveedor, autoriza  al fondo de Bienestar Social de la CGR a recolectar, procesar y almacenar  la Información suministrada en el presente documento conforme a la Política de tratamiento de datos de la entidad disponible en el link:"></asp:Label>
                        <asp:LinkButton ID="Lkbpaginaweb" CausesValidation="False" OnClick="Linkpaginaweb_Click" runat="server"> Política de tratamiento de datos </asp:LinkButton>
                        <br />
                        <br />
                        <span style="color: red;">*</span><input type="checkbox" id="correo" name="orderBox[]" value="correo" />
                        &nbsp;<asp:Label ID="LblAutCorreo" Font-Bold="true" runat="server" Text="Certifico que el correo electrónico ingresado en mis datos personales se encuentra vigente, de igual manera autorizo al Fondo de Bienestar Social de la CGR, para el envío de la respuesta a mi solicitud por este medio."></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="auto-style14"></td>
                <td style="width: 45%">
                    <%--Creación Captcha John Vela 11/08/2016 para evitar Robot Informaticos--%>
                    <img alt="" src="Captcha.aspx" id="cap" />
                    <a href="javascript:actucap();" title="Actualizar Captcha" style="background-position: center center; padding: inherit; margin: inherit; background-image: url('AlfaNetImagen/ToolBar/actualizar.png'); color: #FFFFFF; text-decoration: none; text-transform: none; font-variant: normal; font-style: normal; font-weight: 100; font-size: -66px; font-family: Arial, Helvetica, sans-serif; position: absolute; z-index: auto; height: 65px; width: 74px; background-repeat: no-repeat; background-attachment: scroll;">.....</a>
                    <br />
                    <asp:Label ID="LblCaptcha" runat="server" Text="Favor digite el Texto de la Imagen" Font-Size="9pt"></asp:Label>
                    <asp:TextBox ID="TxtBCaptcha" runat="server"></asp:TextBox>
                    &nbsp;&nbsp;<asp:Label ID="LblCaptchaError" runat="server"
                        AssociatedControlID="TxtBCaptcha" Font-Size="9pt" ForeColor="Red" Width="50%"></asp:Label>
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btEnviarPQR" runat="server" Text="Enviar" OnClick="btEnviarPQR_Click" CssClass="prueba" Enabled="false"/>
                    &nbsp; 
                        <asp:Button ID="btLimpiarFormulario" runat="server"
                            OnClick="btLimpiarFormulario_Click" Text="Limpiar" CausesValidation="False" UseSubmitBehavior="False" />
                    &nbsp; 
                        <input id="Button1" onclick="parent.location.href = 'http://www.fbscgr.gov.co'" type="button" value="Volver al Inicio" /></td>
                <td style="width: 25%"></td>
            </tr>
        </table>
        <p style="color: #24698e;">
            &nbsp;
        </p>
        <p style="color: #24698e; font-size: small; text-align: justify;">
            El Fondo de Bienestar Social de la CGR, garantiza la confidencialidad 
    de los datos personales facilitados por los usuarios y su tratamiento de acuerdo con la legislación sobre 
    protección de datos de carácter personal; siendo de uso exclusivo de la entidad y trasladados a terceros con 
    autorización previa del usuario.
        </p>
        <p style="font-size: small; text-align: justify;">
            Mayor Información:
            <br />
        </p>
        <ul class="info_entidad"
            style="margin: 0px; padding: 0px 19.1875px 0px 9.59375px; list-style-type: none; text-align: left; width: 441.59375px; font-size: 0.75em; font-weight: bold; color: rgb(0, 0, 0); font-family: trebuchet, 'Trebuchet MS', sans-serif; font-style: normal; font-variant: normal; letter-spacing: normal; line-height: normal; orphans: auto; text-indent: 0px; text-transform: none; white-space: normal; widows: 1; word-spacing: 0px; -webkit-text-stroke-width: 0px;">
            <li class="nombre_sitio"
                style="margin: 0px; padding: 0px; float: left; width: 441.59375px; color: rgb(6, 50, 95); font-size: 1.1em; font-weight: bold; background: transparent;">Fondo de Bienestar Social</li>
            <li style="margin: 0px; padding: 0px; float: left; width: 441.59375px; color: rgb(88, 88, 89); background: transparent;">Carrera 69 No. 44-35 Piso 4</li>
            <li style="margin: 0px; padding: 0px; float: left; width: 441.59375px; color: rgb(88, 88, 89); background: transparent;">PBX +57601377987  +576013532760</li>
            <li style="margin: 0px; padding: 0px; float: left; width: 441.59375px; color: rgb(88, 88, 89); background: transparent;">Horario de Atención: 8:00 am - 5:00 pm</li>
            <li style="margin: 0px; padding: 0px; float: left; width: 441.59375px; color: rgb(88, 88, 89); background: transparent;">Horario de Correspondencia: 8:00 am - 4:00 pm</li>
            <li style="margin: 0px; padding: 0px; float: left; width: 441.59375px; color: rgb(88, 88, 89); background: transparent;">
                <a class="webmaster" href="mailto:fondobienestar@fbscgr.gov.co"
                    style="margin: 0px; padding: 0px; color: rgb(6, 50, 95);">fondobienestar@fbscgr.gov.co</a></li>
        </ul>
    </form>
</body>
</html>
