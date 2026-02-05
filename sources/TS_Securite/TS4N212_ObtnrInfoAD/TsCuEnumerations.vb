''' <summary>
'''   Possibilité de Champs sur lequel on peut effectuer la recherche dans l'active directory.
''' </summary>
Public Enum TsIadTypeRequete
    TsIadTrCodeUtilisateur
    TsIadTrNom
    TsIadTrPrenom
    TsIadTrNomComplet
    TsIadTrCourriel
    TsIadTrUniteAdmn
    TsIadTrFonction
    TsIadTrMembreDe
    TsIadTrSid
    TsIadTrSociete
    TsIadTrDescription
    TsIadTrNoEmploye
    TsIadTrNomEtPrenom
    TsIadTrNoTelephone
End Enum

''' <summary>
'''   Type de catégorie possible pour la recherche dans l'active directory.
''' </summary>
Public Enum TsIadObjectCategory
    TsIadOcTous
    TsIadOcPerson
    TsIadOcGroup
End Enum

''' <summary>
''' Énumération de nom de domaine.
''' </summary>
Public Enum TsIadNomDomaine

    ''' <summary>
    ''' Domaine RRQ.
    ''' </summary>
    TsDomaineRQ = 0

    ''' <summary>
    ''' Domaine RRQ.
    ''' </summary>
    TsDomaineRRQ = 1

    ''' <summary>
    ''' Domaine CARRA.
    ''' </summary>
    TsDomaineCARRA = 2

    ''' <summary>
    ''' Domaine RRQ et CARRA.
    ''' </summary>
    TsMultiDomaine = 3

End Enum