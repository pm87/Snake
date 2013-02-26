Module Module1

    'GLobale Enums und Strukturen 
    Enum e_control
        LINKS = 1
        RECHTS = 2
        OBEN = 3
        UNTEN = 4
    End Enum

    Enum e_difficulty
        EASY = 10
        NORMAL = 5
        HELL = 1
    End Enum

    Enum e_field_size
        SMALL = 12
        MEDIUM = 36
        BIG = 75
    End Enum

    Public Structure s_game_options
        Public field_size As e_field_size
        Public difficulty As e_difficulty
        Public changedflag As UShort
    End Structure

    'globale Variablen
    Public ctrl As e_control
    Dim game_options As s_game_options



    'Snake-Game Klasse - beinhaltet alle Funktionen und Varialben die für das Snake Spiel benötigt werden
    Public Class snake_game

        'Game Properties - Eigenschaften des Spiels, momentan nur die Punktzahl
        Public Structure s_game_prop
            Dim score As Integer
        End Structure

        'Struktur der Schlange
        Public Structure cs_snake
            'Array der x-Koordinaten der Schalnge
            Dim x() As Short
            'Array der y Koordinaten der Schalnge
            Dim y() As Short
            'Integerwert mit den Schlangenelementanzahl
            Dim elements As UInteger
            'Array über Rechteckformobjekte-später deklariert
            Dim tail()
        End Structure

        'Struktur des Kuchens - Schlangen mögen Kuchen :)
        Public Structure s_cake
            'x-Koordinate des Kuchens
            Dim x As UShort
            'y-Koordinate des Kuchens
            Dim y As UShort
        End Structure

        'Bezugsobjekt für Schlangenelemente
        Dim canvas As New Microsoft.VisualBasic.PowerPacks.ShapeContainer

        'Lokale Strukturvariablen in der Klasse
        Dim game_prop As s_game_prop
        Dim snake As cs_snake
        Dim cake As s_cake

        'Letzte Steuereingabe
        Dim old_ctrl As UShort

        '-------------------------------------------------------------------------------------------------
        'Klassenfunktionen

        'Initialisierungsroutine - Legt die Größe der Elemente im Spielfenster fest und zeichnet sie
        Public Sub init_game()

            'Zufallsvariable für Kuchen
            Dim temp As New Random()

            'Set the form as the parent of the ShapeContainer.
            canvas.Parent = Spiel

            'Falls keine Vorgaben über Optionen gemacht wurden, dann:
            If game_options.changedflag = 0 Then
                game_options.difficulty = e_difficulty.EASY
                game_options.field_size = e_field_size.MEDIUM
            End If

            'Je nach Größe des Spielfeldes...
            Select Case game_options.field_size
                Case e_field_size.SMALL
                    'Größe des Spielfensters setzen
                    Spiel.Size = New Size(e_field_size.SMALL * 10 + 40, e_field_size.SMALL * 10 + 40 + 100)
                    'Startkorrdinaten für schwarzen Rahmen
                    Spiel.RectangleShape2.Location = New System.Drawing.Point(10, 10)
                    'Größe des schwarzen Rahmens setzen
                    Spiel.RectangleShape2.Size = New Size(e_field_size.SMALL * 10, e_field_size.SMALL * 10)
                    'Texte setzen
                    Spiel.Label2.Location = New System.Drawing.Point(10, e_field_size.SMALL * 10 + 10 + 10)
                    Spiel.Label3.Location = New System.Drawing.Point(90, e_field_size.SMALL * 10 + 10 + 10)
                    Spiel.Label6.Location = New System.Drawing.Point(10, e_field_size.SMALL * 10 + 10 + 30)
                    Spiel.Label7.Location = New System.Drawing.Point(90, e_field_size.SMALL * 10 + 10 + 30)
                    Spiel.Button1.Location = New System.Drawing.Point(90, e_field_size.SMALL * 10 + 10 + 50)
                    'Arraygröße der Schlangenelementkoordinaten dynamisch anhand der Spielfeldgröße
                    ReDim snake.x(e_field_size.SMALL * e_field_size.SMALL)
                    ReDim snake.y(e_field_size.SMALL * e_field_size.SMALL)
                    ReDim snake.tail(e_field_size.SMALL * e_field_size.SMALL)
                    'Setzen des Anfangskuchens
                    cake.x = temp.Next(1, e_field_size.SMALL)
                    cake.y = temp.Next(3, e_field_size.SMALL)
                Case (e_field_size.MEDIUM)
                    'siehe oben...
                    Spiel.Size = New Size(e_field_size.MEDIUM * 10 + 40, e_field_size.MEDIUM * 10 + 40 + 100)
                    Spiel.RectangleShape2.Location = New System.Drawing.Point(10, 10)
                    Spiel.RectangleShape2.Size = New Size(e_field_size.MEDIUM * 10, e_field_size.MEDIUM * 10)
                    Spiel.Label2.Location = New System.Drawing.Point(10, e_field_size.MEDIUM * 10 + 10 + 10)
                    Spiel.Label3.Location = New System.Drawing.Point(150, e_field_size.MEDIUM * 10 + 10 + 10)
                    Spiel.Label6.Location = New System.Drawing.Point(10, e_field_size.MEDIUM * 10 + 10 + 30)
                    Spiel.Label7.Location = New System.Drawing.Point(150, e_field_size.MEDIUM * 10 + 10 + 30)
                    Spiel.Button1.Location = New System.Drawing.Point(e_field_size.MEDIUM * 10 - 10 - 60, e_field_size.MEDIUM * 10 + 10 + 10)
                    ReDim snake.x(e_field_size.MEDIUM * e_field_size.MEDIUM)
                    ReDim snake.y(e_field_size.MEDIUM * e_field_size.MEDIUM)
                    ReDim snake.tail(e_field_size.MEDIUM * e_field_size.MEDIUM)
                    cake.x = temp.Next(1, e_field_size.MEDIUM)
                    cake.y = temp.Next(3, e_field_size.MEDIUM)
                Case e_field_size.BIG
                    'siehe oben...
                    Spiel.Size = New Size(e_field_size.BIG * 10 + 40, e_field_size.BIG * 10 + 40 + 100)
                    Spiel.RectangleShape2.Location = New System.Drawing.Point(10, 10)
                    Spiel.RectangleShape2.Size = New Size(e_field_size.BIG * 10, e_field_size.BIG * 10)
                    Spiel.Label2.Location = New System.Drawing.Point(10, e_field_size.BIG * 10 + 10 + 10)
                    Spiel.Label3.Location = New System.Drawing.Point(150, e_field_size.BIG * 10 + 10 + 10)
                    Spiel.Label6.Location = New System.Drawing.Point(10, e_field_size.BIG * 10 + 10 + 30)
                    Spiel.Label7.Location = New System.Drawing.Point(150, e_field_size.BIG * 10 + 10 + 30)
                    Spiel.Button1.Location = New System.Drawing.Point(e_field_size.BIG * 10 - 10 - 60, e_field_size.BIG * 10 + 10 + 10)
                    ReDim snake.x(e_field_size.BIG * e_field_size.BIG)
                    ReDim snake.y(e_field_size.BIG * e_field_size.BIG)
                    ReDim snake.tail(e_field_size.BIG * e_field_size.BIG)
                    cake.x = temp.Next(1, e_field_size.BIG)
                    cake.y = temp.Next(3, e_field_size.BIG)
            End Select

            'Startwerte vergeben
            snake.elements = 1
            snake.x(1) = 5
            snake.y(1) = 2
            ctrl = e_control.RECHTS
            game_prop.score = 0

            'Setzen des Schwierigkeitslabels - Zahlenkodierung doch etwas unschön...
            Select Case game_options.difficulty
                Case e_difficulty.EASY
                    Spiel.Label3.Text = "Leicht"
                Case e_difficulty.NORMAL
                    Spiel.Label3.Text = "Normal"
                Case e_difficulty.HELL
                    Spiel.Label3.Text = "Hölle"
            End Select


        End Sub


        '-------------------------------------------------------------------------------------------------
        'Renderroutine - Zeichnet die Spielelemente mit jedem Durchlauf neu
        Public Sub render()

            'Ausgabe des momentanen Punktestandes
            Spiel.Label7.Text = game_prop.score

            'Kuchen und Rahmen neuzeichnen, nötig aufgrund von overlay-effekten...
            Spiel.sh_cake.Refresh()
            Spiel.RectangleShape2.Refresh()

            'Den leckeren und schmackhaften Kuchen zeichnen
            Spiel.sh_cake.Location = New System.Drawing.Point(cake.x * 10 + 10, cake.y * 10 + 10)

            'Den Schlangenkopf zeichnen
            Spiel.RectangleShape1.Location = New System.Drawing.Point(snake.x(1) * 10 + 10, snake.y(1) * 10 + 10)

            'Schleife um den Schlangenschwanz zu zeichnen
            For i As Short = 1 To snake.elements - 1 Step 1
                snake.tail(i).Location = New System.Drawing.Point(snake.x(i + 1) * 10 + 10, snake.y(i + 1) * 10 + 10)
            Next

        End Sub


        '-------------------------------------------------------------------------------------------------
        'Checkfunktion - Überprüft Abbruch- und Sonderbedingungen des Spiels und liefert Flag
        Public Function check() As UShort

            Dim temp As New Random()
            Dim a_lim As UShort
            Dim endflag As UShort

            'Flag ob Spiel beendet werden soll, 1 = ja, 0 = nein
            endflag = 0

            'Hat die Schlange den Kuchen erwischt = Koordinaten vom Schlangenkopf und Kuchen gleich
            If snake.x(1) = cake.x And snake.y(1) = cake.y Then

                'Ein neuer leckerer Kuchen
                a_lim = game_options.field_size
                cake.x = temp.Next(1, a_lim)
                cake.y = temp.Next(1, a_lim)

                'Punktestand um 10 erhöhen
                game_prop.score = game_prop.score + 10

                'Schlange wird länger...eigentlich müsste sie doch dicker werden? Komische Welt...
                snake.elements = snake.elements + 1

                'Neues Rechteckobjekt aus den Powerpacks hinzufügen
                snake.tail(snake.elements - 1) = New Microsoft.VisualBasic.PowerPacks.RectangleShape

                ' Set the ShapeContainer as the parent of the RectangleShape.
                snake.tail(snake.elements - 1).Parent = canvas
                ' Set the size of the rectangle.
                snake.tail(snake.elements - 1).Width = 10
                snake.tail(snake.elements - 1).Height = 10
                'Den Kuchen färben, in rot, vermutlich erdbeere
                snake.tail(snake.elements - 1).BackColor = Color.Black
                snake.tail(snake.elements - 1).FillStyle = PowerPacks.FillStyle.Solid
                snake.tail(snake.elements - 1).BorderColor = Color.White
            End If

            'Check ob die Schlange sich selber gefressen hat
            For i As UShort = 2 To snake.elements Step 1
                If snake.x(1) = snake.x(i) And snake.y(1) = snake.y(i) Then
                    endflag = 1
                End If
            Next i

            'Check ob die Schlange das Spielfeld verlassen hat
            If snake.x(1) >= game_options.field_size Or snake.x(1) < 0 Or snake.y(1) >= game_options.field_size Or snake.y(1) < 0 Then
                endflag = 1
            End If

            'Rückgabe des Flags
            Return endflag

        End Function


        '-------------------------------------------------------------------------------------------------
        'Pauseroutine - Sorgt für eine Pause zwischen den Spieldurchläufen anhand der Schwierigkeitseinstellung
        Public Sub pause()

            Dim start, finish As Single

            'Jetztzeit
            start = Microsoft.VisualBasic.DateAndTime.Timer
            'Zielzeit
            finish = start + game_options.difficulty / 10

            'Warten bis Zielzeit erreicht
            Do While Microsoft.VisualBasic.DateAndTime.Timer < finish
                Application.DoEvents() 'Reagiere währenddessen auf Steuereingaben
            Loop

        End Sub


        '-------------------------------------------------------------------------------------------------
        'Moveroutine - Sorgt für die Bewegung der Schlange
        Public Sub move()

            'Koordinatenarray um eins hochschieben
            For i As Short = snake.elements To 1 Step -1
                snake.x(i + 1) = snake.x(i)
                snake.y(i + 1) = snake.y(i)
            Next i

            'Verhindern, dass Schlange über sich selber läuft, geht auch schlecht so dick wie die ist von dem vielen Kuchen...
            If ctrl = e_control.LINKS And old_ctrl = e_control.RECHTS Then
                ctrl = old_ctrl
            End If
            If ctrl = e_control.RECHTS And old_ctrl = e_control.LINKS Then
                ctrl = old_ctrl
            End If
            If ctrl = e_control.OBEN And old_ctrl = e_control.UNTEN Then
                ctrl = old_ctrl
            End If
            If ctrl = e_control.UNTEN And old_ctrl = e_control.OBEN Then
                ctrl = old_ctrl
            End If

            'Bewegen des Schlangenkopfs anhand der Steuereingabe
            Select Case ctrl
                Case e_control.LINKS
                    snake.x(1) = snake.x(2) - 1
                Case e_control.RECHTS
                    snake.x(1) = snake.x(2) + 1
                Case e_control.OBEN
                    snake.y(1) = snake.y(2) - 1
                Case e_control.UNTEN
                    snake.y(1) = snake.y(2) + 1
            End Select

            'Zwischenspeichern der letzten Steuereingabe
            old_ctrl = ctrl

        End Sub


        '-------------------------------------------------------------------------------------------------
        'Shutdownroutine - Sorgt für geordneten Abbruch des Spiels
        Public Sub shutdown()

            Dim msg As String

            'Feedback über Spielergebnis
            If game_prop.score = 0 Then
                msg = "Sie haben leider keine Punkte erzielt"
                MsgBox(msg, 0, "Schade")
            Else
                msg = "Sie haben " & game_prop.score & " Punkte erreicht"
                MsgBox(msg, 0, "Glückwunsch")
            End If

            'Schließen des Spielfensters
            Spiel.Close()

        End Sub

    End Class
    '-------------------------------------------------------------------------------------------------
    'Ende der Klassendefinition






    '-------------------------------------------------------------------------------------------------
    'Optionsroutine - Ändert die Spieloptionen anhand der gewählten Werte
    Sub edit_options()

        Select Case Optionen.ComboBox1.Text
            Case "LEICHT"
                game_options.difficulty = e_difficulty.EASY
            Case "NORMAL"
                game_options.difficulty = e_difficulty.NORMAL
            Case "HÖLLE"
                game_options.difficulty = e_difficulty.HELL
            Case Else
                game_options.difficulty = e_difficulty.NORMAL
        End Select

        Select Case Optionen.ComboBox2.Text
            Case "KLEIN"
                game_options.field_size = e_field_size.SMALL
            Case "MITTEL"
                game_options.field_size = e_field_size.MEDIUM
            Case "GROSS"
                game_options.field_size = e_field_size.BIG
            Case Else
                game_options.field_size = e_field_size.MEDIUM
        End Select

        game_options.changedflag = 1

    End Sub


    '-------------------------------------------------------------------------------------------------
    'Inputroutine - Übernimmt das Keydown-Event des Formulars und wandelt es in Steuerkommando um
    Sub eval_input(ByVal e As System.Windows.Forms.KeyEventArgs)

        If e.KeyCode = Keys.W Then
            ctrl = e_control.OBEN
        End If
        If e.KeyCode = Keys.A Then
            ctrl = e_control.LINKS
        End If
        If e.KeyCode = Keys.S Then
            ctrl = e_control.UNTEN
        End If
        If e.KeyCode = Keys.D Then
            ctrl = e_control.RECHTS
        End If

    End Sub

End Module
