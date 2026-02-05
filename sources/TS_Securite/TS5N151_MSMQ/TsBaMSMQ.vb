Option Explicit On 

Imports System.Runtime.InteropServices

Module TsBaMSMQ
    Public Const OWNER_SECURITY_INFORMATION = &H1&
    Public Const DACL_SECURITY_INFORMATION = &H4&

    Public Const SECURITY_DESCRIPTOR_REVISION = 1
    Public Const SECURITY_DESCRIPTOR_MIN_LENGTH = 20
    Public Const SD_SIZE = (65536 + SECURITY_DESCRIPTOR_MIN_LENGTH)
    Public Const ACL_REVISION2 = 2
    Public Const ACL_REVISION = 2
    Public Const MAXDWORD = &HFFFFFFFF
    Public Const SIDTYPEUSER = 1
    Public Const ACLSIZEINFORMATION = 2

    Public Const READ_CONTROL As Long = &H20000
    Public Const WRITE_DAC As Long = &H40000
    Public Const WRITE_OWNER As Long = &H80000
    Public Const DELETE = &H10000

    Public Const MQ_OK = 0&

    Public Const MQSEC_QUEUE_GENERIC_EXECUTE = &H0
    Public Const MQSEC_DELETE_MESSAGE = &H1
    Public Const MQSEC_PEEK_MESSAGE = &H2
    Public Const MQSEC_WRITE_MESSAGE = &H4
    Public Const MQSEC_DELETE_JOURNAL_MESSAGE = &H8
    Public Const MQSEC_SET_QUEUE_PROPERTIES = &H10
    Public Const MQSEC_GET_QUEUE_PROPERTIES = &H20
    Public Const MQSEC_DELETE_QUEUE = DELETE
    Public Const MQSEC_GET_QUEUE_PERMISSIONS = READ_CONTROL
    Public Const MQSEC_CHANGE_QUEUE_PERMISSIONS = WRITE_DAC
    Public Const MQSEC_TAKE_QUEUE_OWNERSHIP = WRITE_OWNER
    Public Const MQSEC_RECEIVE_MESSAGE = MQSEC_DELETE_MESSAGE + MQSEC_PEEK_MESSAGE

    Public Const MQSEC_RECEIVE_JOURNAL_MESSAGE = MQSEC_DELETE_JOURNAL_MESSAGE + MQSEC_PEEK_MESSAGE

    Public Const MQSEC_QUEUE_GENERIC_READ = MQSEC_GET_QUEUE_PROPERTIES + MQSEC_GET_QUEUE_PERMISSIONS + _
                                            MQSEC_RECEIVE_MESSAGE + MQSEC_RECEIVE_JOURNAL_MESSAGE

    Public Const MQSEC_QUEUE_GENERIC_WRITE = MQSEC_GET_QUEUE_PROPERTIES + MQSEC_GET_QUEUE_PERMISSIONS + _
                                             MQSEC_WRITE_MESSAGE

    Public Const MQSEC_QUEUE_GENERIC_RECEIVE_WRITE = MQSEC_RECEIVE_MESSAGE + MQSEC_GET_QUEUE_PROPERTIES + _
                                                     MQSEC_GET_QUEUE_PERMISSIONS + MQSEC_WRITE_MESSAGE

    Public Const MQSEC_QUEUE_GENERIC_CHANGE = MQSEC_RECEIVE_MESSAGE + MQSEC_DELETE_JOURNAL_MESSAGE + _
                                              MQSEC_WRITE_MESSAGE + MQSEC_SET_QUEUE_PROPERTIES + _
                                              MQSEC_GET_QUEUE_PROPERTIES + MQSEC_GET_QUEUE_PERMISSIONS + _
                                              MQSEC_CHANGE_QUEUE_PERMISSIONS + MQSEC_TAKE_QUEUE_OWNERSHIP

    Public Const MQSEC_QUEUE_GENERIC_ALL = MQSEC_RECEIVE_MESSAGE + MQSEC_DELETE_JOURNAL_MESSAGE + _
                                           MQSEC_WRITE_MESSAGE + MQSEC_SET_QUEUE_PROPERTIES + _
                                           MQSEC_GET_QUEUE_PROPERTIES + MQSEC_DELETE_QUEUE + _
                                           MQSEC_GET_QUEUE_PERMISSIONS + MQSEC_CHANGE_QUEUE_PERMISSIONS + _
                                           MQSEC_TAKE_QUEUE_OWNERSHIP

    Public Const MQ_ERROR_SECURITY_DESCRIPTOR_TOO_SMALL As Int32 = &HC00E0023

    Public Const SID_MAX_SUB_AUTHORITIES As Integer = 15

    Structure ACE_HEADER
        Dim AceType As Byte
        Dim AceFlags As Byte
        Dim AceSize As Short
    End Structure

    Public Structure ACCESS_DENIED_ACE
        Dim Header As ACE_HEADER
        Dim Mask As Integer
        Dim SidStart As Integer
    End Structure

    Structure ACCESS_ALLOWED_ACE
        Dim Header As ACE_HEADER
        Dim Mask As Integer
        Dim SidStart As Integer
    End Structure

    Structure ACCESS_ALLOWED_ACE_TYPE
        Dim Header As ACE_HEADER
        Dim Mask As Integer
        Dim SidStart As Integer
    End Structure

    Structure ACL
        Dim AclRevision As Byte
        Dim Sbz1 As Byte
        Dim AclSize As Short
        Dim AceCount As Short
        Dim Sbz2 As Short
    End Structure

    Structure ACL_SIZE_INFORMATION
        Dim AceCount As Integer
        Dim AclBytesInUse As Integer
        Dim AclBytesFree As Integer
    End Structure

    Structure SECURITY_DESCRIPTOR
        Dim Revision As Byte
        Dim Sbz1 As Byte
        Dim Control As Integer
        Dim Owner As Integer
        Dim Group As Integer
        Dim sACL As ACL
        Dim Dacl As ACL
    End Structure

    'The MQGetQueueSecurity function retrieves the access control security descriptor for the queue that you specify.    

    Public Declare Function MQGetQueueSecurity Lib "mqrt.dll" ( _
                                                ByVal lpwcsFormatName As IntPtr, _
                                                ByVal SecurityInformation As Integer, _
                                                ByVal pSecurityDescriptor As IntPtr, _
                                                ByVal nLength As Integer, _
                                                ByRef lpnLengthNeeded As Integer) As Integer

    Public Declare Function GetSecurityDescriptorOwner Lib "advapi32.dll" ( _
                                                ByVal ppSecurityDescriptor As IntPtr, _
                                                ByRef ppOwner As IntPtr, _
                                                ByRef lpbOwnerDefaulted As Integer) As Integer

    'The GetSecurityDescriptorDacl function retrieves the pointer to the DACL in security descriptor.
    Public Declare Function GetSecurityDescriptorDacl Lib "advapi32.dll" ( _
                                                ByVal pSecurityDescriptor As IntPtr, _
                                                ByRef lpbDaclPresent As Boolean, _
                                                ByRef pDacl As IntPtr, _
                                                ByRef lpbDaclDefaulted As Boolean) As Integer

    'The GetAclInformation function retrieves the ACL_SIZE_INFORMATION structure.
    Public Declare Function GetAclInformation Lib "advapi32.dll" ( _
                                                ByVal pDacl As IntPtr, _
                                                ByVal pAclInformation As IntPtr, _
                                                ByVal nAclInformationLength As Integer, _
                                                ByVal dwAclInformationClass As Short) As Integer

    Public Declare Function GetAce Lib "advapi32.dll" (ByVal pAcl As IntPtr, _
                                                ByVal dwAceIndex As Integer, _
                                                ByRef pAce As IntPtr) As Boolean

    Public Declare Function GetLengthSid Lib "advapi32.dll" (ByVal pSid As Object) As Long

    Public Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" (ByRef hpvDest As Object, ByVal hpvSource As IntPtr, _
                                                ByVal cbCopy As Integer)

    Public Declare Function EqualSid Lib "advapi32.dll" (ByVal pSid1 As Byte, ByVal pSid2 As Long) As Long

    Public Declare Function AddAce Lib "advapi32.dll" ( _
                                                ByVal pAcl As IntPtr, _
                                                ByVal dwAceRevision As Integer, _
                                                ByVal dwStartingAceIndex As Integer, _
                                                ByVal pAceList As IntPtr, _
                                                ByVal nAceListLength As Integer) As Integer

    Public Declare Function AddAccessAllowedAce Lib "advapi32.dll" (ByVal pDacl As Byte, ByVal dwAceRevision As Long, ByVal AccessMask As Long, ByVal pSid As Byte) As Long

    Public Declare Function InitializeAcl Lib "advapi32.dll" ( _
                                                ByVal pDacl As IntPtr, _
                                                ByVal nAclLength As Integer, _
                                                ByVal dwAclRevision As Integer) As Integer

    'The LookupAccountSid function accepts a security identifier (SID) as input. The function retrieves
    'the name of the account for this SID and the name of the first domain on which this SID is found.
    Public Declare Function LookupAccountSid Lib "advapi32.dll" Alias "LookupAccountSidA" ( _
                                                ByVal lpSystemName As String, _
                                                ByVal SID As Integer, _
                                                ByVal Name As IntPtr, _
                                                ByRef cbName As Integer, _
                                                ByVal DomainName As IntPtr, _
                                                ByRef cbDomainName As Integer, _
                                                ByRef peUse As Integer) As Integer

End Module
