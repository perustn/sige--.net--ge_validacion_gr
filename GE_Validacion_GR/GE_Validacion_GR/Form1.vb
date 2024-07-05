Imports System.ComponentModel
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Threading
Imports System.IO
Imports System.Net
Imports System.Text
Imports Newtonsoft.Json
Public Class Form1
    Private colEmpresa As Color
    Property CodEmpresa As String
    Private oHP As clsHELPER = New clsHELPER
    Private strSQL As String
    Private oDt As New DataTable
    Dim Hilo As Threading.Thread
    Property CadenaDeConexion_Operaciones_VBNet As String
    Property CadenaDeConexion_Seguridad_VBNet As String
    Property CadenaDeConexion_Operaciones_VB6 As String
    Property CadenaDeConexion_Seguridad_VB6 As String
    Private Sub DefinirConexionSIGE(strRutaArchivoSIGEConfig As String, Optional ByVal strBD As String = "SIGE_STN")
        Dim oSIGELink As New SIGELink.clsSIGELink

        If My.Computer.FileSystem.FileExists(strRutaArchivoSIGEConfig) = False Then
            MessageBox.Show("No existe el archivo de conexión SIGELink, deberia encontrarse en la siguiente ruta " & vbNewLine & strRutaArchivoSIGEConfig,
                            "SIGE : Autenticación",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Return
        End If

        If oSIGELink.LeerArchivoDeConexionSQL(strRutaArchivoSIGEConfig) = False Then
            Dim strMsgErr As String
            strMsgErr = "No se ha podido enlazar con el Servidor de Base de Datos, vuelva a generar el archivo SIGELink"
            MessageBox.Show(strMsgErr,
                            "SIGE : Autenticación",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Application.Exit()
            Exit Sub
        End If

        Select Case oSIGELink.TipoDeConexion
            Case SIGELink.clsSIGELink.enuTipoDeConexion.MonoEmpresa
                CadenaDeConexion_Operaciones_VBNet = oSIGELink.DevuelveCadenaDeConexionConBDSIGE
                CadenaDeConexion_Seguridad_VBNet = oSIGELink.DevuelveCadenaDeConexionConBDSeguridad

                CadenaDeConexion_Operaciones_VB6 = oSIGELink.DevuelveCadenaDeConexionConBDSIGE_EnVB6
                CadenaDeConexion_Seguridad_VB6 = oSIGELink.DevuelveCadenaDeConexionConBDSeguridad_EnVB6

            Case SIGELink.clsSIGELink.enuTipoDeConexion.MultiEmpresa
                CadenaDeConexion_Operaciones_VBNet = oSIGELink.DevuelveCadenaDeConexionConBDSIGE & strBD
                CadenaDeConexion_Seguridad_VBNet = oSIGELink.DevuelveCadenaDeConexionConBDSeguridad

                CadenaDeConexion_Operaciones_VB6 = oSIGELink.DevuelveCadenaDeConexionConBDSIGE_EnVB6 & strBD
                CadenaDeConexion_Seguridad_VB6 = oSIGELink.DevuelveCadenaDeConexionConBDSeguridad_EnVB6
        End Select


    End Sub
    Private Sub btnSalir_Click(sender As Object, e As EventArgs) Handles btnSalir.Click
        Application.Exit()
    End Sub

    Private Sub RevisandoProcesosPorNotificar()
        Try
            Dim sFecha As String
            Dim sHora As String
            Dim Cad_Traza As String = ""
            Dim Dt_URL As New DataTable
            Dim url As String
            Dim docid As String
            Dim comando As String = "emitir"
            Dim parametro As String = ""
            'Dim Rsp_Mail As String

            sFecha = Date.Now.Year.ToString + Microsoft.VisualBasic.Right("00" + Date.Now.Month.ToString, 2) + Microsoft.VisualBasic.Right("00" + Date.Now.Day.ToString, 2)
            sHora = Now.ToString("HH:mm")
            'strSQL = String.Format("EXEC SIGE_Notificador '{0}'´,'{1}','{2}'", "W", sFecha, sHora)

            strSQL = String.Empty
            strSQL &= vbNewLine & "EXEC SIGE_Revisa_GR_Pendiente_Envio"
            strSQL &= vbNewLine & String.Format(" @Opcion     = '{0}'", "L")
            strSQL &= vbNewLine & String.Format(",@Fecha      = '{0}'", sFecha)
            strSQL &= vbNewLine & String.Format(",@Hora       = '{0}'", sHora)

            oDt = oHP.DevuelveDatos(strSQL, Me.CadenaDeConexion_Operaciones_VBNet)
            If oDt Is Nothing OrElse oDt.Rows.Count = 0 Then Return

            Dt_URL = oHP.DevuelveDatos("SP_IP_SERVER_ACEPTA_GR", CadenaDeConexion_Operaciones_VBNet)
            If Dt_URL.Rows.Count > 0 Then
                url = Dt_URL.Rows(0)(0).ToString
            Else
                MessageBox.Show("No se Encontro Direccion IP del Servidor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            For i = 0 To oDt.Rows.Count - 1

                Cad_Traza = oDt.Rows(i)(5).ToString()

                'url = "http://192.168.30.9:5001/ca4xml"
                '-------------------------------------------------
                docid = "T" + oDt.Rows(i)(3).ToString() & "-" & oDt.Rows(i)(4).ToString()
                'Dim comando As String = "emitir"
                'Dim parametro As String = ""
                Dim respuesta As String = SendCA4XMLRequest(url, docid, comando, parametro, Cad_Traza)

                strSQL = String.Format("EXEC Ventas_Distribuye_por_Partida_Actualzia_Envio_GR_CMT_Rsp_Acepta '{0}','{1}','{2}','{3}','{4}','{5}','{6}'", oDt.Rows(i)(0), oDt.Rows(i)(1).ToString(), oDt.Rows(i)(2).ToString(), oDt.Rows(i)(3).ToString(), oDt.Rows(i)(4).ToString(), respuesta, "AUTO")
                If (respuesta.Substring(0, 2).ToUpper = "OK") Then
                    If oHP.EjecutaOperacion(strSQL, CadenaDeConexion_Operaciones_VBNet) Then
                    End If
                Else
                    If oHP.EjecutaOperacion(strSQL, CadenaDeConexion_Operaciones_VBNet) Then
                    End If
                End If

            Next


        Catch ex As Exception

        End Try
    End Sub

    Public Function SendCA4XMLRequest(ByVal url As String, ByVal docid As String, ByVal comando As String, ByVal parametros As String, ByVal Datos As String) As String
        Dim result As String = ""
        Dim Solicitud As HttpWebRequest
        Dim Respuesta As HttpWebResponse
        Dim cuerpo_peticion As String = ""
        cuerpo_peticion = ("docid=" _
                    + (EncodeURL(docid) + ("&comando=" _
                    + (EncodeURL(comando) + ("&parametros=" _
                    + (EncodeURL(parametros) + ("&datos=" + EncodeURL(Datos))))))))
        Solicitud = CType(WebRequest.Create(url), HttpWebRequest)
        Try
            Dim bytes() As Byte
            bytes = System.Text.Encoding.UTF8.GetBytes(cuerpo_peticion)
            'Set HttpWebRequest properties
            Solicitud.Method = "POST"
            Solicitud.ContentLength = bytes.Length
            Solicitud.ContentType = "application/x-www-form-urlencoded"
            'Solicitud.Connection = "Close";
            Dim requestStream As Stream = Solicitud.GetRequestStream
            'Writes a sequence of bytes to the current stream 
            requestStream.Write(bytes, 0, bytes.Length)
            requestStream.Close()
            'Close stream                
            Respuesta = CType(Solicitud.GetResponse, HttpWebResponse)
            If (Respuesta.StatusCode = HttpStatusCode.OK) Then
                'Get response stream into StreamReader
                Dim responseStream As Stream = Respuesta.GetResponseStream
                Dim reader As StreamReader = New StreamReader(responseStream)
                result = reader.ReadToEnd
            Else
                result = ("ERROR|||||||Error del servidor: " _
                            + (Respuesta.StatusCode + (": " + Respuesta.StatusDescription)))
            End If

            Respuesta.Close()
            'Close HttpWebResponse
        Catch we As WebException
            result = ("ERROR|||||||Error del servidor: " _
                        + (we.Status + (": " + we.Message)))
        Catch ex As Exception
            result = ("ERROR|||||||Error del servidor: " + ex.Message)
            Throw New Exception(ex.Message)
        Finally
            'Respuesta.Close();
            'Release objects
            Respuesta = Nothing
            Solicitud = Nothing
        End Try

        Return result
    End Function

    Private Function EncodeURL(data As String) As String
        Dim result As String = ""
        Dim aChar As String
        Dim theCode As Integer
        Dim N As Integer
        Dim aux As String
        For N = 0 To data.Length - 1
            aChar = data.Substring(N, 1)
            theCode = Microsoft.VisualBasic.AscW(aChar)
            If (theCode >= 0 AndAlso theCode <= 47) OrElse (theCode >= 58 AndAlso theCode <= 64) OrElse (theCode >= 91 AndAlso theCode <= 93) OrElse (theCode >= 123 AndAlso theCode <= 126) Then
                aux = (theCode + 256).ToString("X")
                result = result & "%" & aux.Substring(aux.Length - 2, 2)

            Else
                result = result + aChar

            End If

        Next
        Return result

    End Function

    Private Sub tmProgramacion_Tick(sender As Object, e As EventArgs) Handles tmProgramacion.Tick
        Hilo = New Threading.Thread(AddressOf RevisandoProcesosPorNotificar)
        Hilo.Start()
        'RevisandoProcesosPorNotificar()
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CodEmpresa = ConfigurationSettings.AppSettings("Empresa").ToString
        DefinirConexionSIGE(ConfigurationSettings.AppSettings("SIGELink").ToString, "SEGURIDAD")

        'Me.CadenaDeConexion_Operaciones_VBNet = "Initial Catalog=sige_stn;Data Source=192.168.30.55;Integrated Security = SSPI"
        'Me.CadenaDeConexion_Seguridad_VBNet = "Initial Catalog=Seguridad;Data Source=192.168.30.55;Integrated Security = SSPI"

        Dim oDt As DataTable = oHP.DevuelveDatos(String.Format("Select * FROM SEG_Empresas where cod_empresa = '{0}'", Me.CodEmpresa), Me.CadenaDeConexion_Seguridad_VBNet)
        colEmpresa = Color.FromArgb(oDt.Rows(0)("ColorFondo_R"), oDt.Rows(0)("ColorFondo_G"), oDt.Rows(0)("ColorFondo_B"))
        'colEmpresa = Color.FromArgb(193, 208, 222)
        panHeader.BackColor = colEmpresa
        Me.tmProgramacion.Interval = ConfigurationSettings.AppSettings("Intervalo").ToString '60000 * 30
        Me.tmProgramacion.Enabled = True
        'Timer = New Timer();
        '    Timer.Interval = 1000; // Intervalo de 1 segundo (1000 milisegundos)
        '    Timer.Enabled = True;
        '    Timer.Tick += Timer_Tick;
        'tmProgramacion_Tick(sender, e)
    End Sub

    Private Sub panHeader_Paint(sender As Object, e As PaintEventArgs) Handles panHeader.Paint
        FondoDegradeDiagonalEnPanel(sender, e, colEmpresa)
    End Sub

    Private Sub PictureBox2_Paint(sender As Object, e As PaintEventArgs) Handles PictureBox2.Paint
        'FondoDegradeDiagonalEnPanel(sender, e, colEmpresa)
    End Sub
End Class
