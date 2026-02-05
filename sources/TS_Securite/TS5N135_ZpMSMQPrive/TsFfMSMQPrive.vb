Imports System.IO
Imports System.Messaging
Imports System.Management
Imports System.Text

Public Class TsFfMSMQPrive
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents chkTransac As System.Windows.Forms.CheckBox
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents txtFile As System.Windows.Forms.TextBox
    Friend WithEvents cmdCreer As System.Windows.Forms.Button
    Friend WithEvents tltMSMQ As System.Windows.Forms.ToolTip
    Friend WithEvents lstFiles As System.Windows.Forms.ListBox
    Friend WithEvents imlMSMQ As System.Windows.Forms.ImageList
    Friend WithEvents tlbMSMQ As System.Windows.Forms.ToolBar
    Friend WithEvents cmdAppliquer As System.Windows.Forms.ToolBarButton
    Friend WithEvents cmdExtraire As System.Windows.Forms.ToolBarButton
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(TsFfMSMQPrive))
        Me.lstFiles = New System.Windows.Forms.ListBox
        Me.chkTransac = New System.Windows.Forms.CheckBox
        Me.lblFile = New System.Windows.Forms.Label
        Me.txtFile = New System.Windows.Forms.TextBox
        Me.cmdCreer = New System.Windows.Forms.Button
        Me.tltMSMQ = New System.Windows.Forms.ToolTip(Me.components)
        Me.imlMSMQ = New System.Windows.Forms.ImageList(Me.components)
        Me.tlbMSMQ = New System.Windows.Forms.ToolBar
        Me.cmdExtraire = New System.Windows.Forms.ToolBarButton
        Me.cmdAppliquer = New System.Windows.Forms.ToolBarButton
        Me.SuspendLayout()
        '
        'lstFiles
        '
        Me.lstFiles.Location = New System.Drawing.Point(8, 32)
        Me.lstFiles.Name = "lstFiles"
        Me.lstFiles.Size = New System.Drawing.Size(240, 173)
        Me.lstFiles.TabIndex = 18
        '
        'chkTransac
        '
        Me.chkTransac.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTransac.Location = New System.Drawing.Point(48, 240)
        Me.chkTransac.Name = "chkTransac"
        Me.chkTransac.Size = New System.Drawing.Size(112, 24)
        Me.chkTransac.TabIndex = 20
        Me.chkTransac.Text = "Transactionnelle"
        '
        'lblFile
        '
        Me.lblFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFile.Location = New System.Drawing.Point(8, 216)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(40, 16)
        Me.lblFile.TabIndex = 21
        Me.lblFile.Text = "Nom"
        '
        'txtFile
        '
        Me.txtFile.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtFile.Location = New System.Drawing.Point(48, 216)
        Me.txtFile.Name = "txtFile"
        Me.txtFile.Size = New System.Drawing.Size(168, 20)
        Me.txtFile.TabIndex = 19
        Me.txtFile.Text = ""
        '
        'cmdCreer
        '
        Me.cmdCreer.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCreer.Image = CType(resources.GetObject("cmdCreer.Image"), System.Drawing.Image)
        Me.cmdCreer.Location = New System.Drawing.Point(224, 216)
        Me.cmdCreer.Name = "cmdCreer"
        Me.cmdCreer.Size = New System.Drawing.Size(24, 24)
        Me.cmdCreer.TabIndex = 24
        Me.tltMSMQ.SetToolTip(Me.cmdCreer, "Ajouter la file dans la liste")
        '
        'imlMSMQ
        '
        Me.imlMSMQ.ImageSize = New System.Drawing.Size(16, 16)
        Me.imlMSMQ.ImageStream = CType(resources.GetObject("imlMSMQ.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imlMSMQ.TransparentColor = System.Drawing.Color.Transparent
        '
        'tlbMSMQ
        '
        Me.tlbMSMQ.Buttons.AddRange(New System.Windows.Forms.ToolBarButton() {Me.cmdExtraire, Me.cmdAppliquer})
        Me.tlbMSMQ.DropDownArrows = True
        Me.tlbMSMQ.ImageList = Me.imlMSMQ
        Me.tlbMSMQ.Location = New System.Drawing.Point(0, 0)
        Me.tlbMSMQ.Name = "tlbMSMQ"
        Me.tlbMSMQ.ShowToolTips = True
        Me.tlbMSMQ.Size = New System.Drawing.Size(258, 28)
        Me.tlbMSMQ.TabIndex = 25
        '
        'cmdExtraire
        '
        Me.cmdExtraire.ImageIndex = 1
        Me.cmdExtraire.ToolTipText = "Extraire la sécurité"
        '
        'cmdAppliquer
        '
        Me.cmdAppliquer.ImageIndex = 0
        Me.cmdAppliquer.ToolTipText = "Appliquer la sécurité"
        '
        'TsFfMSMQPrive
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(258, 263)
        Me.Controls.Add(Me.tlbMSMQ)
        Me.Controls.Add(Me.cmdCreer)
        Me.Controls.Add(Me.chkTransac)
        Me.Controls.Add(Me.lblFile)
        Me.Controls.Add(Me.txtFile)
        Me.Controls.Add(Me.lstFiles)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "TsFfMSMQPrive"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gestionnaire MSMQ File Privée"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub cmdCreer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCreer.Click
        Dim strOwner As String
        Dim strAdmins As String
        Dim strSID As String
        Dim strWQL As String

        Try
            If EstFileRRQ(txtFile.Text) Then
                MessageQueue.Create(".\private$\" & txtFile.Text, chkTransac.Checked)
                Dim objTsCuMSMQ As New TS5N151_MSMQ.TsCuMSMQ
                Dim objItemFile As New MessageQueue(".\private$\" & txtFile.Text)

                objItemFile.Label = "private$\" & txtFile.Text
                objTsCuMSMQ.ObtenirNomProprioFile(objItemFile.FormatName, strOwner)

                strWQL = "Select * from Win32_Group"
                Dim searcher As New ManagementObjectSearcher(New ManagementScope("root\cimv2"), New WqlObjectQuery(strWQL))
                For Each objManObj As ManagementObject In searcher.Get()
                    If CType(objManObj("SID"), String).EndsWith("544") Then
                        strAdmins = objManObj("Name")
                        strSID = objManObj("SID")
                        Exit For
                    End If
                Next
                objItemFile.SetPermissions(strAdmins, System.Messaging.MessageQueueAccessRights.FullControl)

                Dim oMsmqACEowner = New MessageQueueAccessControlEntry(New Trustee(strOwner, "int.rrq.qc", TrusteeType.User), MessageQueueAccessRights.FullControl, AccessControlEntryType.Revoke)
                objItemFile.SetPermissions(oMsmqACEowner)

                RafraichirArbre()
            Else
                MessageBox.Show("Le nom de file entré n'est pas valide" + vbCrLf + "Ex: uxw61234_nomDeLaFile", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As MessageQueueException
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub tlbMSMQ_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles tlbMSMQ.ButtonClick
        Dim objFilePrivee As MessageQueue() = MessageQueue.GetPrivateQueuesByMachine(".")
        Dim enuFilePrivee As IEnumerator = objFilePrivee.GetEnumerator()

        Select Case tlbMSMQ.Buttons.IndexOf(e.Button)
            Case 0  'Extraire
                Dim intNbFile As Integer = 0

                While enuFilePrivee.MoveNext
                    Dim strNomFile As String = enuFilePrivee.Current().queuename
                    Dim objFile As New MessageQueue(".\" + strNomFile)
                    Dim objTsCuMSMQ As New TS5N151_MSMQ.TsCuMSMQ
                    Dim strMSMQSecurite As String

                    If objTsCuMSMQ.ObtenirSecuriteFile(objFile.FormatName, strMSMQSecurite) Then
                        EcrireSecurite(strNomFile, strMSMQSecurite)
                        intNbFile += 1
                    End If
                End While

                MessageBox.Show(intNbFile.ToString + " files inscrites au dépot", "Extraire", _
                    MessageBoxButtons.OK, MessageBoxIcon.Information)

            Case 1  'Appliquer
                'Le appliquer ne tient pas compte des entrées 'refusée' de la sécurité puisqu'elles ne sont pas utilisées
                'à la régie.  Pour en tenir compte, il faudrait aussi modifier le dépôt pour y ajouter l'entrée.

                Dim intNonTrouvee As Integer = 0
                Dim intTrouvee As Integer = 0

                While enuFilePrivee.MoveNext
                    Dim strNomFile As String = enuFilePrivee.Current().queuename
                    Dim strSecurite As String = ObtenirSecurite(strNomFile)

                    If strSecurite <> "" Then
                        Dim objFile As New MessageQueue(".\" + strNomFile)
                        Dim objTsCuMSMQ As New TS5N151_MSMQ.TsCuMSMQ
                        Dim strMSMQSecurite As String

                        objTsCuMSMQ.ObtenirSecuriteFile(objFile.FormatName, strMSMQSecurite)
                        Dim curentsecu As Hashtable = ParcoursSecurite(strMSMQSecurite)

                        If strSecurite <> strMSMQSecurite Then
                            AppliquerSecurite(strNomFile, curentsecu, ParcoursSecurite(strSecurite))
                            intTrouvee += 1
                        End If
                    Else
                        intNonTrouvee += 1
                    End If
                End While

                MessageBox.Show(intNonTrouvee.ToString + " files manquantes au dépôt" + vbCrLf + intTrouvee.ToString + " files sécurisées", "Appliquer", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Select
    End Sub

    Private Sub TsFfMSMQPrive_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RafraichirArbre()
    End Sub

    Private Function EstFileRRQ(ByVal strFile As String) As Boolean
        If strFile.Length > 9 Then
            Return (strFile.Chars(0) = "u" Or strFile.Chars(0) = "i" Or _
                    strFile.Chars(0) = "a" Or strFile.Chars(0) = "b" Or _
                    strFile.Chars(0) = "q" Or strFile.Chars(0) = "p") And _
                    Char.IsLetter(strFile.Chars(1)) And Char.IsLetter(strFile.Chars(2)) And _
                    Char.IsDigit(strFile.Chars(3)) And Char.IsDigit(strFile.Chars(4)) And _
                    Char.IsDigit(strFile.Chars(5)) And Char.IsDigit(strFile.Chars(6)) And _
                    Char.IsDigit(strFile.Chars(7)) And strFile.Chars(8) = "_" And _
                    strFile.LastIndexOf("_") = strFile.IndexOf("_")
        Else
            Return False
        End If
    End Function

    Private Sub RafraichirArbre()
        Dim objTouteFilePrivees() As MessageQueue = MessageQueue.GetPrivateQueuesByMachine("localhost")

        Try
            lstFiles.Items.Clear()

            For Each oFile As MessageQueue In objTouteFilePrivees
                lstFiles.Items.Add(oFile.QueueName)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Function AppliquerSecurite(ByVal strNomFile As String, ByVal htbSecuriteCourante As Hashtable, ByVal htbSecurite As Hashtable) As Boolean
        Dim enuSecuriteCourante As IEnumerator = htbSecuriteCourante.Keys.GetEnumerator
        Dim objFile As New MessageQueue(".\" + strNomFile)
        Dim oMsmqACEothers As MessageQueueAccessControlEntry

        While enuSecuriteCourante.MoveNext
            If htbSecurite.ContainsKey(enuSecuriteCourante.Current) <> True Then
                oMsmqACEothers = New MessageQueueAccessControlEntry(New Trustee(enuSecuriteCourante.Current, ".", TrusteeType.User), 0, AccessControlEntryType.Set)
            Else
                oMsmqACEothers = New MessageQueueAccessControlEntry(New Trustee(enuSecuriteCourante.Current, ".", TrusteeType.User), htbSecurite(enuSecuriteCourante.Current), AccessControlEntryType.Set)
            End If
            objFile.SetPermissions(oMsmqACEothers)


        End While

        enuSecuriteCourante = htbSecurite.Keys.GetEnumerator
        While enuSecuriteCourante.MoveNext
            If htbSecuriteCourante.ContainsKey(enuSecuriteCourante.Current) <> True Then
                oMsmqACEothers = New MessageQueueAccessControlEntry(New Trustee(enuSecuriteCourante.Current, ".", TrusteeType.User), htbSecurite(enuSecuriteCourante.Current), AccessControlEntryType.Set)
                objFile.SetPermissions(oMsmqACEothers)
            End If
        End While
    End Function

    Private Function ParcoursSecurite(ByVal strSecurite As String) As Hashtable
        Dim intPV As Integer = -1
        Dim intDP As Integer = -1
        Dim htbRetour As New Hashtable

        While intDP <> strSecurite.LastIndexOf(":")
            Dim intAncienPV As Integer = intPV

            intPV = strSecurite.IndexOf(";", intPV + 1)
            intDP = strSecurite.IndexOf(":", intDP + 1)

            htbRetour.Add(strSecurite.Substring(intAncienPV + 1, intDP - intAncienPV - 1), strSecurite.Substring(intDP + 1, intPV - intDP - 1))
        End While

        Return htbRetour
    End Function

    Private Function ObtenirSecurite(ByVal strFile As String) As String
        Dim strCheminSecuriteMSMQ As String = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Rrq\\TS5").GetValue("CheminSecuriteMSMQ", "") + Environment.MachineName + ".ini"
        Dim INIFile As New Org.Mentalis.Files.IniReader(strCheminSecuriteMSMQ)

        Return INIFile.ReadString(strFile, "Securite", "")
    End Function

    Private Sub EcrireSecurite(ByVal strFile As String, ByVal strSecuriteMSMQ As String)
        Dim strCheminSecuriteMSMQ As String = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE\\Rrq\\TS5").GetValue("CheminSecuriteMSMQ", "") + Environment.MachineName + ".ini"
        Dim INIFile As New Org.Mentalis.Files.IniReader(strCheminSecuriteMSMQ)

        INIFile.Write(strFile, "Securite", strSecuriteMSMQ)
    End Sub
End Class


Namespace Org.Mentalis.Files
    '/// <summary>
    '/// The INIReader class can read keys from and write keys to an INI file.
    '/// </summary>
    '/// <remarks>
    '/// This class uses several Win32 API functions to read from and write to INI files. It will not work on Linux or FreeBSD.
    '/// </remarks>
    Public Class IniReader
        '/// <summary>
        '/// The GetPrivateProfileInt function retrieves an integer associated with a key in the specified section of an initialization file.
        '/// </summary>
        '/// <param name="lpApplicationName">Pointer to a null-terminated string specifying the name of the section in the initialization file.</param>
        '/// <param name="lpKeyName">Pointer to the null-terminated string specifying the name of the key whose value is to be retrieved. This value is in the form of a string; the GetPrivateProfileInt function converts the string into an integer and returns the integer.</param>
        '/// <param name="nDefault">Specifies the default value to return if the key name cannot be found in the initialization file.</param>
        '/// <param name="lpFileName">Pointer to a null-terminated string that specifies the name of the initialization file. If this parameter does not contain a full path to the file, the system searches for the file in the Windows directory.</param>
        '/// <returns>The return value is the integer equivalent of the string following the specified key name in the specified initialization file. If the key is not found, the return value is the specified default value. If the value of the key is less than zero, the return value is zero.</returns>
        Private Declare Ansi Function GetPrivateProfileInt Lib "kernel32.dll" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer
        '/// <summary>
        '/// The WritePrivateProfileString function copies a string into the specified section of an initialization file.
        '/// </summary>
        '/// <param name="lpApplicationName">Pointer to a null-terminated string containing the name of the section to which the string will be copied. If the section does not exist, it is created. The name of the section is case-independent; the string can be any combination of uppercase and lowercase letters.</param>
        '/// <param name="lpKeyName">Pointer to the null-terminated string containing the name of the key to be associated with a string. If the key does not exist in the specified section, it is created. If this parameter is NULL, the entire section, including all entries within the section, is deleted.</param>
        '/// <param name="lpString">Pointer to a null-terminated string to be written to the file. If this parameter is NULL, the key pointed to by the lpKeyName parameter is deleted.</param>
        '/// <param name="lpFileName">Pointer to a null-terminated string that specifies the name of the initialization file.</param>
        '/// <returns>If the function successfully copies the string to the initialization file, the return value is nonzero; if the function fails, or if it flushes the cached version of the most recently accessed initialization file, the return value is zero.</returns>
        Private Declare Ansi Function WritePrivateProfileString Lib "kernel32.dll" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
        '/// <summary>
        '/// The GetPrivateProfileString function retrieves a string from the specified section in an initialization file.
        '/// </summary>
        '/// <param name="lpApplicationName">Pointer to a null-terminated string that specifies the name of the section containing the key name. If this parameter is NULL, the GetPrivateProfileString function copies all section names in the file to the supplied buffer.</param>
        '/// <param name="lpKeyName">Pointer to the null-terminated string specifying the name of the key whose associated string is to be retrieved. If this parameter is NULL, all key names in the section specified by the lpAppName parameter are copied to the buffer specified by the lpReturnedString parameter.</param>
        '/// <param name="lpDefault">Pointer to a null-terminated default string. If the lpKeyName key cannot be found in the initialization file, GetPrivateProfileString copies the default string to the lpReturnedString buffer. This parameter cannot be NULL. <br>Avoid specifying a default string with trailing blank characters. The function inserts a null character in the lpReturnedString buffer to strip any trailing blanks.</br></param>
        '/// <param name="lpReturnedString">Pointer to the buffer that receives the retrieved string.</param>
        '/// <param name="nSize">Specifies the size, in TCHARs, of the buffer pointed to by the lpReturnedString parameter.</param>
        '/// <param name="lpFileName">Pointer to a null-terminated string that specifies the name of the initialization file. If this parameter does not contain a full path to the file, the system searches for the file in the Windows directory.</param>
        '/// <returns>The return value is the number of characters copied to the buffer, not including the terminating null character.</returns>
        Private Declare Ansi Function GetPrivateProfileString Lib "kernel32.dll" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
        '/// <summary>
        '/// The GetPrivateProfileSectionNames function retrieves the names of all sections in an initialization file.
        '/// </summary>
        '/// <param name="lpszReturnBuffer">Pointer to a buffer that receives the section names associated with the named file. The buffer is filled with one or more null-terminated strings; the last string is followed by a second null character.</param>
        '/// <param name="nSize">Specifies the size, in TCHARs, of the buffer pointed to by the lpszReturnBuffer parameter.</param>
        '/// <param name="lpFileName">Pointer to a null-terminated string that specifies the name of the initialization file. If this parameter is NULL, the function searches the Win.ini file. If this parameter does not contain a full path to the file, the system searches for the file in the Windows directory.</param>
        '/// <returns>The return value specifies the number of characters copied to the specified buffer, not including the terminating null character. If the buffer is not large enough to contain all the section names associated with the specified initialization file, the return value is equal to the length specified by nSize minus two.</returns>
        Private Declare Ansi Function GetPrivateProfileSectionNames Lib "kernel32" Alias "GetPrivateProfileSectionNamesA" (ByVal lpszReturnBuffer() As Byte, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
        '/// <summary>
        '/// The WritePrivateProfileSection function replaces the keys and values for the specified section in an initialization file.
        '/// </summary>
        '/// <param name="lpAppName">Pointer to a null-terminated string specifying the name of the section in which data is written. This section name is typically the name of the calling application.</param>
        '/// <param name="lpString">Pointer to a buffer containing the new key names and associated values that are to be written to the named section.</param>
        '/// <param name="lpFileName">Pointer to a null-terminated string containing the name of the initialization file. If this parameter does not contain a full path for the file, the function searches the Windows directory for the file. If the file does not exist and lpFileName does not contain a full path, the function creates the file in the Windows directory. The function does not create a file if lpFileName contains the full path and file name of a file that does not exist.</param>
        '/// <returns>If the function succeeds, the return value is nonzero.<br>If the function fails, the return value is zero.</br></returns>
        Private Declare Ansi Function WritePrivateProfileSection Lib "kernel32.dll" Alias "WritePrivateProfileSectionA" (ByVal lpAppName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer
        '/// <summary>Constructs a new IniReader instance.</summary>
        '/// <param name="file">Specifies the full path to the INI file (the file doesn't have to exist).</param>
        Public Sub New(ByVal file As String)
            Filename = file
        End Sub
        '/// <summary>Gets or sets the full path to the INI file.</summary>
        '/// <value>A String representing the full path to the INI file.</value>
        Public Property Filename() As String
            Get
                Return m_Filename
            End Get
            Set(ByVal Value As String)
                m_Filename = Value
            End Set
        End Property
        '/// <summary>Gets or sets the section you're working in. (aka 'the active section')</summary>
        '/// <value>A String representing the section you're working in.</value>
        Public Property Section() As String
            Get
                Return m_Section
            End Get
            Set(ByVal Value As String)
                m_Section = Value
            End Set
        End Property
        '/// <summary>Reads an Integer from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <param name="defVal">The value to return if the specified key isn't found.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns the default value if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadInteger(ByVal section As String, ByVal key As String, ByVal defVal As Integer) As Integer
            Return GetPrivateProfileInt(section, key, defVal, Filename)
        End Function
        '/// <summary>Reads an Integer from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns 0 if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadInteger(ByVal section As String, ByVal key As String) As Integer
            Return ReadInteger(section, key, 0)
        End Function
        '/// <summary>Reads an Integer from the specified key of the active section.</summary>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <param name="defVal">The section to search in.</param>
        '/// <returns>Returns the value of the specified Key, or returns the default value if the specified Key isn't found in the active section of the INI file.</returns>
        Public Function ReadInteger(ByVal key As String, ByVal defVal As Integer) As Integer
            Return ReadInteger(Section, key, defVal)
        End Function
        '/// <summary>Reads an Integer from the specified key of the active section.</summary>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified key, or returns 0 if the specified key isn't found in the active section of the INI file.</returns>
        Public Function ReadInteger(ByVal key As String) As Integer
            Return ReadInteger(key, 0)
        End Function
        '/// <summary>Reads a String from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <param name="defVal">The value to return if the specified key isn't found.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns the default value if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadString(ByVal section As String, ByVal key As String, ByVal defVal As String) As String
            Dim sb As New StringBuilder(MAX_ENTRY)
            Dim Ret As Integer = GetPrivateProfileString(section, key, defVal, sb, MAX_ENTRY, Filename)
            Return sb.ToString()
        End Function
        '/// <summary>Reads a String from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns an empty String if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadString(ByVal section As String, ByVal key As String) As String
            Return ReadString(section, key, "")
        End Function
        '/// <summary>Reads a String from the specified key of the active section.</summary>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified key, or returns an empty String if the specified key isn't found in the active section of the INI file.</returns>
        Public Function ReadString(ByVal key As String) As String
            Return ReadString(Section, key)
        End Function
        '/// <summary>Reads a Long from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <param name="defVal">The value to return if the specified key isn't found.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns the default value if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadLong(ByVal section As String, ByVal key As String, ByVal defVal As Long) As Long
            Return Long.Parse(ReadString(section, key, defVal.ToString()))
        End Function
        '/// <summary>Reads a Long from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns 0 if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadLong(ByVal section As String, ByVal key As String) As Long
            Return ReadLong(section, key, 0)
        End Function
        '/// <summary>Reads a Long from the specified key of the active section.</summary>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <param name="defVal">The section to search in.</param>
        '/// <returns>Returns the value of the specified key, or returns the default value if the specified key isn't found in the active section of the INI file.</returns>
        Public Function ReadLong(ByVal key As String, ByVal defVal As Long) As Long
            Return ReadLong(Section, key, defVal)
        End Function
        '/// <summary>Reads a Long from the specified key of the active section.</summary>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified Key, or returns 0 if the specified Key isn't found in the active section of the INI file.</returns>
        Public Function ReadLong(ByVal key As String) As Long
            Return ReadLong(key, 0)
        End Function
        '/// <summary>Reads a Byte array from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns null (Nothing in VB.NET) if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadByteArray(ByVal section As String, ByVal key As String) As Byte()
            Try
                Return Convert.FromBase64String(ReadString(section, key))
            Catch
            End Try
            Return Nothing
        End Function
        '/// <summary>Reads a Byte array from the specified key of the active section.</summary>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified key, or returns null (Nothing in VB.NET) if the specified key pair isn't found in the active section of the INI file.</returns>
        Public Function ReadByteArray(ByVal key As String) As Byte()
            Return ReadByteArray(Section, key)
        End Function
        '/// <summary>Reads a Boolean from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <param name="defVal">The value to return if the specified key isn't found.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns the default value if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadBoolean(ByVal section As String, ByVal key As String, ByVal defVal As Boolean) As Boolean
            Return Boolean.Parse(ReadString(section, key, defVal.ToString()))
        End Function
        '/// <summary>Reads a Boolean from the specified key of the specified section.</summary>
        '/// <param name="section">The section to search in.</param>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified section/key pair, or returns false if the specified section/key pair isn't found in the INI file.</returns>
        Public Function ReadBoolean(ByVal section As String, ByVal key As String) As Boolean
            Return ReadBoolean(section, key, False)
        End Function
        '/// <summary>Reads a Boolean from the specified key of the specified section.</summary>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <param name="defVal">The value to return if the specified key isn't found.</param>
        '/// <returns>Returns the value of the specified key pair, or returns the default value if the specified key isn't found in the active section of the INI file.</returns>
        Public Function ReadBoolean(ByVal key As String, ByVal defVal As Boolean) As Boolean
            Return ReadBoolean(Section, key, defVal)
        End Function
        '/// <summary>Reads a Boolean from the specified key of the specified section.</summary>
        '/// <param name="key">The key from which to return the value.</param>
        '/// <returns>Returns the value of the specified key, or returns false if the specified key isn't found in the active section of the INI file.</returns>
        Public Function ReadBoolean(ByVal key As String) As Boolean
            Return ReadBoolean(Section, key)
        End Function
        '/// <summary>Writes an Integer to the specified key in the specified section.</summary>
        '/// <param name="section">The section to write in.</param>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal section As String, ByVal key As String, ByVal value As Integer) As Boolean
            Return Write(section, key, value.ToString())
        End Function
        '/// <summary>Writes an Integer to the specified key in the active section.</summary>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal key As String, ByVal value As Integer) As Boolean
            Return Write(Section, key, value)
        End Function
        '/// <summary>Writes a String to the specified key in the specified section.</summary>
        '/// <param name="section">Specifies the section to write in.</param>
        '/// <param name="key">Specifies the key to write to.</param>
        '/// <param name="value">Specifies the value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal section As String, ByVal key As String, ByVal value As String) As Boolean
            Return (WritePrivateProfileString(section, key, value, Filename) <> 0)
        End Function
        '/// <summary>Writes a String to the specified key in the active section.</summary>
        '///	<param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal key As String, ByVal value As String) As Boolean
            Return Write(Section, key, value)
        End Function
        '/// <summary>Writes a Long to the specified key in the specified section.</summary>
        '/// <param name="section">The section to write in.</param>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal section As String, ByVal key As String, ByVal value As Long) As Boolean
            Return Write(section, key, value.ToString())
        End Function
        '/// <summary>Writes a Long to the specified key in the active section.</summary>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal key As String, ByVal value As Long) As Boolean
            Return Write(Section, key, value)
        End Function
        '/// <summary>Writes a Byte array to the specified key in the specified section.</summary>
        '/// <param name="section">The section to write in.</param>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal section As String, ByVal key As String, ByVal value() As Byte) As Boolean
            If value Is Nothing Then
                Return Write(section, key, CType(Nothing, String))
            Else
                Return Write(section, key, value, 0, value.Length)
            End If
        End Function
        '/// <summary>Writes a Byte array to the specified key in the active section.</summary>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal key As String, ByVal value() As Byte) As Boolean
            Return Write(Section, key, value)
        End Function
        '/// <summary>Writes a Byte array to the specified key in the specified section.</summary>
        '/// <param name="section">The section to write in.</param>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <param name="offset">An offset in <i>value</i>.</param>
        '/// <param name="length">The number of elements of <i>value</i> to convert.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal section As String, ByVal key As String, ByVal value() As Byte, ByVal offset As Integer, ByVal length As Integer) As Boolean
            If value Is Nothing Then
                Return Write(section, key, CType(Nothing, String))
            Else
                Return Write(section, key, Convert.ToBase64String(value, offset, length))
            End If
        End Function
        '/// <summary>Writes a Boolean to the specified key in the specified section.</summary>
        '/// <param name="section">The section to write in.</param>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal section As String, ByVal key As String, ByVal value As Boolean) As Boolean
            Return Write(section, key, value.ToString())
        End Function
        '/// <summary>Writes a Boolean to the specified key in the active section.</summary>
        '/// <param name="key">The key to write to.</param>
        '/// <param name="value">The value to write.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function Write(ByVal key As String, ByVal value As Boolean) As Boolean
            Return Write(Section, key, value)
        End Function
        '/// <summary>Deletes a key from the specified section.</summary>
        '/// <param name="section">The section to delete from.</param>
        '/// <param name="key">The key to delete.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function DeleteKey(ByVal section As String, ByVal key As String) As Boolean
            Return (WritePrivateProfileString(section, key, Nothing, Filename) <> 0)
        End Function
        '/// <summary>Deletes a key from the active section.</summary>
        '/// <param name="key">The key to delete.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function DeleteKey(ByVal key As String) As Boolean
            Return (WritePrivateProfileString(Section, key, Nothing, Filename) <> 0)
        End Function
        '/// <summary>Deletes a section from an INI file.</summary>
        '/// <param name="section">The section to delete.</param>
        '/// <returns>Returns true if the function succeeds, false otherwise.</returns>
        Public Function DeleteSection(ByVal section As String) As Boolean
            Return WritePrivateProfileSection(section, Nothing, Filename) <> 0
        End Function
        '/// <summary>Retrieves a list of all available sections in the INI file.</summary>
        '/// <returns>Returns an ArrayList with all available sections.</returns>
        Public Function GetSectionNames() As ArrayList
            Try
                Dim buffer(MAX_ENTRY) As Byte
                GetPrivateProfileSectionNames(buffer, MAX_ENTRY, Filename)
                Dim parts() As String = Encoding.ASCII.GetString(buffer).Trim(ControlChars.NullChar).Split(ControlChars.NullChar)
                Return New ArrayList(parts)
            Catch
            End Try
            Return Nothing
        End Function
        'Private variables and constants
        '/// <summary>
        '/// Holds the full path to the INI file.
        '/// </summary>
        Private m_Filename As String
        '/// <summary>
        '/// Holds the active section name
        '/// </summary>
        Private m_Section As String
        '/// <summary>
        '/// The maximum number of bytes in a section buffer.
        '/// </summary>
        Private Const MAX_ENTRY As Integer = 32768
    End Class
End Namespace