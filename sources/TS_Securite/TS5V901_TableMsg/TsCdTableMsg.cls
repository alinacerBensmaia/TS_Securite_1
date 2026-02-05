VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "TsCdTableMsg"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = True
Option Explicit

Implements IXyTableMsg

Private Function IXyTableMsg_TxtMsg(NoMsg As Integer) As String
Dim DescrErr As String

    On Error GoTo ERR_HANDLER

    IXyTableMsg_TxtMsg = LoadResString(NoMsg)
    'on sort...
Exit Function
ERR_HANDLER:
    Select Case Err.Number
'       La ressource est absente du fichier.
        Case 326
            Err.Raise XyErrTmMsgAbsent
        Case Else
            RelancerErreur Err, _
                App.EXEName & ".TsCdTableMsg.TxtMsg"
    End Select
End Function


