Option Explicit On 

Imports System.runtime.InteropServices
Imports System.IO

Public Class TsCuMSMQ
    ' Get the user name of the owner of the queue reference by hQueue, and then add the name in strNom.
    ' PARAMETERS:
    '   strFormatNomFile: Format Name of the queue
    '   strNom: A string variable to add the name of the owner in
    ' RETURN: TRUE on success, FALSE on failure
    Public Shared Function ObtenirNomProprioFile(ByVal strFormatNomFile As String, ByRef strNom As String) As Boolean
        Try
            Dim bytSecurityDescriptor() As Byte
            Dim iLength As Integer
            Dim iLengthNeeded As Integer
            Dim iResult As Integer
            Dim iSecurityInformation As Integer
            Dim gchFormatNomFile As GCHandle = GCHandle.Alloc(strFormatNomFile, GCHandleType.Pinned)
            Dim pFormatNomFilePtr As IntPtr = gchFormatNomFile.AddrOfPinnedObject
            Dim gchSecurityDescriptor As GCHandle
            Dim pSecurityDescriptor As IntPtr

            iSecurityInformation = OWNER_SECURITY_INFORMATION

            ' Call MQGetQueueSecurity two times. The first time, set the nLength
            ' parameter to 0. The function then informs you of the size that you need for the
            ' security descriptor in lpnLengthNeeded.           

            iResult = MQGetQueueSecurity(pFormatNomFilePtr, iSecurityInformation, IntPtr.Zero, 0, iLengthNeeded)

            If iResult = MQ_ERROR_SECURITY_DESCRIPTOR_TOO_SMALL Then
                ' This is expected. Continue.
            Else
                Throw New Exception("There was an error calling MQGetQueueSecurity." + vbCrLf + "Error Number: " + Hex(iResult))
            End If

            ' Now you know how big to make the security descriptor.
            iLength = iLengthNeeded
            ReDim bytSecurityDescriptor(iLength)

            gchSecurityDescriptor = GCHandle.Alloc(bytSecurityDescriptor, GCHandleType.Pinned)
            pSecurityDescriptor = gchSecurityDescriptor.AddrOfPinnedObject

            iResult = MQGetQueueSecurity(pFormatNomFilePtr, iSecurityInformation, pSecurityDescriptor, iLength, iLengthNeeded)

            If iResult <> MQ_OK Then
                Throw New Exception("There was an error calling MQGetQueueSecurity." + vbCrLf + "Error Number: " + Hex(iResult))
            End If

            strNom = ObtenirNomProprioDescriptSecurite(pSecurityDescriptor)

            gchFormatNomFile.Free()
            gchSecurityDescriptor.Free()
            
            Return True
        Catch ex As Exception
            Throw New Exception("Une erreur est survenue dans ObtenirNomProprioFile " + vbCrLf + " Description: " + ex.Message, ex)
        End Try
    End Function

    ' Get the name that is associated with the security descriptor.
    ' RETURN: The name of the owner.
    Public Shared Function ObtenirNomProprioDescriptSecurite(ByVal pDescriptSecurite As IntPtr) As String
        Try
            Dim pSid As IntPtr
            Dim bOwnerDefaulted As Boolean
            Dim iRet As Integer
            Dim cbAccountName As Integer = 0
            Dim cbReferencedDomainName As Integer = 0
            Dim pszAccountName As String
            Dim pszReferencedDomainName As String
            Dim iSnu As Integer
            Dim pAccountName As IntPtr
            Dim pReferencedDomainName As IntPtr

            iRet = GetSecurityDescriptorOwner(pDescriptSecurite, pSid, bOwnerDefaulted)

            If iRet = 0 Then
                Throw New Exception("ERROR calling GetSecurityDescriptorOwner!" + vbCrLf + " Error Number:" + Err.LastDllError.ToString() + _
                    vbCrLf + " Error Description:" + Err.Description)
            End If

            ' Call LookupAccountSid two times: one time to find out how large your
            ' string buffers have to be, and another time to get the actual information.
            iRet = LookupAccountSid(vbNullString, pSid.ToInt32, pAccountName, cbAccountName, pReferencedDomainName, _
                cbReferencedDomainName, iSnu)

            ' Make your strings the correct length, and then call LookupAccountSid again.
            pszAccountName = Strings.Space(255)
            pszReferencedDomainName = Strings.Space(255)

            pAccountName = Marshal.StringToHGlobalAnsi(pszAccountName)
            pReferencedDomainName = Marshal.StringToHGlobalAnsi(pszReferencedDomainName)
            
            iRet = LookupAccountSid(vbNullString, pSid.ToInt32, pAccountName, cbAccountName, pReferencedDomainName, _
                cbReferencedDomainName, iSnu)

            If iRet = 0 Then
                Throw New Exception("ERROR calling LookupAccountSid!" + vbCrLf + " Error Number:" + Err.LastDllError.ToString() + vbCrLf + _
                    " Error Description:" + Err.Description)
            End If

            pszReferencedDomainName = Left(Marshal.PtrToStringAnsi(pReferencedDomainName), cbReferencedDomainName)
            pszAccountName = Left(Marshal.PtrToStringAnsi(pAccountName), cbAccountName)

            Marshal.FreeHGlobal(pAccountName)
            Marshal.FreeHGlobal(pReferencedDomainName)

            Return pszReferencedDomainName + "\" + pszAccountName
        Catch ex As Exception
            Throw New Exception("There was an error in ObtenirNomProprioDescriptSecurite " + vbCrLf + " Description: " + ex.Message, ex)
        End Try
    End Function

    ' Get the user name of the owner of the queue reference by hQueue, and then add the name in strNom.
    ' PARAMETERS:
    '   strFormatNomFile: Format Name of the queue
    '   strNom: A string variable to add the name of the owner in
    ' RETURN: TRUE on success, FALSE on failure
    Public Shared Function ObtenirSecuriteFile(ByVal strFormatNomFile As String, ByRef strNom As String) As Boolean
        Try
            Dim bytSecurityDescriptor() As Byte
            Dim iLength As Integer
            Dim iLengthNeeded As Integer
            Dim iResult As Integer
            Dim iSecurityInformation As Integer
            Dim gchFormatNomFilePtr As GCHandle = GCHandle.Alloc(strFormatNomFile, GCHandleType.Pinned)
            Dim pFormatNomFilePtr As IntPtr = gchFormatNomFilePtr.AddrOfPinnedObject
            Dim gchSecurityDescriptor As GCHandle
            Dim pSecurityDescriptor As IntPtr

            iSecurityInformation = DACL_SECURITY_INFORMATION

            ' Call MQGetQueueSecurity two times. The first time, set the nLength
            ' parameter to 0. The function then informs you of the size that you need for the
            ' security descriptor in lpnLengthNeeded.

            iResult = MQGetQueueSecurity(pFormatNomFilePtr, iSecurityInformation, IntPtr.Zero, 0&, iLengthNeeded)

            If iResult = MQ_ERROR_SECURITY_DESCRIPTOR_TOO_SMALL Then
                ' This is expected. Continue.
            Else
                Throw New Exception("There was an error calling MQGetQueueSecurity." + vbCrLf + "Error Number: " + Hex(iResult))
            End If

            ' Now you know how big to make the security descriptor.
            iLength = iLengthNeeded
            ReDim bytSecurityDescriptor(iLength)

            gchSecurityDescriptor = GCHandle.Alloc(bytSecurityDescriptor, GCHandleType.Pinned)
            pSecurityDescriptor = gchSecurityDescriptor.AddrOfPinnedObject

            ' Call MQGetQueueSecurity.        
            iResult = MQGetQueueSecurity(pFormatNomFilePtr, iSecurityInformation, _
                pSecurityDescriptor, iLength, iLengthNeeded)

            If iResult <> MQ_OK Then
                Throw New Exception("There was an error calling MQGetQueueSecurity." + vbCrLf + "Error Number: " + Hex(iResult))
            End If

            strNom = ObtenirDescriptSecuriteDacl(pSecurityDescriptor)

            If iResult <> MQ_OK Then
                Throw New Exception("There was an error calling GetSecurityDescriptorDacl." + vbCrLf + "Error Number: " + Hex(iResult))
            End If

            gchSecurityDescriptor.Free()
            gchFormatNomFilePtr.Free()
            
            Return True
        Catch ex As Exception
            Throw New Exception("There was an error in ObtenirSecuriteFile " + vbCrLf + "Description: " + ex.Message)
        End Try
    End Function

    Public Shared Function ObtenirDescriptSecuriteDacl(ByVal pSecurityDescriptor As IntPtr) As String
        Try
            Dim bDaclPresent As Boolean
            Dim bDaclDefaulted As Boolean
            Dim iIndex As Integer
            Dim iRet As Integer
            Dim iResult As Integer
            Dim iNewACLSize As Integer                 ' Size of new ACL to create.
            Dim pDacl As IntPtr
            Dim pCurrentAce As IntPtr                 ' Our current ACE.
            Dim gchNewACL As GCHandle
            Dim pNewACL As IntPtr
            Dim bNewACL() As Byte                   ' Buffer to hold new ACL.à            
            Dim sSID As String
            Dim sTempSID As String
            Dim ACLInfo As ACL_SIZE_INFORMATION
            Dim CurrentACE As ACCESS_ALLOWED_ACE   ' Current ACE.            

            iRet = GetSecurityDescriptorDacl(pSecurityDescriptor, bDaclPresent, pDacl, bDaclDefaulted)

            If iRet = 0 Then
                MsgBox("ERROR calling GetSecurityDescriptorOwner!" + vbCrLf + " Error Number:" + _
                    Err.LastDllError.ToString() + vbCrLf + " Error Description:" + Err.Description)
                Exit Function
            End If

            ' Attempt to get the ACL from the file's Security Descriptor.
            Dim gchACLInfo As GCHandle = GCHandle.Alloc(ACLInfo, GCHandleType.Pinned)
            Dim pSizePtr As IntPtr = gchACLInfo.AddrOfPinnedObject
            Dim iAclInformationLength As Integer = Marshal.SizeOf(GetType(ACL_SIZE_INFORMATION))

            iResult = GetAclInformation(pDacl, pSizePtr, iAclInformationLength, 2&)

            ' A return code of zero means the call failed; test for this before continuing.
            If (iResult = 0) Then
                MsgBox("Error: Unable to Get ACL from File Security Descriptor")
                Exit Function
            End If

            ' Now that you have the ACL information, compute the new ACL size requirements.
            ACLInfo = CType(Marshal.PtrToStructure(pSizePtr, _
                GetType(ACL_SIZE_INFORMATION)), ACL_SIZE_INFORMATION)
            
            gchACLInfo.Free()

            iNewACLSize = ACLInfo.AclBytesInUse + Marshal.SizeOf(GetType(ACCESS_ALLOWED_ACE)) - _
                Marshal.SizeOf(GetType(Integer)) + (SID_MAX_SUB_AUTHORITIES * Marshal.SizeOf(GetType(Integer)))

            ' Resize our new ACL buffer to its proper size.
            ReDim bNewACL(iNewACLSize)

            gchNewACL = GCHandle.Alloc(bNewACL, GCHandleType.Pinned)
            pNewACL = gchNewACL.AddrOfPinnedObject

            ' Use the InitializeAcl API function call to initialize the new ACL.
            iResult = InitializeAcl(pNewACL, iNewACLSize, ACL_REVISION)

            ' If a DACL is present, copy it to a new DACL.
            If (bDaclPresent) Then

                ' Copy the ACEs from the file to the new ACL.
                If (ACLInfo.AceCount > 0) Then

                    ' Grab each ACE and stuff them into the new ACL.
                    For iIndex = 0 To (ACLInfo.AceCount - 1)

                        ' Attempt to grab the next ACE.
                        iResult = GetAce(pDacl, iIndex, pCurrentAce)

                        ' Make sure you have the current ACE under question.
                        If (iResult = 0) Then
                            MsgBox("Error: Unable to Obtain ACE (" + iIndex + ")")
                            Exit Function
                        End If

                        Dim AceStruct As ACCESS_ALLOWED_ACE = _
                            CType(Marshal.PtrToStructure(pCurrentAce, GetType(ACCESS_ALLOWED_ACE)), ACCESS_ALLOWED_ACE)
                        
                        iResult = AddAce(pNewACL, ACL_REVISION, 0, pCurrentAce, AceStruct.Header.AceSize)
                        If (iResult = 0) Then
                            MsgBox("Error: Unable to Set Ace (Number: " + Err.LastDllError.ToString() + ")")
                        End If

                        Dim cbAccountName As Integer = 0
                        Dim cbReferencedDomainName As Integer = 0
                        Dim pszAccountName As String
                        Dim pszReferencedDomainName As String
                        Dim snu As Integer
                        Dim pAccountName As IntPtr
                        Dim pReferencedDomainName As IntPtr

                        ' Call LookupAccountSid two times: one time to find out how large your
                        ' string buffers have to be, and another time to get the actual information.
                        iRet = LookupAccountSid(vbNullString, pCurrentAce.ToInt32 + 8, pAccountName, cbAccountName, _
                            pReferencedDomainName, cbReferencedDomainName, snu)

                        ' Make your strings the correct length, and then call LookupAccountSid again.
                        pszAccountName = Strings.Space(255)
                        pszReferencedDomainName = Strings.Space(255)

                        pAccountName = Marshal.StringToHGlobalAnsi(pszAccountName)
                        pReferencedDomainName = Marshal.StringToHGlobalAnsi(pszReferencedDomainName)
                        
                        iRet = LookupAccountSid(vbNullString, pCurrentAce.ToInt32 + 8, pAccountName, cbAccountName, _
                            pReferencedDomainName, cbReferencedDomainName, snu)

                        If iRet = 0 Then
                            MsgBox("ERROR calling LookupAccountSid!" + vbCrLf + " Error Number:" + _
                                Err.LastDllError.ToString() + vbCrLf + " Error Description:" + Err.Description)
                            Exit Function
                        End If

                        pszReferencedDomainName = Left(Marshal.PtrToStringAnsi(pReferencedDomainName), cbReferencedDomainName)
                        pszAccountName = Left(Marshal.PtrToStringAnsi(pAccountName), cbAccountName)
                        sSID = pszReferencedDomainName + "\" + pszAccountName

                        CurrentACE = CType(Marshal.PtrToStructure(pCurrentAce, _
                                        GetType(ACCESS_ALLOWED_ACE)), ACCESS_ALLOWED_ACE)

                        If CurrentACE.Header.AceType = 0 Then
                            sTempSID = sTempSID + sSID + ":" + CStr(CurrentACE.Mask) + ";"
                        End If

                        Marshal.FreeHGlobal(pAccountName)
                        Marshal.FreeHGlobal(pReferencedDomainName)
                    Next iIndex
                End If
            End If

            gchNewACL.Free()
            
            Return sTempSID
        Catch ex As Exception
            Throw New Exception("There was an error in GetSidOwnerName " + vbCrLf + " Description: " + ex.Message)
        End Try
    End Function
End Class
