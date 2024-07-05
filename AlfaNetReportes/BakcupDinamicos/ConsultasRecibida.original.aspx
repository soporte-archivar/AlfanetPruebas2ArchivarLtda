﻿<%@ Page Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" CodeFile="ConsultasRecibida.aspx.cs" Inherits="AlfaNetReportes_Dinamicos_ConsultasRecibida" Title="Consulta Dinamica Recibido" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v9.1" Namespace="DevExpress.Web.ASPxPivotGrid"
    TagPrefix="dxwpg" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>

<%@ Register Assembly="DevExpress.XtraCharts.v9.1.Web, Version=9.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts.Web" TagPrefix="dxchartsui" %>
<%@ Register Assembly="DevExpress.XtraCharts.v9.1, Version=9.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.XtraCharts" TagPrefix="cc2" %>
<%--
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v9.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v9.1" Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dxwpg" %>--%>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v9.1, Version=9.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dxwpg" %>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v9.1.Export, Version=9.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraPivotGrid.Web" tagprefix="dxpgw" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register assembly="DevExpress.Web.ASPxEditors.v9.1, Version=9.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxEditors" tagprefix="dxe" %>
<%@ Register assembly="DevExpress.Web.v9.1, Version=9.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dxp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<%--<style type="text/css">
        .style1
        {
            width: 32%;
        }

.dxeButtonEdit_Glass
{
    border: solid 1px #7EACB1;
}
.dxeButtonEdit_Glass 
{
	background-color: white;
	width: 170px;
}
.dxeEditArea_Glass
{
	font-family: Tahoma;
	font-size: 9pt;
	border: 1px solid #A0A0A0;
}
.dxeButtonEditButton_Glass
{
	background-image: url('App_Themes/Glass/Editors/edtButtonEditButtonBack.gif');	
    background-repeat:repeat-x;
    background-position:50% top;
	border: solid 1px #4986a2;    
	background-color: #45829F;	
	padding: 4px 1px;
}

.dxeButtonEditButton_Glass, .dxeCalendarButton_Glass,
.dxeSpinIncButton_Glass, .dxeSpinDecButton_Glass,
.dxeSpinLargeIncButton_Glass, .dxeSpinLargeDecButton_Glass
{	
	background-color: #f0f0f0;	
	vertical-align: middle;
	cursor: pointer;
	cursor: hand;
} 
        </style>--%> 
    <asp:UpdateProgress id="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1"
        DisplayAfter="100">
        <progresstemplate>
Loading... <asp:Image id="Image1" runat="server" ImageUrl="~/App_Themes/Office2003 Blue/Web/Loading.gif" __designer:wfdid="w3"></asp:Image>
</progresstemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <contenttemplate>
<asp:CheckBox id="CheckBox1" runat="server" Width="243px" Text="Graficar los Valores de las Filas" __designer:wfdid="w27" OnCheckedChanged="CheckBox1_CheckedChanged" AutoPostBack="True">
</asp:CheckBox> <dxpgw:ASPxPivotGridExporter id="ASPxPivotGridExporter1" runat="server" __designer:wfdid="w28" ASPxPivotGridID="ConsultaRadicados">
    </dxpgw:ASPxPivotGridExporter><DIV style="TEXT-ALIGN: center"><TABLE class="style1"><TBODY><TR><TD style="WIDTH: 173px"><dxe:ASPxComboBox id="listExportFormat" runat="server" __designer:wfdid="w29" ImageFolder="~/App_Themes/Office2003 Blue/{0}/" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" ValueType="System.String"><Items>
<dxe:ListEditItem Text="Pdf" Value="0"></dxe:ListEditItem>
<dxe:ListEditItem Text="Excel" Value="1"></dxe:ListEditItem>
<dxe:ListEditItem Text="Mht" Value="2"></dxe:ListEditItem>
<dxe:ListEditItem Text="Rtf" Value="3"></dxe:ListEditItem>
<dxe:ListEditItem Text="Text" Value="4"></dxe:ListEditItem>
<dxe:ListEditItem Text="Html" Value="5"></dxe:ListEditItem>
</Items>

<ButtonStyle Width="13px"></ButtonStyle>
</dxe:ASPxComboBox> </TD><TD><dxe:ASPxButton id="ButtonOpen" onclick="ButtonOpen_Click" runat="server" Text="Abrir" __designer:wfdid="w30" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue"></dxe:ASPxButton> </TD><TD><dxe:ASPxButton id="ButtonSaveAs" onclick="ButtonSaveAs_Click" runat="server" Text="Guardar" __designer:wfdid="w31" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue"></dxe:ASPxButton> </TD><TD style="WIDTH: 3px"></TD><TD style="WIDTH: 206px">Grupos Naturalezas</TD><TD style="WIDTH: 3px"><dxe:ASPxComboBox id="ASPxComboBox1" runat="server" __designer:wfdid="w2" AutoPostBack="True" ImageFolder="~/App_Themes/Office2003 Blue/{0}/" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" ValueType="System.String" OnSelectedIndexChanged="ASPxComboBox1_SelectedIndexChanged" OnDataBound="ASPxComboBox1_DataBound" SelectedIndex="0"><Items>
<dxe:ListEditItem Text="TODAS" Value="TODAS"></dxe:ListEditItem>
<dxe:ListEditItem Text="PQR" Value="PQR"></dxe:ListEditItem>
<dxe:ListEditItem Text="TRAMITES" Value="TRA"></dxe:ListEditItem>
<dxe:ListEditItem Text="DERECHOS DE PETICION" Value="DERE"></dxe:ListEditItem>
</Items>

<ButtonStyle Width="13px"></ButtonStyle>
</dxe:ASPxComboBox> </TD><TD style="WIDTH: 3px">&nbsp;</TD><TD style="WIDTH: 1px">Año&nbsp;a&nbsp;consultar </TD><TD style="WIDTH: 3px"><dxe:ASPxComboBox id="ASPxComboBox2" runat="server" DataSourceID="SqlDataSource1" __designer:wfdid="w1" AutoPostBack="True" ImageFolder="~/App_Themes/Office2003 Blue/{0}/" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css" CssPostfix="Office2003_Blue" ValueType="System.String" OnSelectedIndexChanged="ASPxComboBox2_SelectedIndexChanged" OnDataBound="ASPxComboBox2_DataBound" TextField="Expr1" ValueField="Expr1">
<ButtonStyle Width="13px">
</ButtonStyle>
</dxe:ASPxComboBox> <asp:SqlDataSource id="SqlDataSource1" runat="server" __designer:wfdid="w57" ConnectionString="<%$ ConnectionStrings:ConnStrSQLServer %>" SelectCommand="SELECT DISTINCT YEAR(WFMovimientoFecha) AS Expr1 FROM Radicado ORDER BY Expr1"></asp:SqlDataSource></TD></TR></TBODY></TABLE></DIV><TABLE><TBODY><TR><TD style="WIDTH: 621px"><dxwpg:ASPxPivotGrid id="ConsultaRadicados" runat="server" Width="900px" DataSourceID="AlfaWeb" __designer:wfdid="w32" OnPreRender="ConsultaRadicados_PreRender" EnableCallBacks="False"><Fields>
<dxwpg:PivotGridField FieldName="RadicadoCodigo" ID="fieldRadicadoCodigo" Area="FilterArea" AreaIndex="4" Caption="Radicado" SummaryType="Count"></dxwpg:PivotGridField>
<dxwpg:PivotGridField FieldName="WFMovimientoFecha" ID="fieldWFMovimientoFecha" Area="ColumnArea" AreaIndex="0" GroupInterval="DateMonth" UnboundFieldName="fieldWFMovimientoFecha" Caption="Fecha Radicado"></dxwpg:PivotGridField>
<dxwpg:PivotGridField FieldName="ProcedenciaNombre" ID="fieldProcedenciaNombre" Area="FilterArea" AreaIndex="1" Caption="Procedencia"></dxwpg:PivotGridField>
<dxwpg:PivotGridField FieldName="NaturalezaNombre" ID="fieldNaturalezaNombre" Area="RowArea" AreaIndex="0" Caption="Naturaleza"></dxwpg:PivotGridField>
<dxwpg:PivotGridField FieldName="MedioNombre" ID="fieldMedioNombre" Area="FilterArea" AreaIndex="3" Caption="Medio"></dxwpg:PivotGridField>
<dxwpg:PivotGridField FieldName="ExpedienteNombre" ID="fieldExpedienteNombre" Area="FilterArea" AreaIndex="0" Caption="Expediente"></dxwpg:PivotGridField>
<dxwpg:PivotGridField FieldName="DependenciaNombre" ID="fieldDependenciaNombre" Area="FilterArea" AreaIndex="2" Caption="Dependencia"></dxwpg:PivotGridField>
<dxwpg:PivotGridField FieldName="RadicadoCodigo" ID="fielRadicadoCodigoPivot" Area="DataArea" AreaIndex="0" Caption="Radicado Pivot" SummaryType="Count"></dxwpg:PivotGridField>
<dxwpg:PivotGridField FieldName="WFMovimientoFecha" ID="fieldYearRadicado" Area="ColumnArea" AreaIndex="0" Visible="False" GroupInterval="DateYear" UnboundFieldName="field" Caption="A&#241;o Radicado"></dxwpg:PivotGridField>
</Fields>

<Styles CssPostfix="Office2003_Blue" CssFilePath="~/App_Themes/Office2003 Blue/{0}/styles.css">
<CustomizationFieldsHeaderStyle>
<Paddings PaddingLeft="12px" PaddingRight="6px"></Paddings>
</CustomizationFieldsHeaderStyle>
</Styles>

<OptionsView ShowGrandTotalsForSingleValues="True"></OptionsView>

<OptionsChartDataSource ChartDataVertical="False" ShowColumnCustomTotals="False" ShowColumnGrandTotals="False" ShowColumnTotals="False" ShowRowCustomTotals="False" ShowRowGrandTotals="False" ShowRowTotals="False"></OptionsChartDataSource>

<Images ImageFolder="~/App_Themes/Office2003 Blue/{0}/">
<FieldValueCollapsed Height="12px" Width="11px" Url="~/App_Themes/Office2003 Blue/PivotGrid/pgCollapsedButton.png"></FieldValueCollapsed>

<FieldValueExpanded Height="12px" Width="11px" Url="~/App_Themes/Office2003 Blue/PivotGrid/pgExpandedButton.png"></FieldValueExpanded>

<HeaderSortDown Height="8px" Width="7px" Url="~/App_Themes/Office2003 Blue/PivotGrid/pgHeaderSortDown.png"></HeaderSortDown>

<HeaderSortUp Height="8px" Width="7px" Url="~/App_Themes/Office2003 Blue/PivotGrid/pgHeaderSortUp.png"></HeaderSortUp>

<SortByColumn Height="7px" Width="11px" Url="~/App_Themes/Office2003 Blue/PivotGrid/pgSortByColumn.png"></SortByColumn>
</Images>
</dxwpg:ASPxPivotGrid></TD></TR></TBODY></TABLE><asp:SqlDataSource id="AlfaWeb" runat="server" __designer:wfdid="w33" ConnectionString="<%$ ConnectionStrings:ConnStrSQLServer %>" SelectCommand="Radicado_ConsultasRadicadoDinamico" SelectCommandType="StoredProcedure"><SelectParameters>
<asp:Parameter DbType="DateTime" DefaultValue="01/01/2010" Name="WFMovimientoFecha"></asp:Parameter>
<asp:Parameter DbType="DateTime" DefaultValue="31/12/2010" Name="WFMovimientoFecha1"></asp:Parameter>
</SelectParameters>
</asp:SqlDataSource> <dxchartsui:WebChartControl style="TEXT-ALIGN: center" id="WebChartControl1" runat="server" Width="900px" DataSourceID="ConsultaRadicados" ShowLoadingPanel="False" SeriesDataMember="Series" DiagramTypeName="XYDiagram" AlternateText="Grafico de Consultas Dinamicas Documentos Recibidos" AppearanceName="Pastel Kit" Height="500px"><SeriesSerializable>
<cc2:Series Name="Series 1" ArgumentScaleType="Numerical" LabelTypeName="SideBySideBarSeriesLabel" PointOptionsTypeName="PointOptions" SeriesViewTypeName="SideBySideBarSeriesView">
<View HiddenSerializableString="to be serialized"></View>

<Label HiddenSerializableString="to be serialized" LineVisible="True" OverlappingOptionsTypeName="OverlappingOptions">
<FillStyle FillOptionsTypeName="SolidFillOptions">
<Options HiddenSerializableString="to be serialized"></Options>
</FillStyle>
</Label>

<PointOptions HiddenSerializableString="to be serialized"></PointOptions>

<LegendPointOptions HiddenSerializableString="to be serialized"></LegendPointOptions>
</cc2:Series>
</SeriesSerializable>

<SeriesTemplate LabelTypeName="SideBySideBarSeriesLabel" PointOptionsTypeName="PointOptions" SeriesViewTypeName="SideBySideBarSeriesView">
<View HiddenSerializableString="to be serialized"></View>

<Label HiddenSerializableString="to be serialized" LineVisible="True" OverlappingOptionsTypeName="OverlappingOptions">
<FillStyle FillOptionsTypeName="SolidFillOptions">
<Options HiddenSerializableString="to be serialized"></Options>
</FillStyle>
</Label>

<PointOptions HiddenSerializableString="to be serialized"></PointOptions>

<LegendPointOptions HiddenSerializableString="to be serialized"></LegendPointOptions>
</SeriesTemplate>

<Diagram>
<AxisX Reverse="True" VisibleInPanesSerializable="-1">
<Range Auto="False" MinValueSerializable="A" MaxValueSerializable="E" SideMarginsEnabled="True"></Range>
</AxisX>

<AxisY VisibleInPanesSerializable="-1">
<Range SideMarginsEnabled="True"></Range>

<NumericOptions Precision="0"></NumericOptions>
</AxisY>
</Diagram>

<FillStyle FillOptionsTypeName="SolidFillOptions">
<Options HiddenSerializableString="to be serialized"></Options>
</FillStyle>

<Legend MarkerSize="15, 10" Font="Tahoma, 7pt" SpacingVertical="1" SpacingHorizontal="0" MaxHorizontalPercentage="30" BackColor="255, 255, 255"></Legend>
</dxchartsui:WebChartControl> 
</contenttemplate>
        <triggers>
<asp:PostBackTrigger ControlID="ButtonOpen"></asp:PostBackTrigger>
<asp:PostBackTrigger ControlID="ButtonSaveAs"></asp:PostBackTrigger>
</triggers>
    </asp:UpdatePanel>
    
</asp:Content>

