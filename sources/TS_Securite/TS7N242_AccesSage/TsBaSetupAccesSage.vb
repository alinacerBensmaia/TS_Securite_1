Public Module TsBaSetupAccesSage

    Friend partageUnsafePermis As Boolean = False

    ''' <summary>
    ''' Cette méthode permet d'optimiser les connexions HTTPS avec le serveur SAGE, mais en contrepartie ça signifie
    ''' que l'authentification de l'utilisateur n'est pas toujours propagée correctement au serveur s'il peut y
    ''' en avoir plusieurs.
    ''' </summary>
    ''' <remarks>
    ''' Cette méthode ne doit être appelée que dans une application qui n'a qu'un seul utilisateur.
    ''' En particulier elle ne doit pas être appelée dans un application pool ASP.NET.
    ''' </remarks>
    Public Sub PermettrePartageUnsafe()
        partageUnsafePermis = True
    End Sub

End Module
