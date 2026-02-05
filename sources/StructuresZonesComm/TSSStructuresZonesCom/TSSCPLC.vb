Imports System
Imports System.Globalization
Imports System.Collections.Generic
Imports Fujitsu.Progression.Foundation.IBM.Application
Imports Fujitsu.Progression.Foundation.IBM.Database
Imports RRQCommon.RRQ.Common

Public Class TSSCPLCModule
    Inherits VariableModuleBase

#Region " Copybook TSSCPLC "

    ''' <summary> 
    ''' <para>Régie des Rentes du Québec</para> 
    ''' <para>Classe de copybook TSSCPLCModule</para> 
    ''' <para>Cette classe de copybook a été convertie par la suite d'outils Progression de Fujitsu America Inc.</para> 
    ''' </summary> 
    ''' <remarks> 
    ''' <para>Coquille VB RRQ version 1.00; Dictionary version 1.00</para> 
    ''' </remarks>
    
    Public Tsscpl As Literal
    Public Tsshead As Literal
    Public Tssclass As Literal
    Public Tssrname As Literal
    Public Tssppgm As Literal
    Public Tssacc As Literal
    Public Tssrc As Numeric
    Public Tssrok As Condition
    Public Tssrnd As Condition
    Public Tssrna As Condition
    Public Tssripl As Condition
    Public Tssrenv As Condition
    Public Tssrinac As Condition
    Public Tssstat As Numeric
    Public Tsssdef As Condition
    Public Tsssund As Condition
    Public Tsssnso As Condition
    Public Tsssidt As Condition
    Public Tsscrc As Literal
    Public Tsscstat As Literal
    Public Tsscacee As Literal
    Public Tssvol As Literal
    Public Tsslog As Literal
    Public Tssrsvd As Literal
    Public Tssrtn As Literal
    Public Tssacida As Literal
    Public Tssfac As Literal
    Public Tssmode As Literal
    Public Tsstype As Literal
    Public Tssterm As Literal
    Public Tsssys As Literal
    Public Tssacidf As Literal
    Public Tssdepta As Literal
    Public Tssdeptf As Literal
    Public Tssdiva As Literal
    Public Tssdivf As Literal
    Public Tssresv As Literal
    Public Tssplen As Numeric

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal scope As String)
        MyBase.New(scope)
    End Sub

    Public Sub New(ByVal alternateLevel As Integer)
        MyBase.New(alternateLevel)
    End Sub

    Public Sub New(ByVal scope As String, ByVal alternateLevel As Integer)
        MyBase.New(scope, alternateLevel)
    End Sub

    Protected Overrides Sub InstanciateVariables(ByVal alternateLevel As Integer)



        '*    COPY IDMS MODULE TSSCPLC VERSION 1.                          
        '****************************************************************  
        '*                                                              *  
        '*TOP SECRET SECURITY CICS PARAMETER LIST.       V4L3           *  
        '*                                                              *  
        '*NOTE FIELD TSSHEAD MUST HAVE THE VALUE 'TCPLV4L3'             *  
        '*                                                              *  
        '****************************************************************  

        Tsscpl = New Literal(1 + alternateLevel)                     '01 TSSCPL.
        Tsshead = New Literal(2 + alternateLevel, 8, "TCPLV4L3")     '02 TSSHEAD PIC X(8) VALUE 'TCPLV4L3'.
        Tssclass = New Literal(2 + alternateLevel, 8)                '02 TSSCLASS PIC X(8).
        Tssrname = New Literal(2 + alternateLevel, 44)               '02 TSSRNAME PIC X(44).
        Tssppgm = New Literal(2 + alternateLevel, 8)                 '02 TSSPPGM PIC X(8).
        Tssacc = New Literal(2 + alternateLevel, 8)                  '02 TSSACC PIC X(8).
        Tssrc = New Numeric(2 + alternateLevel, 5, +0D, True)        '02 TSSRC PIC S9(5) VALUE +0.
        
        Tssrok= New Condition(Tssrc, _
            Function() Tssrc.Value = +00, _
            Sub() Tssrc.Value = +00)

        Tssrnd= New Condition(Tssrc, _
            Function() Tssrc.Value = +04, _
            Sub() Tssrc.Value = +04)

        Tssrna= New Condition(Tssrc, _
            Function() Tssrc.Value = +08, _
            Sub() Tssrc.Value = +08)

        Tssripl= New Condition(Tssrc, _
            Function() Tssrc.Value = +12, _
            Sub() Tssrc.Value = +12)

        Tssrenv= New Condition(Tssrc, _
            Function() Tssrc.Value = +16, _
            Sub() Tssrc.Value = +16)

        Tssrinac= New Condition(Tssrc, _
            Function() Tssrc.Value = +20, _
            Sub() Tssrc.Value = +20)

        Tssstat = New Numeric(2 + alternateLevel, 5, +0D, True)      '02 TSSSTAT PIC S9(5) VALUE +0.
        
        Tsssdef= New Condition(Tssstat, _
            Function() Tssstat.Value = +00, _
            Sub() Tssstat.Value = +00)

        Tsssund= New Condition(Tssstat, _
            Function() Tssstat.Value = +04, _
            Sub() Tssstat.Value = +04)

        Tsssnso= New Condition(Tssstat, _
            Function() Tssstat.Value = +08, _
            Sub() Tssstat.Value = +08)

        Tsssidt= New Condition(Tssstat, _
            Function() Tssstat.Value = +12, _
            Sub() Tssstat.Value = +12)

        Tsscrc = New Literal(2 + alternateLevel, 2)                  '02 TSSCRC PIC X(2).
        Tsscstat = New Literal(2 + alternateLevel, 2)                '02 TSSCSTAT PIC X(2).
        Tsscacee = New Literal(2 + alternateLevel, 4)                '02 TSSCACEE PIC X(4).
        Tssvol = New Literal(2 + alternateLevel, 6)                  '02 TSSVOL PIC X(6).
        Tsslog = New Literal(2 + alternateLevel, 1)                  '02 TSSLOG PIC X.
        Tssrsvd = New Literal(2 + alternateLevel, 19)                '02 TSSRSVD PIC X(19).
        Tssrtn = New Literal(2 + alternateLevel)                     '02 TSSRTN .
        Tssacida = New Literal(5 + alternateLevel, 8)                '05 TSSACIDA PIC X(8).
        Tssfac = New Literal(5 + alternateLevel, 8)                  '05 TSSFAC PIC X(8).
        Tssmode = New Literal(5 + alternateLevel, 8)                 '05 TSSMODE PIC X(8).
        Tsstype = New Literal(5 + alternateLevel, 8)                 '05 TSSTYPE PIC X(8).
        Tssterm = New Literal(5 + alternateLevel, 8)                 '05 TSSTERM PIC X(8).
        Tsssys = New Literal(5 + alternateLevel, 8)                  '05 TSSSYS PIC X(8).
        Tssacidf = New Literal(5 + alternateLevel, 32)               '05 TSSACIDF PIC X(32).
        Tssdepta = New Literal(5 + alternateLevel, 8)                '05 TSSDEPTA PIC X(8).
        Tssdeptf = New Literal(5 + alternateLevel, 32)               '05 TSSDEPTF PIC X(32).
        Tssdiva = New Literal(5 + alternateLevel, 8)                 '05 TSSDIVA PIC X(8).
        Tssdivf = New Literal(5 + alternateLevel, 32)                '05 TSSDIVF PIC X(32).
        Tssresv = New Literal(5 + alternateLevel, 96)                '05 TSSRESV PIC X(96).
        Tssplen = New Numeric(2 + alternateLevel, 5, +0370D, True)   '02 TSSPLEN PIC S9(5) VALUE +0370.
    End Sub
   

#End Region

End Class
