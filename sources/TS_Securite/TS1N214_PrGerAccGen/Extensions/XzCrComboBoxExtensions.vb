Imports System.Runtime.CompilerServices
Imports Rrq.CS.ServicesCommuns.ScenarioTransactionnel.Controles.CtlRRQ

Namespace XzCrComboBoxExtensions

    Module XzCrComboBoxExtensions

        <Extension>
        Public Function ValeurSelectionnee(source As XzCrComboBox) As String
            Return source.ValeurSelectionnee(String.Empty)
        End Function

        <Extension>
        Public Function ValeurSelectionnee(source As XzCrComboBox, defautSiNull As String) As String
            If source.SelectedValue Is Nothing Then Return defautSiNull
            Return source.SelectedValue.ToString()
        End Function

    End Module

End Namespace