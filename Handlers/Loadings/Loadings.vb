'Imports System.Xml
'Imports PriPROC6.Interface.Web

'Public Module Loadings

'    Public Function WOCompletion(ByRef Trans As OutboundTransaction) As priForm
'        With Trans
'            Using AFORM As New priForm("ALINE_ONE",
'                "SERIALNAME",
'                "ACTNAME"
'            )
'                Dim TRANSORDER_S = AFORM.AddForm("TRANSORDER_S",
'                    "PARTNAME",
'                    "SERIALNAME",
'                    "QUANT"
'                )

'                Dim form As priRow = AFORM.AddRow(
'                        .WONumber,
'                        .Operation
'                    )

'                For Each iConsumed As Inventory In .InventoryConsumed
'                    With iConsumed
'                        TRANSORDER_S.AddRow(form,
'                        .InventoryPackPart,
'                        .InventoryPackNo,
'                        .Quantity
'                    )
'                    End With

'                Next

'                For Each iCreated As Inventory In .InventoryCreated
'                    With iCreated
'                        TRANSORDER_S.AddRow(form,
'                        .InventoryPackPart,
'                        .InventoryPackNo,
'                        .Quantity
'                    )
'                    End With

'                Next

'                Return AFORM

'            End Using

'        End With

'    End Function

'    Public Function BuildRecord(ByRef Trans As OutboundTransaction) As priForm
'        With Trans
'            Using AFORM As New priForm("ALINE_ONE",
'                "SERIALNAME",
'                "ACTNAME"
'            )
'                Dim TRANSORDER_S = AFORM.AddForm("TRANSORDER_S",
'                    "PARTNAME",
'                    "SERIALNAME",
'                    "WARHSNAME",
'                    "QUANT"
'                )

'                Dim form As priRow = AFORM.AddRow(
'                        .WONumber,
'                        .Operation
'                    )

'                TRANSORDER_S.AddRow(form,
'                    .Part,
'                    .InventoryPackNo,
'                    .Location,
'                    .Quantity
'                )

'                Return AFORM

'            End Using

'        End With

'    End Function

'    Public Function InventoryUsage(ByRef Trans As OutboundTransaction) As priForm
'        With Trans

'            Using AFORM As New priForm("ALINE_ONE",
'                "SERIALNAME"
'            )
'                Dim TRANSORDER_S = AFORM.AddForm("TRANSORDER_S",
'                    "PARTNAME",
'                    "#QUANT"
'                )

'                Dim form As priRow = AFORM.AddRow(
'                        .WONumber
'                    )

'                TRANSORDER_S.AddRow(form,
'                    .Part,
'                    .Quantity
'                )

'                Return AFORM

'            End Using

'        End With

'    End Function

'End Module
