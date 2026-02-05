Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media

Public Class TsCuTextBoxSelection
    Inherits TextBox

    Public Sub New()

        AddHandler Me.PreviewMouseLeftButtonDown, AddressOf IgnorerClickSouris
        AddHandler Me.GotKeyboardFocus, AddressOf SelectionnerTexte
        AddHandler Me.MouseDoubleClick, AddressOf SelectionnerTexte

    End Sub

    Private Shared Sub IgnorerClickSouris(ByVal sender As Object, ByVal e As MouseButtonEventArgs)


        Dim parent As DependencyObject = DirectCast(e.OriginalSource, UIElement)
        While parent IsNot Nothing AndAlso Not (TypeOf (parent) Is TextBox)
            parent = VisualTreeHelper.GetParent(parent)
        End While

        If parent IsNot Nothing Then

            Dim objTextBox As TextBox = DirectCast(parent, TextBox)

            If Not objTextBox.IsKeyboardFocusWithin Then

                objTextBox.Focus()
                e.Handled = True

            End If
        End If

    End Sub

    Private Shared Sub SelectionnerTexte(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim objTextBox As TextBox = DirectCast(e.OriginalSource, TextBox)
        If objTextBox IsNot Nothing Then
            objTextBox.SelectAll()
        End If
    End Sub

End Class
