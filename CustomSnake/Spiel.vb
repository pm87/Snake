Public Class Spiel

    'Boolscher Wert ob Gameloop aktiv
    Dim active As Boolean

    Private Sub Spiel_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        active = True

        Dim endflag As UShort
        'Instanzieren des Snake-Spiel-Objekts
        Dim snake As New snake_game

        'Initialisieren des Spiels
        snake.init_game()

        'Zeige das Formular
        Me.Show()
        'Proaktive Abfrage ob User bereit für das Spiel
        MsgBox("Start!")

        'Game-Loop
        Do While active
            'Funktionen in Module1.vb erläutert
            snake.render()



            snake.pause()

            endflag = snake.check()

            snake.move()



            If endflag = 1 Then
                active = False
            End If

        Loop

        snake.shutdown()

    End Sub

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown

        'Keydown-Event des Formulars - Aufrufen der Funktion um Steuereingaben auszuwerten
        eval_input(e)

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        active = False

    End Sub


End Class


