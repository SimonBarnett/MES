Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports System.IO
Imports Newtonsoft.Json
Imports System.Xml.Serialization
Imports mes.LightHouse

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
                objX.WriteStartElement("OutboundTransaction")

                Dim t As New OutboundTransaction(trans)
                objX.WriteElementString("ERPOutboundTransactionID", t.ERPOutboundTransactionID)

                Try
                    Using aform As priForm = t.AFORM
                        Dim ex As Exception = Nothing
                        aform.Post(ex)
                        If Not TypeOf ex Is apiResponse Then Throw (ex)
                        With TryCast(ex, apiResponse)
                            For Each MSG As apiError In .msgs
                                If Not MSG.Loaded Then
                                    log.LogData.AppendFormat("Line {0}: {1}.", MSG.Line, MSG.message).AppendLine()
                                    Throw New Exception(MSG.message)

                                End If
                            Next

                            objX.WriteElementString("ErpProcessingStatus", "Processed")
                            objX.WriteElementString("ErrorMessage", "")

                        End With

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