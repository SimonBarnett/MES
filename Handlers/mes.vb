Imports System.ComponentModel.Composition
Imports System.Xml
Imports System.Web
Imports PriPROC6.Interface.Web
Imports System.IO
Imports Newtonsoft.Json
Imports mes.LightHouse

<Export(GetType(xmlHandler))>
<ExportMetadata("EndPoint", "mes")>
<ExportMetadata("Hidden", False)>
Public Class mesHandler : Inherits iHandler : Implements xmlHandler

    Enum eTransaction
        InventoryPacks
        WorksOrders
    End Enum

    Public Overrides Sub XMLHandler(ByRef w As XmlTextWriter, ByRef Request As XmlDocument)

        With w
            '.WriteStartDocument()
            .WriteStartElement("response")
            .WriteAttributeString("status", CStr(200))
            .WriteAttributeString("bubbleid", BubbleID)
            .WriteAttributeString("message", "OK")
            .WriteAttributeString("stacktr", "")
        End With

        Dim transType As eTransaction
            Select Case Request.FirstChild.Name.ToLower
                Case "InventoryPacks".ToLower
                    transType = eTransaction.InventoryPacks

                Case "WorksOrders".ToLower
                    transType = eTransaction.WorksOrders

                Case Else
                    Throw New Exception("Unknown format.")

            End Select

        For Each trans As XmlNode In Request.SelectNodes(
            String.Format(
                "{0}/{1}",
                Request.FirstChild.Name,
                Request.FirstChild.Name.Substring(
                    0,
                    Len(Request.FirstChild.Name) - 1
                )
            )
        )

            Dim req As New SfolRequestDocument
            Dim upd As New MemoryStream
            Using objX As New XmlTextWriter(upd, Nothing)
                With objX
                    .WriteStartDocument()
                    .WriteStartElement(Request.FirstChild.Name)
                    .WriteRaw(trans.InnerXml)
                    .WriteEndElement()
                    .WriteEndDocument()
                    .Flush()
                End With
                upd.Position = 0
                req.XmlDocument = XDocument.Load(upd).FirstNode

            End Using

            Dim id As String
            Dim resp As SfolWcfReturn
            Using client As New CERE_PriorityErpInterfaceClient()
                Select Case transType
                    Case eTransaction.InventoryPacks
                        id = trans.SelectSingleNode("InventoryPackNo").InnerText
                        resp = client.PutInventoryPack(req)

                    Case Else 'eTransaction.WorksOrders
                        id = trans.SelectSingleNode("WONumber").InnerText
                        resp = client.PutWorksOrder(req)

                End Select

            End Using

            With w
                .WriteStartElement("row")
                .WriteAttributeString("id", id)
                .WriteAttributeString("sucsess", resp.ProcessingSuccess)
                .WriteAttributeString("msg", resp.ErrorMessage)
                .WriteEndElement()

            End With

        Next

    End Sub

End Class