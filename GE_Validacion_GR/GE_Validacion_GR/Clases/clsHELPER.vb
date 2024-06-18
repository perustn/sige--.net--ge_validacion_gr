Imports System.Data
Imports System.Data.SqlClient
Public Class clsHELPER

    Private strCn As String = ""

    Public Sub New()
        'strCn = "Data Source=(local);Initial Catalog=Northwind"
    End Sub


#Region " Colecciones "

    Public Enum enuOrden
        ConParametros = 1
        SinParametros = 0
    End Enum


#End Region

#Region " Parámetros "

    Private Sub CreaParametros(ByVal oCmd As SqlCommand, ByVal aParametros As Object)
        On Error Resume Next
        If Not IsArray(aParametros) Then Exit Sub

        Dim sNombre As String
        Dim Tipo As SqlDbType
        Dim Direccion As ParameterDirection
        Dim lTamaño As Integer
        Dim Valor As Object

        Dim i As Integer
        Dim bUltimo As Byte

        bUltimo = UBound(aParametros)
        For i = 0 To (bUltimo)
            sNombre = aParametros(i)(0)
            Tipo = aParametros(i)(1)
            Direccion = aParametros(i)(2)
            lTamaño = aParametros(i)(3)
            Valor = aParametros(i)(4)
            oCmd.Parameters.Add(New SqlParameter(sNombre, Tipo, Direccion, lTamaño)).Value = Valor
        Next i
    End Sub

#End Region

#Region " Operaciones con COMMAMD "

    Private Sub ConfiguraCmd(ByVal oCmd As SqlCommand, ByVal xSp As String)
        On Error Resume Next
        With oCmd
            .Connection.ConnectionString = strCn
            .CommandType = CommandType.StoredProcedure
            .CommandText = xSp
        End With
    End Sub

    'Public Function EjecutaSp(ByVal xSp As String, _
    '                       ByVal xTipOrden As enuOrden, _
    '                       ByVal ParamArray objParametros() As Object) As Object
    '    Dim oCmd As SqlCommand = New SqlCommand

    '    Try
    '        Call ConfiguraCmd(oCmd, xSp)
    '        If xTipOrden = enuOrden.ConParametros Then Call CreaParametros(oCmd, objParametros)
    '        oCmd.ExecuteNonQuery()
    '        EjecutaSp = oCmd
    '    Catch xErr As Exception
    '    Finally
    '        oCmd.Dispose()
    '    End Try
    'End Function

#End Region

#Region " Operaciones con DATAADAPTER "

    Enum enuMetodo
        '[Consulta] = 0
        [Insertar] = 0
        [Modificar] = 0
        [Elminar] = 0
    End Enum

    Enum enuEjecutarTransaccion
        [SI] = 0
        [NO] = 1
    End Enum

    Function DevuelveDatos(ByVal strSQL As String, ByVal CCONNECT As String) As DataTable
        Dim oDa As SqlDataAdapter
        Dim oCn As SqlConnection

        Try

            oCn = New SqlConnection(CCONNECT)
            oDa = New SqlDataAdapter(strSQL, CCONNECT)
            Dim oDt As DataTable = New DataTable("HPdata")
            oDa.Fill(oDt)
            DevuelveDatos = oDt
        Catch xErr As Exception
            MsgBox(xErr.Message, MsgBoxStyle.Critical)
            DevuelveDatos = Nothing
        End Try
    End Function


    Function DevuelveDato(ByVal strSQL As String,
                          ByVal CCONNECT As String,
                          Optional ByVal bolSWMostrarMsgDeError As Boolean = True,
                          Optional ByRef strErrSQL As String = "")
        Dim oCn As SqlConnection = Nothing
        Dim oCmd As New SqlCommand

        Try
            oCn = New SqlConnection(CCONNECT)
            oCn.Open()
            oCmd = New SqlCommand
            With oCmd
                .Connection = oCn
                .CommandType = CommandType.Text
                .CommandText = strSQL
                DevuelveDato = .ExecuteScalar
            End With
        Catch xSQLErr As SqlException
            DevuelveDato = Nothing
            strErrSQL = xSQLErr.Message
            If bolSWMostrarMsgDeError Then MsgBox(xSQLErr.Message, MsgBoxStyle.Critical)
        Catch xErr As Exception
            MsgBox(xErr.Message, MsgBoxStyle.Critical)
            DevuelveDato = Nothing
        Finally
            oCmd.Dispose()
            If oCn.State = ConnectionState.Open Then oCn.Close()
            oCn.Dispose()
        End Try
    End Function

    Function EjecutaOperacion(ByVal strSQL As String, ByVal CCONNECT As String,
                              Optional ByVal opcMetodo As enuMetodo = enuMetodo.Insertar,
                              Optional ByVal opcEjecutaTransaccion As enuEjecutarTransaccion = enuEjecutarTransaccion.SI,
                              Optional ByVal bolSWMostrarMsgDeError As Boolean = True,
                              Optional ByRef strErrSQL As String = "") As Boolean
        Dim oDa As SqlDataAdapter = Nothing
        Dim oCn As SqlConnection = Nothing
        Dim oCmd As New SqlCommand

        Try
            oCn = New SqlConnection(CCONNECT)
            oCn.Open()
            oCmd = New SqlCommand
            With oCmd
                .Connection = oCn
                .CommandType = CommandType.Text
                .CommandText = strSQL
            End With
            oDa = New SqlDataAdapter()
            Select Case opcMetodo
                Case enuMetodo.Insertar
                    oDa.InsertCommand = oCmd
                    oDa.InsertCommand.ExecuteNonQuery()
                    'If opcEjecutaTransaccion = enuEjecutarTransaccion.SI Then oDa.InsertCommand.ExecuteNonQuery()
                Case enuMetodo.Modificar
                    oDa.UpdateCommand = oCmd
                    oDa.UpdateCommand.ExecuteNonQuery()
                    'If opcEjecutaTransaccion = enuEjecutarTransaccion.SI Then oDa.UpdateCommand.ExecuteNonQuery()
                Case enuMetodo.Elminar
                    oDa.DeleteCommand = oCmd
                    oDa.DeleteCommand.ExecuteNonQuery()
                    'If opcEjecutaTransaccion = enuEjecutarTransaccion.SI Then oDa.DeleteCommand.ExecuteNonQuery()
            End Select
            EjecutaOperacion = True
        Catch xSQLErr As SqlException
            EjecutaOperacion = False
            strErrSQL = xSQLErr.Message
            If bolSWMostrarMsgDeError Then MsgBox(xSQLErr.Message, MsgBoxStyle.Critical)
        Catch xErr As Exception
            EjecutaOperacion = False
            If bolSWMostrarMsgDeError Then MsgBox(xErr.Message, MsgBoxStyle.Critical)
        Finally
            oCmd.Dispose()
            oDa.Dispose()
            If oCn.State = ConnectionState.Open Then oCn.Close()
            oCn.Dispose()
        End Try
    End Function

    Function EjecutaOperacion_TimeOut(ByVal strSQL As String, ByVal CCONNECT As String,
                              Optional ByVal opcMetodo As enuMetodo = enuMetodo.Insertar,
                              Optional ByVal opcEjecutaTransaccion As enuEjecutarTransaccion = enuEjecutarTransaccion.SI) As Boolean
        Dim oDa As SqlDataAdapter = Nothing
        Dim oCn As SqlConnection = Nothing
        Dim oCmd As New SqlCommand

        Try
            oCn = New SqlConnection(CCONNECT)

            oCn.Open()

            oCmd = New SqlCommand
            With oCmd
                .Connection = oCn
                .CommandTimeout = 240
                .CommandType = CommandType.Text
                .CommandText = strSQL
                .CommandTimeout = 0
            End With
            oDa = New SqlDataAdapter()
            Select Case opcMetodo
                Case enuMetodo.Insertar
                    oDa.InsertCommand = oCmd
                    oDa.InsertCommand.ExecuteNonQuery()
                'If opcEjecutaTransaccion = enuEjecutarTransaccion.SI Then oDa.InsertCommand.ExecuteNonQuery()
                Case enuMetodo.Modificar
                    oDa.UpdateCommand = oCmd
                    oDa.UpdateCommand.ExecuteNonQuery()
                'If opcEjecutaTransaccion = enuEjecutarTransaccion.SI Then oDa.UpdateCommand.ExecuteNonQuery()
                Case enuMetodo.Elminar
                    oDa.DeleteCommand = oCmd
                    oDa.DeleteCommand.ExecuteNonQuery()
                    'If opcEjecutaTransaccion = enuEjecutarTransaccion.SI Then oDa.DeleteCommand.ExecuteNonQuery()
            End Select
            EjecutaOperacion_TimeOut = True
        Catch xSQLErr As SqlException
            EjecutaOperacion_TimeOut = False
            MsgBox(xSQLErr.Message, MsgBoxStyle.Critical)
        Catch xErr As Exception
            EjecutaOperacion_TimeOut = False
            MsgBox(xErr.Message, MsgBoxStyle.Critical)
        Finally
            oCmd.Dispose()
            oDa.Dispose()
            If oCn.State = ConnectionState.Open Then oCn.Close()
            oCn.Dispose()
        End Try
    End Function

    Function EjecutaOperacionRetornaDatos(ByVal strSQL As String, ByVal CCONNECT As String,
                                          Optional ByVal bolSWMostrarMsgDeError As Boolean = True,
                                          Optional ByRef strErrSQL As String = "") As DataTable
        Dim oDa As SqlDataAdapter = Nothing
        Dim oCn As SqlConnection = Nothing
        Dim oCmd As New SqlCommand

        Try
            'oCn = New SqlConnection(CCONNECT)
            'oDa = New SqlDataAdapter(strSQL, CCONNECT)


            oCn = New SqlConnection(CCONNECT)
            oCn.Open()
            oCmd = New SqlCommand
            With oCmd
                .Connection = oCn
                .CommandType = CommandType.Text
                .CommandText = strSQL
            End With
            oDa = New SqlDataAdapter(oCmd)
            oDa.InsertCommand = oCmd
            oDa.InsertCommand.ExecuteNonQuery()
            Dim oDt As DataTable = New DataTable("HPdata")
            oDa.Fill(oDt)
            EjecutaOperacionRetornaDatos = oDt
        Catch xSQLErr As SqlException
            strErrSQL = xSQLErr.Message
            If bolSWMostrarMsgDeError Then MsgBox(xSQLErr.Message, MsgBoxStyle.Critical)
        Catch xErr As Exception
            EjecutaOperacionRetornaDatos = Nothing
            Throw xErr
        Finally
            oCmd.Dispose()
            oDa.Dispose()
            If oCn.State = ConnectionState.Open Then oCn.Close()
            oCn.Dispose()
        End Try
    End Function

    Function DevuelveDatosReader(ByVal strSQL As String, ByVal CCONNECT As String) As SqlDataReader

        Dim oDR As SqlDataReader
        Dim oCn As SqlConnection
        Dim oCmd As New SqlCommand

        Try
            oCn = New SqlConnection(CCONNECT)
            oCn.Open()
            oCmd = New SqlCommand
            With oCmd
                .Connection = oCn
                .CommandType = CommandType.Text
                .CommandText = strSQL
            End With
            oDR = oCmd.ExecuteReader(CommandBehavior.CloseConnection)
            DevuelveDatosReader = oDR
        Catch xErr As Exception
            MsgBox(xErr.Message, MsgBoxStyle.Critical)
            DevuelveDatosReader = Nothing
        End Try
    End Function

    Function EjecutaOperacionRetornaDatos2(ByVal strSQL As String, ByVal CCONNECT As String) As DataTable
        Dim oDa As SqlDataAdapter
        Dim oCn As SqlConnection
        Try
            oCn = New SqlConnection(CCONNECT)
            oDa = New SqlDataAdapter(strSQL, CCONNECT)
            Dim oDt As DataTable = New DataTable("HPdata")
            oDa.Fill(oDt)
            EjecutaOperacionRetornaDatos2 = oDt
        Catch xErr As Exception
            EjecutaOperacionRetornaDatos2 = Nothing
            Throw xErr
        End Try
    End Function

    Function DevuelveDatos2(ByVal strSQL As String, ByVal CCONNECT As String) As DataTable
        Dim oDa As SqlDataAdapter
        Dim oCn As SqlConnection
        Dim ocmd As New SqlCommand(strSQL)

        Try

            oCn = New SqlConnection(CCONNECT)
            ocmd.Connection = oCn
            ocmd.CommandTimeout = 0


            oDa = New SqlDataAdapter(ocmd)
            Dim oDt As DataTable = New DataTable("HPdata")
            oDa.Fill(oDt)
            DevuelveDatos2 = oDt


        Catch xErr As Exception
            MsgBox(xErr.Message, MsgBoxStyle.Critical)
            DevuelveDatos2 = Nothing
        End Try
    End Function
#End Region

End Class
