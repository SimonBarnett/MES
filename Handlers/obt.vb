Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports System.IO
Imports Newtonsoft.Json
Imports System.Xml.Serialization
Imports mes.LightHouse
Imports System.Reflection

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "obt")>
<ExportMetadata("Hidden", False)>
Public Class obt : Inherits iHandler : Implements xmlHandler

    Public Overrides Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)

        Dim upd As New MemoryStream
        Using objX As New XmlTextWriter(upd, Nothing)

            objX.WriteStartDocument()
            objX.WriteStartElement("OutboundTransactions")

            For Each trans As XmlNode In Request.SelectNodes("OutboundTransactions/OutboundTransaction")

                Dim t As New OutboundTransaction(trans)

                objX.WriteStartElement("OutboundTransaction")
                objX.WriteElementString("ERPOutboundTransactionID", t.ERPOutboundTransactionID)

                Try
                    Using f As New ALINE_ONE(Assembly.GetExecutingAssembly)
                        With f.AddRow()
                            .SERIALNAME = t.WONumber
                            .ACTNAME = t.Operation
                            If Not .Post Then Throw .Exception

                            Select Case t.SourceTransaction
                                Case OutboundTransaction.eSourceTransaction.BuildRecord
                                    With .TRANSORDER_S.AddRow
                                        .PARTNAME = t.Part
                                        .SERIALNAME = t.InventoryPackNo
                                        .WARHSNAME = t.Location
                                        .QUANT = t.Quantity
                                        If Not .Post Then Throw .Exception

                                    End With

                                Case OutboundTransaction.eSourceTransaction.InventoryUsage
                                    With .TRANSORDER_S.AddRow
                                        .PARTNAME = t.Part
                                        .QUANT = t.Quantity
                                        If Not .Post Then Throw .Exception

                                    End With

                                Case Else 'OutboundTransaction.eSourceTransaction.WOCompletion
                                    For Each iConsumed As Inventory In t.InventoryConsumed
                                        With .TRANSORDER_S.AddRow
                                            .PARTNAME = iConsumed.InventoryPackPart
                                            .SERIALNAME = iConsumed.InventoryPackNo
                                            .QUANT = iConsumed.Quantity
                                            If Not .Post Then Throw .Exception

                                        End With

                                    Next

                                    For Each iCreated As Inventory In t.InventoryCreated
                                        With .TRANSORDER_S.AddRow
                                            .PARTNAME = iCreated.InventoryPackPart
                                            .SERIALNAME = iCreated.InventoryPackNo
                                            .QUANT = iCreated.Quantity
                                            If Not .Post Then Throw .Exception

                                        End With

                                    Next

                            End Select

                        End With

                        objX.WriteElementString("ErpProcessingStatus", "Processed")
                        objX.WriteElementString("ErrorMessage", "")

                    End Using


                Catch ex As Exception
                    objX.WriteElementString("ErpProcessingStatus", "Error")
                    objX.WriteElementString("ErrorMessage", ex.Message)

                Finally
                    objX.WriteEndElement()

                End Try

            Next

            objX.WriteEndElement()
            objX.WriteEndDocument()

            Using client As New CERE_PriorityErpInterfaceClient()
                Dim req As New SfolRequestDocument
                objX.Flush()
                upd.Position = 0
                req.OutboundTransactionsUpdate = XDocument.Load(upd).FirstNode
                client.PutOutboundTransactionUpdate(req)

            End Using

        End Using

        With w
            .WriteStartElement("response")
            .WriteAttributeString("status", CStr(200))
            .WriteAttributeString("bubbleid", BubbleID)
            .WriteAttributeString("message", "OK")
            .WriteAttributeString("stacktr", "")
            .WriteEndElement() 'End Settings 

        End With

    End Sub

End Class