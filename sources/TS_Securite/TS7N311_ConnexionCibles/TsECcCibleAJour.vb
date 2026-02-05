''' <summary>
''' Représente le niveau d'«à-jour-itude» d'un élément
''' </summary>
Public Enum TsECcCibleAJour

    ''' <summary>
    ''' Impossible de savoir si la cible est à jour ou la vérification n'a pas été faite.
    ''' </summary>
    Inconnu = 0

    ''' <summary>
    ''' La cible a été vérifiée et elle n'est définitivement pas à jour.
    ''' </summary>
    ''' <remarks>Utilisé lorsque le connecteur peut accéder directement à la cible.</remarks>
    PasAJour

    ''' <summary>
    ''' La cible a été vérifiée et elle n'est définitivement pas à jour.
    ''' </summary>
    ''' <remarks>Utilisé lorsque le connecteur ne peut pas accéder directement à la cible.</remarks>
    ExtractionPasAJour

    ''' <summary>
    ''' Une extraction des informations de la cible indique que la cible est à jour.
    ''' </summary>
    ''' <remarks>Utilisé lorsque le connecteur ne peut pas accéder directement à la cible.</remarks>
    ExtractionAJour

    ''' <summary>
    ''' Une mise à jour de la cible a été demandée.
    ''' </summary>
    ''' <remarks>Utilisé lorsque le connecteur ne peut pas accéder directement à la cible.</remarks>
    MajDemande

    ''' <summary>
    ''' La cible est à jour ou a été mise à jour.
    ''' </summary>
    ''' <remarks>Utilisé lorsque le connecteur peut accéder directement à la cible.</remarks>
    AJour

End Enum
