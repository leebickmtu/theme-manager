Imports System.IO

Module Dll64Detect

    Public Function UnmanagedDllIs64Bit(ByVal dllPath As String) As System.Nullable(Of Boolean)
        Select Case GetDllMachineType(dllPath)
            Case MachineType.IMAGE_FILE_MACHINE_AMD64, MachineType.IMAGE_FILE_MACHINE_IA64
                Return True
            Case MachineType.IMAGE_FILE_MACHINE_I386
                Return False
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function GetDllMachineType(ByVal dllPath As String) As MachineType
        'see http://www.microsoft.com/whdc/system/platform/firmware/PECOFF.mspx
        'offset to PE header is always at 0x3C
        'PE header starts with "PE\0\0" =  0x50 0x45 0x00 0x00
        'followed by 2-byte machine type field (see document above for enum)
        Dim fs As New FileStream(dllPath, FileMode.Open)
        Dim br As New BinaryReader(fs)
        fs.Seek(&H3C, SeekOrigin.Begin)
        Dim peOffset As Int32 = br.ReadInt32()
        fs.Seek(peOffset, SeekOrigin.Begin)
        Dim peHead As UInt32 = br.ReadUInt32()
        If peHead <> &H4550 Then
            ' "PE\0\0", little-endian
            Throw New Exception("Can't find PE header")
        End If
        Dim machineType As MachineType = DirectCast(br.ReadUInt16(), MachineType)
        br.Close()
        fs.Close()
        Return machineType
    End Function

    Public Enum MachineType As UShort
        IMAGE_FILE_MACHINE_UNKNOWN = &H0
        IMAGE_FILE_MACHINE_AM33 = &H1D3
        IMAGE_FILE_MACHINE_AMD64 = &H8664
        IMAGE_FILE_MACHINE_ARM = &H1C0
        IMAGE_FILE_MACHINE_EBC = &HEBC
        IMAGE_FILE_MACHINE_I386 = &H14C
        IMAGE_FILE_MACHINE_IA64 = &H200
        IMAGE_FILE_MACHINE_M32R = &H9041
        IMAGE_FILE_MACHINE_MIPS16 = &H266
        IMAGE_FILE_MACHINE_MIPSFPU = &H366
        IMAGE_FILE_MACHINE_MIPSFPU16 = &H466
        IMAGE_FILE_MACHINE_POWERPC = &H1F0
        IMAGE_FILE_MACHINE_POWERPCFP = &H1F1
        IMAGE_FILE_MACHINE_R4000 = &H166
        IMAGE_FILE_MACHINE_SH3 = &H1A2
        IMAGE_FILE_MACHINE_SH3DSP = &H1A3
        IMAGE_FILE_MACHINE_SH4 = &H1A6
        IMAGE_FILE_MACHINE_SH5 = &H1A8
        IMAGE_FILE_MACHINE_THUMB = &H1C2
        IMAGE_FILE_MACHINE_WCEMIPSV2 = &H169
    End Enum

End Module
