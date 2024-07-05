Module modRutinas
    Sub FondoDegradeDiagonalEnPanel(ByVal objPanel As Panel,
                                    ByVal e As PaintEventArgs,
                                    ByVal colEmpresa As Color)
        Try

            Dim Brocha As System.Drawing.Drawing2D.LinearGradientBrush
            Dim Superficie As Graphics
            Dim Rectangulo As Rectangle
            Dim Lapiz As Pen

            Try
                Superficie = e.Graphics
                Lapiz = New Pen(Color.Azure, 1)
                Rectangulo = New Rectangle(0, 0, objPanel.Width, objPanel.Height)
                Brocha = New System.Drawing.Drawing2D.LinearGradientBrush(Rectangulo, Color.White, colEmpresa, System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal)


                Superficie.FillRectangle(Brocha, Rectangulo)
                Superficie.DrawRectangle(Lapiz, Rectangulo)
                Lapiz.Dispose()
                Superficie.Dispose()
            Catch ex As Exception
                'No hacemos nada si falla. Si hay error
            End Try
        Catch ex As Exception

        End Try
    End Sub
End Module
