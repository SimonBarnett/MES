<h1>MES Integration</h1>
<h2><a href="/Reference/sql/SP/sp_LoadMES.sql">sp_LoadMES.sql</a></h2>
<p>
    This SQL Procedure imports available data from the MES:
    <li>Gets OutboundTransactions from the <a href="/feeds/trans.vb">trans.ashx</a> feed.
    <li>Posts the transactions to the <a href="/handlers/obt.vb">obt.ashx</a> handler for processing.
    <li><a href="handlers/obt.vb">obt.ashx</a> deserialises transactions into an array of <a href="/handlers/loadings/OutboundTransaction.vb">OutboundTransaction</a>s.
    <li>Depending on the &lt;SourceTransaction&gt; the handler builds the appropriate <a href="/handlers/loadings/Loadings.vb">loading</a> for each transaction.
    <li>The handler then provides sucsess/failure reports back to the MES.
</p>
<h2><a href="/Reference/sql/SP/sp_MESInventoryPacks.sql">sp_MESInventoryPacks.sql</a></h2>
<p>
    This SQL Procedure notifies the <a href="/handlers/mes.vb">mes.ashx</a> handler of a new InventoryPack:
    <li>Un-sent inventory packs are identified by the <a href="/Reference/sql/UDF/sp_InventoryPack.sql">sp_InventoryPack XML function</a>.
    <li>The <a href="/handlers/mes.vb">mes.ashx</a> handler returns a table containing record ID, Sucsess and Message, to set the InventoryPack SENT flag.
</p>
<h2><a href="/Reference/sql/SP/sp_MESWorksOrders.sql">sp_MESWorksOrders.sql</a></h2>    
<p>
    This SQL Procedure notifies the <a href="/handlers/mes.vb">mes.ashx</a> handler of a new WorksOrder:
    <li>Un-sent works orders are identified by the <a href="/Reference/sql/UDF/sp_WorksOrders.sql">sp_WorksOrders XML function</a>.
    <li>The <a href="/handlers/mes.vb">mes.ashx</a> handler returns a table containing record ID, Sucsess and Message, to set the WorksOrder SENT flag.
</p>