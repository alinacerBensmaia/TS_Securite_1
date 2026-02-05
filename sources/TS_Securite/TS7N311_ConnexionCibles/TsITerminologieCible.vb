''' <summary>
''' Interface pour mapper les objets de connexion selon la terminologie d'une cible.
''' </summary>
Public Interface TsITerminologieCible

    Function Traduire(ByVal role As TsCdConnxRole, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String
    Function Traduire(ByVal roleRole As TsCdConnxRoleRole, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String
    Function Traduire(ByVal roleRessr As TsCdConnxRoleRessr, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String
    Function Traduire(ByVal userRole As TsCdConnxUserRole, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String
    Function Traduire(ByVal userRessr As TsCdConnxUserRessr, Optional ByVal action As TsECcAction = TsECcAction.Aucune) As String

    '! Au besoin instancier les traductions des relations manquantes.

End Interface
