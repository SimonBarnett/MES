Imports System.Xml
Imports PriPROC6.Interface.Web

Public MustInherit Class Xhelper
    Public Function xValue(ByRef Trans As XmlNode, Name As String) As String
        If Not Trans.SelectSingleNode(Name) Is Nothing Then
            Return Trans.SelectSingleNode(Name).InnerText
        Else
            Return Nothing

        End If
    End Function

End Class

Public Class OutboundTransaction : Inherits Xhelper

#Region "Properties"
    Enum eSourceTransaction
        InventoryUsage
        BuildRecord
        WOCompletion
    End Enum

    Public ERPOutboundTransactionID As Integer
    Public CreatedDT As String
    Public SourceTransaction As eSourceTransaction
    Public WONumber As String
    Public CreatedPart As String
    Public CreatedUnits As String
    Public ConsumedPart As String
    Public ConsumedUnits As String
    Public ConsumedInventoryPackNo As String
    Public CreatedInventoryPackNo As String
    Public Part As String
    Public TotalCreatedQuantity As Integer
    Public Units As String
    Public InventoryPackNo As String
    Public Reference1 As String
    Public Reference2 As String
    Public Reference3 As String
    Public Reference4 As String
    Public Reference5 As String
    Public Location As String
    Public LocationReference1 As String
    Public LocationReference2 As String
    Public LocationReference3 As String
    Public ConsumedQuantity As Integer
    Public CreatedQuantity As Integer
    Public Quantity As Integer
    Public Operation As String

    Public InventoryConsumed As New List(Of Inventory)
    Public InventoryCreated As New List(Of Inventory)

#End Region

    Public ReadOnly Property AFORM As priForm
        Get
            Select Case SourceTransaction
                Case eSourceTransaction.InventoryUsage
                    SourceTransaction = eSourceTransaction.InventoryUsage
                    Return InventoryUsage(Me)

                Case eSourceTransaction.BuildRecord
                    SourceTransaction = eSourceTransaction.BuildRecord
                    Return BuildRecord(Me)

                Case Else 'eSourceTransaction.WOCompletion
                    SourceTransaction = eSourceTransaction.WOCompletion
                    Return WOCompletion(Me)

            End Select
        End Get
    End Property

    Sub New(ByRef Trans As XmlNode)

        ERPOutboundTransactionID = xValue(Trans, "ERPOutboundTransactionID")
        CreatedDT = xValue(Trans, "CreatedDT")
        WONumber = xValue(Trans, "WONumber")
        CreatedPart = xValue(Trans, "CreatedPart")
        CreatedUnits = xValue(Trans, "CreatedUnits")
        ConsumedPart = xValue(Trans, "ConsumedPart")
        ConsumedUnits = xValue(Trans, "ConsumedUnits")
        ConsumedInventoryPackNo = xValue(Trans, "ConsumedInventoryPackNo")
        CreatedInventoryPackNo = xValue(Trans, "CreatedInventoryPackNo")
        Part = xValue(Trans, "Part")
        TotalCreatedQuantity = xValue(Trans, "TotalCreatedQuantity")
        Units = xValue(Trans, "Units")
        InventoryPackNo = xValue(Trans, "InventoryPackNo")
        Reference1 = xValue(Trans, "Reference1")
        Reference2 = xValue(Trans, "Reference2")
        Reference3 = xValue(Trans, "Reference3")
        Reference4 = xValue(Trans, "Reference4")
        Reference5 = xValue(Trans, "Reference5")
        Location = xValue(Trans, "Location")
        LocationReference1 = xValue(Trans, "LocationReference1")
        LocationReference2 = xValue(Trans, "LocationReference2")
        LocationReference3 = xValue(Trans, "LocationReference3")
        ConsumedQuantity = xValue(Trans, "ConsumedQuantity")
        CreatedQuantity = xValue(Trans, "CreatedQuantity")
        Quantity = xValue(Trans, "Quantity")
        Operation = xValue(Trans, "Operation")

        Select Case Trans.SelectSingleNode("SourceTransaction").InnerText.ToLower
            Case "InventoryUsage".ToLower
                SourceTransaction = eSourceTransaction.InventoryUsage

            Case "BuildRecord".ToLower
                SourceTransaction = eSourceTransaction.BuildRecord

            Case "WOCompletion".ToLower
                SourceTransaction = eSourceTransaction.WOCompletion

            Case Else
                Throw New Exception("Invalid <SourceTransaction>.")

        End Select

        For Each iCreated As XmlNode In Trans.SelectNodes("InventoryCreated")
            InventoryCreated.Add(New Inventory(iCreated))

        Next

        For Each iConsumed As XmlNode In Trans.SelectNodes("InventoryConsumed")
            InventoryConsumed.Add(New Inventory(iConsumed))

        Next

    End Sub

End Class

Public Class Inventory : Inherits Xhelper

    Public InventoryPackPart As String
    Public InventoryPackNo As String
    Public Quantity As Integer
    Public Units As String
    Public Reference1 As String
    Public Reference2 As String
    Public Reference3 As String
    Public Reference4 As String
    Public Reference5 As String

    Sub New(ByRef Inventory As XmlNode)
        InventoryPackPart = xValue(Inventory, "InventoryPackPart")
        InventoryPackNo = xValue(Inventory, "InventoryPackNo")
        Quantity = xValue(Inventory, "Quantity")
        Units = xValue(Inventory, "Units")
        Reference1 = xValue(Inventory, "Reference1")
        Reference2 = xValue(Inventory, "Reference2")
        Reference3 = xValue(Inventory, "Reference3")
        Reference4 = xValue(Inventory, "Reference4")
        Reference5 = xValue(Inventory, "Reference5")

    End Sub

End Class