Imports System.ComponentModel.Composition
Imports System.IO
Imports System.Web
Imports System.Xml
Imports mes.LightHouse
Imports PriPROC6.Interface.Web

<Export(GetType(xmlFeed))>
<ExportMetadata("EndPoint", "trans")>
<ExportMetadata("Hidden", False)>
Public Class trans : Inherits iFeed : Implements xmlFeed

    ''' <summary>
    ''' Override the processing of the current context.
    ''' </summary>
    ''' <param name="context">The current HTTP context</param>
    Overrides Sub ProcessReq(ByVal context As HttpContext)

        log.LogData.Append("Checking for outbound transactions...").AppendLine()

        Using objX As New XmlTextWriter(context.Response.OutputStream, Nothing)
            With objX
                .WriteStartDocument()
                .WriteStartElement("OutboundTransactions")

                Using client As New CERE_PriorityErpInterfaceClient()
                    Dim resp As SfolWcfReturn = client.GetOutboundTransactions
                    If Not resp Is Nothing Then
                        If Not resp.OutboundTransactions Is Nothing Then
                            log.LogData.Append(resp.OutboundTransactions.ToString).AppendLine()

                            Dim doc As New XmlDocument
                            With doc
                                .LoadXml(resp.OutboundTransactions.ToString)
                                .Save(
                                    String.Format(
                                        "C:\inetpub\api\trans\{0}.xml",
                                        System.Guid.NewGuid.ToString
                                    )
                                )

                                For Each OutboundTransaction As XmlNode In .SelectNodes("OutboundTransactions\OutboundTransaction")
                                    objX.WriteRaw(OutboundTransaction.InnerXml)

                                Next

                            End With

                        End If
                    End If

                End Using

                .WriteEndElement()
                .WriteEndDocument()

            End With
        End Using

    End Sub


End Class
